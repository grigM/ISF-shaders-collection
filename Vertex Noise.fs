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
            "NAME": "mm_surf_power",
            "Label": "Noise Power",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0
        }, 
        {
            "NAME": "mm_surf_speed",
            "Label": "Noise Speed",
            "TYPE": "float",
            "DEFAULT": 1.0,
            "MIN": 0.0,
            "MAX": 10.0
        }     
    ],
    "GENERATORS": [
        { "NAME": "mm_surf_noise_time", "TYPE": "time_base", "PARAMS": {"speed": "mm_surf_speed", "reverse": false, "strob": 0, "speed_curve":4, "link_speed_to_global_bpm":true} },
    ],
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
