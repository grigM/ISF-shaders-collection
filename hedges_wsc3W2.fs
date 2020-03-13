/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wsc3W2 by a22dae1d4f0e484ba4114a147eac4e.  hedges",
  "INPUTS" : [

  ]
}
*/


float Hash21(vec2 p)
{
    // PRNG
    vec2 seed = vec2(200);
    p += seed;
    p = fract(p*vec2(234.34, 435.345));
    p += dot(p, p+34.23);
    return fract(p.x*p.y);
}

void main() {



    vec2 rv = (gl_FragCoord.xy-.5*RENDERSIZE.xy)/(RENDERSIZE.y);
    vec2 uv = rv*10.;
    
    vec3 col = vec3(0);
    
    uv += TIME*vec2(-0.05, -0.2);
    float width = .35;
    float s = 0.018;
    
    vec2 gv = fract(uv)-0.5;
    vec2 id = floor(uv);
    
    float n = Hash21(id);
    if(n<.5) gv.x *= -1.;  //apply random
    float mask = smoothstep(s, -s, abs(gv.y + gv.x)-width);
    mask +=  smoothstep(s, -s, abs(gv.y-0.5 + gv.x-0.5)-width);
    mask +=  smoothstep(s, -s, abs(gv.y+0.5 + gv.x+0.5)-width);
    
    //float d= abs(abs(gv.x + gv.y)-.5);
    //d = length(gv - vec2(0.5))-.5;
    //float mask = smoothstep(.01, -.01, abs(d) - width);
    //d = length(gv - vec2(-0.5))-.5;
    //mask += smoothstep(.01, -.01, abs(d) - width);
    
    
    //col += smoothstep(.01, -.01, abs(gv.y-gv.x))*vec3(0,0,1);
    
    float gradient = (rv.x+1.)/2.;
    vec3 gradientv = gradient*vec3(0.2,0.8,0.1) + (1.-gradient)*vec3(0,0.8,0.6);
    
    col += mask*gradientv;
    //col.rg += n;
    
    //if(gv.x > 0.48 || gv.y > .48) col = vec3(1,0,0);
    
    gl_FragColor = vec4(col,1.0);
}
