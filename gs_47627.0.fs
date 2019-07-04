/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
      "NAME": "desaturation",
      "TYPE": "float",
      "MIN": 0.0,
      "MAX": 1,
      "DEFAULT": 0.0
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#47627.0"
}
*/



#ifdef GL_ES
precision mediump float;
#endif
 
#extension GL_OES_standard_derivatives : enable
 
#define tau 6.28
 
 
mat2 m =mat2(0.8,0.8, -0.6, 0.8);
 
float rand(vec2 n) { 
	return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}
 
float noise(vec2 n) {
	const vec2 d = vec2(0.0, 1.0);
  	vec2 b = floor(n), f = smoothstep(vec2(0.0), vec2(1.0), fract(n));
	return mix(mix(rand(b), rand(b + d.yx), f.x), mix(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}
 
float fbm(vec2 p){
	float f=.0;
	f+= .5000*noise(p); p*= m*2.02;
	f+= .2500*noise(p); p*= m*2.03;
	f+= .1250*noise(p); p*= m*2.01;
	f+= .0625*noise(p); p*= m*2.04;
	
	f/= 0.9375;
	
	return f;
}
 
vec3 shape(vec2 st) {
	vec3 c;
	float t = TIME, f = 5.;
	for (int i = 0; i <1; i++) 
		c[i] = (f / 3.) / abs(st.x * 8. + 
		      cos(32. * length(st) + st.y * f * .5 + (t += 6.)) * cos((t += .2) + st.y / 2. * tau * (f += .2 * float(i))));	
	return c;
}
 
vec3 shape2(vec2 st) {
	vec3 c;
	float t = TIME, f = 5.;
	for (int i = 0; i < 1; i++) 
		c[i] = (.05) / abs(sin((t += .1) + 2. * length(st)));
	return c;
}
 
 
void main( void ) {
	
	vec2 R  = RENDERSIZE;
	vec2 uv = (2. * gl_FragCoord.xy - R) / R.y;
	const float STEP = 1.;
	float f=fbm(uv);
	vec3 c = vec3(0.);
	for (float i = .2; i < 1.; i += 1. / STEP) {
		c += fract(smoothstep(.3,0.9,shape(uv * i * f * 8.)));
	}
	c /= STEP;
	vec3 r=fract(shape2(10.*f*uv));
	
	
	
	
	
	
	vec3 grayXfer = vec3(0.5, 0.5, 0.5);
	vec3 gray = vec3(dot(grayXfer, r + c));
		
    
  
  
    
    
    gl_FragColor = vec4(mix(r + c, gray, desaturation), 1.);
	
	
	
 
}