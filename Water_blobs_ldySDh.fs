/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : [
        "grid_512.png",
        "grid_512.png",
        "grid_512.png",
        "grid_512.png",
        "grid_512.png",
        "grid_512.png"
      ],
      "TYPE" : "cube"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ldySDh by jolle.  Metablobs but of water droplets instead of metal. A bit derivative but some interesting shading.\n\nUse camera to look around. You can change number of multiplesamples (default 3).",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    },
    
    {
      "NAME": "showBackground",
      "TYPE": "bool",
      "DEFAULT": 1
    },
  ]
}
*/



#define MULTISAMPLES 4 // Max 4

const float N = 1.33;
const float zoom = 2.0;
const int max_intersections = 12;

const float eyedistance = 7.5; // Note: These depend on each other
const float min_distance = 3.0;
const float max_distance = 10.5;
const float min_stepsize = 0.25;
const int maxsteps = 30;

const float pi = 3.1415926536;

vec4 sphere1;
vec4 sphere2;
vec4 sphere3;

float sq(float x) { return x * x; }
float sq(vec3 x) { return dot(x, x); }

float fresnel(float n1, float n2, float cos_theta)
{
    float r = sq((n1 - n2) / (n1 + n2));
    return r + (1.0 - r) * pow(1.0 - clamp(cos_theta, 0.0, 1.0), 5.0);
}

vec4 background(vec3 d)
{
	return textureCube(iChannel0,d);
    //return IMG_NORM_PIXEL(iChannel0, d, 0.0);
}

float f(vec3 p)
{
    return 1.0 - (
        sphere1.w / sq(sphere1.xyz - p) + 
        sphere2.w / sq(sphere2.xyz - p) +
        sphere3.w / sq(sphere3.xyz - p));
}

vec3 fd(vec3 p)
{
    vec3 d1 = sphere1.xyz - p;
    vec3 d2 = sphere2.xyz - p;
    vec3 d3 = sphere3.xyz - p;
    return 2.0 * (
        sphere1.w * d1 / sq(sq(d1)) +
        sphere2.w * d2 / sq(sq(d2)) +
        sphere3.w * d3 / sq(sq(d3)));
}

float stepsize(vec3 p)
{
    float md = sqrt(min(min(
        sq(p - sphere1.xyz), 
        sq(p - sphere2.xyz)), 
        sq(p - sphere3.xyz)));
    return max(min_stepsize, abs(md - 1.0) * 0.667);
}

vec4 ray(vec3 p, vec3 d)
{
    float k = min_distance;
    float nf = 1.0;
    vec4 c = vec4(0.0);
    float cr = 1.0;
    for (int j = 0; j < max_intersections; ++j)
    {
        for (int i = 0; i < maxsteps; ++i)
        {
            if (k > max_distance)
            	if(showBackground){
                	return c + background(d) * cr;
            	}else{
	                return c + vec4(0.0) * cr;
            	}
            float ss = stepsize(p + d * k);
            if (f(p + d * (k + ss)) * nf < 0.0)
            {
                k += ss - min_stepsize * 0.5;
                k += f(p + d * k) / dot(d, fd(p + d * k));
                k += f(p + d * k) / dot(d, fd(p + d * k));
                p += d * k;
                
                vec3 n = -normalize(fd(p)) * nf;
                vec3 r = refract(d, n, nf > 0.0 ? 1.0 / N : N);

                if (nf < 0.0)
                {
                    float fa = k * 0.025;
                    c += vec4(0.5, 0.75, 1.0, 1.0) * fa * cr;
                    cr *= 1.0 - fa;
                }

                if (r == vec3(0.0))
                {
	                d = reflect(d, n);
                }
                else
                {
                    float f = nf > 0.0 ? 
                        fresnel(1.0, N, dot(-d, n)) : 
                    	fresnel(N, 1.0, dot(-d, n));
                    if (f > 0.5)
                    {
                        c += background(r) * (1.0 - f) * cr;
                        cr *= f;
                        d = reflect(d, n);
                    }
                    else
                    {                    
                        c += background(reflect(d, n)) * f * cr;
                        cr *= 1.0 - f;
                        d = r;
                        nf *= -1.0;
                    }
                }
                k = 0.0;
                break;
            }
            k += ss;
        }
    }
    return c + background(d) * cr;
}

void main() {



    float t = TIME;
    vec4 vs1 = cos(t * vec4(0.87, 1.13, 1.2, 1.0) + vec4(0.0, 3.32, 0.97, 2.85)) * vec4(-1.7, 2.1, 2.37, -1.9);
    vec4 vs2 = cos(t * vec4(1.07, 0.93, 1.1, 0.81) + vec4(0.3, 3.02, 1.15, 2.97)) * vec4(1.77, -1.81, 1.47, 1.9);
    sphere1 = vec4(vs1.x, 0.0, vs1.y, 1.0);
	sphere2 = vec4(vs1.z, vs1.w, vs2.z, 0.9);
	sphere3 = vec4(vs2.x, vs2.y, vs2.w, 0.8);
    vec2 r = -iMouse.yx / RENDERSIZE.yx * pi * 2.0;
    vec4 cs = cos(vec4(r.y, r.x, r.y - pi * 0.5, r.x - pi * 0.5));
    vec3 forward = -vec3(cs.x * cs.y, cs.w, cs.z * cs.y);
	vec3 up = vec3(cs.x * cs.w, -cs.y, cs.z * cs.w);
	vec3 left = cross(up, forward);
    vec3 eye = -forward * eyedistance;
	vec2 uv = zoom * (gl_FragCoord.xy - RENDERSIZE.xy * 0.5) / RENDERSIZE.x;
    vec3 dir = normalize(vec3(forward + uv.y * up + uv.x * left));    
    vec4 color = ray(eye, dir);
#if MULTISAMPLES > 1
    vec2 uvh = zoom * vec2(0.5) / RENDERSIZE.x;
    color += ray(eye, normalize(forward + (uv.y + uvh.y) * up + (uv.x + uvh.x) * left));
#if MULTISAMPLES > 2
    color += ray(eye, normalize(forward + (uv.y + uvh.y) * up  + uv.x * left));
#if MULTISAMPLES > 3
    color += ray(eye, normalize(forward + uv.y * up + (uv.x + uvh.x) * left));
#endif
#endif
    color /= float(MULTISAMPLES);
#endif
    gl_FragColor = color;
}

/*
"94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_1.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_2.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_3.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_4.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_5.png"
        */
