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
      "MAX" : 2040.0,
      "DEFAULT" : 255.0,
      "MIN" : 8.0
    },
    {
      "NAME" : "speed",
      "TYPE" : "float",
      "MAX" : 5.0,
      "DEFAULT" : 2.0,
      "MIN" : 0.1
    },
    {
      "NAME" : "mod_1",
      "TYPE" : "float",
      "MAX" : 64.0,
      "DEFAULT" : 8.0,
      "MIN" : 4.0
    },
    {
      "NAME" : "mod_2",
      "TYPE" : "float",
      "MAX" : 64.0,
      "DEFAULT" : 8.0,
      "MIN" : 4.0
    },

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#16542.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main( void ) {
	
	
	
	float x = gl_FragCoord.x - RENDERSIZE.x / 2.0;
	float y = gl_FragCoord.y - RENDERSIZE.y / 2.0;
	for (float i = 0.; i < iter; i++)
	{
		float xp = float(int((sin((TIME*speed)))*mod_1))+9.0;
		float yp = float(int(sin(TIME*speed)*mod_2))+9.0;
		if (mod((x * xp) / (y / yp), i) == 0.)
		{
			gl_FragColor = vec4( vec3(i / iter), 1.0 );
		}
	}
}