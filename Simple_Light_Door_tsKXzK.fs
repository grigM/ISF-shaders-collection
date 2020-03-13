/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tsKXzK by ParaBellum.  A naive approach to get area light",
  "INPUTS" : [

  ]
}
*/


vec3 A = vec3(-3., 6., 0.); 
vec3 B = vec3(3., 6., 0.); 
vec3 C = vec3(3., 0., 0.); 
vec3 D = vec3(-3., 0., 0.);
vec3 lightColor = vec3(0.9, .9, .9);

vec3 area_light(in vec3 p, in vec3 a, in vec3 b, in vec3 c, in vec3 d, out vec3 n)
{
    vec3 o = (a+b+c+d)/4.;
    vec3 ba = b-a;
    vec3 cb = c-b;
    vec3 dc = d-c;
    vec3 ad = a-d;
    n = normalize(cross(ba, ad));
    p = p-n*dot(p-o, n);
    vec3 pa = p-a;
    vec3 pb = p-b;
    vec3 pc = p-c;
    vec3 pd = p-d;
    float s = 4., l = 100000.;
    vec3 vo, vh, vd;
    if (sign(dot(cross(ba, n), pa)) <= -1. && length(pa) < l)
    {
        s--; l = length(pa); vo = a; vh = b; vd = pa;
    }
    if (sign(dot(cross(cb, n), pb)) <= -1. && length(pb) < l)
    {
        s--; l = length(pb); vo = b; vh = c; vd = pb;
    }
    if (sign(dot(cross(dc, n), pc)) <= -1. && length(pc) < l)
    {
        s--; l = length(pc); vo = c; vh = d; vd = pc;
    }
    if (sign(dot(cross(ad, n), pd)) <= -1. && length(pd) < l)
    {
        s--; l = length(pd); vo = d; vh = a; vd = pd;
    }
    if (s < 4.)
    {
        float t = clamp(dot(vh-vo, vd)/dot(vh-vo, vh-vo), 0., 1.);
        return mix(vo, vh, t);
    }
    else
        return p;
}

// Quad function from IQ
// https://iquilezles.org/www/articles/distfunctions/distfunctions.htm
float dot2( vec3 v ) { return dot(v,v); }
float quad( vec3 p, vec3 a, vec3 b, vec3 c, vec3 d )
{
  vec3 ba = b - a; vec3 pa = p - a;
  vec3 cb = c - b; vec3 pb = p - b;
  vec3 dc = d - c; vec3 pc = p - c;
  vec3 ad = a - d; vec3 pd = p - d;
  vec3 nor = cross( ba, ad );

  return sqrt(
    (sign(dot(cross(ba,nor),pa)) +
     sign(dot(cross(cb,nor),pb)) +
     sign(dot(cross(dc,nor),pc)) +
     sign(dot(cross(ad,nor),pd))<3.0)
     ?
     min( min( min(
     dot2(ba*clamp(dot(ba,pa)/dot2(ba),0.0,1.0)-pa),
     dot2(cb*clamp(dot(cb,pb)/dot2(cb),0.0,1.0)-pb) ),
     dot2(dc*clamp(dot(dc,pc)/dot2(dc),0.0,1.0)-pc) ),
     dot2(ad*clamp(dot(ad,pd)/dot2(ad),0.0,1.0)-pd) )
     :
     dot(nor,pa)*dot(nor,pa)/dot2(nor) );
}

vec2 opU(in vec2 a, in vec2 b)
{
    return a.x<b.x?a:b;
}

vec2 scene(in vec3 p)
{
    vec2 plane1 = vec2(p.y, 1.5);
    vec2 quad1 = vec2(quad(p, A, B, C, D), 2.5);
    return opU(plane1, quad1);
}

vec3 getNormal(in vec3 p)
{
    float d = scene(p).x;
    vec2 e = vec2(.1, 0.);
    vec3 n = d - vec3
        			(
                        scene(p-e.xyy).x,
                        scene(p-e.yxy).x,
                        scene(p-e.yyx).x
    				);
    return normalize(n);
}

vec2 marcher(in vec3 o, in vec3 d)
{
    float t = 0.;
    for (int i = 0; i < 100; i++)
    {
        vec2 s = scene(o + d * t);
        if (s.x < .01)
            return vec2(t, s.y);
        t += s.x*.5;
    }
    return vec2(-1.5);
}

void main() {



    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy*2.-1.;
    uv.x *= RENDERSIZE.x/RENDERSIZE.y;
    vec3 col = vec3(0.);
    
    vec3 o = vec3(0., 3., 10.);
    vec3 d = vec3(uv, -1.);
    
    float T = .6;
    mat2 rot = mat2(cos(T), sin(T), -sin(T), cos(T));
    
    A.xz *= rot;
    B.xz *= rot;
    C.xz *= rot;
    D.xz *= rot;
    
    vec2 m = marcher(o, d);
    int index = int(floor(m.y));
    if (index != -1)
    {
        vec3 p = o + d * m.x;
        if (index == 1)
        {
            vec3 n = vec3(0.);
            vec3 pl = area_light(p, A, B, C, D, n);
            float df = dot(normalize(p-pl), n);
            
            float l = length(p-pl);
            float kc = 1.;
            float kl = .7;
            float kq = 1.8;
            float attenuation = 1. / (kc+kl*l+kq*l*l);
            
        	col += lightColor*df*attenuation;
        }
        if (index == 2)
        {
            col += lightColor;
        }
    }
    gl_FragColor = vec4(sqrt(col),1.0);
}
