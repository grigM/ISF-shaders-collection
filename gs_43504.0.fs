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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43504.0"
}
*/


// Looks oddly like a 3d shape but it isn't really

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
	vec2 p = (gl_FragCoord.xy - RENDERSIZE / 2.) / RENDERSIZE.x;
	vec2 z = vec2(0.7, -0.7);
	float d = 99.0;
	for (int i = 0; i < 10; i++) {
		z =  z.yx * vec2(1.0, 2.0-(mouse.y*5.0)) + (2.5-(mouse.x*2.0));
		p += z*z;
		z = cmul(z, p);
		d = min(d, distance(p, z));
	}
	gl_FragColor = vec4(1.0 - d);
}