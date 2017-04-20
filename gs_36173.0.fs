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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36173.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) 
{
	vec2 uv 	= gl_FragCoord.xy / RENDERSIZE.xy;
	
	vec2 field	= gl_FragCoord.xy * 7./32.;
	field		= fract(field) 	* 1./8.;
	
	float angle	= fract(field.y + mouse.y);
	angle		= float(floor(angle * 8.) == floor(uv.y * 8.));
	
	gl_FragColor 	= vec4(angle);

}