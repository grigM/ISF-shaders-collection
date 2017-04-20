/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "noise",
    "fbm",
    "fractalbrownianmotion",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Ml3SRf by blackpolygon.  Playing with the examples of Fractal brownian motion from the book of shaders\n\nhttps:\/\/thebookofshaders.com\/13\/",
  "INPUTS" : [

  ]
}
*/


// Author: blackpolygon 
// Title: Turbulence
// Date: December 2016

// Based on the example from @patriciogv for Fractal Brownian Motion
// https://thebookofshaders.com/13/


#define PI 3.14159265359
#define TWO_PI 6.28318530718

float random (in vec2 _st) { 
    return fract(sin(dot(_st.xy, vec2(12.9898,78.233))) * 43758.54531237);
}

// Based on Morgan McGuire @morgan3d
// https://www.shadertoy.com/view/4dS3Wd
float noise (in vec2 _st) {
    vec2 i = floor(_st);
    vec2 f = fract(_st);

    // Four corners in 2D of a tile
    float a = random(i);
    float b = random(i + vec2(1.0, 0.0));
    float c = random(i + vec2(0.0, 1.0));
    float d = random(i + vec2(1.0, 1.0));

    vec2 u = f * f * (3. - 2.0 * f);

    return mix(a, b, u.x) + 
            (c - a)* u.y * (1. - u.x) + 
            (d - b) * u.x * u.y;
}

#define NUM_OCTAVES 5

float fbm ( in vec2 _st) {
    float v = 0.0;
    float a = 0.5;
    vec2 shift = vec2(20.0);
    // Rotate to reduce axial bias
    mat2 rot = mat2(cos(0.5), sin(0.5), 
                    -sin(0.5), cos(0.50));
    for (int i = 0; i < NUM_OCTAVES; ++i) {
        v += a * noise(_st);
        _st = rot * _st * 2.2 + shift;
        a *= 0.5;
    }
    return v;
}

void main(){
    vec2 st = (gl_FragCoord.xy - 0.5*RENDERSIZE.xy )/min(RENDERSIZE.x,RENDERSIZE.y);
    st *= 3.5;
    
    vec3 color = vec3(0.);
    vec2 a = vec2(0.);
    vec2 b = vec2(0.);
    vec2 c = vec2(60.,800.);
    
    a.x = fbm( st);
    a.y = fbm( st + vec2(1.0));
    
    b.x = fbm( st + 4.*a);
    b.y = fbm( st);

    c.x = fbm( st + 7.0*b + vec2(10.7,.2)+ 0.215*TIME );
    c.y = fbm( st + 3.944*b + vec2(.3,12.8)+ 0.16*TIME);

    float f = fbm(st+b+c);

    color = mix(vec3(0.445,0.002,0.419), vec3(1.000,0.467,0.174), clamp((f*f),0.2, 1.0));
    color = mix(color, vec3(0.413,0.524,0.880), clamp(length(c.x),0.480, 0.92));
    
    st = st/3.5;
   
    // Make the hexagon mask
    int N = 6;
    float ata = atan(st.x,st.y)+PI;
    float r = TWO_PI/float(N);
    float dist = cos(floor(.5+ata/r)*r-ata)*length(st);
    
    float hexagonMask = 1.0-smoothstep(.45,.452,dist);
    float bgMask = 1.0 - hexagonMask;
    
    vec3 finalColor = vec3(f*1.9*color);
    vec3 bgColor = vec3(0.950,0.951,0.90);

    gl_FragColor = vec4( bgColor*bgMask + finalColor*hexagonMask, 1.);
}