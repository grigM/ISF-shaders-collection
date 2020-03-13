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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57029.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/tlBXWK
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
// Storming Cubes
// inspired by Inigo Quilez live stream shader deconstruction
// Leon Denise (ponk) 2019.08.28
// Licensed under hippie love conspiracy

// Using code from
// Inigo Quilez
// Morgan McGuire

// tweak zone
const float range = .4;
const float radius = .065;
const float blend = .7;
const float balance = 2.0;
const float falloff = 1.5;
const int count = 5;

// increment it at your own GPU risk
const float motion_frames = 10.;

// tool box
const float TAU = 6.283;
float random(vec2 p) { return fract(1e4 * sin(17.0 * p.x + p.y * 0.1) * (0.1 + abs(sin(p.y * 13.0 + p.x)))); }
mat2 rot(float a) { float c=cos(a),s=sin(a); return mat2(c,-s,s,c); }
float smoothmin (float a, float b, float r) { float h = clamp(.5+.5*(b-a)/r, 0., 1.); return mix(b, a, h)-r*h*(1.-h); }
float sdBox (vec3 p, vec3 b) { vec3 d = abs(p) - b; return min(max(d.x,max(d.y,d.z)),0.0) + length(max(d,0.0)); }
vec3 look (vec3 eye, vec3 target, vec2 anchor, float fov) {
    vec3 forward = normalize(target-eye);
    vec3 right = normalize(cross(forward, vec3(0,1,0)));
    vec3 up = normalize(cross(right, forward));
    return normalize(forward * fov + right * anchor.x + up * anchor.y);
}

float geometry (vec3 pos, float TIME) {
    float a = 1.0;
    float scene = 1.;
    float t = TIME * .5;
    float ft = smoothstep(0.,.9,pow(fract(t),.7));
    t = floor(t)+ft;
    float w = sin(ft*3.1415);
    for (int i = count; i > 0; --i) {
        pos.xy *= rot(cos(t)*balance/a+a*2.);
        pos.zy *= rot(sin(t)*balance/a+a*2.);
        pos = abs(pos)-(range+w*.2)*a;
        
        a /= falloff;
    }
    scene = sdBox(pos, vec3(radius*(1.1-w)));
    return scene;
}

float raymarch ( vec3 eye, vec3 ray, float TIME, out float total ) {
    total = 0.0;
    const int count = 10;
    for (int index = count; index > 0; --index) {
        float dist = geometry(eye+ray*total,TIME);
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
    fragColor = vec4(pow(fragColor.r, .3));
    
    // extra color
    fragColor.rgb *= vec3(.8,.85,.8);
    float d = smoothstep(4.,0.,total);
    fragColor.rgb += vec3(0.9,.5,.4) * d;
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
    gl_FragColor.a = 1.0;
}