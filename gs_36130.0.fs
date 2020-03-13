/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "scale",
			"TYPE": "float",
			"DEFAULT": 1.05,
			"MIN": 0.0,
			"MAX": 6.0
	},
	{
			"NAME": "param",
			"TYPE": "float",
			"DEFAULT": 20.0,
			"MIN": 0.0,
			"MAX": 20.0
	},
	{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 0.05,
			"MIN": 0.0,
			"MAX": 0.1
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36130.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable




vec2 p;
void f_2(){
	float rotr = 3.14159/2.;
	if(p.y < 0.){
		gl_FragColor = 1.-gl_FragColor;
	}
	p -= sign(p)*zoom;
	p *= scale*mat2(cos(rotr), sin(rotr), -sin(rotr), cos(rotr));
}
void f_3(){
	
}

void main( void ) {
	gl_FragColor = vec4( 1.0 );
	
	p = (gl_FragCoord.xy / RENDERSIZE) * 2.0 - 1.0;
	float aspect = RENDERSIZE.x / RENDERSIZE.y;
	p.y /= aspect;
	
	if(param>0.0){
		f_2();
	}
	if(param>1.0){
		f_2();
	}
	if(param>2.0){
		f_2();
	}
	if(param>3.0){
		f_2();
	}
	if(param>4.0){
		f_2();
	}
	if(param>5.0){
		f_2();
	}
	if(param>6.0){
		f_2();
	}
	if(param>7.0){
		f_2();
	}
	if(param>8.0){
		f_2();
	}
	if(param>9.0){
		f_2();
	}
	if(param>10.0){
		f_2();
	}
	if(param>11.0){
		f_2();
	}
	if(param>12.0){
		f_2();
	}
	if(param>13.0){
		f_2();
	}
	if(param>14.0){
		f_2();
	}
	if(param>15.0){
		f_2();
	}
	if(param>16.0){
		f_2();
	}
	if(param>17.0){
		f_2();
	}
	if(param>18.0){
		f_2();
	}
	if(param>19.0){
		f_2();
	}
	
	
	
	
}