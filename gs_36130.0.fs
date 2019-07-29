/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36130.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float lengt = 0.05;
float scale = 1.05;
vec2 p;
void f_2(){
	float rotr = 3.14159/2.;
	if(p.y < 0.){
		gl_FragColor = 1.-gl_FragColor;
	}
	p -= sign(p)*lengt;
	p *= scale*mat2(cos(rotr), sin(rotr), -sin(rotr), cos(rotr));
}
void f_3(){
	
}

void main( void ) {
	gl_FragColor = vec4( 1.0 );
	
	p = (gl_FragCoord.xy / RENDERSIZE) * 2.0 - 1.0;
	float aspect = RENDERSIZE.x / RENDERSIZE.y;
	p.y /= aspect;
	
	
	f_2();
	f_2();
	f_2();
	f_2();
	f_2();
	f_2();
	f_2();
	f_2();
	f_2();
	f_2();
	f_2();
	f_2();
	f_2();
	f_2();
	f_2();
	f_2();
	
}