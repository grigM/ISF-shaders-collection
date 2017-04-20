/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35289.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	gl_FragColor = vec4( 1.0 );
	
	vec2 Z = 8.*vv_FragNormCoord;
	float ph = 0.;
	vec2 dZ = vec2(0.);
	float width = 2.;
	for(float i = 0.; i <= 1.; i += 1./16.){
		if(-Z.y - abs(Z.x) > 0. && Z.y > -width) gl_FragColor = vec4(1,0,0,1);
		width /= (2.-pow(i, 0.5));
		dZ = vec2(0.,0.)+10.*(vec2(0.5, sin(TIME*1.1)*0.5+0.5)-.5);
		ph = sign(vv_FragNormCoord.x)*3.14159/4.;
		Z += dZ;
		Z *= mat2(cos(ph), sin(ph), -sin(ph), cos(ph));
	}
	
}