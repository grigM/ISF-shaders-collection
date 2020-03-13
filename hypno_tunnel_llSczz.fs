/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llSczz by public_int_i.  hypno tunnel",
  "INPUTS" : [

  ]
}
*/


/*Ethan Alexander Shulman 2017
@EthanShulman
http://xaloez.com/
*/

void main() {



	vec2 uv = (gl_FragCoord.xy*2.-RENDERSIZE.xy)/RENDERSIZE.x;
    float pixelSize = length(RENDERSIZE.xy)*.4;
    
    float maxis = max(abs(uv.x),abs(uv.y)),
        ldst = log(maxis*20.);
    gl_FragColor = fract(vec4(ldst-TIME)+vec4(0,1,2,3)*fract(TIME*.5));
}
