/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#45360.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main()
{
    vec2 r = RENDERSIZE,
    o = gl_FragCoord.xy - r/2.;
    o = vec2(length(o) / r.y - .3, atan(o.y,o.x));    
    vec4 s = 0.07*cos(1.5*vec4(0,1,2,3) + TIME + o.y + cos(o.y) * cos(TIME)),
    e = s.yzwx, 
    f = max(o.x-s,e-o.x);
    gl_FragColor = dot(clamp(f*r.y,0.,1.), 72.*(s-e)) * (s-.1) + f;
}