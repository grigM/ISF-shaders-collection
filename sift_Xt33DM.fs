/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "procedural",
    "rings",
    "blackandwhite",
    "dots",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xt33DM by mahalis.  Just playing with space",
  "INPUTS" : [

  ]
}
*/


void main()
{
	vec2 uv = vec2(0.5) - gl_FragCoord.xy / RENDERSIZE.xy;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    float dist = length(uv);
    const float pi = 3.14159;
    const float ringIndexMultiplier = 11.0;
    float ang = atan(uv.y, uv.x) / pi + 1.0;
    float ringIndex = ceil(dist * ringIndexMultiplier);
    float direction = (mod(ringIndex, 2.0) * 2.0 - 1.0);
    float v = mod(floor(ang * 10.0 + pow(ringIndex, 1.1) + TIME * direction), 2.0);
    
    //uv = vec2(pow(abs(uv.x), 1.1), pow(abs(uv.y), 1.1));
    uv = abs(uv);
    vec2 dotUV = fract((uv + TIME * 0.05) * 15.0) - 0.5;
    float dotRadius = 0.25 + 0.1 * sin(TIME * 0.5 + v * pi * 0.6);
    float dotValue = smoothstep(0.03, 0.05, length(dotUV) - dotRadius);
    
    v = mix(1.0 - dotValue, dotValue, v);
    v *= smoothstep(0.9, 1.0, dist * ringIndexMultiplier);
    v += 1.0 - smoothstep(0.2, 0.21, abs(1.0 - fract(dist * ringIndexMultiplier)));
    
    
    gl_FragColor = vec4(v, v, v, 1.0);
}