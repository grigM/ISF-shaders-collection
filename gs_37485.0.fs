/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#37485.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define PI 3.1415926

#define speed 0.3
#define size 1.3
const int numParticles = 200;

#define time2 (60.0-TIME)
#define time3 cos(TIME*PI/(2.0*60.0))*time2
float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}


void main( void ) {
	

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.x ) - vec2(0.5,0.5);

	position -= vec2(0.5, 0.3);
	//position *= 0.5;
	
	vec3 color = vec3(0.0);
	for (float i=1.0; i <= float(numParticles); i += 1.0)
	{
		
		float p;
		float angle = 2.0*PI*rand(vec2(i));
		
		//vec2 offset = vec2(rand(vec2(rand(vec2(i)))), rand(vec2(rand(vec2(rand(vec2(i)))))));
		
		float speedFactor = (0.05 + 0.95*rand(vec2(float(numParticles)-i)));
		//offset = vec2(0.);
		vec2 offset = speed *(speedFactor)* time3*vec2(cos(angle),sin(angle));
		
		
		position *= 1.0;
		float q = pow(distance(2.0*fract(position-offset)-1.0, vec2(0.0)), 0.5);
		q = pow(q,0.2);
		//color += step(q, size/100.0);
		
		p = smoothstep(q-0.008,q+0.008,size/1.8);
		
		color += mix (vec3(0.0),vec3(1.0,.8*rand(vec2(rand(vec2(i)))),0.0),p);
		
		
		
		//color += vec3(p);
		
		
	
		
	}	
	
	
	gl_FragColor = vec4( color, 1.0 );

}