/*{
	"CREDIT": "mm team",
	"CATEGORIES": [
		"Image Control"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image",
		},
		{
			"NAME": "mm_surf_filtertype",
			"LABEL": "Filter Type",
			"TYPE": "long",
			"VALUES": ["Pass", "Reject"],
			"DEFAULT": "Reject"
		},
		{
			"NAME": "mm_surf_center",
			"LABEL": "Center",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.5
		},
		{
			"NAME": "mm_surf_width",
			"LABEL": "Width",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.1
		},
		{
			"NAME": "mm_surf_smoothness",
			"LABEL": "Smoothness",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.2
		}
	]
}*/

#include "MadCommon.glsl"

uniform vec4 modulationColor;
uniform float ignoreAlpha;

out vec4 out_color;

void main()
{
	out_color = MM_SHADER_THIS_NORM_PIXEL();
	out_color.a += ignoreAlpha;

    //vec3 hsv=rgb2hsv(out_color.rgb);
    float hue=rgb2hue(out_color.rgb);

   	float distFromCenter=abs(hue-mm_surf_center);
   	out_color.a=distFromCenter/mm_surf_width;
   	if (out_color.a < 1) {
   		// We're rejecting this pixel
   		// Check smoothness
	   	if (out_color.a > 1-mm_surf_smoothness) {
	   		out_color.a -= 1-mm_surf_smoothness;
	   		out_color.a *= 1/mm_surf_smoothness;
	   	} else {
	   		out_color.a=0;
	   	}
   	}

    if (mm_surf_filtertype==0) {
    	out_color.a=1-out_color.a;
	}

	out_color *= modulationColor;
}
