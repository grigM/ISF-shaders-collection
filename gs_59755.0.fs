/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59755.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


#define TIME sin(TIME-gl_FragCoord.y/150.-gl_FragCoord.x/(200.-cos(TIME*4.+gl_FragCoord.x/100.)*50.)+TIME)*2.

void main()
{
    vec2 r = RENDERSIZE,
    o = gl_FragCoord.xy - r/2.;
	
    o = vec2(max(abs(o.x)*.866 + o.y*0.5, -o.y)*2. / r.y - .3+tan(cos(TIME*2.2)*1.)/8., atan(o.y,o.x));    
	o.x = dot(o,o)*0.1;
    vec4 s = .07*cos(1.5*vec4(0,1,2,3) + TIME + o.y + TIME),
    e = s.yzwx*0.6, 
    f = max(o.x-s,e+o.x);
    gl_FragColor = dot(clamp(f*r.y,0.,1.), 72.*(s-e)) * (s-.1) ;
}