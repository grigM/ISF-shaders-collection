/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "0a40562379b63dfb89227e6d172f39fdce9022cba76623f1054a2c83d6c0ba5d.png"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ldSfz3 by eddietree.  fire",
  "INPUTS" : [

  ]
}
*/


/*float random(in vec2 st)
{
    vec4 noiseVal = IMG_NORM_PIXEL(iChannel0,mod(st * 700485.233,1.0));
    return noiseVal.x*2.0-1.0;
}
*/

float random (in vec2 st) { 
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233)))* 
        43758.5453123) * 2.0 - 1.0;
}


float noise(in vec2 st)
{
    vec2 i = floor(st);
    vec2 f = fract(st);
    
    float uvDelta = 1.0;// / 64.0;
    
    float a = random(i);
    float b = random(i + vec2(uvDelta,0.0));
    float c = random(i + vec2(0.0,uvDelta));
    float d = random(i + vec2(uvDelta,uvDelta));
    
    vec2 u = smoothstep(vec2(0.0), vec2(1.0), f);
    
   return mix(a, b, u.x) + 
            (c - a)* u.y * (1.0 - u.x) + 
            (d - b) * u.x * u.y;
}

// from bookofshaders
#define OCTAVES 7
float fbm(in vec2 st)
{
    float value = 0.0;
    float amp = 0.6;
    float freq = 0.0;
    
    for(int i = 0; i < OCTAVES; ++i)
    {
        value += amp * abs(noise(st));
        st *= 2.0;
        amp *= 0.5;
    }
    
    return value;
}

float pattern(in vec2 st)
{
    // f(p) = fbm(p + fmb(p))
    
    vec2 q = vec2(fbm(st + vec2(TIME*0.3, -TIME*0.4333)), fbm(st+vec2(70442.3, 1042.9423+TIME*-0.1)));
    return fbm(st+q) * 1.5;
}


void main() {



	vec2 uvOrigin = gl_FragCoord.xy / RENDERSIZE.xy;
    vec2 uv = uvOrigin;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    float patVal = pattern(uv*vec2(2.0,1.0) + vec2(0.0, TIME*-0.5));
    
    patVal *= smoothstep(0.2, 0.4, 1.2-uvOrigin.y - fbm(uv*1.0 + vec2(0.0,-TIME)));
    patVal *= 1.0-smoothstep(0.1, 0.2, abs(uvOrigin.x-0.5) +fbm(uv*0.4 + vec2(0.0,-TIME*0.4))*0.1);
    
    vec3 color = vec3(0.0);
    
    color.xyz = mix( vec3(0.0,0.0,0.0), vec3(1.0,0.0,0.0), smoothstep(0.0,0.7,patVal));
    color.xyz = mix( color.xyz, vec3(1.0,1.0,0.1), smoothstep(0.4,0.9,patVal));
    color.xyz = mix( color.xyz, vec3(1.0,1.0,1.0), smoothstep(0.7,1.0,patVal));
    
    gl_FragColor = vec4(color, 1);
}
