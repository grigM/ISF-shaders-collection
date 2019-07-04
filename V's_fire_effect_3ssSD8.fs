/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3ssSD8 by vegardno.  This was supposed to be a water effect...",
  "INPUTS" : [

  ]
}
*/


#define SMOOTH 1
#define MULTIPLE 1
#define DEBUG_LINE 0

#if SMOOTH
#define FLAME_BASE_WIDTH .012
#else
#define FLAME_BASE_WIDTH .0
#endif

// https://gist.github.com/patriciogonzalezvivo/670c22f3966e662d2f83
float rand(float n){return fract(sin(n) * 43758.5453123);}

float noise(float r, float x, const float n)
{
    r *= 1337.;
    float noise0 = rand(r + floor(n * x));
    float noise1 = rand(r + floor(n * x + 1.));
    float t = fract(n * x);
	return t * noise1 + (1. - t) * noise0;
}

float line(vec2 uv)
{
#if MULTIPLE
    uv = vec2(mod(uv.x + .25, .5) - .25, uv.y + floor((uv.x + .25) / .5));
#endif
    
	float center = .1 * (noise(1., uv.y, 5.) + .8 * noise(2., uv.y, 10.) - .9);
	float width = FLAME_BASE_WIDTH + .04 * (noise(3., uv.y, 5.) + .8 * noise(4., uv.y, 10.));    
#if SMOOTH
    //return sin(3.14 * clamp(.5 + (uv.x - center) / width, 0., 1.));
    if (uv.x < center - width)
        return 0.;
    if (uv.x < center)
        return smoothstep(center - width, center - .7 * width, uv.x);
    if (uv.x < center + width)
        return 1. - smoothstep(center + .7 * width, center + width, uv.x);
	return 0.;
#else
    return float(uv.x > center - width && uv.x < center + width);
#endif
}

vec2 rot(vec2 uv, float a)
{
    return uv * mat2(cos(a), -sin(a), sin(a), cos(a));
}

float flame(vec2 uv, float spread, float p)
{
	float col = 1.;
    col *= line(rot(uv, 3.14 - spread) + vec2(0., p + TIME));
    col *= line(rot(uv, 3.14 + spread) + vec2(0., p + TIME));
	return col;
}

vec3 fire_color(float x)
{
	return
        // red
        vec3(1., 0., 0.) * x
        // yellow
        + vec3(1., 1., 0.) * clamp(x - .5, 0., 1.)
        // white
        + vec3(1., 1., 1.) * clamp(x - .7, 0., 1.);
}

vec3 smoke_color(float x)
{
    return vec3(.5, .5, .5) * x;
}

void main() {



    vec2 uv = (gl_FragCoord.xy - vec2(.5 * RENDERSIZE.x, 0.)) / RENDERSIZE.y
        - vec2(0., .5);
    //uv *= 3. + 2. * cos(-TIME);
    
#if DEBUG_LINE
   	gl_FragColor = vec4(vec3(1., 0., 0.) * line(uv + vec2(0., TIME)), 1.0);
#else
    const int fire_n = 10;
	float fire_intensity = 0.;
    for (int i = 0; i < fire_n; ++i) {
        float t = float(i)/ float(fire_n) - .5;
    	fire_intensity += flame(uv + vec2(0., .08 + .1 * t), .15 + .1 * t, 273. * float(i));
    }
    gl_FragColor = vec4(fire_color(2. * fire_intensity / float(fire_n)), 1.0);
#endif
}
