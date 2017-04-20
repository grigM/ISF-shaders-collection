/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#38953.0"
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
	
	float ang = 100.25*3.1415926;
	p = mat2(cos(ang),-sin(ang),sin(ang),cos(ang))*p; 
	float d = -TIME*1.5+ abs(p.x)+abs(p.y);
	d = mod(d+10.10,0.2)-0.1; 
	if (abs(d) < 0.05) col=  vec3(10); 
	gl_FragColor =vec4(col, 1.0); 
}