/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
      "NAME" : "scale",
      "TYPE" : "float",
      "MAX" : 5.0,
      "DEFAULT" : 2.5,
      "MIN" : 1.3
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#58809.0"
}
*/


// water turbulence effect by joltz0r 2013-07-04, improved 2013-07-07
#ifdef GL_ES
precision mediump float;
#endif



#define MAX_ITER 6
void main( void ) {

vec2 p = (vec2(gl_FragCoord.xy - 0.5 * RENDERSIZE.xy) / RENDERSIZE.y)*scale;

	vec2 i = p;
	float c = 0.0;
	float inten = 1.0;

	for (int n = 0; n < MAX_ITER; n++) {
		float t = TIME * (1.0 - (1.0 / float(n+5)));
		i = p + vec2(cos(t - i.x) + sin(t + i.y), sin(t - i.y) + cos(t + i.x));
		c += 1.0/length(vec2(p.x / (sin(i.x+t)/inten),p.y / (cos(i.y+t)/inten)));
	}
	c /= float(MAX_ITER);
	
	gl_FragColor = vec4(vec3(pow(c,4.4))*vec3(1.0, 1.37, 1.50), 1.0);
}