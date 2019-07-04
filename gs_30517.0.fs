/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#30517.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

//precision mediump float;

void main(void){
    vec2 p = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
    float c = 0.0;
    c += sin((p.x*p.x+p.y*p.y)*16.0*pow(TIME,3.));
    gl_FragColor = vec4(vec3(c), 1.0);
}