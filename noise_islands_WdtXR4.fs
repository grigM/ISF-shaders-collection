/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WdtXR4 by onak.  attempt to create island-shaped heightmaps from noise.\nnothing fancy here this is a testbed for another project",
  "INPUTS" : [

  ]
}
*/


float pixelsize = 1.0;
float PI = 3.14159265359;

vec2 hash22(vec2 p) {
    p = p*mat2(127.1,311.7,269.5,183.3);
	p = -1.0 + 2.0 * fract(sin(p)*43758.5453123);
	return sin(p*6.283 + TIME);
}

float perlin_noise(vec2 p) {
	vec2 pi = floor(p);
    vec2 pf = p - pi;
    
    // interpolation
    vec2 w = pf * pf * (3.0 - 2.0 * pf);
    
    float f00 = dot(hash22(pi + vec2(0.0, 0.0)), pf - vec2(0.0, 0.0));
    float f01 = dot(hash22(pi + vec2(0.0, 1.0)), pf - vec2(0.0, 1.0));
    float f10 = dot(hash22(pi + vec2(1.0, 0.0)), pf - vec2(1.0, 0.0));
    float f11 = dot(hash22(pi + vec2(1.0, 1.0)), pf - vec2(1.0, 1.0));
    
    // mixing top & bottom edges
    float xm1 = mix(f00, f10, w.x);
    float xm2 = mix(f01, f11, w.x);
    
    // mixing to point
    float ym = mix(xm1, xm2, w.y); 
    
    return ym;
}

float fBM2(float size, int octaves, float persistence, float scale, vec2 coord) {
    float c = 0.0;
    float p = 1.0;
    float n = 0.0;
    
    for (int i = 1; i <= octaves; i++) {
        c += perlin_noise(coord * size) * p;
        n    += p;
        size *= scale;
        p    *= persistence;
    }
    // normalize c
    c /= n;
    
    return c;
}

void main() {

    // normalized pixel coordinates (from 0 to 1)
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    if (pixelsize > 1.0) {
   		uv = (pixelsize * floor(RENDERSIZE.xy * uv / pixelsize)) / RENDERSIZE.xy;
    }
    
   	// noise
	float n = fBM2(3.0, 4, 0.5, 2.0, uv);
        
    // create a half sphere shape
    float col = 0.7 - 2.0 * distance(uv, vec2(0.5));
    
    gl_FragColor = vec4(vec3(n + col), 1.0);
}
