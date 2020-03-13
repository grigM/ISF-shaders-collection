/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llccWH by dahart.  where does it go?",
  "INPUTS" : [

  ]
}
*/


#define S smoothstep
void main() {



    vec2 uv = 5. * (gl_FragCoord.xy - .5 * RENDERSIZE.xy) / RENDERSIZE.x;
    float r = length(uv);
    float a = atan(uv.y, uv.x) - TIME*1.5;
        
    float b = fract((log(abs(log(r)))*12.+a)/6.28);
    float db = fwidth(b)*1.5;
    gl_FragColor = vec4(vec3(S(.25+db, .25, b)*S(db, db+db, b)), 1.);
    
    float cw = 1./3.;
    float c = mod((r+a)/6.28, cw);
    float dc = fwidth(r)*1.5;
    gl_FragColor.rgb *= 1. - S(0., cw, c)*S(cw-dc, cw-2.*dc, c);
}
