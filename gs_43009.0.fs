/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43009.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define TIME TIME+gl_FragCoord.y/100.

float hex(vec2 p) {
	p.x *= 0.57735 * 2.0+sin(TIME+gl_FragCoord.x/400.)/5.;
	p.y += mod(floor(p.x), 2.0) * 0.5;
	p = abs((mod(p, 1.0) - 0.5));
	return abs(max(p.x * 1.5 + p.y, p.y * 2.0) - 1.0);
}


void main( void ) {
	vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE.xy) - min(RENDERSIZE.x, RENDERSIZE.y);
	uv.x += TIME * 50.0;
	//uv *= 10.0;
	//vec2 newUv = fract(uv);
	vec2 newUv = uv;
	
	vec2 p = newUv / 50.0; 
	float strokeWidth = 0.5 * abs(sin(TIME));	
	gl_FragColor = vec4(step(strokeWidth, hex(p)));
}