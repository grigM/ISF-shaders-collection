/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
  {
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 3.5,
			"MIN": 0.0,
			"MAX": 20.0
	},
	{
			"NAME": "size",
			"TYPE": "float",
			"DEFAULT": 8.5,
			"MIN": 0.0,
			"MAX": 10.0
	},
	{
			"NAME": "thick",
			"TYPE": "float",
			"DEFAULT": 0.15,
			"MIN": 0.0,
			"MAX": 2.0
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35137.0"
}
*/



void main(void) {
	vec2 uv = (10.-size) * (2. * gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.y;
	gl_FragColor = (thick * sin(TIME * speed + 6.14159265) / distance(normalize(uv), uv)) *
		       vec4(sin(TIME + 1.0), sin(TIME + speed +0.04), sin(TIME + speed*1.6), 1.);
}