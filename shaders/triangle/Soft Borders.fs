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
            "LABEL": "Width",
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

float distToLine(vec2 pt1, vec2 pt2, vec2 testPt)
{
  vec2 lineDir = pt2 - pt1;
  vec2 perpDir = vec2(lineDir.y, -lineDir.x);
  vec2 dirToPt1 = pt1 - testPt;
  return abs(dot(normalize(perpDir), dirToPt1));
}

void main()
{
    out_color = MM_SHADER_THIS_NORM_PIXEL();
    out_color.a += ignoreAlpha;

    // Apply transparency on borders
    float distFromBorder = min(distToLine(vec2(0,0),vec2(0.5,1),mm_SurfaceCoord),min(distToLine(vec2(0.5,1),vec2(1,0),mm_SurfaceCoord),distToLine(vec2(1,0),vec2(0,0),mm_SurfaceCoord)));
    float alphaMult = distFromBorder / (mm_surf_borderWidth*mm_surf_borderWidth/2);
    out_color.a *= alphaMult;

    out_color *= modulationColor;
}
