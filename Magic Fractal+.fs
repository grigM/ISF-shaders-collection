/*{
	"DESCRIPTION": "Your shader description",
	"CREDIT": "by you",
	"CATEGORIES": [
		"Your category"
	],
	"INPUTS": [
	{
			"NAME": "MAGIC",
			"TYPE": "float",
			"DEFAULT": 0.39,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "Scale",
			"TYPE": "float",
			"DEFAULT": 2.45,
			"MIN": 0.0,
			"MAX": 10.0
		},
		{
			"NAME": "Gamma",
			"TYPE": "float",
			"DEFAULT": 11.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "XOffset",
			"TYPE": "float",
			"DEFAULT": 0.04,
			"MIN": -1.0,
			"MAX": 1.0
		},{
			"NAME": "YOffset",
			"TYPE": "float",
			"DEFAULT": 0.04,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "XScrollSpeed",
			"TYPE": "float",
			"DEFAULT": 0.04,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "YScrollSpeed",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		}
	]
}*/

// Based on original shader "Magic Fractal" by dgreenp: https://www.shadertoy.com/view/4ljGDd

vec3 iResolution = vec3(RENDERSIZE, 1.);
float iGlobalTime = TIME;

const int MAGIC_BOX_ITERS = 16;

float magicBox(vec3 p) {
    // The fractal lives in a 1x1x1 box with mirrors on all sides.
    // Take p anywhere in space and calculate the corresponding position
    // inside the box, 0<(x,y,z)<1
    p = 1.0 - abs(1.0 - mod(p, 2.));
    
    float lastLength = length(p);
    float tot = 0.0;
    // This is the fractal.  More iterations gives a more detailed
    // fractal at the expense of more computation.
    for (int i=0; i < MAGIC_BOX_ITERS; i++) {
      // The number subtracted here is a "magic" paremeter that
      // produces rather different fractals for different values.
      p = abs(p)/(lastLength*lastLength) - MAGIC;
      float newLength = length(p);
      tot += abs(newLength-lastLength);
      lastLength = newLength;
    }

    return tot;
}

// A random 3x3 unitary matrix, used to avoid artifacts from slicing the
// volume along the same axes as the fractal's bounding box.
const mat3 M = mat3(0.28862355854826727, 0.6997227302779844, 0.6535170557707412,
                    0.06997493955670424, 0.6653237235314099, -0.7432683571499161,
                    -0.9548821651308448, 0.26025457467376617, 0.14306504491456504);



void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    // uv are screen coordinates, uniformly scaled to go from 0..1 vertically
	vec2 uv = fragCoord.xy / iResolution.yy;
    
    // scroll a certain number of screenfuls/second
    uv.x += +iGlobalTime*XScrollSpeed+XOffset;
    uv.y += +iGlobalTime*YScrollSpeed+YOffset;

    // Rotate uv onto the random axes given by M, and scale
    // it down a bit so we aren't looking at the entire
    // 1x1x1 fractal volume.  Making the coefficient smaller
    // "zooms in", which may reduce large-scale repetition
    // but requires more fractal iterations to get the same
    // level of detail.
    vec3 p = Scale*M*vec3(uv-.5, 0.0);
    
    float result = magicBox(p);
    // Scale to taste.  Also consider non-linear mappings.
    result *= 0.1/Gamma;
    
	fragColor = vec4(vec3(result),1.0);
}

void main(void) {
    mainImage(gl_FragColor, gl_FragCoord.xy);
}