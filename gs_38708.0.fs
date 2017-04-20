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
		"DEFAULT": 1.0,
		"MIN": -4.0,
		"MAX": 4.0
	},
	{
		"NAME": "circScale",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 2.0
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#38708.0"
}
*/


// By: Brandon Fogerty
// bfogerty at gmail dot com

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float circle( vec2 uv, float radius )
{
	return 1.0 -length( uv / radius );
}

float random(float x)
{
	return clamp(fract(sin(x) * ((TIME)+2389.4392)), 0.0, 1.0 );
}

void main( void ) 
{

	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy ) * 2.0 - 1.0;
	uv.x *= (RENDERSIZE.x / RENDERSIZE.y);
	
	float shape = circle(uv, sin(TIME*speed)*(circScale)) - random(uv.x-uv.y);
	
	vec3 finalColor = vec3( shape );

	gl_FragColor = vec4( finalColor, 1.0 );

}