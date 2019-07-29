/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#55915.1"
}
*/


// original version by aadebdeb
// https://neort.io/art/bjl6pok3p9f9psc9pdjg

#ifdef GL_ES
precision mediump float;
#endif


#define BPM 120.0

const float GRID_SPACING = 5.0;
const vec3 WALL_SIZE = vec3(10.0, 5.0, 10.0) * GRID_SPACING + 0.5 * GRID_SPACING;

#define LIGHT_BLUE vec3(0.05, 0.3, 1.2)
#define LIGHT_ORANGE vec3(1.2, 0.15, 0.05)

float timeToBeat(float TIME) {
    return TIME / 60.0 * BPM;
}

// 1 bar = 4 beat
float timeToBar(float TIME) {
    return timeToBeat(TIME * 0.25);
}

float random(float x){
    return fract(sin(x * 12.9898) * 43758.5453);
}

float random(vec4 x){
    return fract(sin(dot(x,vec4(12.9898, 78.233, 39.425, 27.196))) * 43758.5453);
}

mat2 rotate(float r) {
    float c = cos(r);
    float s = sin(r);
    return mat2(c, s, -s, c);
}

float usin(float x) {
    return sin(x) * 0.5 + 0.5;
}

void setNormalEnvironmentStatus(out vec3 color0, out vec3 color1, out float borderEdge0, out float borderEdge1, out ivec3 lines) {
    color0 = vec3(0.75);
    color1 = vec3(0.0, 0.075, 0.185);
    borderEdge0 = 0.01;
    borderEdge1 = 0.05;
    lines = ivec3(1, 1, 1);
}

void setUnlightedEnvironmentStatus(out vec3 color0, out vec3 color1, out float borderEdge0, out float borderEdge1, out ivec3 lines) {
    color0 = vec3(0.0);
    color1 = vec3(0.0);
    borderEdge0 = 0.0;
    borderEdge1 = 0.0;
    lines = ivec3(0, 0, 0);
}

void setLightBorderEdge(out float borderEdge0, out float borderEdge1) {
    borderEdge0 = 0.0;
    borderEdge1 = 0.1;
}

void setEnvironmentStatus(vec3 pos, int hit, out vec3 color0, out vec3 color1, out float borderEdge0, out float borderEdge1, out ivec3 lines) {
    float beat = timeToBeat(TIME);
    beat = mod(beat, 32.0);
    float bar = timeToBar(TIME);

    if (beat < 8.0) {
        if (beat < 7.0) {
            setNormalEnvironmentStatus(color0, color1, borderEdge0, borderEdge1, lines);
            return;
        } else {
            float r = random(ceil(fract(beat) * 20.0) / 20.0);
            if (r > fract(beat)) {
                setNormalEnvironmentStatus(color0, color1, borderEdge0, borderEdge1, lines);
                return;
            } else {
                setUnlightedEnvironmentStatus(color0, color1, borderEdge0, borderEdge1, lines);
                return;
            }
        }
    } else if (beat < 16.0) {
        if (hit == 0 || hit == 2) {
            color0 = vec3(0.0);
            color1 = mix(LIGHT_ORANGE, LIGHT_BLUE, usin(pos.y * 0.05 + TIME * 10.0));
            setLightBorderEdge(borderEdge0, borderEdge1);
            lines = ivec3(0, 1, 0);
            return;
        } else { 
            setUnlightedEnvironmentStatus(color0, color1, borderEdge0, borderEdge1, lines);
            return;
        }
    } else if (beat < 24.0) {
        if (hit == 0 || hit == 1) {
            color0 = vec3(0.0);
            float end0 = WALL_SIZE.z - max(fract(bar) * 2.0 - 1.0, 0.0) * 2.0 * WALL_SIZE.z;
            float end1 = WALL_SIZE.z - min(fract(bar) * 2.0, 1.0) * 2.0 * WALL_SIZE.z;
            color1 = mix(LIGHT_ORANGE, LIGHT_BLUE, smoothstep(-WALL_SIZE.z, WALL_SIZE.z, pos.z));
            color1 = mix(vec3(0.0), color1, (1.0 - step(end0, pos.z)) * step(end1, pos.z));
            setLightBorderEdge(borderEdge0, borderEdge1);
            lines = ivec3(0, 0, 1);
            return;
        } else { 
            setUnlightedEnvironmentStatus(color0, color1, borderEdge0, borderEdge1, lines);
            return;
        }
    } else if (beat < 32.0) {
        if (beat < 31.0) {
            color0 = vec3(0.0);
            color1 = mix(LIGHT_ORANGE, LIGHT_BLUE, usin(pos.y * 0.05 - TIME * 8.0));
            setLightBorderEdge(borderEdge0, borderEdge1);
            lines = ivec3(1, 1, 1);
            return;
        } else {
            float r = random(ceil(fract(beat) * 20.0) / 20.0);
            if (r < fract(beat)) {
                setNormalEnvironmentStatus(color0, color1, borderEdge0, borderEdge1, lines);
                return;
            } else {
                setUnlightedEnvironmentStatus(color0, color1, borderEdge0, borderEdge1, lines);
                return;
            }
        }
    }
}

