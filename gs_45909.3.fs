/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#45909.3"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.1415926535898


vec3 hash33(vec3 p){     
    float n = sin(dot(p, vec3(7, 157, 113)));    
    return fract(vec3(2097152, 262144, 32768)*n); 
}

float voronoi(vec3 p) {
	vec3 b, r, g = floor(p);
	float d = 1.;

	p = fract(p); // "p -= g;" works on some GPUs, but not all, for some annoying reason.

	for(float j = -1.; j < 1.01; j++) {
		for(float i = -1.; i < 1.01; i++) {
			b = vec3(i, j, -1.);
			r = b - p + hash33(g+b);
			d = min(d, dot(r,r));
			
			b.z = 0.0;
			r = b - p + hash33(g+b);
			d = min(d, dot(r,r));
			
			b.z = 1.;
			r = b - p + hash33(g+b);
			d = min(d, dot(r,r));
		}
	}
	
	return d; // Range: [0, 1]
}

float noiseLayers(in vec3 p) {
	vec3 t = vec3(0., 0., p.z+sin(TIME*.25));

	const int iter = 5; // Just five layers is enough.
	float tot = 0., sum = 0., amp = 1.; // Total, sum, amplitude.

	for (int i = 0; i < iter; i++) {
		tot += voronoi(p + t) * amp; // Add the layer to the total.
		p *= 2.0; // Position multiplied by two.
		t *= 1.5; // Time multiplied by less than two.
		sum += amp; // Sum of amplitudes.
		amp *= 0.5; // Decrease successive layer amplitude, as normal.
	}

	return tot / sum; // Range: [0, 1].
}

//Extracts bit b from the given number.
float extract_bit(float n, float b) {
	b = clamp(b,-1.0,22.0);
	return floor(mod(floor(n / pow(2.0,floor(b))),2.0));   
}

//Returns the pixel at uv in the given bit-packed sprite.
float sprite(vec2 spr, vec2 size, vec2 uv) {
	uv = floor(uv);
	
	float bit = (size.x-uv.x-1.0) + uv.y * size.x;  
	bool bounds = all(greaterThanEqual(uv,vec2(0)))&& all(lessThan(uv,size)); 
	return bounds ? extract_bit(spr.x, bit - 21.0) + extract_bit(spr.y, bit) : 0.0;
}

void mainImage(out vec4 fragColor, in vec2 fragCoord) {
	vec2 uv = (fragCoord.xy - RENDERSIZE.xy*0.5) / RENDERSIZE.x;
	//uv.y *= RENDERSIZE.x/RENDERSIZE.y;

	vec3 rd = normalize(vec3(uv.x, uv.y, PI/8.));

	float cs = cos(TIME*0.525+(uv.x*uv.x+uv.y*uv.y)*10.0), si = sin(TIME*0.424-(uv.x*uv.x+uv.y*uv.y)*10.0);
	rd.xy *= mat2(cs, -si, si, cs);

	float c = noiseLayers(rd*.9);

	c *= sqrt(c*(1.-length(uv)))+sin(1.-length(uv))*2.;
	vec3 col = vec3(c);

	vec3 col2 =  rd.xyz * length(uv) * 1.0;
	col = mix(col, col.xyz*2.7+c*1.86, (rd.x*rd.y)*.95);
	col *= mix(col, col2, 1.-length(uv));

	fragColor = vec4(clamp(col, 0., 1.), 1.);
}

void main(void) {
	vec4 fragCol2 = gl_FragColor;

	mainImage(fragCol2, gl_FragCoord.xy);

	gl_FragColor = vec4( vec3(fragCol2.xyz), 1.0);
}