/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59954.0"
}
*/


// shitter
//#ifdef GL_ES
//precision mediump float;
//#endif

//#extension GL_OES_standard_derivatives : enable


void main(void){
	
	vec2 p = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
	p.y = dot(p,p);
	vec3 color = vec3(0.0, 0.3, 0.5);
	
	float f = 0.8;
	float PI = 3.141592;
	for(float i = 0.0; i < 7.0; i++){
		
		float s = sin(TIME + i * PI / 10.0) * 0.8;
		float c = cos(TIME + i * PI / 10.0) * 0.8;
 
		f += 0.005 / (abs(p.x + c) * abs(p.y + s));
	}
	
	
	gl_FragColor = vec4(vec3(f * color), 1.0);
}