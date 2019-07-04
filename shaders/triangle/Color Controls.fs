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
			"NAME": "mm_surf_brightness",
			"LABEL": "Brightness",
			"TYPE": "float",
			"MIN": -1.0,
			"MAX": 1.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "mm_surf_contrast",
			"LABEL": "Contrast",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 4.0,
			"DEFAULT": 1
		},
		{
			"NAME": "mm_surf_saturation",
			"LABEL": "Saturation",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 4.0,
			"DEFAULT": 1.0
		},
		{
			"NAME": "mm_surf_hue_shift",
			"LABEL": "Hue",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1,
			"DEFAULT": 0.0
		},
		{
			"NAME": "mm_surf_invert",
			"LABEL": "Invert",
			"TYPE": "bool",
			"DEFAULT": 0,
			"FLAGS": "button"
		}
	]
}*/

#include "MadCommon.glsl"

uniform vec4 modulationColor;
uniform float ignoreAlpha;

out vec4 out_color;

void main()
{
    vec4 tex_color = MM_SHADER_THIS_NORM_PIXEL();
    tex_color.a += ignoreAlpha;
	
    out_color = tex_color;

	// Apply Hue Shift	        
	if (mm_surf_hue_shift > 0.01) {
		out_color.rgb = hsv2rgb(fract(0.9999999*(rgb2hsv(out_color.rgb)+vec3(mm_surf_hue_shift,0,0))));
	}

	// Apply invert
    if (mm_surf_invert) out_color.rgb=1-out_color.rgb;

	// Apply contrast
	const vec3 LumCoeff = vec3(0.2125, 0.7154, 0.0721);
	const vec3 AvgLumin = vec3(0.5, 0.5, 0.5);
	vec3 intensity = vec3(dot(out_color.rgb, LumCoeff));

	vec3 satColor = mix(intensity, out_color.rgb, mm_surf_saturation);
	out_color.rgb = mix(AvgLumin, satColor, mm_surf_contrast);

	// Apply brightness
	out_color.rgb += mm_surf_brightness;

	out_color *= modulationColor;
}
