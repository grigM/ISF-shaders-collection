/*{
	"DESCRIPTION": "Your shader description",
	"CREDIT": "by you",
	"CATEGORIES": [
		"Distortion Effect"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "SPEED",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MAX": 1.0,
			"MIN": 0.0
		},
		{
			"NAME": "SYMMETRY",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MAX": 8.0,
			"MIN": -8.0
		},
		{
			"NAME": "SPIRALICITY",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MAX": 10.0,
			"MIN": -10.0
		}
      	]
}*/

// Based off "SPIRALING VIDEO" by FabriceNeyret2: https://www.shadertoy.com/view/MddSRB#

// SYMMETRY is the number of spiral arms. Use integer values for aligned tiling.
// SPIRALICITY is the steepness of the spiral. Use integers or 1/integer for aligned tiling.

vec3 iResolution = vec3(RENDERSIZE, 1.);

void mainImage( out vec4 O, vec2 U )
{
    vec2 R = iResolution.xy; U = (U+U-R)/R.y; 
    U = vec2(atan(U.y,U.x)*SYMMETRY/2./3.1416,log(length(U))); // conformal polar
    // multiply U for smaller tiles
    U.y += U.x*SPIRALICITY; // comment for concentric circles instead of spiral
    O = IMG_NORM_PIXEL(inputImage, fract(U+TIME*SPEED));
}


void main(void) {
    mainImage(gl_FragColor, gl_FragCoord.xy);
}