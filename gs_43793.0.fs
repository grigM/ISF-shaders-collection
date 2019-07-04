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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43793.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float hex(vec2 p) 
{
	p.x *= 0.57735*2.0;
	p.y += mod(floor(p.x), 2.0)*0.5;
	p = abs((fract(p) - 0.5));
	float par = 0.5;
	return abs(max(p.x*1.5 + p.y + par, p.y*2.0 + par) - 1.0);
}

float sqr(vec2 p) 
{
	float s = 1.0;
	float a = mod(p.x, s);
	float b = mod(p.y, s);
	return (a<0.3 || b<0.3) ? 1.0 : 0.0;
}

float Koef(vec2 offset) 
{
	float t = TIME*2.0;
	vec2 p = 50. * offset / RENDERSIZE.x;
	float s = sin(dot(p, p) / -128. + t * 2.);
	s = pow(abs(s), 2.5) * sign(s);
	float  r = .15 + .3 * s;
	return smoothstep(r - 0.1, r + 0.1, sqr(p + p * r + 0.5));
}

void main( void ) {

	vec2 pos = gl_FragCoord.xy - RENDERSIZE / 2.;
	//vec2 mouseKoef = (mouse -.5);
	vec2 epsi = pos / 128.;
	epsi = epsi*0.5;
	gl_FragColor = vec4(Koef(pos - epsi), Koef(pos), Koef(pos + epsi), 1.0);
}