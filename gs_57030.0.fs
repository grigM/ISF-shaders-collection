/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57030.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/Wl2SWG
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE
const vec4 iMouse = vec4(0.0);

// --------[ Original ShaderToy begins here ]---------- //
// Storming Pixels
// inspired by Inigo Quilez live stream shader deconstruction
// Leon Denise (ponk) 2019.08.28
// Licensed under hippie love conspiracy

// Using code from
// Inigo Quilez
// Morgan McGuire

// tweak zone
const float range = 1.;
const float radius = .35;
const float blend = .7;
const float balance = 1.9;
const float falloff = 1.8;

// increment it at your own GPU risk
const float motion_frames = 1.;

// tool box
float random(vec2 p) { return fract(1e4 * sin(17.0 * p.x + p.y * 0.1) * (0.1 + abs(sin(p.y * 13.0 + p.x)))); }
mat2 rot(float a) { float c=cos(a),s=sin(a); return mat2(c,-s,s,c); }
float smoothmin (float a, float b, float r) { float h = clamp(.5+.5*(b-a)/r, 0., 1.); return mix(b, a, h)-r*h*(1.-h); }
vec3 look (vec3 eye, vec3 target, vec2 anchor, float fov) {
    vec3 forward = normalize(target-eye);
    vec3 right = normalize(cross(forward, vec3(0,1,0)));
    vec3 up = normalize(cross(right, forward));
    return normalize(forward * fov + right * anchor.x + up * anchor.y);
}

float geometry (vec3 pos, float TIME) {
    float a = 1.0;
    float scene = 1.;
    float t = TIME + pos.x / 4.;
    t = floor(t)+pow(fract(t),.5);
    for (int i = 6; i > 0; --i) {
        pos.xy *= rot(cos(t)*balance/a+a*2.);
        pos.zy *= rot(sin(t)*balance/a+a*2.);
        pos.xz = abs(pos.xz)-range*a;
        scene = smoothmin(scene, length(pos)-radius*a, blend*a);
        a /= falloff;
    }
    return scene;
}

float raymarch ( vec3 eye, vec3 ray, float TIME, out float total ) {
    float dither = random(ray.xy+fract(iTime));
    total = 0.0;
    const int count = 30;
    for (int index = count; index > 0; --index) {
        float dist = geometry(eye+ray*total,TIME);
        dist *= 0.9+0.1*dither;
        total += dist;
        if (dist < 0.001 * total)
            return float(index)/float(count);
    }
    return 0.;
}

vec3 camera (vec3 eye) {
    vec2 mouse = iMouse.xy/iResolution.xy*2.-1.;
    if (iMouse.z > 0.5) {
        eye.yz *= rot(mouse.y*3.1415);
        eye.xz *= rot(mouse.x*3.1415);
    } else {
        eye = vec3(1,3.5,1.);
    }
    return eye;
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec2 uv = 2.*(fragCoord-0.5*iResolution.xy)/iResolution.y;
    vec3 eye = camera(vec3(0,0,3.5));
    vec3 ray = look(eye, vec3(0), uv, 2.);
    float total = 0.;
    fragColor = vec4(0);
    for (float index = motion_frames; index > 0.; --index) {
        float dither = random(ray.xy+fract(iTime+index));
        float TIME = iTime+(dither+index)/10./motion_frames;
        fragColor += vec4(raymarch(eye, ray, TIME,total))/motion_frames;
    }
    
    // extra color
    float gray = fragColor.r;
    fragColor.rgb *= vec3(.8,.8,.9);
    float d = smoothstep(0.0,2.,length(uv));
    vec3 sky = mix(vec3(0.35,.37,.4), vec3(0.), d);
    d = smoothstep(5.,0.,total);
    fragColor.rgb += vec3(0.8,.7,.6) * d;
    fragColor.rgb = mix(sky.rgb, fragColor.rgb, pow(fragColor.r,.3));
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
    gl_FragColor.a = 1.0;
}