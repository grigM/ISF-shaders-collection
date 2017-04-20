/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "sun",
    "light",
    "radial",
    "gradient",
    "moon",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XsdXDj by Danich.  Two dynamic circles",
  "INPUTS" : [

  ]
}
*/


void main()
{
    vec2 sunXY;
    vec2 moonXY;
    
    sunXY.x = RENDERSIZE.x * 0.5 + RENDERSIZE.x * sin(2.0 * 3.14 / 24.0 * TIME) * 0.4 * (RENDERSIZE.y / RENDERSIZE.x);
    sunXY.y = RENDERSIZE.y * 0.5 + RENDERSIZE.y * cos(2.0 * 3.14 / 24.0 * TIME) * 0.4;
    
    moonXY.x = RENDERSIZE.x - sunXY.x;
    moonXY.y = RENDERSIZE.y - sunXY.y;

    vec4 sunColor = vec4(0.0, 0.0, 0.0, 1.0);
    vec4 moonColor = vec4(0.0, 0.0, 0.0, 1.0);
//    vec4 skyColor = vec4(0.0, 0.0, 0.1, 1.0);
    
//    skyColor.r = cos(TIME);
//    skyColor.b = sin(TIME);
    
    
    float dSun = sin(3.14 / (distance(sunXY, gl_FragCoord.xy) + 1.0)) * 10.0;
    float dMoon = sin(2.00 / (distance(moonXY, gl_FragCoord.xy) + 1.0)) * 8.0;
    
    
    sunColor.r = dSun;
    sunColor.g = dSun * 0.7;
    
    moonColor.r = dMoon * 0.7;
    moonColor.g = dMoon * 0.7;
    moonColor.b = dMoon;
    
    
    
    gl_FragColor = sunColor + moonColor;
    
}