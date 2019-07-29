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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#56158.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define SRC_N 5.0
#define SRC_D .05
#define SRC_R .003
#define SRC_A .4
#define TWO_PI (2.0 * 3.1415)

void main( void ) {

	vec2 position = gl_FragCoord.xy / RENDERSIZE.y;
	float max_x = RENDERSIZE.x / RENDERSIZE.y;
	vec2 mouse_p = vec2(mouse.x * max_x, mouse.y);

	float q = 0.0;
	float src_f = TWO_PI * mix(2.0, 100.0, mouse.x);
	float src_d = SRC_D * mix(.1, 5.0, mouse.y);
	
	for (float i = 0.0; i < SRC_N; i++) {
		vec2 src_pos = vec2((max_x - src_d * (SRC_N - 1.0)) / 2.0 + float(i) * src_d, 0.5);
		float l = abs(abs(position.x-src_pos.x) - abs(position.y-src_pos.y));
		if (l < SRC_R) {
			gl_FragColor = vec4(1.0);
			return;
		}
		q += sin(l * src_f + i * SRC_A - TIME * 10.0);
	}
	q /= SRC_N;
	gl_FragColor = vec4(q * q);
}