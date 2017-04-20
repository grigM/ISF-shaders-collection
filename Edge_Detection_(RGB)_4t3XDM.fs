/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "sobel",
    "gradient",
    "edgedetection",
    "rgb",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4t3XDM by RebelMoogle.  Just a simple Sobel filter on each channels (Visualizing Edges)",
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    }
  ]
}
*/


void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    //gl_FragColor = 4.*abs(fwidth(IMG_NORM_PIXEL(inputImage,mod(uv,1.0))));
    
    vec3 TL = IMG_NORM_PIXEL(inputImage,mod(uv + vec2(-1, 1)/ RENDERSIZE.xy,1.0)).rgb;
    vec3 TM = IMG_NORM_PIXEL(inputImage,mod(uv + vec2(0, 1)/ RENDERSIZE.xy,1.0)).rgb;
    vec3 TR = IMG_NORM_PIXEL(inputImage,mod(uv + vec2(1, 1)/ RENDERSIZE.xy,1.0)).rgb;
    
    vec3 ML = IMG_NORM_PIXEL(inputImage,mod(uv + vec2(-1, 0)/ RENDERSIZE.xy,1.0)).rgb;
    vec3 MR = IMG_NORM_PIXEL(inputImage,mod(uv + vec2(1, 0)/ RENDERSIZE.xy,1.0)).rgb;
    
    vec3 BL = IMG_NORM_PIXEL(inputImage,mod(uv + vec2(-1, -1)/ RENDERSIZE.xy,1.0)).rgb;
    vec3 BM = IMG_NORM_PIXEL(inputImage,mod(uv + vec2(0, -1)/ RENDERSIZE.xy,1.0)).rgb;
    vec3 BR = IMG_NORM_PIXEL(inputImage,mod(uv + vec2(1, -1)/ RENDERSIZE.xy,1.0)).rgb;
                         
    vec3 GradX = -TL + TR - 2.0 * ML + 2.0 * MR - BL + BR;
    vec3 GradY = TL + 2.0 * TM + TR - BL - 2.0 * BM - BR;
   	
    
   /* vec2 gradCombo = vec2(GradX.r, GradY.r) + vec2(GradX.g, GradY.g) + vec2(GradX.b, GradY.b);
    
    gl_FragColor = vec4(gradCombo.r, gradCombo.g, 0, 1);*/
    
    gl_FragColor.r = length(vec2(GradX.r, GradY.r));
    gl_FragColor.g = length(vec2(GradX.g, GradY.g));
    gl_FragColor.b = length(vec2(GradX.b, GradY.b));
}