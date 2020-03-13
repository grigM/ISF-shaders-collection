/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/tsKXR3 by cornusammonis.  A mipmap-based approach to multiscale fluid dynamics.",
    "IMPORTED": {
    },
    "INPUTS": [
        {
            "NAME": "iMouse",
            "TYPE": "point2D"
        }
    ],
    "PASSES": [
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferA"
        },
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferB"
        },
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferC"
        },
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferD"
        },
        {
        },
        {
        }
    ]
}

*/


#define TURBULENCE_SAMPLER iChannel3
#define CONFINEMENT_SAMPLER iChannel2
#define POISSON_SAMPLER iChannel1
#define VELOCITY_SAMPLER iChannel0

#define V(d) texture(TURBULENCE_SAMPLER, fract(uv+(d+0.))).xy

vec2 gaussian_turbulence(vec2 uv) {
    vec2 texel = 1.0/RENDERSIZE.xy;
    vec4 t = vec4(texel, -texel.y, 0);

    vec2 d =    V( t.ww); vec2 d_n =  V( t.wy); vec2 d_e =  V( t.xw);
    vec2 d_s =  V( t.wz); vec2 d_w =  V(-t.xw); vec2 d_nw = V(-t.xz);
    vec2 d_sw = V(-t.xy); vec2 d_ne = V( t.xy); vec2 d_se = V( t.xz);
    
    return 0.25 * d + 0.125 * (d_e + d_w + d_n + d_s) + 0.0625 * (d_ne + d_nw + d_se + d_sw);
}

#define C(d) texture(CONFINEMENT_SAMPLER, fract(uv+(d+0.))).xy

vec2 gaussian_confinement(vec2 uv) {
    vec2 texel = 1.0/RENDERSIZE.xy;
    vec4 t = vec4(texel, -texel.y, 0);

    vec2 d =    C( t.ww); vec2 d_n =  C( t.wy); vec2 d_e =  C( t.xw);
    vec2 d_s =  C( t.wz); vec2 d_w =  C(-t.xw); vec2 d_nw = C(-t.xz);
    vec2 d_sw = C(-t.xy); vec2 d_ne = C( t.xy); vec2 d_se = C( t.xz);
    
    return 0.25 * d + 0.125 * (d_e + d_w + d_n + d_s) + 0.0625 * (d_ne + d_nw + d_se + d_sw);
}

#define D(d) texture(POISSON_SAMPLER, fract(uv+d)).x

vec2 diff(vec2 uv) {
    vec2 texel = 1.0/RENDERSIZE.xy;
    vec4 t = vec4(texel, -texel.y, 0);

    float d =    D( t.ww); float d_n =  D( t.wy); float d_e =  D( t.xw);
    float d_s =  D( t.wz); float d_w =  D(-t.xw); float d_nw = D(-t.xz);
    float d_sw = D(-t.xy); float d_ne = D( t.xy); float d_se = D( t.xz);
    
    return vec2(
        0.5 * (d_e - d_w) + 0.25 * (d_ne - d_nw + d_se - d_sw),
        0.5 * (d_n - d_s) + 0.25 * (d_ne + d_nw - d_se - d_sw)
    );
}

#define N(d) texture(VELOCITY_SAMPLER, fract(uv+(d+0.)))

vec4 gaussian_velocity(vec2 uv) {
    vec2 texel = 1.0/RENDERSIZE.xy;
    vec4 t = vec4(texel, -texel.y, 0);

    vec4 d =    N( t.ww); vec4 d_n =  N( t.wy); vec4 d_e =  N( t.xw);
    vec4 d_s =  N( t.wz); vec4 d_w =  N(-t.xw); vec4 d_nw = N(-t.xz);
    vec4 d_sw = N(-t.xy); vec4 d_ne = N( t.xy); vec4 d_se = N( t.xz);
    
    return 0.25 * d + 0.125 * (d_e + d_w + d_n + d_s) + 0.0625 * (d_ne + d_nw + d_se + d_sw);
}

vec2 vector_laplacian(vec2 uv) {
    const float _K0 = -20.0/6.0, _K1 = 4.0/6.0, _K2 = 1.0/6.0;
    vec2 texel = 1.0/RENDERSIZE.xy;
    vec4 t = vec4(texel, -texel.y, 0);

    vec4 d =    N( t.ww); vec4 d_n =  N( t.wy); vec4 d_e =  N( t.xw);
    vec4 d_s =  N( t.wz); vec4 d_w =  N(-t.xw); vec4 d_nw = N(-t.xz);
    vec4 d_sw = N(-t.xy); vec4 d_ne = N( t.xy); vec4 d_se = N( t.xz);
    
    return (_K0 * d + _K1 * (d_e + d_w + d_n + d_s) + _K2 * (d_ne + d_nw + d_se + d_sw)).xy;
}

    


#define TURB_CH xy
#define TURB_SAMPLER iChannel0
#define DEGREE TURBULENCE_SCALES

