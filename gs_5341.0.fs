/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#5341.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main( void ) {

	vec2 position = ( gl_FragCoord.xy -  RENDERSIZE.xy*.5 ) / RENDERSIZE.x;

	// 256 angle steps
	float angle = atan(position.y,position.x)/(2.*3.14159265359);
	angle -= floor(angle);
	float rad = length(position);
	
	float color = 0.0;
	for (int i = 0; i < 100; i++) {
		float angleMul = angle*(float(i)+1.)*32.;
		float angleMulDeriv = (float(i)+1.)*32.;
		float angleFract = fract(angleMul+.1);
		float angleRnd = floor(angleMul+.1)+1.;
		float angleRnd1 = fract(angleRnd*fract(angleRnd*.7235)*45.1);
		float angleRnd2 = fract(angleRnd*fract(angleRnd*.82657)*13.724);
		float t = TIME*4.+angleRnd1*10.;
		float radDist = sqrt(angleRnd2+float(i)+1.);
		
		float adist = radDist/rad*.1*angleMulDeriv/32.;
		float dist = (t*.1+adist);
		dist = abs(fract(dist)-.5);
		float xdist = dist*5./adist;
		float ydist = abs(angleFract-.5);
		
		
		color += max(0.,5.-(xdist*xdist+ydist*ydist)*(adist*adist*radDist*radDist)*100.)*5.;
		
		angle = fract(angle+.61);
	}

	gl_FragColor = vec4( color )*.3;

}