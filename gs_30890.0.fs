/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "p1",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    },
    {
      "NAME" : "p2",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    },
    {
      "NAME" : "p3",
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
    ,
    {
      "NAME" : "p4",
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
    ,
    {
			"NAME": "dist_p_1",
			"TYPE": "float",
			"DEFAULT":0.01,
			"MIN": 0.001,
			"MAX": 0.1
		},
		{
			"NAME": "dist_p_2",
			"TYPE": "float",
			"DEFAULT":0.01,
			"MIN": 0.001,
			"MAX": 0.1
		},
		{
			"NAME": "dist_p_3",
			"TYPE": "float",
			"DEFAULT":0.01,
			"MIN": 0.001,
			"MAX": 0.1
		},
		{
			"NAME": "dist_p_4",
			"TYPE": "float",
			"DEFAULT":0.01,
			"MIN": 0.001,
			"MAX": 0.1
		},
		{
			"NAME": "steps_count",
			"TYPE": "float",
			"DEFAULT":10.0,
			"MIN": 5.0,
			"MAX": 30.0
		},
   
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#30890.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float abs_dist(vec2 a, vec2 b){
	vec2 d = abs(b - a);
	return max(d.x, d.y);
}

void main() {

	vec2 position = gl_FragCoord.xy;
	
	vec2 point0 = p1 * RENDERSIZE;
	vec2 point1 = p2 * RENDERSIZE;
	vec2 point2 = p3 * RENDERSIZE;
	vec2 point3 = p4 * RENDERSIZE;
	
	float dist = 1e20;
	
	dist = min(dist, abs_dist(position, point0));
	dist = min(dist, abs_dist(position, point1));
	dist = min(dist, abs_dist(position, point2));
	dist = min(dist, abs_dist(position, point3));
	
	dist *= dist_p_1;

	
	float f = floor(dist*steps_count)/steps_count;

	gl_FragColor = vec4(f);

}