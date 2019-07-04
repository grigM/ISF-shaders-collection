/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43710.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float hex(vec2 p) 
{
  p.x *= 0.57735;
	p.y += mod(ceil(p.y), 1.);
	p = abs((mod(p, 0.8) - 0.4));
	return abs(max(p.x + p.x, p.y * 2.) - 1.);
}


void main( void ) {

	vec2 pos = gl_FragCoord.xy - RENDERSIZE / 2.;
	vec2 p = 40. * pos / RENDERSIZE.x;
	float s = sin(dot(p, p) / -64. + TIME * 4.);
	s = pow(abs(s), 0.5) * sign(s);
	float  r = .35 + .25 * s;
	float t = pow(abs(sin(TIME * 4.)), 0.2) * sign(sin(TIME * 4.));
	t *= 0.25;
	p *= mat2(cos(t), -sin(t), sin(t), cos(t));
	gl_FragColor = vec4(smoothstep(r - 0.1, r + 0.1, hex(p)));
}