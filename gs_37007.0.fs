/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#37007.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	vec2 position = gl_FragCoord.xy;


	vec4 finalColor = vec4(0.0, 0.0, 0.0, 1.0);
	
	for (int i = 0; i < 10; i++) {
		float a = float(i)*3.14/5.;
		vec2 loc = 100.0*vec2(cos(a)*cos(TIME), sin(TIME)*sin(a)) + RENDERSIZE.xy/2.0;
	
		if(distance(position, loc) < 23.0+sin(TIME*1.0+a)*22.0){
			finalColor += 0.5*vec4(.5, 0.0, .5, 1.0);
		}
	}
	
	gl_FragColor = finalColor;
}