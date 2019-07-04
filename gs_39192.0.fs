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
		"DEFAULT": 27.0,
		"MIN": -30.0,
		"MAX": 30.0
	},
	{
		"NAME": "atanP1",
		"TYPE": "float",
		"DEFAULT": 0.27,
		"MIN": 0.0,
		"MAX": 1.0
	},
	{
		"NAME": "atanP2",
		"TYPE": "float",
		"DEFAULT": 0.27,
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
		"NAME": "atanP4",
		"TYPE": "float",
		"DEFAULT": 4.0,
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39192.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable
//converted by batblaster


float factor = 1.0;

vec4 fn(vec2 uv,float t)
{
    float j = sin(uv.y * 3.14 + t * 5.0);
    float i = sin(uv.x * fnSin1 - uv.y * 2.0 * 3.14 + t * fnSin2);
    float n = -clamp(i, -0.2, 0.0) - 0.0 * clamp(j, -0.2, 0.0);
    
    return fade * (color_1 * n);
}

vec4 main2( vec2 fragCoord, float t )
{
    vec2 p = -1.0 + 2.0 * fragCoord.xy / RENDERSIZE.xy;
    vec2 uv;
    
    float r = sqrt(dot(p, p*p*p));
    float a = atan(
        p.y * (atanP1 + 0.1 * cos(t * atanP3 + p.y)),
        p.x * (atanP2 + 0.1 * sin(t * atanP4 + p.x))
    ) + (TIME*speed);
    
    uv.x = (TIME*speed) + 1.0 / (r + .01);
    uv.y = 0.9 * a / .31416;
    
    return mix(vec4(0.0), fn(uv,t) * r * r * 2.0, factor*2.0); //abs(mouse.x.x*2.0));
}

void main( void )
{
    vec4 col = main2( gl_FragCoord.xy, (TIME*speed) );
    gl_FragColor = col;
	
}