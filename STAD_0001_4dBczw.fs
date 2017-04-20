/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "procedural",
    "2d",
    "stad",
    "copyagif",
    "shadereveryday",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4dBczw by tpen.  Shadertoy-a-day, entry #1!\n\nShadertoy version of http:\/\/i.imgur.com\/pGTAjg1.gifv",
  "INPUTS" : [

  ]
}
*/


float pointCircle(vec2 p, vec2 center, float radius)
{
    float d = distance(p, center);
    float aaf = fwidth(d);
	return 1.0 - smoothstep(radius - aaf, radius, d);
}

float dotPattern(vec2 p)
{
    const float radius = 0.038;
    const float pi = 3.14159265359;
    const float numLayers = 9.0;
    
    float pr = length(p);
    
    if (pr > 0.95 || pr < 0.05)
    {
    	return 0.0;
    }

    float q = clamp(pr - 0.05, 0.0, 0.8);
    float layer = 8.0 - floor(q * 10.0);

    float d = 0.0;
    float numCircles = 36.0 - layer * 4.0;
    float r = 0.9 * (numLayers - layer) / numLayers;
    float dist = r * pi * 2.0;
    float speed = 4.0 / dist;	
    for (float index = 0.0; index < 36.0; index += 1.0)
    {
        if (index >= numCircles) break;
        float a = 2.0 * pi * index / numCircles + pi * 0.25 + TIME * 0.6;
        a -= TIME * speed;
        vec2 pos = vec2(cos(a), sin(a)) * r;
        d = max(d, pointCircle(p, pos, radius * (1.0 - layer * 0.04)));
    }

    return d;
}

void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv *= 2.0;
    uv -= vec2(1.0);
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    
	float d = dotPattern(uv);
    float shadow_offset = 0.01;
    float ds = dotPattern(uv + vec2(-shadow_offset, shadow_offset));                          
         
    vec3 bkg = vec3(56.0/255.0,	137.0/255.0,	157.0/255.0);	
    vec3 dots = vec3(253.0/255.0,	248.0/255.0,	255.0	/255.0);
    vec3 shadow = vec3(44.0/255.0,	118.0/255.0,	130.0/255.0);
                  
    vec3 c = mix(bkg, shadow, ds);
    c = mix(c, dots, d);
    
	gl_FragColor = vec4(c, 1.0);
}

// version history:
// 1.0 - base version [tpen]
// 1.1 - size/movement fixes [void room]
// 1.2 - some performance improvements [tpen]
