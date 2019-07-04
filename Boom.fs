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
			"DEFAULT": 0.01,
			"MAX": 3.0,
			"MIN": 0.0
		},
		{
			"NAME": "SENSE",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MAX": 2.0,
			"MIN": -2.0
		},
		
	{
			"NAME": "BLEND",
			"TYPE": "float",
			"DEFAULT": 2.1,
			"MAX": 8.0,
			"MIN": 1.0
		}
      	]
}*/

// Based off "SPIRALING VIDEO" by FabriceNeyret2: https://www.shadertoy.com/view/MddSRB#


vec3 iResolution = vec3(RENDERSIZE, 1.);

void mainImage( out vec4 O, vec2 U )
{
    vec2 R = iResolution.xy; U = (U+U-R)/R.y; 
    U = vec2(atan(U.y,U.x)*SENSE/3./BLEND,log(length(U))); // conformal polar
    // multiply U for smaller tiles
    U.y += U.x*SENSE; 
    O = IMG_NORM_PIXEL(inputImage, fract(U+TIME*SPEED));
}
void main(void) {
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
