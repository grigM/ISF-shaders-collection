/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43111.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


// Spiral Hypnosis
	
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
  vec2 uv = (2.*fragCoord - RENDERSIZE.xy ) / RENDERSIZE.x;
  float t = TIME * 4.0;
    uv = abs(mod(uv*1.0,1.0)) - 0.5;
    uv *= mat2(cos(t),sin(t),-sin(t),cos(t)); // image rotation

    float theta = (atan(uv.y,uv.x)) / 6.28;
    float dist = length(uv)*2.;
    float spirals = 2.;
    float value = theta + dist*spirals;
    value = fract(value);
    value = (sin(value*6.28)+1.) * 0.5;
    fragColor = vec4(value);
}

void main( void )   
{
  mainImage(gl_FragColor, gl_FragCoord.xy );   
}