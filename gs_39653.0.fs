/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "moveSpeed",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0,
		"MAX": 10.0
	},
	{
		"NAME": "colorSpeed",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0,
		"MAX": 10.0
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
      "NAME" : "color_1",
      "TYPE" : "color",
      "DEFAULT" : [
        0.01,
        0.01,
        0.2,
        1
      ],
      "LABEL" : ""
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39653.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif




float saturate(float x) { return clamp(x, 0.0, 1.0); }
vec3 saturate(vec3 x) { return clamp(x, 0.0, 1.0); }

float rand(vec2 uv)
{
    return fract(sin(dot(uv, vec2(12.9898, 78.233))) * 43758.5453);
}

vec3 palette(float z)
{
    vec3 a = vec3(0.90, 0.60, 0.69);
    vec3 b = vec3(0.17, 0.41, 0.41);
    return saturate(a + b * sin(z * 9.0 + (TIME*colorSpeed) * 2.0));
}

void main(void)
{
    float scale = RENDERSIZE.y / (zoom + sin((TIME*moveSpeed) * 0.4) * zoomDif);
    vec2 p = (gl_FragCoord.xy - RENDERSIZE / 2.0) / scale;
    vec2 p1 = fract(p) - 0.5;
    vec2 p2 = fract(p - 0.5) - 0.5;

    float z1 = rand(0.19 * floor(p));
    float z2 = rand(0.31 * floor(p - 0.5));

    vec3 c1 = palette(z1)-vec3(color_1);
    vec3 c2 = palette(z2);

    c1 *= saturate((0.25 - abs(0.5 - fract(length(p1) * 10.0 + 0.26))) * scale / 10.0);
    c2 *= saturate((0.25 - abs(0.5 - fract(length(p2) * 10.0 + 0.26))) * scale / 10.0);

    float a1 = saturate((0.5 - length(p1)) * scale);
    float a2 = saturate((0.5 - length(p2)) * scale);

    vec3 c =  mix(
        mix(mix(vec3(0), c1, a1), c2, a2),
        mix(mix(vec3(0), c2, a2), c1, a1),
        step(z1, z2)
    );


	gl_FragColor = vec4(vec3(color_1/0.7)/(c*c), 1);

}

