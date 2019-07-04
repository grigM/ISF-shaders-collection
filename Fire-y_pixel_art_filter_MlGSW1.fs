/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "fire",
    "filter",
    "pixelart",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MlGSW1 by Vel0city.  Just a very simple test.\nCode is terrible. Yes even those 20 lines.",
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    }
  ]
}
*/




vec2 texel_size = vec2(6.0,6.0);

void main()
{    
	vec2 uv = floor(gl_FragCoord.xy/texel_size);	// Pixelify
    uv.xy = RENDERSIZE.xy / texel_size;	// Correct scale

    float reaction_coordinate = IMG_NORM_PIXEL(inputImage,mod(uv.xy,1.0)).r;	// Use red channel
    
    float mixval = (((reaction_coordinate - 0.55) * 10.0 + 0.5) * 2.0);
    
    gl_FragColor = vec4(mix(vec3(1.0, 0.58, 0.0), 
                         vec3(1.0, 0.7, 0.4),
                         mixval),
                	 reaction_coordinate);
    
    gl_FragColor.rgb = vec3(1.0, 0.2, 0.0);	// Red
    if (gl_FragColor.a > 0.65) gl_FragColor.rgb = vec3(1.0, 1.0, 1.0);	// White
    else if (gl_FragColor.a > 0.37) gl_FragColor.rgb = vec3(1.4, 0.8, 0.0);	// Yellow
    gl_FragColor.a = float(gl_FragColor.a > 0.1);
}