#define D(d) textureLod(TURB_SAMPLER, fract(uv+d), mip).TURB_CH

void tex(vec2 uv, inout mat3 mx, inout mat3 my, int degree) {
    vec2 texel = 1.0/RENDERSIZE.xy;
    float stride = float(1 << degree);
    float mip = float(degree);
    vec4 t = stride * vec4(texel, -texel.y, 0);

    vec2 d =    D( t.ww); vec2 d_n =  D( t.wy); vec2 d_e =  D( t.xw);
    vec2 d_s =  D( t.wz); vec2 d_w =  D(-t.xw); vec2 d_nw = D(-t.xz);
    vec2 d_sw = D(-t.xy); vec2 d_ne = D( t.xy); vec2 d_se = D( t.xz);
    
    mx =  mat3(d_nw.x, d_n.x, d_ne.x,
               d_w.x,  d.x,   d_e.x,
               d_sw.x, d_s.x, d_se.x);
    
    my =  mat3(d_nw.y, d_n.y, d_ne.y,
               d_w.y,  d.y,   d_e.y,
               d_sw.y, d_s.y, d_se.y);
}

float reduce(mat3 a, mat3 b) {
    mat3 p = matrixCompMult(a, b);
    return p[0][0] + p[0][1] + p[0][2] +
           p[1][0] + p[1][1] + p[1][2] +
           p[2][0] + p[2][1] + p[2][2];
}

void turbulence(vec2 fragCoord, inout vec2 turb, inout float curl)
{
	vec2 uv = fragCoord.xy / RENDERSIZE.xy;
     
    mat3 turb_xx = (2.0 - TURB_ISOTROPY) * mat3(
        0.125,  0.25, 0.125,
       -0.25,  -0.5, -0.25,
        0.125,  0.25, 0.125
    );

    mat3 turb_yy = (2.0 - TURB_ISOTROPY) * mat3(
        0.125, -0.25, 0.125,
        0.25,  -0.5,  0.25,
        0.125, -0.25, 0.125
    );

    mat3 turb_xy = TURB_ISOTROPY * mat3(
       0.25, 0.0, -0.25,  
       0.0,  0.0,  0.0,
      -0.25, 0.0,  0.25
    );
    
    const float norm = 8.8 / (4.0 + 8.0 * CURL_ISOTROPY);  // 8.8 takes the isotropy as 0.6
    float c0 = CURL_ISOTROPY;
    
    mat3 curl_x = mat3(
        c0,   1.0,  c0,
        0.0,  0.0,  0.0,
       -c0,  -1.0, -c0
    );

    mat3 curl_y = mat3(
        c0, 0.0, -c0,
       1.0, 0.0, -1.0,
        c0, 0.0, -c0
    );
    
    mat3 mx, my;
    vec2 v = vec2(0);
    float turb_wc, curl_wc = 0.0;
    curl = 0.0;
    for (int i = 0; i < DEGREE; i++) {
        tex(uv, mx, my, i);
        float turb_w = TURB_W_FUNCTION;
        float curl_w = CURL_W_FUNCTION;
    	v += turb_w * vec2(reduce(turb_xx, mx) + reduce(turb_xy, my), reduce(turb_yy, my) + reduce(turb_xy, mx));
        curl += curl_w * (reduce(curl_x, mx) + reduce(curl_y, my));
        turb_wc += turb_w;
        curl_wc += curl_w;
    }

    turb = float(DEGREE) * v / turb_wc;
    curl = norm * curl / curl_wc;
}

#define CURL_CH w
#define CURL_SAMPLER iChannel0
#define DEGREE VORTICITY_SCALES

#define CURL(d) textureLod(CURL_SAMPLER, fract(uv+(d+0.0)), mip).CURL_CH
#define D(d) abs(textureLod(CURL_SAMPLER, fract(uv+d), mip).CURL_CH)

void tex(vec2 uv, inout mat3 mc, inout float curl, int degree) {
    vec2 texel = 1.0/RENDERSIZE.xy;
    float stride = float(1 << degree);
    float mip = float(degree);
    vec4 t = stride * vec4(texel, -texel.y, 0);

    float d =    D( t.ww); float d_n =  D( t.wy); float d_e =  D( t.xw);
    float d_s =  D( t.wz); float d_w =  D(-t.xw); float d_nw = D(-t.xz);
    float d_sw = D(-t.xy); float d_ne = D( t.xy); float d_se = D( t.xz);
    
    mc =  mat3(d_nw, d_n, d_ne,
               d_w,  d,   d_e,
               d_sw, d_s, d_se);
    
    curl = CURL();
    
}

float reduce(mat3 a, mat3 b) {
    mat3 p = matrixCompMult(a, b);
    return p[0][0] + p[0][1] + p[0][2] +
           p[1][0] + p[1][1] + p[1][2] +
           p[2][0] + p[2][1] + p[2][2];
}

