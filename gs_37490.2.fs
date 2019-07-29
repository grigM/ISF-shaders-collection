/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#37490.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



void main( void ) {

	vec2 position=((gl_FragCoord.xy*2.-RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y))*0.4; 
	float color = 0.0;

	for(int i = 0; i < 30; i++) {	   
	   vec2 ikinci = vec2(sin(TIME * float(i) * 0.2) * 0.02 * float(i),
			      cos(TIME * float(i) * 0.2) * 0.02 * float(i));
		
	   if(length(position + ikinci) < 0.01 * sqrt(float(i))) {
	      color = abs(cos(TIME * float(i)));
	   }
	}

	gl_FragColor = vec4( color, color, color, 1.0 );

}