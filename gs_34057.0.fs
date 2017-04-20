/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34057.0",
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
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


#define mouse (.5+(.25+.125*cos(20.*length(position)+TIME*.345678901))*vec2(sin(TIME), sin(TIME*.22222)))
#define S(N) (1./float(N+1))

void main(void) {
	vec2 aspect = RENDERSIZE.xy / min(RENDERSIZE.x, RENDERSIZE.y);
	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy ) * aspect;
	vec4 color = IMG_NORM_PIXEL(backbuffer,mod(fract(vec2(1,3)/RENDERSIZE+(position) / aspect),1.0));
	if(color.a > S(1)){
		color.r -=  1./256.;
		color.g -=  2./256.;
		color.b -=  3./256.;
		if(length(color.r) < 1./256.){
			color.a = S(2);
		}
	}else if(color.a > S(2)){
		color.r +=  1./256.;
		if(length(color.r) > 1.- 1./256.){
			color.a = S(3);
		}
	}else if(color.a > S(99)){
		color.r +=  1./256.;
		color.g +=  2./256.;
		color.b +=  3./256.;
		if(length(color) > sqrt(3.)){
			color.a = S(0);
		}
	}
	
	
	
	if(length(pow(position/aspect-mouse, vec2(8))) < 1e-10) color = vec4(1.0);

	gl_FragColor = color;
}