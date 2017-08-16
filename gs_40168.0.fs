/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40168.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


// so here's an easy one... if two things were revolving in opposite
// directions and one was going twice as fast as the other, how many
// unique times would they pass each other during one full revolution
// by the slower item... and of course the answer is three because
// the slower one goes 1/3 of the way around and the faster goes 2/3

void main(void)
{
	vec2 pos = gl_FragCoord.xy / RENDERSIZE - 0.5;
	pos.x /= RENDERSIZE.y / RENDERSIZE.x;
	float pi = atan(1.0, 0.0) * 2.0;
	float twopi = pi * 2.0;
	float band = (mouse.y > 0.5 ? 1.0 : smoothstep(0.35, 0.36, length(pos)) - smoothstep(0.45, 0.46, length(pos)));
	float ang = atan(pos.y, pos.x) + pi;
	float t1 = mod(-TIME, twopi);
	float t2 = mod(TIME * 2.0, twopi);
	vec2 a1 = vec2(cos(t1), sin(t1));
	vec2 a2 = vec2(cos(t2), sin(t2));
	float doppel1 = (mouse.x > 0.5 ? 0.4 : dot(pos, vec2(a1.y,-a1.x)));
	float doppel2 = (mouse.x > 0.5 ? 0.4 : dot(pos, vec2(a2.y,-a2.x)));
	float g = 0.1/abs(dot(pos, a1)) * doppel1;
	float r = 0.1/abs(dot(pos, a2)) * doppel2;
	gl_FragColor = vec4(r, r*0.1, band * 0.15+r, 1.0);
}