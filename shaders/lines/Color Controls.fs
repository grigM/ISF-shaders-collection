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

in vec2 mm_SurfaceCoord;

uniform bool mmTexturedLine;

uniform vec4 modulationColor;
uniform float ignoreAlpha;

uniform mat4 textureMatrix;
uniform float mmLineWidth;
uniform float mmLineLength;
uniform float mmBlur;
uniform bool mmRounded;
uniform float mmStartStroke;
uniform float mmEndStroke;

out vec4 out_Color;

void main()
{
    vec2 uv=mm_SurfaceCoord;

    float stroke=mmStartStroke*(1.-uv.x)+mmEndStroke*uv.x;

    uv.y+=0.5;
    float vDist=abs(1.-uv.y*2.);
    uv.x*=mmLineLength/mmLineWidth;
    float alpha=1.;
    // Create rounded borders
    if (mmRounded) {
	    if (uv.x<0.5) {
	        float hDist=2.*(0.5-uv.x);
	        alpha*=1.-sqrt(vDist*vDist+hDist*hDist);
	    }
	    if (uv.x>(mmLineLength/mmLineWidth)-0.5) {
	        float hDist=2.*(0.5-((mmLineLength/mmLineWidth)-uv.x));
	        alpha=min(alpha,1.-sqrt(vDist*vDist+hDist*hDist));
	    }
	}
	// If this pixel is not in rounded border, process vertical blur
    if (alpha==1.) {
	    alpha-=vDist;
	}
    if (alpha<mmBlur) 
    	alpha=alpha*(1./mmBlur);
    else 
    	alpha=1.;

    // Process start/end stroke
    alpha*=stroke;

    vec4 texColor;
    if (mmTexturedLine) {
        texColor = MM_SHADER_THIS_NORM_PIXEL();
	    texColor.a += ignoreAlpha;
	} else {
		texColor = vec4(1.,1.,1.,1.);
	}
    texColor.a*=alpha;

    // Apply Hue Shift
	if (mm_surf_hue_shift > 0.01) {
		texColor.rgb = hsv2rgb(fract(0.9999999*(rgb2hsv(texColor.rgb)+vec3(mm_surf_hue_shift,0,0))));
	}

	// Apply invert    
    if (mm_surf_invert) texColor.rgb=1-texColor.rgb;

	// Apply contrast
	const vec3 LumCoeff = vec3(0.2125, 0.7154, 0.0721);
	vec3 AvgLumin = vec3(0.5, 0.5, 0.5);
	vec3 intensity = vec3(dot(texColor.rgb, LumCoeff));

	vec3 satColor = mix(intensity, texColor.rgb, mm_surf_saturation);
	texColor.rgb = mix(AvgLumin, satColor, mm_surf_contrast);

	// Apply brightness
	texColor.rgb += mm_surf_brightness;

    out_Color = texColor * modulationColor;
}
