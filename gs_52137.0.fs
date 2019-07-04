/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#52137.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {
	float c1 = length(mouse * RENDERSIZE - gl_FragCoord.xy) - 10.0;
	float c2 = length(0.5 * RENDERSIZE - gl_FragCoord.xy) - 10.0;
	float c3 = length(0.1 * RENDERSIZE - gl_FragCoord.xy) - 10.0;
	
	gl_FragColor = vec4(fract(smoothstep(mod(mod(c1, c2), c3), 0.0, 30.0)));
}