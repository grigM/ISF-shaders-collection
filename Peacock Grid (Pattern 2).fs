/*{
	"DESCRIPTION": "Your shader description",
	"CREDIT": "by you",
	"CATEGORIES": [
		"Your category"
	],
	"INPUTS": [
	{
			"NAME": "SCALE",
			"TYPE": "float",
			"DEFAULT": 50,
			"MIN": 0.0,
			"MAX": 1000.0
		},
		{
			"NAME": "SPEED",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 4.0
		}
	]
}*/

// Based on "Pattern2" by FebriceNeyet2: https://www.shadertoy.com/view/XsKXzG

vec3 iResolution = vec3(RENDERSIZE, 1.);

void mainImage( out vec4 O,  vec2 U )
{
   U = sin(U*SCALE/iResolution.y); O += fract( U.x+U.y + (TIME*SPEED)) -O;
}

void main(void) {
    mainImage(gl_FragColor, gl_FragCoord.xy);
}