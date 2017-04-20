/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "palette",
    "stripes",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4dcXDl by airtight.  Playing with Andy Gilmore style color palette",
  "INPUTS" : [

  ]
}
*/


const float PI = 3.14159;
const float numStripes = 16.0;

//convert HSV to RGB
vec3 hsv2rgb(vec3 c){
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

vec3 gilmoreCol(float x){
    
     //stepped hue
     x = floor(x*numStripes)/numStripes;
    
     //offset hue to put red in middle
    //flip it and reverse it
    float hue = fract((1.0 - x) - 0.45);
    
    //saturation is higher for warmer colors
    float sat = 0.3 + sin(x*PI)*0.5;
    
    //brightness higher in middle
    float bri = (smoothstep(0.,0.6, x) - smoothstep(0.6,1.0,x))*.6 + 0.3;
   
    //darker
    bri *= 0.85;
    
    return vec3(hue, sat,bri);
    
}

void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    //stripes
    float x = uv.x;
    x = fract( x  +  TIME/10. );
   
    //rings
    vec2 p =(gl_FragCoord.xy-.5*RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y);
    x = distance(p , vec2(0)); 
    x = fract( x  +  TIME/10. );
    
    vec3 hsv = gilmoreCol(x);
    
    //vertical brightness gradient
    hsv.z -= fract( uv.y  )/2.;
    
    //add in gradient stripes
    //th-th-thats all folks
    float stripes = 1. - fract(x*numStripes);
    hsv.z = mix(hsv.z,stripes,0.03); 
    
    vec3 col = hsv2rgb(hsv);
  	gl_FragColor = vec4(col,1.0);
    
}