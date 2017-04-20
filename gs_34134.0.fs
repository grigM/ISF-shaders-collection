/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34134.0",
  "INPUTS" : [
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 4.0
		},
		{
			"NAME": "amplitude",
			"TYPE": "float",
			"DEFAULT": 0.8,
			"MIN": 0.4,
			"MAX": 1.6
		},
		{
			"NAME": "repeat",
			"TYPE": "float",
			"DEFAULT": 1.25,
			"MIN": 0.3,
			"MAX": 8.0
		},{
			"NAME": "step",
			"TYPE": "float",
			"DEFAULT": 100,
			"MIN": 2,
			"MAX": 200
		},{
			"NAME": "contrast",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": 0.1,
			"MAX": 0.5
		}
		
  ],
  "PERSISTENT_BUFFERS" : [
    "backbuffer"
  ],
  "PASSES" : [
    {
      "TARGET" : "backbuffer"
    }
  ]
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



//#define p vv_FragNormCoord

#define stepped(x, n) (floor(x*n)/n)

vec2 rot(in vec2 v, in float t) {
	mat2 r = mat2(cos(1.0), -sin(t),
		      sin(t), cos(1.0));
	return r * v;
}

void main( void ) {
	vec2 p=(gl_FragCoord.xy*2.-RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y);

	gl_FragColor = vec4( contrast );
	
	vec2 a  = rot(abs(p), TIME*(speed/amplitude));
	float m = max(a.x,a.y);
	float s = stepped(m, step);
	float f = fract(-TIME*speed+repeat*s);
	gl_FragColor += f * IMG_NORM_PIXEL(backbuffer,mod((gl_FragCoord.xy / RENDERSIZE),1.0)) + 0.1;
	
}