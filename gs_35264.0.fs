/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35264.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {
	
	vec2 pos = ( gl_FragCoord.xy / RENDERSIZE.xy )*vec2(100,100);
	
	pos.y -= sin(pos.x / (3.14 * 10.0)) * sin(pos.y / (3.14 * 10.0) - 1.5) * sin(TIME) * 20.0 ;
	
	float c = TIME*10.0;
	float x = floor(pos.x);
	float y = pos.y+c;	
	gl_FragColor = vec4(0.1+cos(x*4.0),cos(y),cos(y),1);
	
			

}