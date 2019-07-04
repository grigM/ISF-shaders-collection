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
            "Label": "Block Size",
            "NAME": "mm_surf_block_size",
            "TYPE": "float",
            "MIN": 1.0,
            "MAX": 200.0,
            "DEFAULT": 20.0
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
    // set the block size on the x and y axes   
    vec2 blockSize = vec2(mm_surf_block_size) / IMG_SIZE(inputImage);
    
    // "pixelate" the uv coordinates, by performing a floor() function.
    // this makes all the pixels in the block sample their color from the same positiokn
    vec2 uvPixels = vec2(0.5) + round((isf_FragNormCoord-vec2(0.5)) / blockSize) * blockSize;
    
    out_color = MM_SHADER_NORM_PIXEL(uvPixels);
    out_color.a += ignoreAlpha;
    out_color *= modulationColor;

    #ifdef SOFT_EDGE_1_ACTIVE
        out_color.rgb = out_color.rgb * texture(softedge1, mm_SurfaceCoord).r;
    #endif
    
    #ifdef SOFT_EDGE_2_ACTIVE
        out_color.rgb = out_color.rgb * texture(softedge2, mm_SurfaceCoord).r;
    #endif
}
