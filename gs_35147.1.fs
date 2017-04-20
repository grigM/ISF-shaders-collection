/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 3.5,
			"MIN": 0.0,
			"MAX": 10.0
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35147.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {
	
	vec2 pos=(gl_FragCoord.xy*2.-RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y);
	
	
	float len = length(pos*100.);
	gl_FragColor = vec4( (vec3(0.5+0.5*sin(TIME*speed-vec3(20.*len/pow(len,0.88))+(3.14*sin(len/pow(vec3(len),vec3(.77,.66,.45))))))), 1.0 );

}