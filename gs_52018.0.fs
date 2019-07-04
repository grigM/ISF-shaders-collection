/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#52018.0"
}
*/


//precision highp float;


#define PI 3.14159265359
#define INV_PI 0.31830988618
#define TAU 6.28318530718

float box(vec3 p, vec3 b) {
    p = abs(p) - b;
    return length(max(p, 0.0)) + min(0.0, max(p.x, max(p.y, p.z)));
}

mat2 rotate(float r) {
    float c = cos(r);
    float s = sin(r);
    return mat2(c, s, -s, c);
}

float map(vec3 p) {

    float d = 10000.0;
    for (float i = 0.0; i < 20.0; i++) {
        p.xz *= rotate(TIME * 0.13 + 0.84);
        p.yx *= rotate(TIME * 0.19 + 1.43);
        vec3 q = p;
        q.xy = vec2(abs(q.x), sign(q.x) * q.y);
        q -= vec3(1.0, 0.0, 0.0);
        q.yz *= rotate(TIME * 0.45);
        d = min(d, box(q, vec3(0.05, 0.05, 2.0)));
    }
    return d;

    p.xy = vec2(abs(p.x), sign(p.x) * p.y);

    p -= vec3(1.0, 0.0, 0.0);
    p.yz *= rotate(0.5);

    return box(p, vec3(0.3, 0.3, 2.0));
}

vec3 calcNormal(vec3 p) {
    float d = 0.01;
    return normalize(vec3(
        map(p + vec3(d, 0.0, 0.0)) - map(p - vec3(d, 0.0, 0.0)),
        map(p + vec3(0.0, d, 0.0)) - map(p - vec3(0.0, d, 0.0)),
        map(p + vec3(0.0, 0.0, d)) - map(p - vec3(0.0, 0.0, d))
    ));
}

vec3 diffuseColor = vec3(0.75);
vec3 specularColor = vec3(0.8, 0.9, 1.0);
vec3 lightColor = vec3(1.0) * 7.0;
vec3 lightDir = normalize(vec3(-0.5, 0.3, -1.0));

vec3 raymarch(vec3 ro, vec3 rd) {
    vec3 p = ro;
    for (int i = 0; i < 64; i++) {
        float d = map(p);
        p += d * rd;
        if (d < 0.01) {
            float ao = 1.0 - 0.5 * float(i) / 64.0;
            vec3 n = calcNormal(p);
            vec3 diffuse = diffuseColor * max(0.0, dot(n, lightDir)) * INV_PI;
            vec3 specular = specularColor * pow(max(0.0, dot(reflect(rd, n), lightDir)), 32.0) * TAU / (32.0 + 1.0);
            return lightColor * mix(diffuse, specular, 0.5) * ao;
        }
    }
    return mix(vec3(0.8), vec3(0.2), abs(rd.y));
}

void main(void) {
    vec2 st = (2.0 * gl_FragCoord.xy - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);

    vec3 ro = vec3(0.0, 0.0, -5.0);
    vec3 ta = vec3(0.0);
    vec3 z = normalize(ta - ro);
    vec3 up = vec3(0.0, 1.0, 0.0);
    vec3 x = normalize(cross(z, up));
    vec3 y = normalize(cross(x, z));
    vec3 rd = normalize(x * st.x + y * st.y + z * 1.5);

    vec3 c = raymarch(ro, rd);

    gl_FragColor = vec4(c, 1.0);
}