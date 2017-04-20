/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39885.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



const float PI = 3.1415926535897932384626433832795;

mat2 rotate2d(float angle){
    return mat2(cos(angle),-sin(angle),
                sin(angle),cos(angle));
}

float stripes(vec2 st){
    st = rotate2d( PI*-0.202 ) * st*10.896;
    return step(.5,1.0-smoothstep(.3,1.,abs(sin(st.x*PI))));
}

void main(){
    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
    st.x *= RENDERSIZE.x/RENDERSIZE.y;
   
    vec3 color = vec3(stripes(st));
    gl_FragColor = vec4(color, 1.0);
}