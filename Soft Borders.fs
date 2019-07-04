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
            "Label": "Thickness X",
            "NAME": "mm_surf_borderWidthX",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.25
        },
        {
            "Label": "Thickness Y",
            "NAME": "mm_surf_borderWidthY",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.25
        },
        {
            "Label": "Shape",
            "NAME": "mm_surf_borderShape",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 1
        }
    ]
}*/

in vec2 mm_SurfaceCoord;

#ifdef SOFT_EDGE_1_ACTIVE
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

    // Apply transparency on borders
    vec2 distFromBorder = min(mm_SurfaceCoord,vec2(1)-mm_SurfaceCoord);
    float distX=smoothstep(0,mm_surf_borderWidthX*mm_surf_borderWidthX,distFromBorder.x);
    float distY=smoothstep(0,mm_surf_borderWidthY*mm_surf_borderWidthY,distFromBorder.y);
    float dist=mix(min(distX,distY),sqrt(distX*distY),mm_surf_borderShape);
    float alphaMult=dist;
    out_color.a *= alphaMult;

    #ifdef SOFT_EDGE_1_ACTIVE
        out_color.rgb = out_color.rgb * texture(softedge1, mm_SurfaceCoord).r;
    #endif
    
    #ifdef SOFT_EDGE_2_ACTIVE
        out_color.rgb = out_color.rgb * texture(softedge2, mm_SurfaceCoord).r;
    #endif
}
