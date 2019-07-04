/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#52180.1"
}
*/


// very nice shader !!!
//precision highp float;


#define PI 3.14159265359

float random(vec2 x){
    return fract(sin(dot(x,vec2(12.9898, 78.233))) * 43758.5453);
}

mat2 rotate(float r) {
    float c = cos(r);
    float s = sin(r);
    return mat2(c, s , -s, c);
}

float circle(vec2 p, float r) {
    return length(p) - r;
}

float rect(vec2 p, vec2 r) {
    p = abs(p) - r;
    return length(max(p, 0.0)) + min(max(p.x, p.y), 0.0);
}

void main(void) {
    vec2 uv = (2.0 * gl_FragCoord.xy - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);


    if (circle(uv, 0.5) < 0.0) {
        uv *= rotate(PI * 0.25);
        uv *= -1.0;
     
    }

    vec3 c = mix(
        vec3(0.0),
        vec3(0.7),
        step(smoothstep(-0.5, 0.5, uv.x), random(uv * (TIME*0.00002)))
    );

    gl_FragColor = vec4(c, 1.0);
}