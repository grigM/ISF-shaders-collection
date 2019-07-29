/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#37372.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {
	//vec2 position = vv_FragNormCoord;
	vec2 position=(gl_FragCoord.xy*2.-RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y); 
	float color;
	
	if((mod(sin(TIME*0.5)*abs(sin(position.x*1.5)), 0.10) < 0.05) ){
	   color = 1.0;
	}
	
	if((mod(cos(TIME*0.5)*abs(cos(position.y*1.5)), 0.10) < 0.03)){
	   color = 0.5;
	}
	
	gl_FragColor = vec4(color,color,color,1.0);
}