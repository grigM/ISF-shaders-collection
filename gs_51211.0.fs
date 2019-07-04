/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#51211.0"
}
*/


/*
 * by @aa_debdeb (https://twitter.com/aa_debdeb)
 * 2018/12/24
 */

//precision highp float;


float random(vec3 p){
    return fract(sin(dot(p,vec3(12.9898, 78.233, 29.425))) * 43758.5453);
}

float sphere(vec3 p, float r) {
    return length(p) - r;
}

vec3 repeat(vec3 p, vec3 m) {
    return mod(p, m) - 0.5 * m;
}

float map(vec3 p) {
    float rep = 10.0;
    vec3 idx = floor(p / rep);
    p = repeat(p, vec3(rep));

    vec3 offset = vec3(
        sin(random(idx * 0.32) * TIME * 7.43),
        sin(random(idx * 0.19) * TIME * 6.19),
        sin(random(idx * 0.43) * TIME * 8.32)
    );

    p += 0.08 * rep * offset;

    float d = random(idx * 0.1) < 0.05 ? sphere(p, 0.75) : rep * 0.5;
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

vec3 materialColor = vec3(1.5, 0.9, 1.1);

vec3 raymarch(vec3 ro, vec3 rd) {
    vec3 p = ro;
    float minD = 10000.0;
    for (int i = 0; i < 128; i++) {
        float d = map(p);
        p += d * rd;
        minD = min(d, minD);
        if (d < 0.01) {
            return materialColor;
        }
    }

    float atten = exp(-minD * 2.0);
    return materialColor * atten;
}

void main(void) {
    vec2 st = (2.0 * gl_FragCoord.xy - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);

    float d = TIME * 50.0;
    vec3 ro = vec3(0.0, 0.0, -5.0 + d);
    vec3 ta = vec3(0.0, 0.0, d);
    vec3 z = normalize(ta - ro);
    vec3 up = vec3(0.0, 1.0, 0.0);
    vec3 x = normalize(cross(z, up));
    vec3 y = normalize(cross(x, z));
    vec3 rd = normalize(x * st.x + y * st.y + z * 1.5);

    vec3 c = raymarch(ro, rd);

    gl_FragColor = vec4(c, 1.0);
}