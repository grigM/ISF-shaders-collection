/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#55440.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec2 rotate(vec2 p, float d) {
	return vec2(cos(d)*p.x+sin(d)*p.y,-sin(d)*p.x+cos(d)*p.y);
}
	
float sdf(vec3 p){
	p.xy = rotate(p.xy, .5*sin(TIME+p.z));
	p += vec3(.5);
	p = fract(p+vec3(.5))-vec3(.5);
	return length(p)-.25;
}

void main( void ) {

	vec2 p = (2.*gl_FragCoord.xy - RENDERSIZE.xy) / min(RENDERSIZE.x, RENDERSIZE.y);

	vec3 cameraPos = vec3(cos(TIME)*sin(TIME*.2),sin(TIME)*sin(TIME*.2),TIME);
	vec3 cUp = vec3(0.,1.,0.);
	cUp.xy = rotate(cUp.xy, TIME);
	vec3 cDir = normalize(vec3(0.)-cameraPos);
	vec3 cRi = cross(cUp, cDir);
	float screenZ = 2.5;
	vec3 rayDirection = normalize(screenZ*cDir+cRi*p.x+cUp*p.y);
	
	vec3 col = vec3(1., .8, .8);
	
	float depth = 1.;
	
	for (int i=0;i<64;i++) {
		vec3 rayPos = cameraPos + rayDirection * depth;
		float dist = sdf(rayPos);
		depth += dist;
		if(dist<0.001){
			col = 2.*vec3(1.,cos(depth)*.5+.5,sin(depth*.6)*.5+.5)/pow(depth, .2);
			break;
		}
	}
	
	gl_FragColor = vec4(col, 1.0 );
}