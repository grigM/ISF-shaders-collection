/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59821.0"
}
*/



//precision mediump float;

const float PI = 3.1415926;


vec3 hsv(float h, float s, float v)
{
    vec4 t = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(vec3(h) + t.xyz) * 6.0 - vec3(t.w));
    return v * mix(vec3(t.x), clamp(p - vec3(t.x), 0.0, 1.0), s);
}

float sdBox( vec3 p, vec3 b )
{
  vec3 d = abs(p) - b;
  return length(max(d,0.0))
         + min(max(d.x,max(d.y,d.z)),0.0); // remove this line for an only partially signed sdf 
}

mat2 rot(float a)
{
    return mat2(cos(a), -sin(a), sin(a), cos(a));
}

float dist(vec3 pos)
{
    //mat2 r = rot(TIME * 0.1);
    
    pos = mod(pos, 10.0) - 5.0;
    
    float d = 1000000000000.0;
    d = min(sdBox(pos, vec3(2, 4, 2)), d);
    
    //d = min(pos.y - 10.5, d);
    
    return d;
}


vec3 calcNormal(vec3 pos)
{
    vec2 ep = vec2(0.001, 0);
    return normalize(vec3(
        dist(pos) - dist(pos + ep.xyy),
        dist(pos) - dist(pos + ep.yxy),
        dist(pos) - dist(pos + ep.yyx)
    ));
}

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main()
{
    vec2 uv = (gl_FragCoord.xy - RENDERSIZE / 2.0) / RENDERSIZE.y;
    
    uv.x += rand(vec2(floor(uv.y * 20.0), 0.0)) * pow(1.0 - fract(TIME), 8.0) * 1.1;
    uv.x += rand(vec2(floor(uv.x * 20.0), 0.0)) * pow(1.0 - fract(TIME + 0.5), 8.0) * 1.1;
    vec3 col = vec3(0.0);
    
    //col = 0.01 / abs(length(uv.xy) * vec3(1) - 0.2 + sin(atan(uv.x, uv.y) * 8.0 + TIME * 1.0) * sin(TIME * 1.0) * 0.1);
    
    /*
    if (abs(mod(uv.x + TIME * 0.1, 0.1)) - 0.05 < 0.001)
    {
        col += 0.5 * vec3(0, 0, 1);
    }
    if (abs(mod(uv.y + TIME * 0.01, 0.1)) - 0.05 < 0.001)
    {
        col += 0.5 * vec3(1, 0, 0);
    }
    */
    
    vec3 startPos = vec3(0, 0, TIME * 2.0);
    vec3 pos = startPos;
    vec3 dir = normalize(vec3(uv, 1));
    
    dir.xy = rot(sin(TIME) * 0.4) * dir.xy;
    dir.xz = rot(sin(TIME * 0.2) * 0.2) * dir.xz;
    
    float depth = 0.0;
    
    for (int i = 0; i < 28; ++i)
    {
        float d = dist(pos);
        pos += d * dir;
        
        depth = float(i);
        
        if (d < 0.001)
        {
            if (mod(pos.y, 1.0) < 0.5 && mod(pos.x, 1.0) < 0.8)
            {
                col = hsv(floor(pos.x) * 0.5 + floor(pos.y) * 0.2 + floor(pos.z) * 0.1 + TIME, 0.8, rand(floor(pos.xy)));
            }
            else
            {
                col = vec3(0.1);
            }
            break;
        }
    }
    
    
    col += depth / 128.0;
    
    gl_FragColor = vec4(col, 1.0);
    
}