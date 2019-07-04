/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#47658.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

#define tau 6.28

vec3 shape(vec2 st) {
	vec3 c;
	float t = TIME, f = 5.;
	for (int i = 0; i < 3; i++) 
		c[i] = (f / 3.) / abs(st.x * 8. + 
		      sin(32. * length(st) + st.y * f * .5 + (t += 6.)) * cos((t += .2) + st.y / 2. * tau * (f += .2 * float(i))));	
	return c;
}

vec3 shape2(vec2 st) {
	vec3 c;
	float t = TIME, f = 5.;
	for (int i = 0; i < 3; i++) 
		c[i] = (.0) / abs(sin((t += 0.001) + TIME * - normalize(length(st))));
	return c;
}

void main( void ) {
	
	vec2 R  = RENDERSIZE;
	vec2 uv = (2. * gl_FragCoord.xy - R) / R.y;
	const float STEP = 10.;
	vec3 c = vec3(0.01);
	for (float i = 0.; i < 0.5; i += 1. / STEP) {
		c += shape(normalize(uv));
	}
	c /= STEP;
		
	vec3 r = shape2(uv);
	
	gl_FragColor = vec4(mix(c,r,1.) + (c), 2018.);

}