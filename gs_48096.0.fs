/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#48096.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	
	vec2 p = 2.0*( gl_FragCoord.xy / RENDERSIZE.xy )-1.0;
	p.x *= RENDERSIZE.x/RENDERSIZE.y; 
	vec3 col = vec3(0);
	
	
	col = vec3(0.98,0.99,1.00)*sin(p.y*5.0+TIME+clamp(1.0/(50.0*abs(length(p.xy)-0.5)), 0.0, 1.0)*2.0);
	gl_FragColor = vec4(col, 1.0); 
}