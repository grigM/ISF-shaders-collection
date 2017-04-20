/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex07.jpg"
    }
  ],
  "CATEGORIES" : [
    "2d",
    "distortion",
    "layer",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XlcXWs by CyanSprite.  Tile Distortion I made, just having fun honestly.\nGave four different bools to play around with.",
  "INPUTS" : [

  ]
}
*/


//speed
float speed = .05;
//how many tiles do you want? 
float tiles = 9.0;
//the vers I did first versus second
bool vers1 = true;
//render simplish
bool simple = true;
//don't flood through X
bool noFloodX = true;
//don't flood through Y
bool noFloodY = true;

void main()
{   
    vec2 uv = 2.0 * gl_FragCoord.xy / RENDERSIZE.xy - 1.0;
	uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    
    float m1 = 1.0;
    float m2 = 1.0;
    float m3 = uv.x;
    float m4 = uv.y;
    float m5 = 1.0;
    
    if(!simple){
    	m1 = uv.y;
        m2 = uv.x;
    }
    if(vers1){
    	m3 = uv.y;
        m4 = uv.x;
    }
        
    
    uv.x = cos(uv.x * tiles) * m3 * m1;
    if(noFloodX)
    	m5 = uv.x;
    uv.y = sin(uv.y * tiles) * m4 * m2 * m5;
    if(noFloodY)
        uv.x*=uv.y;
    vec4 col = IMG_NORM_PIXEL(iChannel0,mod(uv+TIME*speed,1.0));
    
    gl_FragColor = col;
}