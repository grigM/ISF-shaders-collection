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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#42629.2"
}
*/


// water turbulence effect by joltz0r 2013-07-04, improved 2013-07-07
// Altered
#ifdef GL_ES
precision mediump float;
#endif

//vec2 vv_FragNormCoord = vec2 (1024.,128.);
#define MAX_ITER 8
void main( void ) {
	vec2 p = vv_FragNormCoord*3.0- vec2(15.0);
	vec2 i = p;
	float c = 1.0;
	float inten = .05;

	for (int n = 0; n < MAX_ITER; n++) 
	{
		float t = TIME/0.5 * (1.0 - (3.0 / float(n+1)));
		i = p + vec2(cos(t - i.x) + sin(t + i.y), sin(t - i.y) + cos(t + i.x));
		c += 1.0/length(vec2(p.x / (2.*sin(i.x+t)/inten),p.y / (cos(i.y+t)/inten)));
	}
	c /= float(MAX_ITER);
	c = 1.5-sqrt(pow(c,3.+mouse.x*0.5));
	gl_FragColor = vec4(vec3(c*c*c*c*1.0,c*c*c*c*1.0,c*c*c*c*1.0), 1.0);

}