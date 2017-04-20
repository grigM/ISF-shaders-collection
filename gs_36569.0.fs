/*
{
  "CATEGORIES" : [
    "Automatically Converted"
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36569.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy ) + mouse / 4.0;

	float color = 0.0;
	color +=  position.x/position.y*TIME;
	color *= fract(TIME-position.x+sin(position.y)-TIME+abs(position.x*TIME));

	gl_FragColor = vec4( color,vec3(color-TIME) );

}