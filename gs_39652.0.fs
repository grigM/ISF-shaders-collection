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
		"MIN": 0,
		"MAX": 8.0
	},
	{
		"NAME": "rotate",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": -1,
		"MAX": 1
	},
	{
		"NAME": "zoomSpeed",
		"TYPE": "float",
		"DEFAULT": 0.4,
		"MIN": 0,
		"MAX": 5.0
	},
	{
		"NAME": "zoom",
		"TYPE": "float",
		"DEFAULT": 6.0,
		"MIN": 0.0,
		"MAX": 10.0
	},
	{
		"NAME": "zoomDif",
		"TYPE": "float",
		"DEFAULT": 3.0,
		"MIN": 0.0,
		"MAX": 20.0
	},
	{
		"NAME": "cirParam1",
		"TYPE": "float",
		"DEFAULT": 0.2,
		"MIN": -0.2,
		"MAX": 0.3
	},
	{
		"NAME": "cirParam2",
		"TYPE": "float",
		"DEFAULT": 0.2,
		"MIN": -0.2,
		"MAX": 1.8
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39652.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265359


float saturate(float x) { return clamp(x, 0.0, 1.0); }

float rand(vec2 uv)
{
    return fract(sin(dot(uv, vec2(12.9898, 78.233))) * 43758.5453);
}

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}


void main(void)
{
	float scale = RENDERSIZE.y / (zoom + sin(TIME * zoomSpeed) * zoomDif);
    vec2 p = (gl_FragCoord.xy - RENDERSIZE / 2.0) / scale;
    
    
    //float scale = RENDERSIZE.y / zoomScale;
    //vec2 p = gl_FragCoord.xy / scale;
    
    p = rotate2d(rotate*PI ) * p;
    
    vec2 p1 = fract(p) - 0.5;
    vec2 p2 = fract(p - 0.5) - 0.5;
	
	
	
    float z1 = rand(0.12 * floor(p));
    float z2 = rand(0.23 * floor(p - 0.5));

    float r1 = 0.2 + cirParam2 * sin((TIME*speed) * 1.9 + z1 * 30.0);
    float r2 = cirParam1 + 0.2 * sin((TIME*speed) * 1.9 + z2 * 30.0);

    float c1 = saturate((r1 - length(p1)) * scale);
    float c2 = saturate((r2 - length(p2)) * scale);

    float a1 = saturate((r1 + 0.08 - length(p1)) * scale);
    float a2 = saturate((r2 + 0.08 - length(p2)) * scale);

    float c = mix(
        mix(mix(0.0, c1, a1), c2, a2),
        mix(mix(0.0, c2, a2), c1, a1),
        step(z1, z2)
    );



	gl_FragColor = vec4(c, c, c, 1);

}

