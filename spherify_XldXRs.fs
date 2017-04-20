/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "warp",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XldXRs by glkt.  A simple spherify effect that I needed for a VJ project.\nClic-drag on screen to activate it.",
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    },
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


void main()
{
    float pi = 3.14;
    
    // mix factor (with mouse drag)
    float m = (iMouse.x/RENDERSIZE.x-0.5)*2. ;
    m -= min(m,0.)/2.;
    
    // image ratio
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    
    // normalize and center vs
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;    
    uv -= 0.5;
    
    // spherify    
    uv.x *= mix(1.,ratio,m); 
    float k = 1. / max(abs(uv.x),abs(uv.y));
    
    // spherify on edges only  
    
    /*float ll = (uv.x*uv.x + uv.y*uv.y)*4.;
    ll *= ll * ll;
    ll /= 2.;*/   
    
    float l = length(uv)*2.;
    l *= l * l;
    l /= 2.;
    
    // mix with mouse    
    uv *= (k-1.) * (l*m*2.) + 1.;
    uv /= m+1.;
    
   	// reset uv center in corner
    uv += 0.5;    
    
    vec4 c = IMG_NORM_PIXEL(inputImage,mod(uv,1.0));
    
    // black outside
    if(uv.x > 1. || uv.y > 1. || uv.x < 0. || uv.y < 0.){
        c = vec4(0.,0.,0.,0.);
    }
        
	gl_FragColor = c;
}