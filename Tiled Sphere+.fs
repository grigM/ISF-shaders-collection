/*{
	"DESCRIPTION": "Your shader description",
	"CREDIT": "by you",
	"CATEGORIES": [
		"Your category"
	],
	"INPUTS": [
	{
			"NAME": "FEATURE_SIZE",
			"TYPE": "float",
			"DEFAULT": 8.0,
			"MIN": 0.0,
			"MAX": 10.0
		},
		{
			"NAME": "SPHERE_SPEED",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "SPHERE_RADIUS",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 5.0
		},
		{
			"NAME": "OUTER_RADIUS",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "ZOOM",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 7.0,
			"MAX": 10.0
		},
		{
			"NAME": "XSHIFT",
			"TYPE": "float",
			"DEFAULT": -0.2,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "YSHIFT",
			"TYPE": "float",
			"DEFAULT": -0.18,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "PATTERN",
			"TYPE": "float",
			"DEFAULT": 3.0,
			"MIN": 0.0,
			"MAX": 10.0
		},
		{
			"NAME": "GREEN_COS_MULTIPLIER",
			"TYPE": "float",
			"DEFAULT": 9.0,
			"MIN": 0.0,
			"MAX": 100.0
		}
	]
}*/

// Ported from "Tiled Sphere" by G3Kappa: https://www.shadertoy.com/view/4ld3zX

vec3 iResolution = vec3(RENDERSIZE, 1.);

// The size of the sphere's tiling
//#define FEATURE_SIZE 8.
// The sphere's rotational speed
//#define SPHERE_SPEED 1.0
// The outer circle radius. Small changes!
//#define SPHERE_RADIUS 0.5
// Changing this value affects the green color in various ways.
//#define GREEN_COS_MULTIPLIER 9.

float fcircle(vec2 p) { return 1. - dot(p-=.5,p); }

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	fragCoord = ((fragCoord - vec2(iResolution.x / 2., iResolution.y / 2.)) / iResolution.y);
    
    float outerCircle = OUTER_RADIUS * sin(TIME) + ((SPHERE_RADIUS+.01) * fcircle((fragCoord + vec2(-XSHIFT, -YSHIFT)) * (10.0-ZOOM)));
    float tilePattern = PATTERN * fcircle(fract(fragCoord * 1. / outerCircle * FEATURE_SIZE + TIME / (1. / SPHERE_SPEED)));
    float z = (tilePattern * outerCircle / 6.) * tilePattern * outerCircle;
    
    fragColor = vec4(sin(z * 3.5), pow(z * 8., 8.) * cos(z * GREEN_COS_MULTIPLIER), 1. * z, 1.0);
}

void main(void) {
    mainImage(gl_FragColor, gl_FragCoord.xy);
}