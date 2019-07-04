/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#15074.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

float rand(float num)
{
   return fract((sin(num*3146.18654)) * 43758.5453);
}
void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy  * 2.0 - 1.0 ) * vec2(RENDERSIZE.x / RENDERSIZE.y, 1.0);
	float color = 0.0;
	float a = atan(sin(position.x*3.0+TIME), cos(position.y*2.5));
	const float count = 32.0;
	for(float i = 0.0; i < count; i++)
	{
		color += (1.0 / (i+1.0))*(sin(a*i)*0.5+0.5);
	}
	color /= 3.0;
	gl_FragColor = vec4( vec3(color)*vec3(1.0, 0.86, 0.6), 1.0 );

}