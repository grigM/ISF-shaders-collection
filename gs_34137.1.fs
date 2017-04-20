/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34137.1",
  "INPUTS" : [

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
	gl_FragColor = vec4( 0. );
	vec2 a  = rot(abs(p), stepped(TIME, 12.));
	float m = max(a.x,a.y);
	float s = stepped(m, 100.);
	float f = fract(-stepped(TIME, 12.)*0.12+(1.+p.y)+1.5*s);
	gl_FragColor += f * IMG_NORM_PIXEL(backbuffer,mod(((gl_FragCoord.xy+8.*normalize(gl_FragCoord.xy-RENDERSIZE/2.)) / RENDERSIZE),1.0)) + 0.1;
	
}