vec3 environment(vec3 ro, vec3 rd) {
    float t = 1e6;
    int hit = 0;
    for (int i = 0; i < 3; i++) {
        if (rd[i] != 0.0) {
            float tn = (-WALL_SIZE[i] - ro[i]) / rd[i];
            if (tn > 0.0 && tn < t) {
                t = tn;
                hit = i;
            }
            float tp = (WALL_SIZE[i] - ro[i]) / rd[i];
            if (tp > 0.0 && tp < t) {
                t = tp;
                hit = i;
            }
        }
    }
    if (t == 1e6) {
        return vec3(0.0);
    }
    vec3 p = ro + t * rd;

    vec3 color0, color1;
    float borderEdge0, borderEdge1; // 0 <= borderEdge0 <= birderEdge1 <= 0.5
    ivec3 lines;
    setEnvironmentStatus(p, hit, color0, color1, borderEdge0, borderEdge1, lines);

    float v = 0.0;
    for (int i = 0; i < 3; i++) {
        if (lines[i] == 1) {
            v = max(v, smoothstep(borderEdge1, borderEdge0, 0.5 - abs(fract(p[i] / GRID_SPACING) - 0.5)));
        }
    }

    return mix(color0, color1, v);
}

float smoothUnion(float d1, float d2, float k) {
    float h = clamp(0.5 + 0.5 * (d2 - d1) / k, 0.0, 1.0);
    return mix(d2, d1, h) - k * h * (1.0 - h);
}

float random(float x, inout float seed) {
    seed += 0.1;
    return random(x + seed);
}

float map(vec3 p) {
    float d = 1e6;
    float seed = 0.0;
    for (float i = 1.0; i <=5.0; i += 1.0) {
        vec3 c = vec3(20.0 * sin(TIME + 100.0 * random(i, seed)), 0.0, 0.0);
        c.xy *= rotate(TIME * random(i, seed) + 100.0 * random(i, seed));
        c.xz *= rotate(TIME * random(i, seed) + 100.0 * random(i, seed));
        float r = mix(1.0, 10.0, smoothstep(0.0, 1.0, random(i, seed)));
        d = smoothUnion(d, length(p - c) - r, 6.0);
    }
    return d;
}

vec3 calcNormal(vec3 p) {
    float d = 0.01;
    return normalize(vec3(
        map(p + vec3(d, 0.0, 0.0)) - map(p - vec3(d, 0.0, 0.0)),
        map(p + vec3(0.0, d, 0.0)) - map(p - vec3(0.0, d, 0.0)),
        map(p + vec3(0.0, 0.0, d)) - map(p - vec3(0.0, 0.0, d))
    ));
}

vec3 schlickFresnel(vec3 f90, float cosine) {
    return f90 + (1.0 - f90) * pow(1.0 - cosine, 5.0);
}

vec3 raymarch(vec3 ro, vec3 rd) {
    vec3 p = ro;
    for (int i = 0; i < 24; i++) {
        float d = map(p);
        p += d * rd;
        if (d < 0.5) {
            vec3 n = calcNormal(p);
            vec3 reflectDir = reflect(rd, n);
            float dotNR = max(0.0, dot(n, reflectDir));
            vec3 specColor = vec3(0.001);
            vec3 fresnel = schlickFresnel(specColor, dotNR);
            return fresnel * environment(p, reflectDir);
        }
    }
    return environment(ro, rd);
}

void main(void) {
    vec2 st = (2.0 * gl_FragCoord.xy - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);

    vec3 ro = vec3(20.0, -10.0, 38.0);
    vec3 ta = vec3(0.0);
    vec3 z = normalize(ta - ro);
    vec3 up = vec3(0.0, 1.0, 0.0);
    vec3 x = normalize(cross(z, up));
    vec3 y = normalize(cross(x, z));
    vec3 rd = normalize(x * st.x + y * st.y + z * 1.5);

    vec3 c = raymarch(ro, rd);

    gl_FragColor = vec4(pow(c, vec3(1.0 / 2.2)), 1.0);
}