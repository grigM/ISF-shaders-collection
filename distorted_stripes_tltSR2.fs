/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tltSR2 by C_H.  \"Stolen\" from Étienne Jacob\nCheck out [url]https:\/\/necessarydisorder.wordpress.com\/2017\/09\/02\/animated-stripes-gifs-from-scalar-fields\/[\/url]",
  "INPUTS" : [

  ]
}
*/


#define PI 3.14159
#define TWO_PI 6.28318

// Perlin Noise from https://gist.github.com/patriciogonzalezvivo/670c22f3966e662d2f83
float rand(vec2 c){
	return fract(sin(dot(c.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

float noise(vec2 p, float freq ){
	float unit = 1./freq;
	vec2 ij = floor(p/unit);
	vec2 xy = mod(p,unit)/unit;
	xy = .5*(1.-cos(PI*xy));
	float a = rand((ij+vec2(0.,0.)));
	float b = rand((ij+vec2(1.,0.)));
	float c = rand((ij+vec2(0.,1.)));
	float d = rand((ij+vec2(1.,1.)));
	float x1 = mix(a, b, xy.x);
	float x2 = mix(c, d, xy.x);
	return mix(x1, x2, xy.y);
}

float pNoise(vec2 p, int res){
	float persistance = .5;
	float n = 0.;
	float normK = 0.;
	float f = 4.;
	float amp = 1.;
	int iCount = 0;
	for (int i = 0; i<50; i++){
		n+=amp*noise(p, f);
		f*=2.;
		normK+=amp;
		amp*=persistance;
		if (iCount == res) break;
		iCount++;
	}
	float nf = n/normK;
	return nf*nf*nf*nf;
}

void main() {



    vec2 uv = (gl_FragCoord.xy-.5*RENDERSIZE.xy)/RENDERSIZE.y;
	float n = pNoise(uv, 2);
    vec3 col = vec3(smoothstep(0.5, 0.8, (sin((TIME + uv.x * 20. + uv.y * 20. + n * smoothstep(.5, -.5, length(uv)) * 200.) * TWO_PI) + 1.) / 2.));
    col.b = .65;
    gl_FragColor = vec4(col,1.0);
}
