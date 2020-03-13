/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/3stGDl by cerebral_m.  quick quality reference implementation of trilinear filtering just cause",
    "IMPORTED": {
        "iChannel0": {
            "NAME": "iChannel0",
            "PATH": "8de3a3924cb95bd0e95a443fff0326c869f9d4979cd1d5b6e94e2a01f5be53e9.jpg"
        }
    },
    "INPUTS": [
        {
            "NAME": "iMouse",
            "TYPE": "point2D"
        }
    ]
}

*/










//click and drag to change gradient angle















#define SCALE 10.0


void main() {



    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy*2.0 - vec2(1.0,1.0);
    vec2 mouse = iMouse.xy/RENDERSIZE.xy*2.0 - vec2(1.0,1.0);
    
    float m = dot(uv, mouse) *4.0;
    float n = floor(m);
    float o = m-n;
    
    
    
    
    
    
    uv.x += TIME/2.0;
    uv.y += TIME/2.0;
    
    float scale = SCALE * pow(2.0, n);
    
    vec2 bl = vec2(floor(uv.x*scale), floor(uv.y*scale));
    vec2 br = vec2(     bl.x+1.0     ,      bl.y        )/scale;
    vec2 tl = vec2(     bl.x         ,      bl.y+1.0    )/scale;
    vec2 tr = vec2(     bl.x+1.0     ,      bl.y+1.0    )/scale; 
    bl /= scale;
    
    
        
    float distx = (uv.x - bl.x)*scale;
    float disty = (uv.y - bl.y)*scale;
    
    
    
    vec3 col = vec3(.0,.0,.0);
    col += IMG_NORM_PIXEL(iChannel0,mod(bl,1.0)).xyz * (1.0-distx) * (1.0-disty);
    col += IMG_NORM_PIXEL(iChannel0,mod(br,1.0)).xyz * (    distx) * (1.0-disty);
    col += IMG_NORM_PIXEL(iChannel0,mod(tl,1.0)).xyz * (1.0-distx) * (    disty);
    col += IMG_NORM_PIXEL(iChannel0,mod(tr,1.0)).xyz * (    distx) * (    disty);
    
    col *= 1.0-o;
    
    
    
    
    
    
    scale *= 2.0;
    
    
    bl = vec2(floor(uv.x*scale), floor(uv.y*scale));
    br = vec2(     bl.x+1.0     ,      bl.y        )/scale;
    tl = vec2(     bl.x         ,      bl.y+1.0    )/scale;
    tr = vec2(     bl.x+1.0     ,      bl.y+1.0    )/scale; 
    bl /= scale;
    
    
        
    distx = (uv.x - bl.x)*scale;
    disty = (uv.y - bl.y)*scale;
    
    
    col += IMG_NORM_PIXEL(iChannel0,mod(bl,1.0)).xyz * (1.0-distx) * (1.0-disty) * o;
    col += IMG_NORM_PIXEL(iChannel0,mod(br,1.0)).xyz * (    distx) * (1.0-disty) * o;
    col += IMG_NORM_PIXEL(iChannel0,mod(tl,1.0)).xyz * (1.0-distx) * (    disty) * o;
    col += IMG_NORM_PIXEL(iChannel0,mod(tr,1.0)).xyz * (    distx) * (    disty) * o;
    
    
    // Output to screen
    gl_FragColor = vec4(col,1.0);
}
