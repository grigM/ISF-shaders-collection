/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#44548.0"
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
	
	
	float d = length(p.xy); 
	col = vec3(1,1,1)*1.0/(1.0 + 50.0*abs(d-0.5)-0.5); 
	
	float ang = -TIME/10.0;
	float ang2 = -TIME/100.0;
	d = dot(p.xy,vec2(cos(ang),sin(ang)));
	float d2 = dot(p.xy,vec2(sin(ang),-cos(ang)));
	col += vec3(1,1,1)*clamp(1.0-length(2.0*p.xy),0.0,d2)*4.0/(1.0 + 50.0*abs(d)-0.5); 
	
	d = dot(p.xy,vec2(cos(ang2),sin(ang2)));
	d2 = dot(p.xy,vec2(sin(ang2),-cos(ang2)));
	col += vec3(1,1,1)*clamp(1.0-length(3.0*p.xy),0.0,d2*2.0)*4.0/(1.0 + 50.0*abs(d)-0.5); 
	gl_FragColor = vec4(col, 1.0); 
}