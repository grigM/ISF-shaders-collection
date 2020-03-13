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
      "DEFAULT" : [
        0.5,
        0.5
      ],
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#56585.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

//#extension GL_OES_standard_derivatives : enable


#define saturate(value) clamp(value, 0.0, 1.0)


vec3 drawCircle(vec2 pos, vec2 center, float radius) {
	return vec3(distance(pos, center) > radius ? 0. : 1.);
}

vec3 drawLine(vec2 pos, vec2 center, float radius) {
	float offset = abs(center.y - pos.y) * 0.7;
	return vec3(distance(pos.x + offset, center.x) > radius ? 0. : 1.);
}

float GetState(float speed) {
	return abs(fract(TIME * speed) * 2.0 - 1.0);
}


void main( void ) {
	
	vec2 m = (mouse * 2.0 - 1.0) * vec2(RENDERSIZE.x / RENDERSIZE.y, 1.0);
	vec2 pos = (gl_FragCoord.xy * 2. - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
	
	float state = GetState(0.5);
	float state1 = state;
	float widthLine = 0.2;
	
	vec3 color = drawCircle(pos, m, 0.01);
	color += drawCircle(pos, vec2(0.0), 0.01);
	float offset = state1 + widthLine;
	float valueOffset = 0.0;
	
	for (int i=0; i<30; i++) {
		color += drawLine(pos + vec2(valueOffset, 0.0), m, widthLine * 0.5);
		valueOffset += offset;
	}
	gl_FragColor = vec4(color, 1.0);
}
