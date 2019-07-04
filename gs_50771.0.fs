/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#50771.0"
}
*/


// water turbulence effect by joltz0r 2013-07-04, improved 2013-07-07
// Altered
#ifdef GL_ES
precision mediump float;
#endif


#define MAX_ITER 8
void main( void ) {
	vec2 p = vv_FragNormCoord*8.0- vec2(20.0);
	vec2 i = p;
	float c = 1.0;
	float inten = .02;

	for (int n = 0; n < MAX_ITER; n++) 
	{
		float t = TIME + c*c/40.-p.x*0.2;
		i = p + vec2(cos(t - i.x) + sin(t + i.y), sin(t - i.y) + cos(t + i.x));
		c += 1.0/length(vec2(p.x / (sin(i.x+t)/inten),p.y / (cos(i.y+t)/inten)));
	}
	c /= float(MAX_ITER);
	c = 1.5-sqrt(c);
	gl_FragColor = vec4(vec3(c*c*c*c), 1.0) + vec4(0.0, 0.3, 0.5, 1.0);

}