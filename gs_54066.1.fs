/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54066.1"
}
*/


#ifdef GL_ES
precision highp float;   /// iOS needs high?
#endif

#extension GL_OES_standard_derivatives : enable


// "[SH17A] Fireworks" by Martijn Steinrucken aka BigWings/Countfrolic - 2017
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// Based on https://www.shadertoy.com/view/lscGRl

#define N(h) fract(sin(vec4(6,9,1,0)*h) * 9e2)

void main(void)
{
  vec4 o; 
  vec2 u = gl_FragCoord.xy/RENDERSIZE.y;
    
// proper pixelate
float s = 50.;
	u = floor(u * s) / s;
	
  float e, d, i=0.;
  vec4 p;
    
  for(float i=1.; i<30.; i++) {
    d = floor(e = i*9.1+TIME);
    p = N(d)+.3;
    e -= d;
    for(float d=0.; d<5.;d++)
      o += p*(2.9-e)/1e3/length(u-(p-e*(N(d*i)-.5)).xy);  
  }
	 
  gl_FragColor = vec4(o.rgb, 1);
}