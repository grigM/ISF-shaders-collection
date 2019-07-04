/*{
    "CREDIT": "mm team",
    "CATEGORIES": [
        "Image Control"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image",
        }
    ]
}*/

uniform vec4 modulationColor;
uniform float ignoreAlpha;

out vec4 out_color;

void main()
{
    out_color = MM_SHADER_THIS_NORM_PIXEL();
    out_color.a += ignoreAlpha;
	out_color *= modulationColor;
}
