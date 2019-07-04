/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tlXXz8 by clintolibre.  implementation of a popular gif on reddit.",
  "INPUTS" : [

  ]
}
*/


//Thanks for checking out my shader. Have a great day. -Clint

float distanceToLine(vec2 p1, vec2 p2, vec2 point) {
    float a = p1.y-p2.y;
    float b = p2.x-p1.x;
    return abs(a*point.x+b*point.y+p1.x*p2.y-p2.x*p1.y) / sqrt(a*a+b*b);
}

void main() {



    // Output to screen
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    //Pretty colors from default shader
    vec3 col = 0.5 + 0.5*cos(TIME+uv.xyx+vec3(0,2,4));
    
    //flatten out waves
    uv.x /= 2.0;
    
    //how many tiles
    float frac = RENDERSIZE.x/22.0;
    
    //value for which row in the tiling the pixel is in
    float weird = floor(uv.y*frac);
    
    //if the pixel is in a odd or even column of the tiling
    float even = mod(floor(uv.x*frac),2.0);
    
    //offset the oscillation of the endpoint on the top of each tile
    float calc = (sin((TIME*2.0)+(weird/4.0)+(even*3.14))/4.0)+.5;
    
    //declare endpoints to for the two lines in each tile
    vec2 p1 = vec2(calc,floor(uv.y+1.0));
    vec2 p2 = vec2(0.0,0.0);
    vec2 p3 = vec2(1.0,0.0);
	
    //tile our 2space
    uv = fract(uv*frac);
    
    //thickness of lines
    float radius =0.05;
    
    //aliasing of lines
    float AA = 0.02;
    
    //distances from our pixel to the lines we want to draw
    float distance = min(distanceToLine (p1,p2,uv),distanceToLine (p1,p3,uv));
	
    //smoothstep for drawing our lines
   	float line =  smoothstep (radius/2.,radius/2.-AA,distance);
    
    //drawing lines
    gl_FragColor = vec4(col*line,1.0);
    
}
