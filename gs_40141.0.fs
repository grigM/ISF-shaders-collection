/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40141.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main(void){
	
	vec2 p = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
	
	vec3 color = vec3(0.0, 0.6, 0.3);
	
	float f = 0.0;
	float PI = 3.141592;
	
	for(float i = 0.0; i < 200.0; i++){
		
		float s = sin(TIME + i * PI / 100.0) * 0.7;
		float c = cos(TIME + i * PI / 100.0) * 0.7;
 
		f += 0.001 / (abs(p.x + c) - abs(p.y + s));
	}
	
	gl_FragColor = vec4(vec3(f * color), 1.0);
}