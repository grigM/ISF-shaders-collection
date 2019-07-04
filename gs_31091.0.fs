/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#31091.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define W 0.01
void main( void ) {
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	
	// distortion
	uv += (uv - 0.5) * (1.0 + 0.25 * sin(TIME)) * pow(1.0 - length(uv - 0.5), 1.0 + 1.9 * sin(TIME * 5.0));
	
	// grid
	float color = step(mod(uv.x+TIME*0.5, 0.1), W) + step(mod(uv.y, 0.1), W);
	
	gl_FragColor = vec4(color);

}