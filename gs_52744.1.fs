/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#52744.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float speed = 4.;

#define tau 6.2831853

float circ(vec2 p) {
  float r = length(p);
  r = log(sqrt(r));
  return abs(mod(r*4.,tau)-3.14)*3.+.2;
}

void main( void ) {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE - .5;
    vec2 p = vec2(uv.x,uv.y);
    p.x *= RENDERSIZE.x/RENDERSIZE.y;
    p*=4.;
    
    p /= exp(mod(TIME*speed,3.14159));

    float c = abs( fract(TIME*3.14159*8.) - uv.x/(uv.y+mod(TIME,uv.y)));
	
    vec3 col = vec3(c);

    col *= pow(abs((0.9-circ(p))),.9);
    
    gl_FragColor =  vec4(col,1.0);	
}