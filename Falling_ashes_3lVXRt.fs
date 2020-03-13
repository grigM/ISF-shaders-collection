/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3lVXRt by ilyaev.  Falling particles via UV cells",
  "INPUTS" : [

  ]
}
*/


//precision mediump float;
#define PI 3.14159265359
#define speed 1.

float Hash21(vec2 p) {
    p = fract(p * vec2(233.34, 856.43));
    p += dot(p, p + 32.57);
    return fract(p.x * p.y);
}

mat2 Rot(float angle) {
    float c = cos(angle);
    float s = sin(angle);
    return mat2(vec2(c,-s), vec2(s,c));
}


vec3 Grid(vec2 ouv) {
    vec3 col = vec3(0.);
    float cols = 16.;
    float rows = cols;

    vec2 nuv = ouv * cols + vec2(0., TIME * speed);
    vec2 cell = floor(nuv);
    vec2 uv = fract(nuv);

    uv -= .5;

    for(int x = -1 ; x <= 1 ; x++) {
        for(int y = -1 ; y <= 1 ; y++) {
            vec2 offset = vec2(float(x), float(y));
            vec2 ocell = cell + offset;
            float h = Hash21(ocell);
            vec2 center = vec2(fract(h*123.22)*0.5 - .25, fract(h*1123.323)*.5 - .25) + vec2(sin(ocell.x*ocell.y + TIME)*.4, 0.);
            float size = h*0.2 * sin(TIME*2. + (cell.y)*h) * 0.2;
            float d = smoothstep(.1,1.,.0 + size/distance(uv - offset, center));
            col += d * sin(vec3(ocell.y + TIME, 0., 0.));
        }
    }

    return col;
}

void main() {

    vec2 uv = (gl_FragCoord.xy - 0.5 * RENDERSIZE.xy) / RENDERSIZE.y + .5;
    vec3 col = vec3(0.);
    vec3 l1 = Grid(uv);
    vec3 l2 = Grid(((uv - vec2(.5, 1.)) / 2. ) * Rot(3.14/16.*sin(TIME/2.)) + vec2(4.3,322.));
    vec3 l3 = Grid(((uv - vec2(.5,1.)) / 4. ) * Rot(3.14/16.*sin(TIME)) + vec2(443.,233.44));
    col = max(l1, l2);
    col = max(col ,l3);
    gl_FragColor = vec4(col, 1.);
}
