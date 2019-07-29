/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#37458.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define PI 3.1415926

#define speed 0.3
#define size 16.0*(1.0+.5*cos(TIME))
const int numParticles = 200;

#define time2 (60.0-TIME)
#define time3 cos(TIME*PI/(2.0*60.0))*time2
float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main( void ) {
	

	vec2 p = ( gl_FragCoord.xy / RENDERSIZE.x ) - vec2(0.5,0.5);

	float v = mix( 1.-dot(p,p), sqrt(abs(1.-dot(p,p)*(7.0+14.0*sin(TIME*0.5)))), sin(TIME)*0.5+0. );
	v *= pow(v,102.0*abs(sin(TIME)));
	p *= v;
	vec2 position = p;
	
	position -= vec2(0.5, 0.3);
	//position *= 0.5;
	
	float color = 0.0;
	for (float i=1.0; i <= float(numParticles); i += 1.0)
	{
		float angle = 2.0*PI*rand(vec2(i));
		
		vec2 offset = vec2(rand(vec2(rand(vec2(i)))), rand(vec2(rand(vec2(rand(vec2(i)))))));
		
		float speedFactor = (0.05 + 0.95*rand(vec2(float(numParticles)-i)));
		offset = vec2(0.);
		offset += speed *(speedFactor)* time3*vec2(cos(angle),sin(angle));
		
		
		position *= 1.0;
		color += step(pow(distance(2.0*fract(position-offset)-1.0, vec2(0.0)), 0.5), size/100.0);
	
		
	}	
	
	
	gl_FragColor = vec4( vec3( color,0.,0. ), 1.0 );

}