/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tdjSWm by tristanwhitehill.  simple explorations",
  "INPUTS" : [
	{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.1,
			"MAX": 10.0
		},
		
		{
			"NAME": "offset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 5.0
		}
  ]
}
*/


void main() {



 
    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    vec2 r = vec2( gl_FragCoord.xy - 0.5*RENDERSIZE.xy );
	
	r = 3.0 * r.xy / RENDERSIZE.xy;
  
    vec3 col1 = vec3 (0.2,0.2,.2);
    vec3 col2 = vec3 (0.0,0.8,.9);
     vec3 col3 = vec3 (.7,.7,.7);
    vec3 pixi;
    
    float width = (sin(.2 * ((TIME*speed)-offset))*50.);
    float width2 = sin(.003 * ((TIME*speed)-offset))*3.;
    float mody = mod(width/width2,floor(cos(r.x+((TIME*speed)-offset))*10.));
    if(r.y > mody-width*(sin(.3*((TIME*speed)-offset))*.3)){
        
        pixi = col1;
        
    	}
    else {
        
        pixi = col2;
    
        }
      if(r.y < mody*(cos(.03*(TIME*speed))/10.)){
        
        pixi = col3;
        
    	}
    gl_FragColor = vec4((pixi-sin(r.x)/-mody),1.0);
}
