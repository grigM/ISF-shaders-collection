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
			"MAX": 6.0
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34036.0"
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
	float f = fract(-TIME*speed+1.5*s);
	gl_FragColor *= f*f;
	
}