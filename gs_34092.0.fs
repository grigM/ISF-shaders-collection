/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
  		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 3.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "rowC",
			"TYPE": "float",
			"DEFAULT": 40.0,
			"MIN": 4.0,
			"MAX": 50.0
		},
		{
			"NAME": "function",
			"TYPE": "long",
			"VALUES": [
				0,
				1,
				2,
				3,
				4,
				5,
				6
			],
			"LABELS": [
				"acos",
				"abs",
				"tan",
				"cos",
				"sin",
				"floor",
				"fract",
			],
			"DEFAULT": 8
		},
		{
			"NAME": "colomn",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": 2.0,
			"MAX": 50.0
		},
		{
			"NAME": "osc",
			"TYPE": "float",
			"DEFAULT": 3.14159,
			"MIN": 0.0,
			"MAX": 9.424
		},
		{
			"NAME": "sinV",
			"TYPE": "float",
			"DEFAULT": 0.025,
			"MIN": 0.0,
			"MAX": 0.5
		},
		{
			"NAME": "colorShift",
			"TYPE": "float",
			"DEFAULT": 4,
			"MIN": 1,
			"MAX": 10
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34092.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	vec2 pos = (gl_FragCoord.xy / RENDERSIZE - .5) * vec2(2, rowC);
	
	float row = pos.y;
	
	float xshift =  /*2.0*(sin(TIME));*/(sin(TIME+row*sinV) + sin(TIME*speed + row*sinV));
	
	float fnc;
	
	if (function == 0) fnc = acos(pos.x);
	
	else if (function == 1) fnc = abs(pos.x);
	else if (function == 2) fnc = tan(pos.x);
	else if (function == 3) fnc = cos(pos.x);
	else if (function == 4) fnc = sin(pos.x);
	else if (function == 5) fnc = floor(pos.x);
	else if (function == 6) fnc = fract(pos.x);
	
	
	float angle = xshift + fnc /osc * colomn;

	if (fract(pos.y) < .13 || fract(angle) < 0.3) {
		gl_FragColor = vec4(0);
		return;
	}

	float color = colorShift*sin(floor(angle));
	
	
	

	gl_FragColor = vec4(cos(color),cos(color+2.),cos(color+10.),1.)*.5+.75;

}