vec2 confinement(vec2 fragCoord)
{
	vec2 uv = fragCoord.xy / RENDERSIZE.xy;
    
    float k0 = CONF_ISOTROPY;
    float k1 = 1.0 - 2.0*(CONF_ISOTROPY);

    mat3 conf_x = mat3(
       -k0, -k1, -k0,
        0.0, 0.0, 0.0,
        k0,  k1,  k0
    );

    mat3 conf_y = mat3(
       -k0,  0.0,  k0,
       -k1,  0.0,  k1,
       -k0,  0.0,  k0
    );
    
    mat3 mc;
    vec2 v = vec2(0);
    float curl;
    
    float cacc = 0.0;
    vec2 nacc = vec2(0);
    float wc = 0.0;
    for (int i = 0; i < DEGREE; i++) {
        tex(uv, mc, curl, i);
        float w = CONF_W_FUNCTION;
        vec2 n = w * normz(vec2(reduce(conf_x, mc), reduce(conf_y, mc)));
        v += curl * n;
        cacc += curl;
        nacc += n;
        wc += w;
    }

    #ifdef PREMULTIPLY_CURL
        return v / wc;
    #else
    	return nacc * cacc / wc;
    #endif

}

#define VORT_CH xy
#define VORT_SAMPLER iChannel0
#define POIS_SAMPLER iChannel1
#define POIS_CH x
#define DEGREE POISSON_SCALES

#define D(d) textureLod(VORT_SAMPLER, fract(uv+d), mip).VORT_CH
#define P(d) textureLod(POIS_SAMPLER, fract(uv+d), mip).POIS_CH

float laplacian_poisson(vec2 fragCoord) {
    const float _K0 = -20.0/6.0, _K1 = 4.0/6.0, _K2 = 1.0/6.0;
    vec2 texel = 1.0/RENDERSIZE.xy;
    vec2 uv = fragCoord * texel;
    vec4 t = vec4(texel, -texel.y, 0);
    float mip = 0.0;

    float p =    P( t.ww); float p_n =  P( t.wy); float p_e =  P( t.xw);
    float p_s =  P( t.wz); float p_w =  P(-t.xw); float p_nw = P(-t.xz);
    float p_sw = P(-t.xy); float p_ne = P( t.xy); float p_se = P( t.xz);
    
    return _K0 * p + _K1 * (p_e + p_w + p_n + p_s) + _K2 * (p_ne + p_nw + p_se + p_sw);
}

void tex(vec2 uv, inout mat3 mx, inout mat3 my, inout mat3 mp, int degree) {
    vec2 texel = 1.0/RENDERSIZE.xy;
    float stride = float(1 << degree);
    float mip = float(degree);
    vec4 t = stride * vec4(texel, -texel.y, 0);

    vec2 d =    D( t.ww); vec2 d_n =  D( t.wy); vec2 d_e =  D( t.xw);
    vec2 d_s =  D( t.wz); vec2 d_w =  D(-t.xw); vec2 d_nw = D(-t.xz);
    vec2 d_sw = D(-t.xy); vec2 d_ne = D( t.xy); vec2 d_se = D( t.xz);
    
    float p =    P( t.ww); float p_n =  P( t.wy); float p_e =  P( t.xw);
    float p_s =  P( t.wz); float p_w =  P(-t.xw); float p_nw = P(-t.xz);
    float p_sw = P(-t.xy); float p_ne = P( t.xy); float p_se = P( t.xz);
    
    mx =  mat3(d_nw.x, d_n.x, d_ne.x,
               d_w.x,  d.x,   d_e.x,
               d_sw.x, d_s.x, d_se.x);
    
    my =  mat3(d_nw.y, d_n.y, d_ne.y,
               d_w.y,  d.y,   d_e.y,
               d_sw.y, d_s.y, d_se.y);
    
    mp =  mat3(p_nw, p_n, p_ne,
               p_w,  p,   p_e,
               p_sw, p_s, p_se);
}

float reduce(mat3 a, mat3 b) {
    mat3 p = matrixCompMult(a, b);
    return p[0][0] + p[0][1] + p[0][2] +
           p[1][0] + p[1][1] + p[1][2] +
           p[2][0] + p[2][1] + p[2][2];
}

vec2 pois(vec2 fragCoord)
{
	vec2 uv = fragCoord.xy / RENDERSIZE.xy;
    
    float k0 = POIS_ISOTROPY;
    float k1 = 1.0 - 2.0*(POIS_ISOTROPY);
    
    mat3 pois_x = mat3(
        k0,  0.0, -k0,
        k1,  0.0, -k1,
        k0,  0.0, -k0
    );
     
    mat3 pois_y = mat3(
       -k0,  -k1,  -k0,
        0.0,  0.0,  0.0,
        k0,   k1,   k0
    );

    mat3 gauss = mat3(
       0.0625, 0.125, 0.0625,  
       0.125,  0.25,  0.125,
       0.0625, 0.125, 0.0625
    );
    
    mat3 mx, my, mp;
    vec2 v = vec2(0);
    
    float wc = 0.0;
    for (int i = 0; i < DEGREE; i++) {
        tex(uv, mx, my, mp, i);
        float w = POIS_W_FUNCTION;
        wc += w;
    	v += w * vec2(reduce(pois_x, mx) + reduce(pois_y, my), reduce(gauss, mp));
    }

    return v / wc;

}

