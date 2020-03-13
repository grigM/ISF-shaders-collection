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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57554.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif
// forked from http://glslsandbox.com/e#7138.2 

#define PI 3.1415927
#define PI2 (PI*2.0)



void main(void)
{
	vec2 position = 1000.0* ((.1 * gl_FragCoord.xy -  RENDERSIZE.xy)/1000.);

	float r = length(position);
	float a = atan(position.y,mouse.x* position.x);
	float d = r - a + PI2;
	float n = PI2 * float(int(d / PI2));
	float k = a + n;
	float rand = sin(floor(0.017 * k * k - TIME));

	gl_FragColor.rgba = vec4(fract(rand*vec3(1.0, 100.0, 10000.0)), 1.0);
}