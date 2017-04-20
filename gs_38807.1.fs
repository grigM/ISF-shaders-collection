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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#38807.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	vec2 pos = ( gl_FragCoord.xy / RENDERSIZE.xy ) + mouse / 4.0;
	
	pos.x += cos(pos.y*5.)*.2;
	pos.y += cos(pos.y*5.0+TIME)*.2+sin(pos.x*10.0+TIME)*.2;
	
	float color = clamp(-100.0+mod(pos.x+pos.y, .2)*1000.0, 0.0, 1.0);
	
	gl_FragColor = vec4( vec3( color, color, color ), 1.0 );

}