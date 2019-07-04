/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#53636.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float hash(vec2 uv) {
	return fract(74455.45 * sin(dot(vec2(78.54, 214.45), uv)));
}

vec2 hash2(vec2 uv) {
	float  k = hash(uv);
	return vec2(k, hash(uv + k));
}

// IQ
vec3 palette(float t, vec3 a, vec3 b, vec3 c, vec3 d)
{
    return a + b*cos( 6.28318*(c*t+d) );
}
//

void main() {
	vec2 uv = (2. * gl_FragCoord.xy - RENDERSIZE) / RENDERSIZE.y;
	vec3 col = vec3(0.);
	for (int i = 0; i < 120; i++) {
		vec2 p = 2. * hash2(float(i) + vec2(22.)) - 1.;
		p -= vec2(sin(.1 * hash(float(i) + vec2(10., 50.)) * TIME + hash(float(i) + vec2(10.))), 
			  cos(.1 * hash(float(i) + vec2(20., 40.)) * TIME + hash(float(i) + vec2(10.))));
		float k = (.5 * hash(float(i) + vec2(25., 75.)) + .01);
		col += palette(k * 3., vec3(.5), vec3(.5), vec3(1.), vec3(.0, .33, .67)) / length(uv - p*p*p*p*sin(TIME/2.0));
		
		
	}
	col /= 360.;
	gl_FragColor = vec4(col, 1.);
}