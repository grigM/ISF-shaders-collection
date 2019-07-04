/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": -4.0,
		"MAX": 4.0
	},
	{
		"NAME": "fnSin1",
		"TYPE": "float",
		"DEFAULT": 15.0,
		"MIN": -30.0,
		"MAX": 30.0
	},
	{
		"NAME": "fnSin2",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": -30.0,
		"MAX": 30.0
	},
	{
		"NAME": "atanP1",
		"TYPE": "float",
		"DEFAULT": 0.3,
		"MIN": 0.0,
		"MAX": 1.0
	},
	{
		"NAME": "atanP2",
		"TYPE": "float",
		"DEFAULT": 0.3,
		"MIN": 0.0,
		"MAX": 2.0
	},
	{
		"NAME": "fade",
		"TYPE": "float",
		"DEFAULT": 3.5,
		"MIN": 0.0,
		"MAX": 10.0
	},
	{
		"NAME": "atanP3",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": -10.0,
		"MAX": 10.0
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
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#38956.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable
//converted by batblaster



float factor = 1.0;

vec4 t(vec2 uv)
{
    float j = sin(uv.y * 3.14 + (TIME*speed) * 5.0);
    float i = sin(uv.x * fnSin1 - uv.y * 2.0 * 3.14 + (TIME*speed) * fnSin2);
    float n = -clamp(i, -0.2, 0.0) - 0.0 * clamp(j, -0.2, 0.0);
    
    return fade * (color_1 * n);
}

void main( void )
{
    float aspectRatio = RENDERSIZE.x / RENDERSIZE.y;
    vec2 p = -1.0 + 2.0 * gl_FragCoord.xy / RENDERSIZE.xy;
    p.x *= aspectRatio;
    vec2 uv;
    
    float r = sqrt(dot(p, p));
    float a = atan(
        p.y * (atanP1 + 0.1 * cos((TIME*speed) * atanP1 + p.y)),
        p.x * (atanP2 + 0.1 * sin((TIME*speed) + p.x))
    ) + (TIME*speed);
    
    uv.x = (TIME*speed) + 1.0 / (r + .01);
    uv.y = 4.0 * a / 3.1416;
    
    gl_FragColor = mix(vec4(0.0), t(uv) * r * r * 2.0, factor);
}