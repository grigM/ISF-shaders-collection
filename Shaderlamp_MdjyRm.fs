/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "raymarch",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MdjyRm by lherm.  Playing with a torus",
  "INPUTS" : [

  ]
}
*/


#define eps 0.005
#define far 40.
#define time TIME*.25
#define PI 3.1415926

// Variants
//#define rings
//#define polar
//#define warp

vec2 rotate(vec2 p, float a)
{
    float t = atan(p.y, p.x)+a;
    float l = length(p);
    return vec2(l*cos(t), l*sin(t));
}

float sdTorus( vec3 p, vec2 t )
{
  vec2 q = vec2(length(p.xz)-t.x,p.y);
  return length(q)-t.y;
}

vec3 tri(in vec3 x){return abs(x-floor(x)-.5);} // Triangle function.

float distort(vec3 p)
{
    //return sin(p.x + sin(p.y + time * .1) + sin(p.z)*p.z + p.x + p.y + time);
    return dot(tri(p+time) + sin(tri(p+time)), vec3(.666));
}

float trap;

float map(vec3 p)
{
    p.z += .2;
    p += distort(p*distort(p))*.1;
    trap = dot(sin(p), 1.-abs(p))*1.2;
    float d = -sdTorus(p, vec2(1., .7)) + distort(p)*.05;
    
    #ifdef rings
    p.y -= .2;
    for(int i = 0; i < 3; i++)
    {
        p.y += float(i)*.1;
        
    	d = min(d, sdTorus(p, vec2(.75, .01))-distort(p*float(i))*.01);
    }
    #endif
    
    return d;
}

vec3 calcNormal(vec3 p)
{
    vec2 e = vec2(eps, 0);
    return normalize(vec3(
        map(p+e.xyy)-map(p-e.xyy),
        map(p+e.yxy)-map(p-e.yxy),
        map(p+e.yyx)-map(p-e.yyx)
        ));
}

float trace(vec3 r, vec3 d, float start)
{
    float m, t=start;
    for (int i = 0; i < 100; i++)
    {
        m = map(r + d * t);
        t += m;
        if (m < eps || t > far) break;
    }
    return t;
}

void main() {



	vec2 R=RENDERSIZE.xy, u = (gl_FragCoord.xy+gl_FragCoord.xy-R)/R.y;
    
    #ifdef polar
    u = rotate(u, 2.*atan(u.y, u.x));
    #endif
    
    #ifdef warp
    u = abs(u)/dot(u, u);
    #endif
    
    vec3 r = vec3(0, 0, 1), d = normalize(vec3(u, -1)), p, n, col;
    col = vec3(0.);
    float t = trace(r, d, 0.);
    p = r + d * t;
    
    n = calcNormal(p);
    
    if (t < far)
    {
        vec3 objcol = vec3(trap/abs(1.-trap), trap*trap, 1.-trap);
        vec3 lp = vec3(1, 3, 3);
        vec3 ld = lp - p;
        float len = length(ld);
        float atten = max(0., 1./(len*len));
        ld /= len;
        float amb = .25;
        float diff = max(0., dot(ld, n));
        float spec = pow(max(0., dot(reflect(-ld, n), r)), 8.);
        float ref = trace(r, reflect(d, n), eps*5.);
        col = objcol * (((diff*.8+amb*.8)+.1*spec)+atten*.1)*ref;
    }
    
    gl_FragColor = vec4(col, 1);
}
