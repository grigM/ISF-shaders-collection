/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59937.0"
}
*/


// Looks oddly like a 3d shape but it isn't really
//+
#ifdef GL_ES
precision mediump float;
#endif


vec2 cmul(const vec2 c1, const vec2 c2)
{
	return vec2(
		c1.x * c2.x - c1.y * c2.y,
		c1.x * c2.y + c1.y * c2.x
	);
}

void main( void ) {
	vec2 p = vv_FragNormCoord*4.0 + vec2(0.0, -1.0);
	vec2 z = vec2(0.9, -(sin(TIME*.25))*0.7);
	float d = 99.0;
	for (int i = 0; i < 10; i++) {
		z =  z.yx * vec2(1.0, -1.0) + 1.0;
		p += (sin(TIME*.3))*z*z;
		z = (cos(TIME*.2))*cmul(z, p);
		d = min(d, distance(p, z));
	}
	gl_FragColor = vec4(1.0 - d);
}