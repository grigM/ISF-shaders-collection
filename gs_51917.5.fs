/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#51917.5"
}
*/


//precision mediump float;


#define PI 3.14159265358979323846

const float ROTATION_SPEED = 0.2;
const float SPEED = 0.5;

vec2 rotate2D(vec2 _st, float _angle){
    _st -= 0.5;
    _st =  mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle)) * _st;
    _st += 0.5;
    return _st;
}

vec2 tile(vec2 _st, float _zoom){
    _st *= _zoom;
    return fract(_st);
}

float box(vec2 _st, vec2 _size, float _smoothEdges){
    _size = vec2(0.5)-_size*0.5;
    vec2 aa = vec2(_smoothEdges*0.5);
    vec2 uv = smoothstep(_size,_size+aa,_st);
    uv *= smoothstep(_size,_size+aa,vec2(1.0)-_st);
    return uv.x*uv.y;
}

void main(void){
    vec2 st = gl_FragCoord.xy/min(RENDERSIZE.x, RENDERSIZE.y);
    vec3 color = vec3(0.0);

    vec2 move = vec2(0.0, mod(TIME * SPEED, 1.0));

    // Divide the space in 4
    // st = tile(st,4.);
    float n = 4.0;
    vec2 index = floor(st * n) / n; //整数部分を返すので、1-4が返ってくる
    st = fract(st * n + move); //0-1が何回か返ってくる

    // Use a matrix to rotate the space 45 degrees
    st = rotate2D(st,PI * mod(TIME * ROTATION_SPEED + index.x, 1.0));
    // st = rotate2D(st,PI * (sin(TIME * ROTATION_SPEED * index.x) + 0.5 + 0.5));

    // Draw a square
    // float scale = 0.5 * (sin(TIME) * 0.5 + 0.5);
    // color = vec3(box(st,vec2(0.5 + scale),0.01));
    // color = vec3(box(st,vec2(0.5),0.01));
    color = vec3(box(st,vec2(0.5),0.01 + 0.3 * (sin(TIME * 2.0 + index.x) * 0.5 + 0.5)));
    // color = vec3(st,0.0);

    gl_FragColor = vec4(color,1.0);
}