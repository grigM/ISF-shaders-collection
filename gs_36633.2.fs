/*
{
  "CATEGORIES" : [
    "Automatically Converted"
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36633.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float pli(float x, vec2 v[4], int k) {
	float val = 0.0;
	
	for (int j = 0; j < 4; j++) {
		float val2;
		val2 = (x-v[0].x)/(v[1].x-v[0].x);
		for (int i = 1; i < 4; i++) {
			if (i != j) {
				val2 *= (x-v[i].x)/(v[j].x-v[i].x);
			}
		}
		val += v[j].y * val2;
	}
	return val;
}

void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );

	float color = 1.0;

	vec2 v[4];
	
	v[0] = vec2(0.0, distance(vec2(position.x,position.y), mouse));
	v[1] = vec2(0.25, distance(vec2(position.x,position.y), mouse));
	v[2] = vec2(0.75, distance(vec2(position.x,position.y), mouse));
	v[3] = vec2(1.0, distance(vec2(position.x,position.y), mouse));
	float p = pli(1.0+position.y, v, 4);
	color -= p / 5.;
	
	gl_FragColor = vec4( vec3(0.0 , color * tan(p - TIME*5.), 0.2), 1.0 );

}