/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60761.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main(void){
	
	vec2 p = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
	vec3 color = vec3(0.0, 0.3, 0.5);
	float d = 0.8-smoothstep(0.0,.8,length(p));
	float f = 0.0;
	float PI = 3.141592;
	for(float i = 0.0; i < 8.0; i++){
		
		float s = sin(0.5*TIME + i * PI / 40.0) * 0.01;
		float c = cos(0.5*TIME + i * PI / 40.0) * 10.5;
 
		f +=  0.01 / ( d + p.x + c * d + p.y + s);
	}
	
	
	gl_FragColor = vec4(vec3(f * color), 1.0);
}