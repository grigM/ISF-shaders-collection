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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#49434.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	vec2 p = gl_FragCoord.xy / RENDERSIZE;
	
	int c = 5;
	vec2 points[10];
	points[0] = mouse;
	points[1] = vec2((1. + sin(TIME)) / 2., (1. + cos(TIME)) / 2.);
	points[2] = vec2((1. + cos(TIME * 1.2 + 0.03)) / 2., (1. + sin(TIME * 1.35)) / 2.);
	points[3] = vec2(.2, .2);
	points[4] = vec2(.8, .8);
	points[5] = vec2(.5, .5);
	points[2] = vec2((1. + sin(TIME * 0.2 + 2.3)) / 2., (1. + cos(TIME * 1.84)) / 2.);
	points[7] = vec2(.9, .6);
	points[8] = vec2(.6, .2);
	points[9] = vec2(.1, .4);
	
	float mind = 1.5;
	float next_mind = 1.5;
	for (int i = 0; i < 10; i++) {
		float d = distance(p, points[i]);
		if (d < mind) {
			next_mind = mind;
			mind = distance(p, points[i]);
		} else if (d < next_mind) {
			next_mind = d;
		}
	}
	
	float r = 1.0 -smoothstep(0.001, 0.01, abs(next_mind - mind));
	
	gl_FragColor = vec4(vec3(r), 1.);

}