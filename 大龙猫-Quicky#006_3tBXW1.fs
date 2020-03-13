/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/3tBXW1 by totetmatt.  Neon light remembers me of Macau Night",
    "IMPORTED": {
    },
    "INPUTS": [
    ]
}

*/


#define time TIME
#define clock time/5.
void main() {



    vec2 uv =  ( gl_FragCoord.xy -.5* RENDERSIZE.xy ) / RENDERSIZE.y;
    
    uv.x += tan(abs(uv.x)*5.);
    
    float d = fract(10.*uv.x+clock);
    d = smoothstep(0.2,0.10,d);
    
    float p = tan(uv.x+time);
    vec3 col = mix(vec3(0.1,0.1,0.1),vec3(p*0.1+step(.1,abs(cos(clock*2.))*uv.x),p*0.9,0.1+step(1.,sin(clock)*sin(clock)*abs(uv.x))),d);
   
    gl_FragColor = vec4(
        col,
        1.0);
}
