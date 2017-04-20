/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "raytracing",
    "raymarching",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XlGGWw by Imsure1200q_1UWE130.  Describe your shader",
  "INPUTS" : [

  ]
}
*/


const float pi = asin(1.) * 2.;
const float twopi = asin(1.) * 4.;
#define ds1 if(
#define ds2 )
#define cs1 {
#define cs2 }
vec3 rand(vec3 p)
{
    float k = dot(p.xy, vec2(127.1,311.7));
    return vec3(fract(sin(k)*43758.5453123));
}
float rand(vec2 p)
{
    float k = dot(p.xy, vec2(127.1,311.7));
    return float(fract(sin(k)*43758.5453123));
}
float map(vec3 pos)
{
    return length(sqrt(sqrt(abs(sin(pos)))))-0.9;
}
float trace(vec3 rd, vec3 loc)
{
    float t = 0.0;
    for(int i = 0; i < 24; i++)
    {
        vec3 p = loc+t*rd;
        float d = map(p);
        t += d * 0.5;
    }
    return t;
}
void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv = uv * 2.0 - 1.0;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    vec3 rd = normalize(vec3(uv, 1.0));
    vec3 ro = vec3(cos((TIME)), sin(TIME), TIME);
    for(int i = 0; i < 1; i++)
    {
        vec3 loc = vec3(ro.x + rand(uv*float(i)),
                        ro.y + rand(uv*float(i)),
                        ro.z + rand(uv*float(i)));
        float t = trace(rd, loc);
        vec3 fc = vec3(1.0/(1.0+t*t*0.1));
        gl_FragColor = vec4(fc, 1.0);
    }
}