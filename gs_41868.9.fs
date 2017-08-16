/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#41868.9",
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
      "TARGET" : "backbuffer",
      "PERSISTENT" : true
    }
  ]
}
*/


#ifdef GL_ES
precision lowp float;
#endif

#extension GL_OES_standard_derivatives : enable


#define rmouse mouse*RENDERSIZE

float xx(float f)
{	return sin(TIME * f) * .5 + .5;
}

void main( void ) {
	float radius = 1.0*(sin(TIME*4.0)+1.0)+50.0;
	float gradius = 2.0*(sin((TIME-2.0)*3.5)+1.0)+48.0;
	
	
	vec2 c = vec2(xx(1.0), xx(1.111)) * RENDERSIZE;
	vec2 dist = c-gl_FragCoord.xy;
	//vec2 dist = rmouse.xy-gl_FragCoord.xy;
	float i = 1.0-smoothstep(radius,radius+1.5,length(dist));
	float gi = 1.0-smoothstep(gradius,gradius+50.0,length(dist));
	i=(i+gi);
	vec4 f = vec4(i*(gl_FragCoord.x/RENDERSIZE.x),i*(gl_FragCoord.y/RENDERSIZE.y),i,1.0);
	gl_FragColor = max(f,IMG_NORM_PIXEL(backbuffer,mod(gl_FragCoord.xy/RENDERSIZE.xy,1.0),0.1))-.002;
}