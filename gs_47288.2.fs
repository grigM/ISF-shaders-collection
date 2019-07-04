/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#47288.2"
}
*/


// http://glslsandbox.com/e#44284.1

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


mat2 rotate2d(float _angle){
	return mat2(cos(_angle), -sin(_angle),  sin(_angle), cos(_angle));
}

float fade(float x) {
	return x * x * x * (x * (x * 6.0 - 15.0) + 10.0);
}

float phash(float p) {
	p = fract(7.8233139 * p);
	p = ((2384.2345 * p - 1324.3438) * p + 3884.2243) * p - 4921.2354;
	return fract(p) * 2.0 - 1.0;
}

float cnoise(float p) {
	float ip = floor(p);
	float fp = fract(p);
	float d0 = phash(ip) *  fp;
	float d1 = phash(ip + 1.0) * (fp - 1.0);
	return mix(d0, d1, fade(fp));
}

void main( void ) {
	vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE.xy) / min(RENDERSIZE.x, RENDERSIZE.y);

	//  Backgroud.
	float noise = cnoise(uv.x / TIME * .0);
	vec3 color = vec3(11.0, 0.0, 245.0) * noise;
	
	// Rotate.
	uv *= rotate2d(TIME);
	
	// Main object.
	for (float i = 0.0; i < 10.0; i += 2.0) {
		float circle = smoothstep(0.1, 0.9, 1.0 - length(uv + cnoise(i /15.0)));
		color.r += circle;
		gl_FragColor = vec4(color, 1.0);
	}
}