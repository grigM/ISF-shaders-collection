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
            "NAME": "mm_surf_steps",
            "LABEL": "Steps",
            "TYPE": "int",
            "MIN": 0.0,
            "MAX": 255.0,
            "DEFAULT": 5
        },
        {
            "NAME": "mm_surf_animated",
            "LABEL": "Animated",
            "TYPE": "bool",
            "DEFAULT": "True"
        }
	]
}*/

in vec2 mm_SurfaceCoord;

uniform vec4 modulationColor;
uniform float ignoreAlpha;

out vec4 out_color;

highp float mm_surf_rand(vec2 co)
{
    highp float a = 12.9898;
    highp float b = 78.233;
    highp float c = 43758.5453;
    highp float dt= dot(co.xy ,vec2(a,b));
    highp float sn= mod(dt,3.14);
    return fract(sin(sn) * c);
}

vec3 dither(vec4 inColor, vec2 uv)
{
    float luma = 0.2125 * out_color.r + 0.7154 * out_color.g + 0.0721 * out_color.b;

    float quantizedLuma = int(luma * mm_surf_steps) / float(mm_surf_steps); 

    // Points
    vec2 randIn = isf_FragNormCoord;
    if (mm_surf_animated) randIn.x += fract(TIME);
    return mm_surf_rand(randIn) < (quantizedLuma) ? vec3(1) : vec3(0);
}

void main()
{
    out_color = MM_SHADER_NORM_PIXEL(isf_FragNormCoord);
    out_color.a += ignoreAlpha;
    out_color.rgb = dither(out_color,isf_FragNormCoord);
	out_color *= modulationColor;
}
