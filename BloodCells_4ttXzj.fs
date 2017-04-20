/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "noise",
    "cells",
    "blood",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4ttXzj by kuvkar.  Something that looks like blood.",
  "INPUTS" : [

  ]
}
*/


/**
 Just fooling around basicly. Some sort of bloodstream. 
*/


// http://iquilezles.org/www/articles/smin/smin.htm
float smin( float a, float b, float k )
{
    float h = clamp( 0.5+0.5*(b-a)/k, 0.0, 1.0 );
    return mix( b, a, h ) - k*h*(1.0-h);
}

float cells(vec2 uv){  // Trimmed down.
    uv = mix(sin(uv + vec2(1.57, 0)), sin(uv.yx*1.4 + vec2(1.57, 0)), .75);
    return uv.x*uv.y*.3 + .7;
}

/*
float cells(vec2 uv)
{
    float sx = cos(uv.x);
    float sy = sin(uv.y);
    sx = mix(sx, cos(uv.y * 1.4), .75);
    sy = mix(sy, sin(uv.x * 1.4), .75);
    return .3 * (sx * sy) + .7;
}
*/

const float BEAT = 4.0;
float fbm(vec2 uv)
{
    
    float f = 200.0;
    vec2 r = (vec2(.9, .45));    
    vec2 tmp;
    float T = 100.0 + TIME * 1.3;
    T += sin(TIME * BEAT) * .1;
    // layers of cells with some scaling and rotation applied.
    for (int i = 1; i < 8; ++i)
    {
        float fi = float(i);
        uv.y -= T * .5;
        uv.x -= T * .4;
        tmp = uv;
        
        uv.x = tmp.x * r.x - tmp.y * r.y; 
        uv.y = tmp.x * r.y + tmp.y * r.x; 
        float m = cells(uv);
        f = smin(f, m, .07);
    }
    return 1. - f;
}

vec3 g(vec2 uv)
{
    vec2 off = vec2(0.0, .03);
    float t = fbm(uv);
    float x = t - fbm(uv + off.yx);
    float y = t - fbm(uv + off);
    float s = .0025;
    vec3 xv = vec3(s, x, 0);
    vec3 yv = vec3(0, y, s);
    return normalize(cross(xv, -yv)).xzy;
}

vec3 ld = normalize(vec3(1.0, 2.0, 3.));

void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv -= vec2(0.5);  
    float a = RENDERSIZE.x / RENDERSIZE.y;
    uv.y /= a;
    vec2 ouv = uv;
    float B = sin(TIME * BEAT);
    uv = mix(uv, uv * sin(B), .035);
    vec2 _uv = uv * 25.;
    float f = fbm(_uv);
    
    // base color
    gl_FragColor = vec4(f);
    gl_FragColor.rgb *= vec3(1., .3 + B * .05, 0.1 + B * .05);
    
    vec3 v = normalize(vec3(uv, 1.));
    vec3 grad = g(_uv);
    
    // spec
    vec3 H = normalize(ld + v);
    float S = max(0., dot(grad, H));
    S = pow(S, 4.0) * .2;
    gl_FragColor.rgb += S * vec3(.4, .7, .7);
    // rim
    float R = 1.0 - clamp(dot(grad, v), .0, 1.);
    gl_FragColor.rgb = mix(gl_FragColor.rgb, vec3(.8, .8, 1.), smoothstep(-.2, 2.9, R));
    // edges
    gl_FragColor.rgb = mix(gl_FragColor.rgb, vec3(0.), smoothstep(.45, .55, (max(abs(ouv.y * a), abs(ouv.x)))));
    
    // contrast
    gl_FragColor = smoothstep(.0, 1., gl_FragColor);
}