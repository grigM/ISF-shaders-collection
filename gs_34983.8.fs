/*
{
  "CATEGORIES" : [
    "Automatically Converted"
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34983.8"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

#define r RENDERSIZE
#define PI 3.1415926535897932384626
#define CS(n) (0.5*cos(TIME*n)+0.5)
#define SN(n) (0.5*sin(TIME*n)+0.5)
#define COS(n) cos(TIME*n)
#define SIN(n) sin(TIME*n)

vec3 cc(float x, vec3 a, vec3 b, vec3 c, vec3 d)
{
	return a + b*cos(2.*PI*cos(c*x + d));
}
float sdCappedCylinder( vec2 p, vec2 h )
{
  vec2 d = abs(vec2(length(p.x),p.y)) - h;
  return min(max(d.x,d.y),0.0) + length(max(d,0.0));
}

void main( void ) {
	vec2 p = gl_FragCoord.xy/r*2.-1.;
	p.x *= r.x/r.y;
	vec2 m = mouse*2. - 1.;
	m.x *= r.x/r.y;
	
	float cyl = sdCappedCylinder(
		mat2(COS(m.x*.05), -SIN(m.x*.05), SIN(m.x*.05), COS(m.x*.05))*(p - m),
		vec2(abs(m.y), 0.001));
	
	float rpcyl = cos(2.*PI*2.*cyl + 3.*CS(2.));
	
	vec3 color = cc(rpcyl, vec3(.5), vec3(.5), vec3(1.), vec3(.1, .2, .3));
	
	gl_FragColor = vec4(color, 1.0);
}