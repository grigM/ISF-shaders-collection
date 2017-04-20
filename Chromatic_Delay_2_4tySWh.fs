/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex07.jpg"
    }
  ],
  "CATEGORIES" : [
    "chromaticaberration",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tySWh by DifferentName.  Modified from shaders by LordSk and Sintel, to add more color variety!",
  "INPUTS" : [

  ]
}
*/


#define RADIUS 0.75
#define AB_SCALE 0.75

float diskColorr(in vec2 uv, vec2 offset)
{
    uv = uv - smoothstep(0.01,1.8,IMG_NORM_PIXEL(iChannel0,mod((uv*1.0 - vec2((TIME+0.06) /3.6,(TIME+0.06) /9.2)) + offset,1.0)).r) * 0.3;
    
    float d = length(uv)-RADIUS;
    return smoothstep(0.01,0.015,d);
}
float diskColorg(in vec2 uv, vec2 offset)
{
    uv = uv - smoothstep(0.01,1.8,IMG_NORM_PIXEL(iChannel0,mod((uv*1.0 - vec2(TIME /3.0,(TIME) /8.0)) + offset,1.0)).r) * 0.3;
    
    float d = length(uv)-RADIUS;
    return smoothstep(0.01,0.015,d);
}
float diskColorb(in vec2 uv, vec2 offset)
{
    uv = uv - smoothstep(0.01,1.8,IMG_NORM_PIXEL(iChannel0,mod((uv*1.0 - vec2((TIME-0.06) /2.65,(TIME-0.06) /7.0)) + offset,1.0)).r) * 0.3;
    
    float d = length(uv)-RADIUS;
    return smoothstep(0.01,0.015,d);
}

void main()
{
	vec2 uv = (-RENDERSIZE.xy + 2.0 * gl_FragCoord.xy) / RENDERSIZE.y;
   	
    vec3 color = vec3(0);
    color.r+=diskColorr(uv, vec2(0.00, 0.00) * AB_SCALE);
    color.g+=diskColorg(uv, vec2(0.00, 0.00) * AB_SCALE);
    color.b+=diskColorb(uv, vec2(0.00, 0.00) * AB_SCALE);
    gl_FragColor = vec4(color, 1.0);
}
