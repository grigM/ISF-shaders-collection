/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tllSzn by nkaretnikov.  Trying to mimic a Piet Mondrian painting:\nhttps:\/\/thebookofshaders.com\/07\/",
  "INPUTS" : [
	{
			"NAME": "anim_switch",
			"TYPE": "float",
			"DEFAULT": 3.0,
			"MIN": 0.0,
			"MAX": 5.0
	}
  ]
}
*/


// Public domain.

#define SHIFT .5

vec3 rect(vec3 color, vec2 uv, vec2 bl, vec2 tr)
{
    float res = 1.0;
    
    // Bottom left.
    bl = step(bl, uv);  // if arg2 > arg1 then 1 else 0
    res = bl.x * bl.y;  // similar to logic AND
    
    // Top right.
    tr = step(SHIFT - tr, SHIFT - uv);
    res *= tr.x * tr.y;
    
    return res * color;
}

void main() {



    // Normalized pixel coordinates (from 0 to 1).
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    // Remap coordinates to make 0.0 be at the center.
    uv -= SHIFT;
    
    // Account for the aspect ratio.
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    // Paint colors.
    vec3 red    = vec3(.667, .133, .141);
    vec3 blue   = vec3(0.,   .369, .608);
    vec3 yellow = vec3(1.,   .812, .337);
    vec3 beige  = vec3(.976, .949, .878);
    vec3 black  = vec3(0.);
    vec3 white  = vec3(1.);
    
    // Output color.
    vec3 color = black;
    color += rect(beige,          uv, vec2(-.5, -.5), vec2(.5,  .5));
    
    if(anim_switch>=0.0 && anim_switch<1.0){
    	
    
    	color -= rect(beige - red,    uv, vec2(-.5,  .1), vec2(-.3, .5));
    	color -= rect(beige - yellow, uv, vec2(.45,  .1), vec2(.5,  .5));
    	color -= rect(beige - blue,   uv, vec2(.25, -.5), vec2(.5, -.45));
    
    }else if(anim_switch>=1.0 && anim_switch<2.0){
    	
    	color -= rect(beige - blue,    uv, vec2(-.5,  .1), vec2(-.3, .5));
    	color -= rect(beige - red, uv, vec2(.45,  .1), vec2(.5,  .5));
    	color -= rect(beige - yellow,   uv, vec2(.25, -.5), vec2(.5, -.45));
    
    }else if(anim_switch>=2.0 && anim_switch<3.0){
    	
    	color -= rect(beige - yellow,    uv, vec2(-.5,  .1), vec2(-.3, .5));
    	color -= rect(beige - blue, uv, vec2(.45,  .1), vec2(.5,  .5));
    	color -= rect(beige - red,   uv, vec2(.25, -.5), vec2(.5, -.45));
    
    }else if(anim_switch>=3.0 && anim_switch<4.0){
    	
    	color -= rect(beige - yellow,    uv, vec2(-.28,  .1), vec2(.25, .5));
    	color -= rect(beige - blue, uv, vec2(-.5,  -.43), vec2(-.3,  .1));
    	color -= rect(beige - red,   uv, vec2(.25, -.45), vec2(.5, .10));
    
    }
    
    
    // Vertical black lines.
    color -= rect(white, uv, vec2(-.44, .1), vec2(-.42, .5));
    color -= rect(white, uv, vec2(-.3, -.5), vec2(-.28, .5));
    color -= rect(white, uv, vec2(.43, -.5), vec2(.45,  .5));
    color -= rect(white, uv, vec2(.23, -.5), vec2(.25,  .5));
    
    // Horizontal black lines.
    color -= rect(white, uv, vec2(-.5,  .28), vec2(.5, .30));
    color -= rect(white, uv, vec2(-.5,  .08), vec2(.5, .1));
    color -= rect(white, uv, vec2(-.5, -.45), vec2(.5, -.43));
    
    // Output to screen.
    gl_FragColor = vec4(color, 1.);
}
