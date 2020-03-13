/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WtjSRz by DBBH.  uv",
  "INPUTS" : [

  ]
}
*/


void main() {



    float size=4.;
    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    uv=uv*2.-1.;
    uv.x*=RENDERSIZE.x/RENDERSIZE.y;
    float d= length(uv);
    uv*=fract(d+TIME);//cos(d+TIME);
    
    // Time varying
    vec2 v=uv*size;
    uv=fract(uv*size); 
    
	float x=mod(v.x,2.);
    x=x-1.;
    x=sign(x);
    x=smoothstep(0.,1.,x);
    float y=mod(v.y,2.);
    y=y-1.;
    y=sign(y);
    y=smoothstep(0.,1.,y);
    float l =(x+y)==0.?1.:0.;
    float l2=(x+y)>1.?1.:0.;
    // Output to screen
    gl_FragColor = vec4(l+l2);
}
