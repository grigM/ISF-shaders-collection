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
		"NAME": "zoom",
		"TYPE": "float",
		"DEFAULT": 50.0,
		"MIN": 10.0,
		"MAX": 100.0
	},
	{
		"NAME": "zoomDif",
		"TYPE": "float",
		"DEFAULT": 10.0,
		"MIN": 5.0,
		"MAX": 20.0
	},
	{
		"NAME": "swirlParam",
		"TYPE": "float",
		"DEFAULT": 10.0,
		"MIN": -15.0,
		"MAX": 15.0
	},
	
	{
		"NAME": "swirlParam2",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0.0,
		"MAX": 3.0
	},
	{
		"NAME": "swirlParam3",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0.0,
		"MAX": 3.0
	},
	
	{
		"NAME": "swirlSpeed",
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39654.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif



float swirl(vec2 coord)
{
    float l = length(coord) / RENDERSIZE.x;
    float phi = atan(coord.y, coord.x + 1e-6);
    return sin(l * swirlParam + phi - (TIME*speed) * swirlSpeed) * swirlParam2 + swirlParam3;
}

float halftone(vec2 coord)
{
    coord -= RENDERSIZE * 0.5;
    float size = RENDERSIZE.x / ((zoom+zoomDif) + sin((TIME*speed) * 0.5) * zoom);
    vec2 uv = coord / size; 
    vec2 ip = floor(uv); // column, row
    vec2 odd = vec2(0.5 * mod(ip.y, 2.0), 0.0); // odd line offset
    vec2 cp = floor(uv - odd) + odd; // dot center
    float d = length(uv - cp - 0.5) * size; // distance
    float r = swirl(cp * size) * (size - 2.0) * 0.5; // dot radius
    return clamp(d - r, 0.0, 1.0);
}

void main(void)
{
    gl_FragColor = vec4(vec3(color_1) * halftone(gl_FragCoord.xy), 1.0);
}