#define V(d) textureLod(VORT_SAMPLER, fract(uv+d), mip).zw

/*
	Number of scales to use in computation of each value. Lowering these will change the 
    result drastically, also note that the heightmap is used for rendering, so changing 
    POISSON_SCALES will alter the appearance of lighting/shadows. Weighting functions
    for each scale are defined below.
*/
#define TURBULENCE_SCALES 11
#define VORTICITY_SCALES 11
#define POISSON_SCALES 11



// If defined, recalculate the advection offset at every substep. Otherwise, subdivide the offset.
// Leaving this undefined is much cheaper for large ADVECTION_STEPS but yields worse results; this
// can be improved by defining the BLUR_* options below.
#define RECALCULATE_OFFSET
// Number of advection substeps to use. Useful to increase this for large ADVECTION_SCALE. Must be >= 1
#define ADVECTION_STEPS 3
// Advection distance multiplier.
#define ADVECTION_SCALE 40.0
// Scales the effect of turbulence on advection.
#define ADVECTION_TURBULENCE 1.0
// Scales the effect of turbulence on velocity. Use small values.
#define VELOCITY_TURBULENCE 0.0000
// Scales the effect of vorticity confinement on velocity.
#define VELOCITY_CONFINEMENT 0.01
// Scales diffusion.
#define VELOCITY_LAPLACIAN 0.02
// Scales the effect of vorticity confinement on advection.
#define ADVECTION_CONFINEMENT 0.6
// Scales the effect of divergence on advection.
#define ADVECTION_DIVERGENCE  0.0
// Scales the effect of velocity on advection.
#define ADVECTION_VELOCITY -0.05
// Amount of divergence minimization. Too much will cause instability.
#define DIVERGENCE_MINIMIZATION 0.1
// If 0.0, compute the gradient at (0,0). If 1.0, compute the gradient at the advection distance.
#define DIVERGENCE_LOOKAHEAD 1.0
// If 0.0, compute the laplacian at (0,0). If 1.0, compute the laplacian at the advection distance.
#define LAPLACIAN_LOOKAHEAD 1.0
// Scales damping force.
#define DAMPING 0.0001
// Overall velocity multiplier
#define VELOCITY_SCALE 1.0
// Mixes the previous velocity with the new velocity (range 0..1).
#define UPDATE_SMOOTHING 0.0



// These control the (an)isotropy of the various kernels
#define TURB_ISOTROPY 0.9  // [0..2.0]
#define CURL_ISOTROPY 0.6  // >= 0
#define CONF_ISOTROPY 0.25 // [0..0.5]
#define POIS_ISOTROPY 0.16 // [0..0.5]



// If defined, multiply curl by vorticity, then accumulate. If undefined, accumulate, then multiply.
#define PREMULTIPLY_CURL



// These apply a gaussian blur to the various values used in the velocity/advection update. More expensive when defined.
//#define BLUR_TURBULENCE
//#define BLUR_CONFINEMENT
//#define BLUR_VELOCITY



// These define weighting functions applied at each of the scales, i=0 being the finest detail.
//#define TURB_W_FUNCTION 1.0/float(i+1)
#define TURB_W_FUNCTION 1.0
//#define TURB_W_FUNCTION float(i+1)

//#define CURL_W_FUNCTION 1.0/float(1 << i)
#define CURL_W_FUNCTION 1.0/float(i+1)
//#define CURL_W_FUNCTION 1.0

//#define CONF_W_FUNCTION 1.0/float(i+1)
#define CONF_W_FUNCTION 1.0
//#define CONF_W_FUNCTION float(i+1)
//#define CONF_W_FUNCTION float(1 << i)

//#define POIS_W_FUNCTION 1.0
#define POIS_W_FUNCTION 1.0/float(i+1)
//#define POIS_W_FUNCTION 1.0/float(1 << i)
//#define POIS_W_FUNCTION float(i+1)
//#define POIS_W_FUNCTION float(1 << i)



// These can help reduce mipmap artifacting, especially when POIS_W_FUNCTION emphasizes large scales.
//#define USE_PRESSURE_ADVECTION
// Scales pressure advection distance.
#define PRESSURE_ADVECTION 0.0002 // higher values more likely to cause blowup if laplacian > 0.0
// Pressure diffusion.
#define PRESSURE_LAPLACIAN 0.1 // [0..0.3] higher values more likely to cause blowup
// Mixes the previous pressure with the new pressure.
#define PRESSURE_UPDATE_SMOOTHING 0.0 // [0..1]



