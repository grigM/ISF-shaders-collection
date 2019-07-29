/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tlfSzr by Yusef28.  Another set of gears\nMVP line in this code: \"fract(a*leafNum\/radians(360.))\" The \"\/radians(360.)\" made the gears even",
  "INPUTS" : [

  ]
}
*/


mat2 rot(float a)
{
 return mat2(cos(a), -sin(a), sin(a), cos(a));   
}

float rnd(vec2 p)
{
 return fract(sin(dot(p, vec2(12.9898, 78.233)))*43753.225432);   
}

float flower(vec2 p, float size, float leafTh, float leafNum)
{
    //size depends on the leafTh somehow
    //size = *2.;
    vec2 st = p*rot(-TIME);
    p = p*rot(TIME/2.);
    
    st = st*size;
    p = p*size;
    
    float c = 1.0-step(0.3, length(p));
    float c2 = 1.0-step(0.5, length(p));
    
    
    float a = atan(p.y, p.x);
    float a1 = atan(st.y, st.x);

    // a = round(atan(p.y, p.x)*a0)/a0;
    // a1 = round(atan(st.y, st.x)*a0)/a0;
    
    float sq = step(0.4, fract(a*leafNum/radians(360.)))+leafTh;
    float sq2 = step(0.2, fract(a*leafNum/radians(360.)))+leafTh;
    float sq3 = step(0.5, fract(a1*leafNum/radians(360.)))+leafTh;
    
    
    float fl = 1.-step(sq, length(p));
    float f2 = 1.-step(sq2, length(p)*1.7);
    
    float f3 = 1.-step(sq3, length(p)*4.);
 return fl+c+f2*2.+f3*3.;//+leafs3/2.;   
}


void main() {



    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    
    uv.x*=RENDERSIZE.x/RENDERSIZE.y;
    
    vec2 index = floor(uv*6.);
    vec2 st = fract(uv*6.);
    st = st*2.0-1.0;
    float seed = rnd(index+floor((TIME/4.)*(index.x+1.)));
    // Time varying pixel color
    float f = max(0.0, flower(st, 4.+seed*2.4, 1.5+seed*2.5, 3.+floor(seed*15.)));
    float solidF = step(-2., f);
    
    vec3 col = vec3(0.7);
    
    col = mix(col, vec3(0.7, 0.1+seed,0.05+seed)*(f+0.4),  solidF);
    col = mix(col, vec3(0.3, 0.,0.), step(1.5, f));
	col = mix(col, vec3(213., 123., 15.)/255., step(3.5, f));
    // Output to screen
    gl_FragColor = vec4(col/1.5,1.0);
}
