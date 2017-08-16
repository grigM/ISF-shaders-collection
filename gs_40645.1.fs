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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40645.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



#define PI  3.14159265358979323846
#define TAU 6.28318530717958647692

float random(in float val) {
    return fract(sin(val * 12.9898) * 43758.5453123);
}

float random(in vec2 st) {
    return fract(sin(dot(st.xy, vec2(12.9898, 78.233))) * 43758.5453123);
}

vec2 random2(in float val){
    vec2 st = vec2( dot(vec2(val), vec2(127.1, 311.7)), dot(vec2(val), vec2(269.5, 183.3)) );
    return -1.0 + 2.0 * fract(sin(st) * 43758.5453123);
}

vec2 random2(in vec2 st){
    st = vec2( dot(st, vec2(127.1, 311.7)), dot(st, vec2(269.5, 183.3)) );
    return -1.0 + mouse.xy * fract(sin(st) * 43758.5453123);
}

void main( void )
{
    float d = min(RENDERSIZE.x, RENDERSIZE.y);
    vec2 st = (gl_FragCoord.xy - (RENDERSIZE.xy / 2.0)) / d;
    vec2 p =  (mouse.xy - (RENDERSIZE.xy / 2.0)) / d;
    vec3 color = vec3(0.0);
    float t = TIME;
    
    vec3 m_dist = vec3(0.0);
    const int n = 10;
    for(int i=0; i<n; i++){
        vec2 pos = random2(vec2(float(i), 9.0)) *.06;
        float angle  = mix(0.0,  TAU, random(float(i)));
        float radius = mix(0.01, 0.04, random(float(i)));
        float freq = 1.0;
        if(random(float(i)) > 0.5){
            freq = -1.0;
        }
        vec2 offset = radius*vec2(sin(freq*t + angle), cos(freq*t + angle));

        m_dist.r += 1.0 / distance(st + vec2(0.00, 0.00), pos + offset);
        m_dist.g += 1.0 / distance(st + vec2(0.01, 0.01), pos + offset);
        m_dist.b += 1.0 / distance(st + vec2(0.01, 0.00), pos + offset);
    }
    m_dist.r -= 2.0 / distance(st + vec2(0.00, 0.00), p);
    m_dist.g -= 2.0 / distance(st + vec2(0.01, 0.01), p);
    m_dist.b -= 2.0 / distance(st + vec2(0.05, 0.01), p);

    color += smoothstep(0.51, 0.5, m_dist * .008);
    
    gl_FragColor = vec4(color, 1.0);
}