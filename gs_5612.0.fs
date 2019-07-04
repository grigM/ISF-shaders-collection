/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#5612.0"
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
	for (int i = 0; i < 1; i++) {
		float angleFract = fract(angle*256.);
		float angleRnd = floor(angle*256.)+1.;
		float angleRnd1 = fract(angleRnd*fract(angleRnd*.7235)*45.1);
		float angleRnd2 = fract(angleRnd*fract(angleRnd*.82657)*13.724);
		float t = TIME+angleRnd1*10.;
		float radDist = sqrt(angleRnd2+float(i));
		
		float adist = radDist/rad*.1;
		float dist = (t*.1+adist);
		dist = abs(fract(dist)-.5);
		color += max(0.,.5-dist*40./adist)*(.5-abs(angleFract-.5))*5./adist/radDist;
		
		angle = fract(angle+.61);
	}

	gl_FragColor = vec4( color )*.3;

}