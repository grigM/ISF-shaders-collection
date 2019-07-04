/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtVyR1 by frozenshady.  Simple lemniscate equation produces nearly chaotic bubbles.\n",
  "INPUTS" : [

  ]
}
*/



#define PI 3.14159265359

float sin01(in float x)
{
    return (sin(x) + 1.0) / 2.0;
}

float smoothStep2(in float edge, in float x)
{
    const float fadeWidth = 0.4;
    return smoothstep(edge - fadeWidth, edge + fadeWidth, x);
}

vec2 complexProd(in vec2 v1, in vec2 v2)
{
    return vec2(v1.x * v2.x - v1.y * v2.y, v1.x * v2.y + v2.x * v1.y);
}

float lem(in vec2 uv, in vec2 f1, in vec2 f2, in float r, in float width)
{
    vec2 cp12 = complexProd(uv - f1, uv - f2);
    return smoothStep2(abs(length(cp12) - (r * r)), width);
}

float lem(in vec2 uv, in vec2 f1, in vec2 f2, in vec2 f3, in float r, in float width)
{
    vec2 cp12 = complexProd(uv - f1, uv - f2);
    vec2 cp123 = complexProd(uv - f3, cp12);
    return smoothStep2(abs(length(cp123) - (r * r * r)), width);
}

float lem(in vec2 uv, in vec2 f1, in vec2 f2, in vec2 f3, in vec2 f4, in float r, in float width)
{
    vec2 cp12 = complexProd(uv - f1, uv - f2);
    vec2 cp123 = complexProd(uv - f3, cp12);
    vec2 cp1234 = complexProd(uv - f4, cp123);
    return smoothStep2(abs(length(cp1234) - pow(r, 4.0)), width);
}

float lem(in vec2 uv, in vec2 f1, in vec2 f2, in vec2 f3, in vec2 f4, in vec2 f5, in float r, in float width)
{
    vec2 cp12 = complexProd(uv - f1, uv - f2);
    vec2 cp123 = complexProd(uv - f3, cp12);
    vec2 cp1234 = complexProd(uv - f4, cp123);
    vec2 cp12345 = complexProd(uv - f5, cp1234);
    return smoothStep2(abs(length(cp12345) - pow(r, 5.0)), width);
}

vec2 circMotion(in vec2 center, in float r, in float freq, in float phase)
{
    float s = sin(TIME * freq + phase);
    float c = cos(TIME * freq + phase);
    return center + r * vec2(c, s);
}

vec2 ellipseMotion(in vec2 center, in float a, in float b, in float angle, in float freq, in float phase)
{
    float s = b * sin(TIME * freq + phase);
    float c = a * cos(TIME * freq + phase);
    float rs = sin(angle);
    float rc = cos(angle);
    return center + mat2(rc, -rs, rs, rc) * vec2(c, s);
}

void main() {



    vec2 uv = (gl_FragCoord.xy/RENDERSIZE.xy) * 2.0 - vec2(1.0);
    uv.x *= (RENDERSIZE.x / RENDERSIZE.y);
    float r = sin01(TIME) * 0.6 + 0.6;
    const vec2 f1 = vec2(-0.5, 0.0);
    const vec2 f2 = vec2(0.8, -0.3);
    const vec2 f3 = vec2(0.0, 0.3);
    const vec2 f4 = vec2(-0.85, 0.6);
    
    vec2 f1m = circMotion(f1, 0.4, 5.1, 0.7);
    vec2 f2m = ellipseMotion(f2, 0.2, 0.4, PI / 4.0, 4.3, 0.3);
    vec2 f3m = circMotion(f3, 0.3, 7.0, 0.0);
    vec2 f4m = circMotion(f4, 0.3, 2.0, 1.0);
    
    vec3 c1 = vec3(1.0, 0.0, 0.0);
    vec3 c2 = vec3(0.0, 1.0, 0.0);
    vec3 c3 = vec3(0.0, 0.0, 1.0);
    vec3 c4 = vec3(1.0);
    
    float d1 = distance(uv, f1m);
    float d2 = distance(uv, f2m);
    float d3 = distance(uv, f3m);
    float d4 = distance(uv, f4m);
    
    vec3 col = (d1 * c1 + d2 * c2 + d3 * c3 + d4 * c4) / 1.5;
    
    float c = lem(uv, f1m, f2m, f3m, f4m, r, 0.008);
    
    
    // Output to screen
    gl_FragColor = vec4(col * c,1.0);
}
