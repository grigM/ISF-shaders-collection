/*{
	"DESCRIPTION": "Your shader description",
	"CREDIT": "by you",
	"CATEGORIES": [
		"Your category"
	],
	"INPUTS": [
	
	{
			"LABEL": "TWISTS",
			"NAME": "TWISTS",
			"TYPE": "float",
			"DEFAULT": 5.0,
			"MIN": -50.0,
			"MAX": 50.0
		},
		{
			"LABEL": "SEGMENTS",
			"NAME": "SEGMENTS",
			"TYPE": "float",
			"DEFAULT": -5.0,
			"MIN": -50.0,
			"MAX": 50.0
		},
		{
			"LABEL": "SKEW",
			"NAME": "SKEW",
			"TYPE": "float",
			"DEFAULT": 5.0,
			"MIN": -50.0,
			"MAX": 50.0
		},
		{
			"LABEL": "PITCH",
			"NAME": "PITCH",
			"TYPE": "float",
			"DEFAULT": 5.0,
			"MIN": -50.0,
			"MAX": 50.0
		},
		{
			"LABEL": "SHARPNESS",
			"NAME": "SHARPNESS",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"LABEL": "SHARPNESSSCALE",
			"NAME": "SHARPNESSSCALE",
			"TYPE": "float",
			"DEFAULT": 1000.0,
			"MIN": 0.0,
			"MAX": 1000.0
		}
	]
}*/

// Ported from "B/W Sunflower Zoom" by roywig: https://www.shadertoy.com/view/4lc3Wn

vec3 iResolution = vec3(RENDERSIZE, 1.);
float iGlobalTime = TIME;

void mainImage( out vec4 fragColor, in vec2 z )
{
    z = z/iResolution.xy-.5;
    z.x *= iResolution.x/iResolution.y;
    z = vec2(log(length(z)),atan(z.y,z.x)); //complex logarithm
    z.x -= (iGlobalTime);
    z *= mat2(int(TWISTS),int(SEGMENTS),int(SKEW),int(PITCH)); //mat2(0.707,-0.707,0.707,0.707)*2./0.283;
    // Drawing time.
    fragColor = vec4(SHARPNESS*SHARPNESSSCALE*sin(z.x)*sin(z.y));

}

void main(void) {
    mainImage(gl_FragColor, gl_FragCoord.xy);
}