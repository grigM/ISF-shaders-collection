/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60351.4"
}
*/


// 2D vector map
// Author: @amagitakayosi

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define PI 3.141593

void main( void ) {

	vec2 p = ( gl_FragCoord.xy * 2. - RENDERSIZE )/ min(RENDERSIZE.x, RENDERSIZE.y );
	//p *= 1.3;

	
	float l = length(p);
	
	float a = atan(p.y, p.x);
	vec4 c = vec4(cos(a), sin(a), 0, 1.);
	c *= (1. - l) * (1. - l);
	c.b = 0.;
	
	c = c * 0.5 + 0.5;
	
	c *= smoothstep(1., .99, length(p));
	

	gl_FragColor = c;

}