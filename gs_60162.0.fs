/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60162.0"
}
*/


// your dogs shitbox
#ifdef GL_ES
precision mediump float;
#endif



const float PI = 3.14159265;

vec2 rotate(vec2 v, float a) {
	float sinA = sin(a);
	float cosA = cos(a);
	return vec2(v.x * cosA - v.y * sinA, v.y * cosA + v.x * sinA); 	
}

float square(vec2 uv, float d) {
	return max(abs(uv.x), abs(uv.y)) - d;	
}

float smootheststep(float edge0, float edge1, float x)
{
    x = clamp((x - edge0)/(edge1 - edge0), 0.0, 1.0) * 3.14159265;
    return 0.5 - (cos(x) * 0.5);
}


void main( void ) {
	
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	uv = uv * 2.0 - 1.0;
	uv.x *= RENDERSIZE.x / RENDERSIZE.y;
	uv *= .75;
	
    	float blurAmount = -0.005 * 1000.0 / RENDERSIZE.y;
    
	float period = 2.0;
	float btime = TIME / period;
	btime = mod(btime, 1.0);
	btime = smootheststep(0.0, 1.0, btime);
	
	gl_FragColor = vec4(0.0, 0.0, 0.0, 1.0);
	for (int i = 0; i < 9; i++) {
		float n = float(i);
		float size = 1.0 - n / 9.0;
		float rotateAmount = (n * 0.5 + 0.25) * PI * 2.0; 
		gl_FragColor.rgb = mix(gl_FragColor.rgb, vec3(1.0), smoothstep(0.0, blurAmount, square(rotate(uv, -rotateAmount * btime), size)));
		float blackOffset = mix(1.0 / 4.0, 1.0 / 2.0, n / 9.0) / 9.0;
		gl_FragColor.rgb = mix(gl_FragColor.rgb, vec3(0.0), smoothstep(0.0, blurAmount, square(rotate(uv, -(rotateAmount + PI / 2.0) * btime), size - blackOffset)));
		gl_FragColor.gb *= 0.8;
		gl_FragColor.b *= 0.8-uv.y;
    }
}