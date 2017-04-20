/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35340.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


// Simple Sine Wanve

void main( void ) {

	vec2 position = gl_FragCoord.xy / RENDERSIZE.xy;
	position -= 0.5;
	position *= 4.0;
	position.x += TIME;
	position.x *= RENDERSIZE.x / RENDERSIZE.y;  // Aspect Ratio Correction
	
	float sum = 0.;
	for(float i = 0.; i <= 1.; i += 1./23.){
		sum += sin(cos(position.y/(1.+i)+TIME*i)+position.x*(i+1.)+100./(0.01+i))/(i+4.);
	}
	
	if (position.y < sum) {
		gl_FragColor = vec4(0.0, 0.0, 0.0, 1.0);
	} else {
		gl_FragColor = vec4(1.0, 1.0, 1.0, 1.0);
	}

}