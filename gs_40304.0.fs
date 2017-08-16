/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40304.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main( void ) {	
    vec2 p=(gl_FragCoord.xy-.5*RENDERSIZE)/RENDERSIZE.y;
    float tun=.2/sqrt(dot(p, p)),en=float(fract(1./tun+TIME)>=.02/fract(tun+sin(TIME))),l=float(.5<fract(tun+sin(TIME*1.)+atan(p.x, p.y)*3.));
    gl_FragColor=vec4(tun<.05||en<l);
}