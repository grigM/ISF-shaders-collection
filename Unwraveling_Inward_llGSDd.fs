/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "ad56fba948dfba9ae698198c109e71f118a54d209c0ea50d77ea546abad89c57.png"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llGSDd by sea.  Mesmerizing.",
  "INPUTS" : [

  ]
}
*/


#define pi 3.1415926

void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy * 2.0 - 1.0;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    float angle = -TIME;
    
    float x = uv.x * cos(angle) - uv.y * sin(angle);
    float y = uv.y * cos(angle) + uv.x * sin(angle);
    
    uv.x = x;
    uv.y = y;
    
    float a = (atan(uv.y, uv.x) / pi + 1.0);
    float r = sqrt(uv.x * uv.x + uv.y * uv.y);
    
    r = 2.0 * r * (1.0 - cos(a * 1.0));
    
    float t = 1.0;
    uv.x = 2.0 * r * cos(t) * (1.0 - cos(t));
    uv.y = 2.0 * r * sin(t) * (1.0 - cos(t)) * 10.0;
    
    gl_FragColor = vec4(IMG_NORM_PIXEL(iChannel0,mod(uv,1.0)) * vec4(1.0, 0.0, 0.0, 1.0) * (1.0 - vec4(uv, 1.0, 1.0)));
}
