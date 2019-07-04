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
		"DEFAULT": 20.0,
		"MIN": 0,
		"MAX": 100.0
		
	},
	{
		"NAME": "ofset",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0,
		"MAX": 5.0
		
	},
	{
		"NAME": "iter",
		"TYPE": "float",
		"DEFAULT": 15.0,
		"MIN": 0,
		"MAX": 20.0
		
	},
	{
		"NAME": "mod1",
		"TYPE": "float",
		"DEFAULT": 2,
		"MIN": 1,
		"MAX": 2
		
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#44978.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	
	float divisor = iter;
	
	divisor += mod(divisor, RENDERSIZE.y);
		
	float lum = (sin((floor(uv.y * divisor)) + (TIME * speed) - ofset) + 1.0) / mod1;

	gl_FragColor = vec4(mix(0.0, 1.0, lum));

}