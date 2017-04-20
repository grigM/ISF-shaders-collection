/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "trig",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Ml33Ds by pcr3w.  A nice sine colour curve I got whilst messing around with the starter shader.",
  "INPUTS" : [

  ]
}
*/


void main(){
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    float speed = 5.0;
    float multiplier = sin(uv.x * pow(uv.y, speed));
    
	float red = (sin((multiplier + speed) * TIME + 0.0) * 127.0 + 128.0) / 255.0;
    float green = (sin((multiplier + speed) * TIME + 2.0) * 127.0 + 128.0) / 255.0;
    float blue = (sin((multiplier + speed) * TIME + 4.0) * 127.0 + 128.0) / 255.0;
    
	gl_FragColor = vec4(red, green, blue, 1.0);
}