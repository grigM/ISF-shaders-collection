/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43753.0"
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
	p = abs((mod(p, 1.0) - 0.5));
	return abs(max(p.x*1.5 + p.y, p.y*2.0) - 1.0);
}


void main( void ) {

	vec2 pos = gl_FragCoord.xy - RENDERSIZE / 2.;
	vec2 p = 30. * pos / RENDERSIZE.x;
	float s = sin(dot(p, p) / -64. + TIME * 8.);
	s = pow(abs(s), 0.5) * sign(s);
	float  r = .1 + .25 * s;
	gl_FragColor = vec4(smoothstep(r - 0.1, r + 0.1, hex(p + p * r)));
}