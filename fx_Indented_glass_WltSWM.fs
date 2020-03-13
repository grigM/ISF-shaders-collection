/*
{
  "IMPORTED" : [
    
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WltSWM by luluco250.  Simple trick with coordinates, I wanna try and do some lighting shading later.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
    ,
    
     {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
  ]
}
*/


void main() {

    vec2 mouse = mix(
        vec2(TIME * 100.0),
        iMouse.xy,
        // step(0.0, iMouse.x)) * 0.01;
        iMouse.x) * 0.01;
    
    vec2 glass_offset = sin(gl_FragCoord.xy * 0.1 - mouse) * 10.0;
    vec2 glass_coord = gl_FragCoord.xy + glass_offset;
    
    vec2 ps = vec2(1.0) / RENDERSIZE.xy;
    vec2 uv = gl_FragCoord.xy * ps;
    
    vec2 glass_uv = glass_coord * ps;
    
    gl_FragColor = IMG_NORM_PIXEL(inputImage,mod(glass_uv,1.0));
}