// Scales mouse interaction effect
#define MOUSE_AMP 0.05
// Scales mouse interaction radius
#define MOUSE_RADIUS 0.001



// If defined, "pump" velocity in the center of the screen. If undefined, alternate pumping from the sides of the screen.
//#define CENTER_PUMP
// Amplitude and cycle time for the "pump" at the center of the screen.
#define PUMP_SCALE 0.001
#define PUMP_CYCLE 0.2


vec4 normz(vec4 x) {
	return x.xyz == vec3(0) ? vec4(0,0,0,x.w) : vec4(normalize(x.xyz),0);
}

vec3 normz(vec3 x) {
	return x == vec3(0) ? vec3(0) : normalize(x);
}

vec2 normz(vec2 x) {
	return x == vec2(0) ? vec2(0) : normalize(x);
}


// Only used for rendering, but useful helpers
float softmax(float a, float b, float k) {
	return log(exp(k*a)+exp(k*b))/k;    
}

float softmin(float a, float b, float k) {
	return -log(exp(-k*a)+exp(-k*b))/k;    
}

vec4 softmax(vec4 a, vec4 b, float k) {
	return log(exp(k*a)+exp(k*b))/k;    
}

vec4 softmin(vec4 a, vec4 b, float k) {
	return -log(exp(-k*a)+exp(-k*b))/k;    
}

float softclamp(float a, float b, float x, float k) {
	return (softmin(b,softmax(a,x,k),k) + softmax(a,softmin(b,x,k),k)) / 2.0;    
}

vec4 softclamp(vec4 a, vec4 b, vec4 x, float k) {
	return (softmin(b,softmax(a,x,k),k) + softmax(a,softmin(b,x,k),k)) / 2.0;    
}

vec4 softclamp(float a, float b, vec4 x, float k) {
	return (softmin(vec4(b),softmax(vec4(a),x,k),k) + softmax(vec4(a),softmin(vec4(b),x,k),k)) / 2.0;    
}




// GGX from Noby's Goo shader https://www.shadertoy.com/view/lllBDM

// MIT License: https://opensource.org/licenses/MIT
float G1V(float dnv, float k){
    return 1.0/(dnv*(1.0-k)+k);
}

float ggx(vec3 n, vec3 v, vec3 l, float rough, float f0){
    float alpha = rough*rough;
    vec3 h = normalize(v+l);
    float dnl = clamp(dot(n,l), 0.0, 1.0);
    float dnv = clamp(dot(n,v), 0.0, 1.0);
    float dnh = clamp(dot(n,h), 0.0, 1.0);
    float dlh = clamp(dot(l,h), 0.0, 1.0);
    float f, d, vis;
    float asqr = alpha*alpha;
    const float pi = 3.14159;
    float den = dnh*dnh*(asqr-1.0)+1.0;
    d = asqr/(pi * den * den);
    dlh = pow(1.0-dlh, 5.0);
    f = f0 + (1.0-f0)*dlh;
    float k = alpha/1.0;
    vis = G1V(dnl, k)*G1V(dnv, k);
    float spec = dnl * d * f * vis;
    return spec;
}
// End Noby's GGX


// Modified from Shane's Bumped Sinusoidal Warp shadertoy here:
// https://www.shadertoy.com/view/4l2XWK
vec3 light(vec2 uv, float BUMP, float SRC_DIST, vec2 dxy, float TIME, inout vec3 avd) {
    vec3 sp = vec3(uv-0.5, 0);
    vec3 light = vec3(cos(TIME/2.0)*0.5, sin(TIME/2.0)*0.5, -SRC_DIST);
    vec3 ld = light - sp;
    float lDist = max(length(ld), 0.001);
    ld /= lDist;
    avd = reflect(normalize(vec3(BUMP*dxy, -1.0)), vec3(0,1,0));
    return ld;
}
// End Shane's bumpmapping section


// The MIT License
// Copyright © 2017 Inigo Quilez
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
float hash1( uint n ) 
{
    // integer hash copied from Hugo Elias
	n = (n << 13U) ^ n;
    n = n * (n * n * 15731U + 789221U) + 1376312589U;
    return float( n & uvec3(0x7fffffffU))/float(0x7fffffff);
}

vec3 hash3( uint n ) 
{
    // integer hash copied from Hugo Elias
	n = (n << 13U) ^ n;
    n = n * (n * n * 15731U + 789221U) + 1376312589U;
    uvec3 k = n * uvec3(n,n*16807U,n*48271U);
    return vec3( k & uvec3(0x7fffffffU))/float(0x7fffffff);
}

// a simple modification for this shader to get a vec4
vec4 rand4( vec2 fragCoord, vec2 RENDERSIZE, int FRAMEINDEX ) {
    uvec2 p = uvec2(fragCoord);
    uvec2 r = uvec2(RENDERSIZE);
    uint c = p.x + r.x*p.y + r.x*r.y*uint(FRAMEINDEX);
	return vec4(hash3(c),hash1(c + 75132895U));   
}
// End IQ's integer hash
/* 
	Created by Cornus Ammonis (2019)
	Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
*/

