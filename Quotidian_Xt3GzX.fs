/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "sun",
    "moon",
    "shaderaday",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xt3GzX by ivansafrin.  #shaderaday #2",
  "INPUTS" : [

  ]
}
*/


#define PI 3.14159265359 

void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.y;
    float reflectVal = 0.0;
    if(uv. y < 0.35) {
        reflectVal = 0.35-(0.35-uv.y);
        uv.y = 0.35 + (0.35-uv.y);
        uv.x += sin(TIME+(uv.y* 130.0)) * 0.01;
    }
    vec2 sunC = vec2((0.5 * RENDERSIZE.x/RENDERSIZE.y) + cos(TIME)*0.5
                         , 0.5 + sin(TIME)*0.4);
    vec2 moonC = vec2((0.5 * RENDERSIZE.x/RENDERSIZE.y) + cos(TIME+PI)*0.5
                      , 0.5 + sin(TIME+PI)*0.4);  
	gl_FragColor = vec4(0.05, 0.1, sunC.y, 1.0) +
        (vec4(1.0, 0.2, 0.0, 1.0) * ((distance(uv, sunC) < 0.06)  ? 1.0 : 0.0))+
        (vec4(1.0, 0.2+ (abs(sin(TIME)) * 0.8), 0.0, 1.0) * (1.0-distance(uv, sunC) * abs(sin(TIME-PI)) * 3.0) * 1.5
        *(0.5 + sin(TIME)*0.5))+
        (vec4(0.8, 0.6, 0.8, 1.0) * ((distance(uv, moonC) < 0.04)  ? 1.0 : 0.0))
        + (vec4(1.0, 0.3, 0.0, 1.0) * reflectVal);
}