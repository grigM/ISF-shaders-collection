/*{
	"DESCRIPTION": "Your shader description",
	"CREDIT": "by you",
	"CATEGORIES": [
		"Your category"
	],
	"INPUTS": [
		{
     		"NAME" : "inputImage",
      		"TYPE" : "image"
    	},
		{
			"LABEL":"RADIUS",
			"NAME": "RADIUS",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 3.0
		},
		{
			"LABEL":"BBC",
			"NAME": "BBC",
			"TYPE": "float",
			"DEFAULT": 5.0,
			"MIN": -1.0,
			"MAX": 5.0
		},
		{
			"LABEL":"BBC2",
			"NAME": "BBC2",
			"TYPE": "float",
			"DEFAULT": 0.35,
			"MIN": -1.0,
			"MAX": 1.0
		}
      	]
}*/

// Based on original shader "testing texture" by mhorga: https://www.shadertoy.com/view/lsySWm

vec3 iResolution = vec3(RENDERSIZE, 1.);
float iGlobalTime = TIME;


void mainImage( out vec4 fragColor, in vec2 fragCoord )
{   
    vec2 uv = (2. * fragCoord.xy - iResolution.xy ) / iResolution.xy.y / RADIUS;
    vec3 norm = vec3( uv, sqrt( BBC2 - dot(uv,uv) ));
    float s = 1.0-fract (atan( norm.z, norm.x ) / 6.283185307179586);
    float t = 1.0-fract (0.5-asin( 1.7*norm.y ) / 3.14159265358979);
    
    fragColor = (dot(uv,uv) > BBC ? vec4( 0.0, 0.0, 0.0, 1.0 ) : 
                 IMG_NORM_PIXEL(inputImage, fract (vec2( s + iGlobalTime * -0.1, t))));
}


void main(void) {
    mainImage(gl_FragColor, gl_FragCoord.xy);
}