/*
	This is a mipmap-based approach to multiscale fluid dynamics.

	Check the Common tab for lots of configurable parameters.

	Click to interact with your mouse. I'd recommend turning off the "pump" by
	setting PUMP_SCALE to 0.0 on line 113 of the Common tab to play around with
	just mouse interaction.

	Buffer B is a multiscale method for computing turbulence along the lines of 
	the Large Eddy Simulation method; multiscale curl is also computed in Buffer B, 
    to be passed along to Buffer C.
	
	Buffer C is a fairly conventional Vorticity Confinement method, also multiscale, 
    using the curl computed in Buffer B. It probably makes more sense to compute 
    each curl scale separately before accumulating, but for the sake of efficiency 
    and simplicity (a larger kernel would be required), I haven't done that here.

	Buffer D is a multiscale Poisson solver, which converges rapidly but not to an 
    exact solution - this nonetheless works well for the purposes of divergence 
    minimization since we only need the gradient, with allowances for the choice of
    scale weighting. 

	Buffer A computes subsampled advection and velocity update steps, sampling
    from Buffers B, C, and D with a variety of smoothing options.

	There are a number of options included to make this run faster.

	Using mipmaps in this way has a variety of advantages:

	1. The scale computations have no duplicative or dependent reads, we only need 
       that for advection.
	2. No randomness or stochastic sampling is involved.
	3. The total number of reads can be greatly reduced for a comparable level of 
       fidelity to some other methods.
	4. We can easily sample the entire buffer in one pass (on average).
	5. The computational complexity is deferred to mipmap generation (though with
       a large coefficient), because: 
	6. The algorithm itself is O(n) with a fixed number of scales (or we could 
       potentially do scale calculations in parallel with mipmap generation, 
       equalling mipmap generation complexity at O(nlogn))

	Notable downsides:

	1. Using mipmaps introduces a number of issues, namely:
       a. Mipmaps can introduce artifacts due to interpolation and downsampling. 
          Using Gaussian pyramids, or some other lowpass filtering method would 
          be better. 
       b. Using higher-order sampling of the texture buffer (e.g. bicubic) would 
          also be better, but that would limit our performance gains. 
       c. NPOT textures are problematic (as always). They can introduce weird 
          anisotropy issues among other things.
	2. Stochastic or large-kernel methods are a better approximation to the true
       sampling distribution approximated here, for a large-enough number of
       samples.
    3. We're limited in how we construct our scale-space. Is a power-of-two stride 
       length on both axes always ideal, even along diagonals? I'm not particularly 
       sure. There are clever wavelet methods out there for Navier-Stokes solvers, 
       and LES in particular, too.

*/


#define BUMP 3200.0

#define D(d) -textureLod(BufferD, fract(uv+(d+0.0)), mip).w

vec2 diff(vec2 uv, float mip) {
    vec2 texel = 1.0/RENDERSIZE.xy;
    vec4 t = float(1<<int(mip))*vec4(texel, -texel.y, 0);

    float d =    D( t.ww); float d_n =  D( t.wy); float d_e =  D( t.xw);
    float d_s =  D( t.wz); float d_w =  D(-t.xw); float d_nw = D(-t.xz);
    float d_sw = D(-t.xy); float d_ne = D( t.xy); float d_se = D( t.xz);
    
    return vec2(
        0.5 * (d_e - d_w) + 0.25 * (d_ne - d_nw + d_se - d_sw),
        0.5 * (d_n - d_s) + 0.25 * (d_ne + d_nw - d_se - d_sw)
    );
}

vec4 contrast(vec4 col, float x) {
	return x * (col - 0.5) + 0.5;
}

