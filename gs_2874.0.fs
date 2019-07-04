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
    },
	{
      "NAME" : "grid_size_width",
      "TYPE" : "float",
      "DEFAULT" : 1.0,
      "MIN" : 0.0,
      "MAX" : 8.0
    },
    
    {
      "NAME" : "grid_size_height",
      "TYPE" : "float",
      "DEFAULT" : 1.0,
      "MIN" : 0.0,
      "MAX" : 8.0
    },
    
    {
      "NAME" : "thing_amp",
      "TYPE" : "float",
      "DEFAULT" : 0.05,
      "MIN" : 0.0,
      "MAX" : 0.2
    },
    {
      "NAME" : "speed",
      "TYPE" : "float",
      "DEFAULT" : 1.00,
      "MIN" : 0.0,
      "MAX" : 10.0
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#2874.0"
}
*/


#ifdef GL_ES
precision highp float;
#endif

uniform vec2 reyboard;


float thing(vec2 pos) 
{
	float ret = 0.0;
	pos.x = cos(pos.x / 3.14) * (sin((TIME*speed) * 0.5) ) / sign(pos.y) * tan(pos.x);
	pos.y = sin(pos.y * 3.14) + sign(pos.x) - sqrt(pos.y/pos.x);
	ret = max(pos.x - pos.y + sin((TIME*speed) * 0.1), pos.y) - cos((TIME*speed));
	return ret;
}

void main(void) 
{
	vec2 position = ( gl_FragCoord.xy / RENDERSIZE );
	vec2 world = (position - mouse) * 15.0;
	world.x *= (RENDERSIZE.x / RENDERSIZE.y)*grid_size_width;
	world.y *= (RENDERSIZE.x / RENDERSIZE.y)*grid_size_height;
	float dist = thing(world)*thing_amp;

	gl_FragColor = vec4( dist, dist, dist, 1.0 );


}