/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#31086.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	
	float t = TIME * 0.35;

	float distortion = length(uv - 0.5);
	distortion = pow( mod(1.0 - distortion + t, 1.0) ,24.0);
	uv += distortion;

	vec2 p = mod(uv, 0.1);
	float color = step(p.x, 0.005) + step(p.y, 0.005);
	gl_FragColor = vec4( color);

}