void main() {
	if (PASSINDEX == 0)	{


	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	    vec2 tx = 1.0/RENDERSIZE.xy;
	
	    
	    vec2 turb, confine, div, delta_v, offset, lapl = vec2(0);
	    vec4 vel, adv = vec4(0);
	    vec4 init = N();
	
	    #ifdef RECALCULATE_OFFSET
	        for (int i = 0; i < ADVECTION_STEPS; i++) {
	            #ifdef BLUR_TURBULENCE
	            turb = gaussian_turbulence(uv + tx * offset);
	            #else
	            turb = V(tx * offset);
	            #endif
	
	            #ifdef BLUR_CONFINEMENT
	            confine = gaussian_confinement(uv + tx * offset);
	            #else
	            confine = C(tx * offset);
	            #endif
	
	            #ifdef BLUR_VELOCITY
	            vel = gaussian_velocity(uv + tx * offset);
	            #else
	            vel = N(tx * offset);
	            #endif
	
	            // an alternative, but seems to give less smooth results:
	            // offset += (1.0 / float(ADVECTION_STEPS)) * ...
	            offset = (float(i+1) / float(ADVECTION_STEPS)) * - ADVECTION_SCALE * (ADVECTION_VELOCITY * vel.xy + ADVECTION_TURBULENCE * turb - ADVECTION_CONFINEMENT * confine + ADVECTION_DIVERGENCE * div);
	
	            div = diff(uv + tx * DIVERGENCE_LOOKAHEAD * offset);
	
	            lapl = vector_laplacian(uv + tx * LAPLACIAN_LOOKAHEAD * offset);
	
	            adv += N(tx * offset);
	
	            delta_v += VELOCITY_LAPLACIAN * lapl + VELOCITY_TURBULENCE * turb + VELOCITY_CONFINEMENT * confine - DAMPING * vel.xy - DIVERGENCE_MINIMIZATION * div;
	        }
	        adv /= float(ADVECTION_STEPS);
	        delta_v /= float(ADVECTION_STEPS);
	    #else
	        #ifdef BLUR_TURBULENCE
	        turb = gaussian_turbulence(uv);
	        #else
	        turb = V();
	        #endif
	
	        #ifdef BLUR_CONFINEMENT
	        confine = gaussian_confinement(uv);
	        #else
	        confine = C();
	        #endif
	
	        #ifdef BLUR_VELOCITY
	        vel = gaussian_velocity(uv);
	        #else
	        vel = N();
	        #endif
	    
	    	offset = - ADVECTION_SCALE * (ADVECTION_VELOCITY * vel.xy + ADVECTION_TURBULENCE * turb - ADVECTION_CONFINEMENT * confine + ADVECTION_DIVERGENCE * div);
	        
	    	div = diff(uv + tx * DIVERGENCE_LOOKAHEAD * offset);
	        
	    	lapl = vector_laplacian(uv + tx * LAPLACIAN_LOOKAHEAD * offset);
	    	
	    	delta_v += VELOCITY_LAPLACIAN * lapl + VELOCITY_TURBULENCE * turb + VELOCITY_CONFINEMENT * confine - DAMPING * vel.xy - DIVERGENCE_MINIMIZATION * div;
	    
	        for (int i = 0; i < ADVECTION_STEPS; i++) {
	            adv += N((float(i+1) / float(ADVECTION_STEPS)) * tx * offset);   
	        }   
	        adv /= float(ADVECTION_STEPS);
	    #endif
	    
	
	    
	    // define a pump, either at the center of the screen,
	    // or alternating at the sides of the screen.
	    vec2 pq = 2.0*(uv*2.0-1.0) * vec2(1,tx.x/tx.y);
	    #ifdef CENTER_PUMP
	    	vec2 pump = sin(PUMP_CYCLE*TIME)*PUMP_SCALE*pq.xy / (dot(pq,pq)+0.01);
	    #else
	    	vec2 pump = vec2(0);
	    	#define AMP 15.0
	    	#define SCL -50.0
	        float uvy0 = exp(SCL*pow(pq.y,2.0));
	        float uvx0 = exp(SCL*pow(uv.x,2.0));
	        pump += -AMP*vec2(max(0.0,cos(PUMP_CYCLE*TIME))*PUMP_SCALE*uvx0*uvy0,0);
	    
	    	float uvy1 = exp(SCL*pow(pq.y,2.0));
	        float uvx1 = exp(SCL*pow(1.0 - uv.x,2.0));
	        pump += AMP*vec2(max(0.0,cos(PUMP_CYCLE*TIME + 3.1416))*PUMP_SCALE*uvx1*uvy1,0);
	
	        float uvy2 = exp(SCL*pow(pq.x,2.0));
	        float uvx2 = exp(SCL*pow(uv.y,2.0));
	        pump += -AMP*vec2(0,max(0.0,sin(PUMP_CYCLE*TIME))*PUMP_SCALE*uvx2*uvy2);
	    
	    	float uvy3 = exp(SCL*pow(pq.x,2.0));
	        float uvx3 = exp(SCL*pow(1.0 - uv.y,2.0));
	        pump += AMP*vec2(0,max(0.0,sin(PUMP_CYCLE*TIME + 3.1416))*PUMP_SCALE*uvx3*uvy3);
	    #endif
	    
	    gl_FragColor = mix(adv + vec4(VELOCITY_SCALE * (delta_v + pump), offset), init, UPDATE_SMOOTHING);
	    
	    if (iMouse.z > 0.0) {
	        vec4 mouseUV = iMouse / RENDERSIZE.xyxy;
	        vec2 delta = normz(mouseUV.zw - mouseUV.xy);
	        vec2 md = (mouseUV.xy - uv) * vec2(1.0,tx.x/tx.y);
	        float amp = exp(max(-12.0,-dot(md,md)/MOUSE_RADIUS));
	        gl_FragColor.xy += VELOCITY_SCALE * MOUSE_AMP * clamp(amp * delta,-1.0,1.0);
	    }
	    
	    // Adding a very small amount of noise on init fixes subtle numerical precision blowup problems
	    if (FRAMEINDEX==0) gl_FragColor=1e-6*rand4(gl_FragCoord.xy, RENDERSIZE.xy, FRAMEINDEX);
	}
	else if (PASSINDEX == 1)	{


	    vec2 turb;
	    float curl;
	    turbulence(gl_FragCoord.xy, turb, curl);
	    gl_FragColor = vec4(turb,0,curl);
	    // Adding a very small amount of noise on init fixes subtle numerical precision blowup problems
	    if (FRAMEINDEX==0) gl_FragColor=1e-6*rand4(gl_FragCoord.xy, RENDERSIZE.xy, FRAMEINDEX);
	}
	else if (PASSINDEX == 2)	{


	    gl_FragColor = vec4(confinement(gl_FragCoord.xy),0,0);
	    // Adding a very small amount of noise on init fixes subtle numerical precision blowup problems
	    if (FRAMEINDEX==0) gl_FragColor=1e-6*rand4(gl_FragCoord.xy, RENDERSIZE.xy, FRAMEINDEX);
	}
	else if (PASSINDEX == 3)	{


	
	    vec2 p = pois(gl_FragCoord.xy);
	    #ifdef USE_PRESSURE_ADVECTION
	        float mip = 0.0;
	        vec2 tx = 1.0 / RENDERSIZE.xy;
	        vec2 uv = gl_FragCoord.xy * tx;
	        float prev = P(0.0002 * PRESSURE_ADVECTION * tx * V(vec2(0)));
	        gl_FragColor = vec4(mix(p.x + p.y, prev + PRESSURE_LAPLACIAN * laplacian_poisson(gl_FragCoord.xy), PRESSURE_UPDATE_SMOOTHING));
	    #else
	    	gl_FragColor = vec4(p.x + p.y);
	    #endif
	    // Adding a very small amount of noise on init fixes subtle numerical precision blowup problems
	    if (FRAMEINDEX==0) gl_FragColor=1e-6*rand4(gl_FragCoord.xy, RENDERSIZE.xy, FRAMEINDEX);
	}
	else if (PASSINDEX == 4)	{
	}
	else if (PASSINDEX == 5)	{
	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	
	    vec2 dxy = vec2(0);
	    float occ, mip = 0.0;
	    float d   = D();
	    
	    // blur the gradient to reduce appearance of artifacts,
	    // and do cheap occlusion with mipmaps
	    #define STEPS 10.0
	    #define ODIST 2.0
	    for(mip = 1.0; mip <= STEPS; mip += 1.0) {	 
	        dxy += (1.0/pow(2.0,mip)) * diff(uv, mip-1.0);	
	    	occ += softclamp(-ODIST, ODIST, d - D(),1.0)/(pow(1.5,mip));
	    }
	    dxy /= float(STEPS);
	    
	    // I think this looks nicer than using smoothstep
	    occ = pow(max(0.0,softclamp(0.2,0.8,100.0*occ + 0.5,1.0)),0.5);
	 
	    vec3 avd;
	    vec3 ld = light(uv, BUMP, 0.5, dxy, TIME, avd);
	    
	    float spec = ggx(avd, vec3(0,1,0), ld, 0.1, 0.1);
	    
	    #define LOG_SPEC 1000.0
	    spec = (log(LOG_SPEC+1.0)/LOG_SPEC)*log(1.0+LOG_SPEC*spec);    
	    
	    #define VIEW_VELOCITY
	    
	    #ifdef VIEW_VELOCITY
			vec4 diffuse = softclamp(0.0,1.0,6.0*vec4(IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).xy,0,0)+0.5,2.0);    
	    #elif defined(VIEW_CURL)
			vec4 diffuse = mix(vec4(1,0,0,0),vec4(0,0,1,0),softclamp(0.0,1.0,0.5+2.0*IMG_NORM_PIXEL(BufferB,mod(uv,1.0)).w,2.0));    
	    #elif defined(VIEW_ADVECTION)
			vec4 diffuse = softclamp(0.0,1.0,0.0004*vec4(IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).zw,0,0)+0.5,2.0); 
	    #elif defined(VIEW_GRADIENT)
	    	vec4 diffuse = softclamp(0.0,1.0,10.0*vec4(diff(uv,0.0),0,0)+0.5,4.0); 
	    #else // Vorticity confinement vectors
	    	vec4 diffuse = softclamp(0.0,1.0,4.0*vec4(IMG_NORM_PIXEL(BufferC,mod(uv,1.0)).xy,0,0)+0.5,4.0);
	    #endif
	    
	    
	    gl_FragColor = (diffuse + 4.0*mix(vec4(spec),1.5*diffuse*spec,0.3));
	    gl_FragColor = mix(1.0,occ,0.7) * (softclamp(0.0,1.0,contrast(gl_FragColor,4.5),3.0));
	    
	    //gl_FragColor = vec4(occ);
	    //gl_FragColor = vec4(spec);
	    //gl_FragColor = diffuse;
	    //gl_FragColor = vec4(diffuse+(occ-0.5));
	}

}
