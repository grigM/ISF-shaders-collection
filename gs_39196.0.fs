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
		"MAX": 1.0
	},
    {
      "NAME" : "color_1",
      "TYPE" : "color",
      "DEFAULT" : [
        0.0,
        0.0,
        0.0,
        1
      ],
      "LABEL" : ""
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39196.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

//#extension GL_OES_standard_derivatives : enable
//converted by batblaster


float factor = 1.0;
//vec3 color = vec3(0.0, 0.0, 0.0);

vec4 fn(vec2 uv,float t)
{
    float j = sin(uv.y * 3.14 + t * 5.0);
    float i = sin(uv.x * fnSin1 - uv.y * 2.0 * 3.14 + t * fnSin2);
    float n = -clamp(i, -0.2, 0.0) - 0.0 * clamp(j, -0.2, 0.0);
    
    return 3.5 * (color_1 * n);
}

vec4 main2( vec2 fragCoord, float t )
{
    vec2 p = -1.0 + 2.0 * fragCoord.xy / RENDERSIZE.xy;
    vec2 uv;

    vec3 p2 = vec3(p, 1.0), p3;
    float alphaa = 3.14*sin((TIME*speed)*0.4)+(TIME*speed)*0.2, alphab = 3.14*cos((TIME*speed)*0.3)+(TIME*speed)*0.2;
    float cosx = cos(alphaa);
    float sinx = sin(alphaa);
    float cosx2 = cos(alphab);
    float sinx2 = sin(alphab);
	
    p3.x = cosx * p2.x + sinx * p2.z;
    p3.z = sinx * p2.x - cosx * p2.z;
    p3.y = p2.y;
    p2.x = p3.x;
    p2.z = p3.z;

    p3.y = cosx2 * p2.y + sinx2 * p2.z;
    p3.z = sinx2 * p2.y - cosx2 * p2.z;
    p3.x = p2.x;
	
    p2.y = p3.y;
    p2.z = p3.z;

    vec2 pp = vec2(p2.x,p2.y);
	
    float r = sqrt(dot(pp, pp*pp*pp));
    float a = atan(
        pp.y * (atanP1 + 0.1 * cos(t * 2.0 + pp.y)),
        pp.x * (atanP2 + 0.1 * sin(t * 4.0 + pp.x))
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