/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#45968.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float func(float x){
	return  3.*asin(sin(x*1.+TIME*2.));	
}


void main( void ) {

	vec2 position = (( gl_FragCoord.xy / RENDERSIZE.xy ) * 2.0 - 1.0) * 10.0;

	float color = 0.0;
	// start from 4;
	for (float i=4.0;i<22.0;i++) {
	
		color += 1.- smoothstep(0.01, 0.1, abs(position.y + func(position.x/i*2.)));
	 
	 

	gl_FragColor = vec4( color*(i/4.), color, color, 1.0 );

	}// i inside
		
}