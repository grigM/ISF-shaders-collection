/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39879.0"
}
*/



// Author: Patricio Gonzalez Vivo
// Title: IChing


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



#define PI 3.14159265359
#define TWO_PI 6.28318530718


float random (in vec2 _st) { 
    return fract(sin(dot(_st.xy, vec2(12.9898,78.233))) * 43758.5453123);
}

float rect(vec2 _st, vec2 _size){
    _size = vec2(0.5)-_size*0.5;
    vec2 uv = smoothstep(_size,_size+vec2(1e-4),_st);
    uv *= smoothstep(_size,_size+vec2(1e-4),vec2(1.0)-_st);
    return uv.x*uv.y;
}

float box(vec2 st, vec2 size){
    return 1.-rect(st,size);
}

float hex(vec2 st, float t){
    st = st*vec2(2.,6.);
    vec2 fpos = fract(st);
    vec2 ipos = floor(st);
    
    if (ipos.x == 1.0) {
        fpos.x = 1.-fpos.x;
    }
    fpos.x += .2;
    
    if (ipos.y < 0. || ipos.y > 5. || 
        ipos.x < 0. || ipos.x > 1. ) {
        return 1.;
    }
    
    float value = random(vec2(ipos.y, floor(t)));
    value = step(.5,value);
    
    return 1.0- mix(rect(fpos, vec2(1.,.7)),
            		rect(fpos, vec2(1.5,.7)),
               		value);
}

void main(){
    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
    st = (st-.5)*2.328+.5;
    st.x *= RENDERSIZE.x/RENDERSIZE.y;
    st.x -= (RENDERSIZE.x*.5-RENDERSIZE.y*.5)/RENDERSIZE.y;
    
    float t = TIME*.01;
    float df = mix(hex(st,t), hex(st,t+1.), fract(t));

    gl_FragColor = vec4(mix(vec3(0.),vec3(1.),step(0.7,df)),1.0);
}