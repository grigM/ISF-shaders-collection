/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#42721.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main( void ) {
	vec3 light_color = vec3(2,2,2);
	
	float t = TIME*0.4;
	
	vec2 position = ( gl_FragCoord.xy * 2.0 -  RENDERSIZE.xy) / RENDERSIZE.x;

	// 256 angle steps
	float angle = atan(position.y, position.x) / (3.14159265359);
	//angle -= floor(angle);
	float rad = 0.2+length(position);
	
	float color = 0.0;

	float angleFract = fract(angle*256.);
	float angleRnd = floor(angle*256.)+100.;
	float angleRnd1 = fract(angleRnd*fract(angleRnd*.7235)*45.1);
	float angleRnd2 = fract(angleRnd*fract(angleRnd*.82657)*13.724);
	float t2 = t + angleRnd1*100.0;
	float radDist = sqrt(angleRnd2);
	
	float adist = radDist / rad * 0.05;
	float dist = (t2*.1+adist);
	dist = abs(fract(dist) / 1.0);
	color +=  (1.0 / dist) * cos(sin(t)) * adist / radDist / 30.0;  // cos(sin(t)) make endless.
	
	gl_FragColor = vec4(color, color, color,1);
}