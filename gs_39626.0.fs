/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS": [
   	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": -4.0,
		"MAX": 4.0
	},
	{
		"NAME": "frequency",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": -4.0,
		"MAX": 4.0
	}
	,
	{
		"NAME": "thickness",
		"TYPE": "float",
		"DEFAULT": 0.01,
		"MIN": 0,
		"MAX": 1.0
	},
	{
		"NAME": "scaleOfset",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": -10.0,
		"MAX": 10.0
	},
	{
		"NAME": "posOfset",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": -4,
		"MAX": 4.0
	},
	{
      "NAME" : "color_1",
      "TYPE" : "color",
      "DEFAULT" : [
        1.0,
        1.0,
        1.0,
        1
      ],
      "LABEL" : ""
    },
	{
      "NAME" : "color_2",
      "TYPE" : "color",
      "DEFAULT" : [
        0.0,
        0.0,
        0.0,
        1
      ],
      "LABEL" : ""
    },
	{
      "NAME" : "colorPos",
      "TYPE" : "float",
      "DEFAULT": 0.7,
		"MIN": 0.0,
		"MAX": 2.0
    }
],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39626.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable




void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );

	position = position * scaleOfset - posOfset;
	
	float tH = sin(position.y * frequency + (TIME*speed));
	
	if(abs(tH - position.x) < thickness){
		gl_FragColor = color_1;
	}
	if(abs(position.y) < abs(tH - position.x)){
		gl_FragColor = color_1;
		if(distance(abs(position.y), abs(tH-position.x)) >= colorPos){
			gl_FragColor = color_2;
		}
	}
}