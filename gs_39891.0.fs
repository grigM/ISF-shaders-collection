/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39891.0"
}
*/



// Author @kyndinfo - 2016
// http://www.kynd.info
// Title: Wipes



#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



#define PI 3.14159265359
#define TWO_PI 6.28318530718
#define AQUA vec3(0.3, 1.0, 1.0)
#define SUNFLOWER vec3(1.0, 1.0, 0.6)
#define SWEETPEA vec3(1.0, 0.7, 0.75)
#define NAVY vec3(0.0, 0.1, 0.2)
#define TURQUOISE vec3(0.0, 1.0, 0.7)


float easeInOutCubic(float t) {
    if ((t *= 2.0) < 1.0) {
        return 0.5 * t * t * t;
    } else {
        return 0.5 * ((t -= 2.0) * t * t + 2.0);
    }
}

float linearstep(float begin, float end, float t) {
    return clamp((t - begin) / (end - begin), 0.0, 1.0);
}

float clockWipe(vec2 p, float t) {
    float a = atan(-p.x, -p.y);
    float v = (t * TWO_PI > a + PI) ? 1.0 : 0.0;
    return v;
}

float smoothedge(float v, float f) {
    return smoothstep(0.0, f / RENDERSIZE.x, v);
}

float circle(vec2 p, float radius) {
  return length(p) - radius;
}

float circlePlot(vec2 p, float radius) {
  return 1.0 - smoothedge(circle(p, radius), 1.0);
}

void main(){
    vec2 st = gl_FragCoord.xy / RENDERSIZE.xy;
    float t = mod(TIME, 4.0), v;

    vec3 color = NAVY;
    float v0 = 1.0 - step(easeInOutCubic(linearstep(0.0, 0.7, t)), st.x);
    float v1 = 1.0 - step(easeInOutCubic(linearstep(0.3, 1.0, t)), st.x);
    color = mix(color, TURQUOISE, v0 - v1);

    float v2 = 1.0 - step(easeInOutCubic(linearstep(1.0, 1.7, t)), st.y);
    float v3 = 1.0 - step(easeInOutCubic(linearstep(1.3, 2.0, t)), st.y);
    color = mix(color, SUNFLOWER, v2 - v3);

   	float v4 = circlePlot(st - vec2(0.5), easeInOutCubic(linearstep(2.0, 2.7, t)));
    float v5 = circlePlot(st - vec2(0.5), easeInOutCubic(linearstep(2.3, 3.0, t)));
    color = mix(color, SWEETPEA, v4 - v5);

   	float v6 = clockWipe(st - vec2(0.5), easeInOutCubic(linearstep(3.0, 3.6, t)));
    float v7 = clockWipe(st - vec2(0.5), easeInOutCubic(linearstep(3.4, 4.0, t)));
    color = mix(color, AQUA, v6 - v7);

    gl_FragColor = vec4(color, 1.0);
}