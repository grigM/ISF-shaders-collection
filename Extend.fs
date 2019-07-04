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
            "LABEL": "Extend Factor",
            "NAME": "mm_surf_extend",
            "TYPE": "float",
            "DEFAULT": 1.0,
            "MIN": 1.0,
            "MAX": 5.0
        },
        {
            "LABEL": "Offset X",
            "NAME": "mm_surf_offset_x",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": -2.0,
            "MAX": 2.0
        },
        {
            "LABEL": "Offset Y",
            "NAME": "mm_surf_offset_y",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": -2.0,
            "MAX": 2.0
        }    
    ]
}*/

#ifdef SOFT_EDGE_1_ACTIVE
    in vec2 mm_SurfaceCoord;
    uniform sampler2D softedge1;
#endif
#ifdef SOFT_EDGE_2_ACTIVE
    uniform sampler2D softedge2;
#endif

uniform vec4 modulationColor;
uniform float ignoreAlpha;

out vec4 out_color;

void main()
{
    out_color = MM_SHADER_THIS_NORM_PIXEL();
    out_color.a += ignoreAlpha;
    out_color *= modulationColor;

    #ifdef SOFT_EDGE_1_ACTIVE
        out_color.rgb = out_color.rgb * texture(softedge1, mm_SurfaceCoord).r;
    #endif
    
    #ifdef SOFT_EDGE_2_ACTIVE
        out_color.rgb = out_color.rgb * texture(softedge2, mm_SurfaceCoord).r;
    #endif
}
