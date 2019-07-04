/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3ll3zs by flyingrub.  Tribute to vasarely 3",
  "INPUTS" : [

  ]
}
*/


#define TAU 6.28318530718

const float grid = 9.;
#define pixel_width 2./RENDERSIZE.y*grid
#define slowt TIME/5.

float easeInOut(float t) {
    if ((t *= 2.0) < 1.0) {
        return 0.5 * t * t;
    } else {
        return -0.5 * ((t - 1.0) * (t - 3.0) - 1.0);
    }
}

float linearstep(float begin, float end, float t) {
    return clamp((t - begin) / (end - begin), 0.0, 1.0);
}

vec2 rotate(vec2 _uv, float _angle){
    _uv =  mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle)) * _uv;
    return _uv;
}

float random (vec2 st) {
    return fract(sin(dot(st.xy,vec2(12.9898,78.233)))*43758.5453123);
}

void main() {



    vec2 uv = (gl_FragCoord.xy-RENDERSIZE.xy*.5)/RENDERSIZE.y;
    uv *= grid;
    uv.y += mod(grid,2.)* .5;
    vec2 id = floor(uv);
    vec2 gv = fract(uv)*2.-1.;
    
    float a = floor(random(id*floor(slowt))*4.)/4.;
    float next_a = floor(random(id*(floor(slowt)+1.))*4.)/4.;
    float angle = mix(a,next_a,easeInOut(linearstep(.5,1.,fract(slowt))));
    gv = rotate(gv,angle*TAU);
    
    float col = smoothstep(pixel_width,0.,gv.x);
    
    // Output to screen
    gl_FragColor = vec4(vec3(col),1.0);
}
