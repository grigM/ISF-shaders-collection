/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#3135.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

// quantum mechanical wave function or whatever...


void main( void ) {

	vec2 position = (( gl_FragCoord.xy / RENDERSIZE.xy ) - .5) * sqrt(vec2(RENDERSIZE.x/RENDERSIZE.y,RENDERSIZE.y/RENDERSIZE.x)) + .5;
	
	vec3 ssum = vec3(0.), csum = vec3(0.), rsum = vec3(0.);
	float particle = 0.;
	
	float t = TIME * 0.37; 
	
	for (float i = .5; i < 100.; i+=1.) {
		float i2 = i*.04;
		float c0 = cos(t+i2);
		float s0 = sin(t+i2);
		float c1 = cos(t*.9+i2*.9);
		float s1 = sin(t*.9+i2*.9);
		float c2 = cos(t*2.3+i2*1.3);
		float s2 = sin(t*2.3+i2*1.3);
		
		float x2 = ((c0+1.)*(s1+1.)+c2+5.)*.06+.15;
		float y2 = ((s0+1.)*(-c1+1.)+s2+5.)*.06+.15;
		float dx2 = (-s0*(s1+1.)+(c0+1.)*.9*c1-2.3*s2)*.06;
		float dy2 = (c0*(-c1+1.)+(s0+1.)*.9*s1+2.3*c2)*.06;

		vec2 p = position-vec2(x2,y2);
		float a = (p.x*dx2+p.y*dy2)*300.;
		vec3 av = a*(vec3(1.,1.1,1.2))*2.;
		float mix = (1.-cos((i+1.)*2.*3.141592653589/101.));
		float e = exp(-a*a*.04);
		csum += cos(av)*e*mix;
		ssum += sin(av)*e*mix;
		rsum += (1.-e*e)*mix*mix;
	}
	
	gl_FragColor = vec4(sqrt(csum*csum+ssum*ssum+rsum)*.01, 1.0 );
}