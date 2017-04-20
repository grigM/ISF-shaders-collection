/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35224.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float pi = 3.14159265358979323846264338;

float pulse(float a, float b, float t, float x) {
	return smoothstep(a, a + t, x) - smoothstep(b, b + t, x);
}

void main( void ) {

	vec2 p = ( gl_FragCoord.xy / RENDERSIZE.xy );
	p = 2.0 * p - 1.0;
	p.x *= RENDERSIZE.x / RENDERSIZE.y;

	float theta = atan(p.y, p.x);
	float d = length(p);
	float c = 0.0;
	for(int i = 0; i < 8; i++) {
		float j = float(i + 1);
		float s = (0.5 + 0.5 * sin(TIME * 1.0));
		s = s*s*(3.0 - 2.0*s);
		float s2 = 1.0 - abs(sin(TIME * 1.0));
		float t = 0.05 * s * TIME;
		float ph = t/j;
		float k = sin(theta * pow(2.0, j) + ph);
		k = smoothstep(0.0, 0.01 + 0.9 * s2, k);
		float r = i == 0 ? 0.0 : 0.1;
		float ring = pulse(r * j, 0.1 * j + 0.1, 0.005, d);
		c += clamp(ring * k, 0.0, 1.0);
	}
	gl_FragColor = vec4( vec3( c ), 1.0 );

}