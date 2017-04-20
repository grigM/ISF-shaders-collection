/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "circles",
    "blackandwhite",
    "hypnose",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4sd3Wj by Jespertheend.  Keep looking at the center for 30 seconds or so. Then look at an object in your room.",
  "INPUTS" : [

  ]
}
*/


void main()
{
    vec2 coord = gl_FragCoord.xy;
    vec2 center = RENDERSIZE.xy /2.0;
    float dist = length(center - coord);
    float circlesOut = cos(dist/7.0 - TIME*6.0);
    circlesOut *= 5.0;
    float circlesIn = cos(dist/7.0 + TIME*6.0);
    circlesIn *= 5.0;
    circlesIn = clamp(circlesIn,0.0,1.0);
    circlesOut = clamp(circlesOut,0.0,1.0);
    float edge = clamp(dist-100.0,0.0,1.0);
    circlesOut *= edge;
    circlesIn *= 1.0-edge;
    float c = circlesOut + circlesIn;
	gl_FragColor = vec4(c,c,c,1.0);
}