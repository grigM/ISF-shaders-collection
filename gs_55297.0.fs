/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#55297.0"
}
*/


// @paulofalcao
//
// Blue Pattern
//
// A old shader i had lying around
// Although it's really simple, I like the effect :)

// slow mod by @kapsy1312

#ifdef GL_ES
precision highp float;
#endif


void main(void)
{
	vec2 u=(gl_FragCoord.xy/RENDERSIZE.x)/1.0-vec2(1.0,RENDERSIZE.y/RENDERSIZE.x);
	
	float t=TIME*0.35;
	float tt=sin(t/5.0)*64.0;
	float x=u.x*tt+sin(t*2.1)*4.0;
	float y=u.y*tt+cos(t*2.3)*4.0;
	float c=cos(y)+sin(x);
	float zoom=sin(t);
	
	x=x*zoom/2.0+sin(t*1.1);
	y=y*zoom/2.0+cos(c*1.3);
	
	float xx=cos(t*0.7)*x-sin(t*0.7)*y;
	float yy=sin(t*0.7)*x+cos(t*0.7)*y;
	
	c=(sin(c+sin(xx)+sin(yy))+.80)*1.95;
	
	gl_FragColor=vec4((1.0-length(u)*0.90)*vec3(c*1.4,c*1.4,c*1.4),22.0);
}