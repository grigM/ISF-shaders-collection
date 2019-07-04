/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#16542.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main( void ) {
	
	
	const float iterations = 255.;
	float x = gl_FragCoord.x - RENDERSIZE.x / 2.0;
	float y = gl_FragCoord.y - RENDERSIZE.y / 2.0;
	for (float i = 2.; i < iterations; i++)
	{
	if (mod((x * 8.0) / (y / 8.0), i) == 0.)
	{
		gl_FragColor = vec4( vec3(i / iterations), 1.0 );
	}
	}
}