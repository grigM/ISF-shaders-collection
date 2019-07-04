/*{
	"DESCRIPTION": "Your shader description",
	"CREDIT": "by you",
	"CATEGORIES": [
		"Your category"
	],
	"INPUTS": [
	
	{
			"NAME": "Angle",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "MidPoint",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.5
		},
		{
			"NAME": "startColor",
			"TYPE": "color",
			"DEFAULT": [
				1.0,
				0.0,
				0.0,
				1.0
			]
		},
		{
			"NAME": "endColor",
			"TYPE": "color",
			"DEFAULT": [
				0.0,
				1.0,
				1.0,
				1.0
			]
		}
	]
}*/

// Ported from "Linear Gradient with Rotation" by smack0007: https://www.shadertoy.com/view/Mt2XDK

vec3 iResolution = vec3(RENDERSIZE, 1.);
float iGlobalTime = TIME;

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{   
    float Angle = Angle * 360.;
    
    vec2 uv = fragCoord.xy / iResolution.xy;
    
    vec2 origin = vec2(0.5, 0.5);
    
    uv -= origin;
    
    float angle = radians(90.0) - radians(Angle) + atan(uv.y, uv.x);

    float len = length(uv);
    uv = vec2(cos(angle) * len, sin(angle) * len) + origin;
	    
    fragColor = mix(startColor, endColor, smoothstep(0.0, MidPoint*2.0, uv.x));
}

void main(void) {
    mainImage(gl_FragColor, gl_FragCoord.xy);
}