/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0,
		"MAX": 1
	},
	{
		"NAME": "glow",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": -40000,
		"MAX": 40000
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43677.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif



//float speed = .5;

vec2 startPos = vec2( 0.5, .5 );

void main( void ) {
	float intensity = 0.;
	for (float i = 0.; i < 20.; i++) {
		float angle = i/55. * 3. * 3.14159;
		vec2 xy = vec2(0. * cos(angle), 0.5 * cos(angle));
		xy += gl_FragCoord.xy/RENDERSIZE.xy-startPos;
		intensity += pow(100000., (0.77 - length(xy) * 1.5) * (1. + 0.25 * fract(-i / 55. - TIME/speed*.5))) / (80000.-glow);
	}
	gl_FragColor = vec4(clamp(intensity * vec3(1., 1., 1.), vec3(0.), vec3(1.)), 1.);
}