/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "GLSLSandbox"
    ],
    "DESCRIPTION": "Automatically converted from http://glslsandbox.com/e#58736.0",
    "INPUTS": [
    ]
}

*/


// original https://www.shadertoy.com/view/tdGXWm

//precision highp float;

#define PI 3.14159

float VDrop2(vec2 uv)
{
    uv.x *= sin(1.+uv.y*.525)*0.4;			// ADJUST PERSP
    float t =  TIME*0.1;
    uv.x = uv.x*64.0;					// H-Count
    float dx = fract(uv.x);
    uv.x = floor(uv.x);
    uv.y *= 0.15;					// stretch
    float o=sin(uv.x*215.4);				// offset
    float s=cos(uv.x*33.1)*.3 +.7;			// speed
    float trail = mix(145.0,15.0,s);			// trail length
    float yv = 1.0/(fract(uv.y + t*s + o) * trail);
    yv = smoothstep(0.0,0.1,yv*yv);
    yv = sin(yv*PI)*(s*5.0);
    float d = sin(dx*PI);
    return yv*(d*d);
}


void main(void)
{ 
 vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
 vec3 col = vec3(2.1,0.6,0.2)*VDrop2(uv);
 gl_FragColor=vec4(col,1.);
}