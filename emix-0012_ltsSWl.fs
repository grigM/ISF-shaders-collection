/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ltsSWl by sm.  A prototype of one of the many overlay effects for demoscene production \"Emix\" (https:\/\/www.youtube.com\/watch?v=F415chvZhHo)",
  "INPUTS" : [

  ]
}
*/


const int Npoly = 3;
const float TWOPI = 6.283185;

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

vec3 rayPlane(vec3 p, vec3 o, vec3 n, vec3 d) {
    float dn = dot(d, n);
    float s = 1e8;
    
    if (dn != 0.0) {
        s = dot(p - o, n) / dn;
        s += float(s < 0.0) * 1e8;
    }
    return o + s * d;;
}

vec3 rayTriangle(vec3 o, vec3 ray) {
    const float R = 2.0;
    vec3 cand = o;
	float cdist = 1e10;
    float phase = 0.2 * TIME;
    
    for (int i = 0; i < Npoly; i++) {
        vec3 p = R * vec3(cos(TWOPI * float(i) / float(Npoly) + phase),
                          sin(TWOPI * float(i) / float(Npoly) + phase),
                          0.0);
        vec3 n = normalize(-p);
        vec3 rh = rayPlane(p, o, n, ray);
        float dist = length(rh - o);
        
        if (dist < cdist) {
            cand = rh;
            cdist = dist;
        }
    }
    return cand;
}

vec4 colorize(vec3 pos) {
    float c = float(mod(pos.z, 0.03) < 0.015);
    return vec4(vec3(c), 1.0);
}

vec4 trace(vec2 p) {
    vec3 ray = normalize(vec3(p, 5.0/200.0));
    vec3 campos = vec3(0.0, 0.0, 0.05*TIME);
   	vec3 hitpos = rayTriangle(campos, ray);
    vec4 col = colorize(hitpos);
    
    // fog me beautiful
    float fogfactor = exp(3.5 - 2.0 * length(hitpos - campos));
    vec4 oc =  col * min(1.0, fogfactor);
    return oc;
}

void main() {



    vec2 buv = gl_FragCoord.xy / RENDERSIZE.xy - vec2(0.5);
	vec2 uv = buv;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    gl_FragColor = trace(uv);
    for (int i = 0; i < 4; i++) {
        uv = buv + 0.25*vec2(cos(gl_FragCoord.x+gl_FragCoord.y + float(i)*TWOPI/4.0),
                             sin(gl_FragCoord.x+gl_FragCoord.y + float(i)*TWOPI/4.0)) / RENDERSIZE.xy;
	    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
        gl_FragColor += trace(uv);
    }
    gl_FragColor /= 5.0;
    gl_FragColor += 0.1*rand(gl_FragCoord.xy + 0.1*TIME);
}

