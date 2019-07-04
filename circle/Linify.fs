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
            "NAME": "mm_surf_mode",
            "LABEL": "Mode",
            "TYPE": "long",
            "VALUES": ["Density","Width","Density & Width"],
            "DEFAULT": "Density & Width"
        },
        {
            "NAME": "mm_surf_steps",
            "LABEL": "Steps",
            "TYPE": "int",
            "MIN": 2,
            "MAX": 30,
            "DEFAULT": 5
        },
        {
            "NAME": "mm_surf_density",
            "LABEL": "Density",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 100.0,
            "DEFAULT": 50
        },
        {
            "NAME": "mm_surf_rotation",
            "LABEL": "Rot. Base",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 360.0,
            "DEFAULT": 90.0
        },
        {
            "NAME": "mm_surf_rotation_inc",
            "LABEL": "Rot. Inc",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 360.0,
            "DEFAULT": 360.0
        },
        {
            "NAME": "mm_surf_step_rotation",
            "LABEL": "Step Rot",
            "TYPE": "bool",
            "DEFAULT": "True",
            "FLAGS": "button"
        }
	]
}*/

in vec2 mm_SurfaceCoord;

uniform vec4 modulationColor;
uniform float ignoreAlpha;

out vec4 out_color;

vec3 linify(vec4 inColor, vec2 uv)
{
    float luma = 0.2125 * inColor.r + 0.7154 * inColor.g + 0.0721 * inColor.b;

    float quantizedLuma = int(luma * mm_surf_steps) / float(mm_surf_steps); 

    float rotation;
    if (mm_surf_step_rotation)
        rotation = (mm_surf_rotation+ mm_surf_rotation_inc * quantizedLuma) * 3.141592654 / 180 ;
    else
        rotation = (mm_surf_rotation + mm_surf_rotation_inc * luma) * 3.141592654 / 180;

    vec2 rotatedUv = vec2(uv.x * cos(rotation), uv.y * sin(rotation));

    if (mm_surf_mode == 0)
        // Change density
        return mod(rotatedUv.x - rotatedUv.y,1/(mm_surf_density*(0.3+2*quantizedLuma))) < (0.5/(mm_surf_density*(1+2*quantizedLuma))) ? vec3(1) : vec3(0);
    else if (mm_surf_mode == 1)
        // Change width
        return mod(rotatedUv.x - rotatedUv.y,1/mm_surf_density) < (quantizedLuma*0.5/mm_surf_density) ? vec3(1) : vec3(0);
    else
        // Change width
        return mod(rotatedUv.x - rotatedUv.y,1/(mm_surf_density*(0.3+2*quantizedLuma))) < (quantizedLuma/(mm_surf_density*(0.3+2*quantizedLuma))) ? vec3(1) : vec3(0);
}

void main()
{
    out_color = clamp(MM_SHADER_NORM_PIXEL(isf_FragNormCoord),0,1);
    out_color.a += ignoreAlpha;

    out_color.rgb = linify(out_color, isf_FragNormCoord);

	out_color *= modulationColor;
}
