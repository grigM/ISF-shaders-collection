/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3sKXRK by mahalis.  today’s Nodevember theme is “Star”. here is a star!",
  "INPUTS" : [

  ]
}
*/


// license: CC BY-NC https://creativecommons.org/licenses/by-nc/4.0/

vec2 r(vec2 p, float a) {
    float c = cos(a);
    float s = sin(a);
    return vec2(c * p.x - s * p.y, s * p.x + c * p.y);
}

float oneArmDistance(vec2 p) {
    float d = dot(p, vec2(0.0, 1.0));
    d = max(d, -dot(vec2(abs(p.x), p.y), normalize(vec2(-1.0,0.3))) - 0.1);
    return d;
}

vec3 palette(float v) {
    return vec3(0.6,0.3,0.8) + vec3(0.2,0.5,0.2) * cos(6.28318 * (v + vec3(0.0, 0.665, 0.667)));
}

void main() {

    vec2 uv = (gl_FragCoord.xy - RENDERSIZE.xy / 2.) / RENDERSIZE.y;
    uv.y = -uv.y;
    
    uv *= (2.5 + sin(TIME * 0.23) * 0.5);
    float radius = length(uv);
    uv = r(uv, pow(radius * 0.5, 2.0) * -0.2);
    
    float d = 1e5;
    for (int i = 0; i < 5; i++) {
        float a = float(i) * 6.283 / 5.0 + TIME * 0.2;
        d = min(d, oneArmDistance(r(uv, a)));
    }
    
    float innerMask = smoothstep(fwidth(d),0.0, d);
    
    float angle = atan(uv.y, uv.x) - 0.2 * pow(radius * 0.5,2.0);
    d -= 0.05 * (sin(angle * 11. + TIME * 1.1 + radius * 0.3) + 0.5 * cos(angle * 19. + TIME * 1.31 + radius * 0.4)) * max(radius - 0.3, 0.);
    
    float outerDistance = floor(d * 15.0 - TIME * 0.3);
    
    vec3 outerColor = clamp(palette(outerDistance * 0.2 - TIME * 0.1 + angle / 3.1416), 0.0, 1.0);
    
    vec3 c = mix(pow(outerColor, vec3(1.4)) * 1.2, vec3(0.0), innerMask);
    gl_FragColor = vec4(vec3(c),1.0);
}
