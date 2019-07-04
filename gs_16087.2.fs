/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
      "NAME" : "iter",
      "TYPE" : "float",
      "MAX" : 30.0,
      "DEFAULT" : 15.0,
      "MIN" : 0.0
    },
    {
      "NAME" : "speed",
      "TYPE" : "float",
      "MAX" : 3.0,
      "DEFAULT" : 1.0,
      "MIN" : 0.0
    },
    {
      "NAME" : "width",
      "TYPE" : "float",
      "MAX" : 0.5,
      "DEFAULT" : 0.25,
      "MIN" : 0.0
    },
    {
      "NAME" : "sin_transform_amnt",
      "TYPE" : "float",
      "MAX" : 10000000.0,
      "DEFAULT" : 10000000.0,
      "MIN" : 0.1
    },
    {
      "NAME" : "dot_item_width",
      "TYPE" : "float",
      "MAX" : 1000.0,
      "DEFAULT" : 500.0,
      "MIN" : 10.1
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#16087.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif



#define PI 0.01

void main( void ) {

	vec2 p = ( gl_FragCoord.xy / RENDERSIZE.xy ) - 0.5;
	
	vec4 color;
	
	for (float i = 0.0; i < iter; i++)
	{
		
		float sx = width*sin( sin_transform_amnt * p.y - ((TIME*speed) + i));
		
		float dy = 1.0/ ( dot_item_width * abs(p.x - sx));
		
		color += vec4( dy - abs(p.y), dy - abs(p.y), dy - abs(p.y), dy - abs(p.y) );
		
	}
	
	gl_FragColor = color;

}