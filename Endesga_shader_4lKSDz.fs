/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel1",
      "PATH" : "tex07.jpg"
    }
  ],
  "CATEGORIES" : [
    "displacement",
    "chromatic",
    "aberration",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4lKSDz by Sintel.  STILL CREDS TO MY BOY @LordSk_",
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    }
  ]
}
*/


#define AB_SCALE 0.95

vec2 displace(vec2 uv, vec2 offset)
{   
    float d = smoothstep(0.2,2.0,IMG_NORM_PIXEL(iChannel1,mod((uv*1.0 - vec2(TIME /8.0,0)) + offset,1.0)).r) * 0.25;
    
    return vec2(d);
}

void main()
{
	vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    
    vec2 dr = displace(uv, vec2(0   , 0.02) * AB_SCALE),
         dg = displace(uv, vec2(0.01, 0.01) * AB_SCALE),
         db = displace(uv, vec2(0.01, 0   ) * AB_SCALE);
    
    vec3 color = vec3(0);
    color += vec3(1, 0, 0)*IMG_NORM_PIXEL(inputImage,mod(uv - dr,1.0)).r;
    color += vec3(0, 1, 0)*IMG_NORM_PIXEL(inputImage,mod(uv - dg,1.0)).g;
    color += vec3(0, 0, 1)*IMG_NORM_PIXEL(inputImage,mod(uv - db,1.0)).b;
    
    gl_FragColor = vec4(color, 1.0);
}