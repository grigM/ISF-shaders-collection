/*{
	"DESCRIPTION": "Your shader description",
	"CREDIT": "by you",
	"CATEGORIES": [
		"Your category"
	],
	"INPUTS": [
	
	{
			"NAME": "Color1",
			"TYPE": "color",
			"DEFAULT": [
				1.0,
				0.0,
				0.0,
				1.0
			]
		},
		{
			"NAME": "Color1_Radius",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 1.0
		},
		{
			"NAME": "Color1_Y",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "Color2",
			"TYPE": "color",
			"DEFAULT": [
				0.0,
				1.0,
				0.0,
				1.0
			]
		},
		{
			"NAME": "Color2_Radius",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 1.0
		},
		{
			"NAME": "Color2_Y",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.5
		},
		{
			"NAME": "Color3",
			"TYPE": "color",
			"DEFAULT": [
				0.0,
				0.0,
				1.0,
				1.0
			]
		},
		{
			"NAME": "Color3_Radius",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 1.0
		},
		{
			"NAME": "Color3_Y",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 1.0
		},
		{
			"NAME": "CanvasColor",
			"TYPE": "color",
			"DEFAULT": [
				0.0,
				0.0,
				0.0,
				1.0
			]
		},
		{
			"NAME": "VERTICAL",
			"TYPE": "bool",
			"DEFAULT": 0.0
		}
	
	]
}*/

// Adapted from "RGB Soft Lights" by BitOfGold: https://www.shadertoy.com/view/llVGz1
// I've been playing a lot of "No Man's Sky" and thought this mod would help me recreate the vibe. :-)

vec3 iResolution = vec3(RENDERSIZE, 1.);

vec3 softLight(vec3 canvas, vec2 uv, vec2 center, float r, vec3 color) {
    float d = clamp(1.0-length(center-uv)/r,0.0,1.0);
    return(canvas + d*color);
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	vec2 uv = fragCoord.xy / RENDERSIZE.xy;
	
	if (VERTICAL) {uv.y=uv.x;}
	
	float ASPECT = (RENDERSIZE.x/RENDERSIZE.y);
	uv.x = uv.y;
	
	vec3 canvas = vec3(CanvasColor.rgb);
    canvas = softLight(canvas, uv, vec2(.5, Color1_Y), Color1_Radius, vec3(Color1.rgb));
    canvas = softLight(canvas, uv, vec2(.5, Color2_Y), Color2_Radius, vec3(Color2.rgb));
    canvas = softLight(canvas, uv, vec2(.5, Color3_Y), Color3_Radius, vec3(Color3.rgb));
	fragColor = vec4(canvas,1.0);
}


void main(void) {
    mainImage(gl_FragColor, gl_FragCoord.xy);
}