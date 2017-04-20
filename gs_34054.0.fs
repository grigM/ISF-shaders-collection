/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 3.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "speed2",
			"TYPE": "float",
			"DEFAULT": 0.25,
			"MIN": 0.1,
			"MAX": 3.0
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34054.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

//#define p vv_FragNormCoord

#define stepped(x, n) (floor(x*n)/n)

void main( void ) {
	
	vec2 p=(gl_FragCoord.xy*2.-RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y);
	gl_FragColor = vec4( 1.0 );
	vec2 a = abs(p);
	float m = max(a.x,a.y);
	float s = stepped(m, 45.);
	float f = fract(-TIME*speed2+1.5*s+0.1*cos(20.*length(p)+TIME*speed));
	gl_FragColor *= f*f;
	
}