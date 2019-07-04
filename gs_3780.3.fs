/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#3780.3"
}
*/


// stripey cube thing
// by gngbng
// this is mostly just a terrible mishmash of stolen code

#ifdef GL_ES
precision mediump float;
#endif


float scene(vec3 p, float t) {
	float coobs = 9999.; // wat the fuck am i doing
	for(int i = -2; i < 3; i++) {
		t += float(i+2);
		
		// help how do i matrice
		mat3 r = mat3(
			1, 0, 0,
			0, cos(t), -sin(t),
			0, sin(t), cos(t)
		);
		
		r *= mat3(
			cos(t), 0, sin(t),
			0, 1, 0,
			-sin(t), 0, cos(t)
		);
		
		r *= mat3 (
			cos(t/2.), -sin(t/2.), 0,
			sin(t/2.), cos(t/2.), 0,
			0, 0, 1
		);
				
		coobs = min(length(max(abs((p+vec3(i*2,0,0))*r)-1.,0.)),coobs);
	}
	
	return min(coobs,-p.z);	
}

float stripes(vec2 pos, float ratio) {
	return mod(pos.x - pos.y + TIME * 2., 1.) > ratio ? 1. : .75;
}

vec3 normal(vec3 p, float t)
{
	float d = 0.001;	
	float dx = scene(p + vec3(d, 0.0, 0.0), t) - scene(p + vec3(-d, 0.0, 0.0), t);
	float dy = scene(p + vec3(0.0, d, 0.0), t) - scene(p + vec3(0.0, -d, 0.0), t);
	float dz = scene(p + vec3(0.0, 0.0, d), t) - scene(p + vec3(0.0, 0.0, -d), t);
	return normalize(vec3(dx, dy, dz));
}

vec4 render(float t) {
	vec3 pos = vec3(0,0,-5);
	vec3 dir = normalize(vec3((gl_FragCoord.xy - RENDERSIZE.xy * .5) / RENDERSIZE.x, .5));
	
	float what = 0.;
	for(int i = 0; i < 16; i++) {
		float dist = scene(pos, t);
		pos += dist*dir;
		what += (1./(1.+dist))*dist; // cool occlusion shit stolen from kabuto i think
	}

	vec3 nrm = normal(pos, t);
	return vec4(vec3(stripes(gl_FragCoord.xy / RENDERSIZE.y * 20. - nrm.xy, 1.25-1./what))/(what*3.)*(1.+nrm.y*.5), 1.);
}

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main(void) {
	// lameshit stochastic moblur
	vec4 final = vec4(0);
	for(int i = 0; i < 4; i++) {
		final += render(TIME + rand(vec2(TIME+float(i)*.01+gl_FragCoord)) *  0.05);	
	}
	gl_FragColor = final / 4.;
}