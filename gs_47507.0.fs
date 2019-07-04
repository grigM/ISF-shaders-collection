/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#47507.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


float scene(vec3 p) {
	float d = length(p * (1. / vec3(1., 1., 100))) - .1;
	return d;
}	

vec3 repeat(vec3 v) { vec3 r = vec3(2.); return mod(v + r / 2., r) - r / 2.; }
void main() {
	vec2 st = (2. * gl_FragCoord.xy - RENDERSIZE) / RENDERSIZE.y;
	float t = 0.;
	for (int i = 0; i < 32; i++) 
		t += .5 * (scene(
			repeat(vec3(2. * TIME, 1.+2. * TIME, TIME) +
			vec3(st, 2.) * t)
		));
	gl_FragColor = vec4(vec3(1. / t), 1.);	
}