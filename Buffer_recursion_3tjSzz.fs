/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3tjSzz by avin.  Buffer experiment\nIt looks better at fullscreen",
  "PASSES" : [
    {
      "TARGET" : "BufferA",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {

    }
  ],
  "INPUTS" : [

  ]
}
*/


void main() {
	if (PASSINDEX == 0)	{
       

	    vec2 ouv = gl_FragCoord.xy/RENDERSIZE.xy;
	    
	    vec2 uv = ouv - vec2(.5);    
	    
	    float t = TIME * 2.;    
	    
	    uv += vec2(sin(t), cos(t))*.0125;        
	    
	    float l = length(uv);
	    gl_FragColor = 
	        vec4(1.) *
	        smoothstep(l, l+.01, .45);
	    
	    if(step(length(uv), .44) == 1.){        
	    	gl_FragColor = IMG_NORM_PIXEL(BufferA,mod((uv + vec2(.5)),1.0))*.975;        
	    }    
	}
	else if (PASSINDEX == 1)	{


	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	
	    gl_FragColor = IMG_NORM_PIXEL(BufferA,mod(uv,1.0));
	}
}
