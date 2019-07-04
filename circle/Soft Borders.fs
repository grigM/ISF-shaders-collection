/*{
    "CREDIT": "mm team",
    "CATEGORIES": [
        "Image Control"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "Label": "Width",
            "NAME": "mm_surf_borderWidth",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.25
        }
    ]
}*/

in vec2 mm_SurfaceCoord;

uniform vec4 modulationColor;
uniform float ignoreAlpha;

out vec4 out_color;

void main()
{
    out_color = MM_SHADER_THIS_NORM_PIXEL();
    out_color.a += ignoreAlpha;

    // Apply transparency on borders
    float distFromBorder = 0.5 - length(mm_SurfaceCoord - vec2(0.5,0.5));
    float alphaMult = distFromBorder / (mm_surf_borderWidth*mm_surf_borderWidth/2);
    out_color.a *= alphaMult;

    out_color *= modulationColor;
}
