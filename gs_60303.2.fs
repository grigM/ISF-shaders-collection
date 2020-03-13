/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60303.2"
}
*/


// arse gout
#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


mat2 rot(float a)
{
 return mat2(cos(a*2.2), sin(2.*a), -sin(a*4.5), cos(a));
}
void main( void )
{
	vec2 p = gl_FragCoord.xy;
	p -= 0.5 * RENDERSIZE.xy;
	p /= RENDERSIZE.y;
	p *= rot(0.5 / length(p) + TIME);
	p = log((p));
	gl_FragColor = vec4(0.15 / length( p +2.) );
}