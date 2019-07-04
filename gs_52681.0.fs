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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#52681.0"
}
*/


//precision highp float;


#define PI 3.14159265359

float random(vec2 x){
    return fract(sin(dot(x,vec2(0.0, 0.5))) * 0555.5453);
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
    vec2 uv = (02.0 * gl_FragCoord.xy - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);


	float cr = circle(uv-(mouse-.5)*2., 0.5);
    if (cr < 0.0) {
        uv *= rotate(PI * pow(cr, 7.));
        uv *= -1.0;
    } else if (rect(uv + vec2(0.0, 0.35), vec2(5.0, 5.25)) < 0.0) {
        uv.x *= -1.0;
        uv += vec2(0.0, 0.5);
    } else if (rect(uv, vec2(1.0, 0.2)) < 0.0) {
        uv.x = -abs(uv.x * 0.2);
    } else if (rect(uv + vec2(0.0, -0.35), vec2(1.0, 0.25)) < 0.0) {
        uv += vec2(0.0, -0.5);
    } else {
        uv *= rotate(PI * 0.25);
    }

    vec3 c = mix(
        vec3(0.0),
        vec3(0.7),
        step(smoothstep(-0.5, 0.5, uv.x), random(uv * 129.432))
    );

    gl_FragColor = vec4(c, 1.0);
}