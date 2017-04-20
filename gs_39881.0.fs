/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39881.0"
}
*/


// Author: Patricio Gonzalez Vivo
// Title: Ikeda Tribute


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable




float random (in float x) {
    return fract(sin(x)*1e4);
}

float random (in vec2 st) { 
    return fract(sin(dot(st.xy, vec2(12.9898,78.233)))* 43758.5453123);
}

float randomSerie(float x, float freq, float t) {
    return step(.8,random( floor(x*freq)-floor(t) ));
}

void main() {
    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
    st.x *= RENDERSIZE.x/RENDERSIZE.y;
    
    vec3 color = vec3(0.0);

    float cols = 2.;
    float freq = random(floor(TIME))+abs(atan(TIME)*0.1);
    float t = 60.+TIME*(1.0-freq)*30.;

    if (fract(st.y*cols* 0.5) < 0.5){
        t *= -1.0;
    }

    freq += random(floor(st.y));

    float offset = 0.025;
    color = vec3(randomSerie(st.x, freq*100., t+offset),
                 randomSerie(st.x, freq*100., t),
                 randomSerie(st.x, freq*100., t-offset));

    gl_FragColor = vec4(1.0-color,1.0);
}