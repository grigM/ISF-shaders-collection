/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Wt23D3 by adelciotto.  A retro plasma cube'\n\nSet the RETRO_MODE variable to 0 to disable pixelated effect.",
  "INPUTS" : [
	{
			"NAME": "pix_speed",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": 0.0,
			"MAX": 4.0
			
		},
		{
			"NAME": "min_pix_amount",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 128.0
			
		},
	{
			"NAME": "max_pix_amount",
			"TYPE": "float",
			"DEFAULT": 32.0,
			"MIN": 16.0,
			"MAX": 128.0
			
		}
  ]
}
*/


// Plasma Cube by adelciotto
// References:
// - https://github.com/ajweeks/RaymarchingWorkshop
// - https://www.iquilezles.org/www/articles/distfunctions/distfunctions.htm 

#define RETRO_MODE 1

const float RETRO_PIXEL_SIZE = 4.0; 
const float PI = 3.1415926535; 
const float MIN_DIST = 0.0; 
const float MAX_DIST = 100.0; 
const float EPSILON = 0.001; 
const float GAMMA_CORRECTION = 0.4545; 
const int MAX_MARCHING_STEPS = 63; 

float t = 0.0; 

mat2 rotate(float angle) {
    float s = sin(angle);
    float c = cos(angle);
    return mat2(c, s, - s, c);
}

float cubeSDF(vec3 p) {
    p.xz *= rotate(t);
    p.yx *= rotate(t * 0.6);
    p.zx *= rotate(t * 0.4);
    p.y += sin(t * 0.2);
    vec3 d = abs(p) - vec3(1.0);
    float dist = length(max(d, 0.0)) + min(max(d.x, max(d.y, d.z)), 0.0);
    return dist - 0.1;
}

vec3 estimateNormal(vec3 p) {
    return normalize(
        vec3(
            cubeSDF(vec3(p.x + EPSILON, p.yz)) - cubeSDF(vec3(p.x - EPSILON, p.yz)),
            cubeSDF(vec3(p.x, p.y + EPSILON, p.z)) - cubeSDF(vec3(p.x, p.y - EPSILON, p.z)),
            cubeSDF(vec3(p.xy, p.z + EPSILON)) - cubeSDF(vec3(p.xy, p.z - EPSILON))
        )
    );
}

// modified plasma effect from https://www.bidouille.org/prog/plasma
vec3 plasma(vec3 p, float scale) {
    p *= scale;
    
    float time = t * 0.3;
    float v1 = sin(p.x + time);
    float v2 = sin(p.y + time);
    float v3 = sin(p.z + time);
    float v4 = sin(p.x + p.y + p.z + time);
    float v5 = sin(length(p) + 1.7 * time);
    float v = v1 + v2 + v3 + v4 + v5;
    
    v *= 2.0;
    vec3 col = vec3(sin(v * PI), sin(v * PI + 2.0 * PI / 3.0), sin(v * PI + 4.0 * PI / 3.0));
    return col * 0.5 + 0.5;
}

void main() {
	vec2 p = gl_FragCoord.xy/RENDERSIZE.y-vec2((RENDERSIZE.x/RENDERSIZE.y)/2.0,0.5);


	
		float ss = min_pix_amount+abs((max_pix_amount*sin(TIME*pix_speed)));

	
    //gl_FragCoord.xy = gl_FragCoord.xy;
    #if RETRO_MODE
    //gl_FragCoord.xy = ceil(gl_FragCoord.xy / RETRO_PIXEL_SIZE) * RETRO_PIXEL_SIZE;
    #endif
    
    vec2 gg = gl_FragCoord.xy;
	gg = ceil(gg / ss) * ss;
	
	
    vec2 uv = vec2(gg.xy - 0.5 * RENDERSIZE.xy);
    uv = 2.0 * uv.xy / RENDERSIZE.y;
    
    t = TIME + 0.33 * sin(uv.x * 1.76 + uv.y + TIME);
    
    vec3 camPos = vec3(0, 0, - 6);
    vec3 at = vec3(0, 0, 0);
    vec3 camForward = normalize(at - camPos);
    vec3 camRight = normalize(cross(vec3(0.0, 1.0, 0.0), camForward));
    vec3 camUp = normalize(cross(camForward, camRight));
    vec3 rayDir = normalize(uv.x * camRight + uv.y * camUp + camForward * 2.0);
    
    float depth = MIN_DIST;
    vec3 col = vec3(0.0);
    for(int i = 0; i < MAX_MARCHING_STEPS; i ++ ) {
        vec3 p = camPos + depth * rayDir;
        float dist = cubeSDF(p);
        if (dist < EPSILON) {
            vec3 light = normalize(vec3(sin(t) * 1.0, cos(t * 0.5) + 0.5, - 0.5));
            vec3 norm = estimateNormal(p);
            vec3 directional = vec3(1.80, 1.27, 0.99) * max(dot(norm, light), 0.0);
            vec3 ambient = vec3(0.02, 0.02, 0.02);
            vec3 diffuse = plasma(p, 1.0) * (directional + ambient);
            col = diffuse;
            break;
        }
        depth += dist;
        if (depth >= MAX_DIST) {
            break;
        }
    }
    
    col = pow(col, vec3(GAMMA_CORRECTION));
    gl_FragColor = vec4(smoothstep(0.0, 1.0, col), 1.0);
 }
