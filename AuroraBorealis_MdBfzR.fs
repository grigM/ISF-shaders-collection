/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel1",
      "PATH" : "52d2a8f514c4fd2d9866587f4d7b2a5bfa1a11a0e772077d7682deb8b3b517e5.jpg"
    },
    {
      "NAME" : "iChannel0",
      "PATH" : "ad56fba948dfba9ae698198c109e71f118a54d209c0ea50d77ea546abad89c57.png"
    }
  ],
  "CATEGORIES" : [
    "sky",
    "stars",
    "aurora",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MdBfzR by albertelwin.  Northern lights shader.",
  "INPUTS" : [

  ]
}
*/



#define TAU 6.2831853071

void main() {

	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    float o = IMG_NORM_PIXEL(iChannel1,mod(uv * 0.25 + vec2(0.0, TIME * 0.025),1.0)).r;
    float d = (IMG_NORM_PIXEL(iChannel0,mod(uv * 0.25 - vec2(0.0, TIME * 0.02 + o * 0.02),1.0)).r * 2.0 - 1.0);
    
    float v = uv.y + d * 0.1;
    v = 1.0 - abs(v * 2.0 - 1.0);
    v = pow(v, 2.0 + sin((TIME * 0.2 + d * 0.25) * TAU) * 0.5);
    
    vec3 color = vec3(0.0);
    
    float x = (1.0 - uv.x * 0.75);
    float y = 1.0 - abs(uv.y * 2.0 - 1.0);
    color += vec3(x * 0.5, y, x) * v;
    
    vec2 seed = gl_FragCoord.xy;
    vec2 r;
    r.x = fract(sin((seed.x * 12.9898) + (seed.y * 78.2330)) * 43758.5453);
    r.y = fract(sin((seed.x * 53.7842) + (seed.y * 47.5134)) * 43758.5453);
    float s = mix(r.x, (sin((TIME * 2.5 + 60.0) * r.y) * 0.5 + 0.5) * ((r.y * r.y) * (r.y * r.y)), 0.04); 
    color += pow(s, 70.0) * (1.0 - v);
    
    gl_FragColor.rgb = color;
    gl_FragColor.a = 1.0;
}
