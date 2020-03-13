/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Mslyzf by battlebottle.  a simple tunnel",
  "INPUTS" : [

  ]
}
*/


void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
	vec2 centre = RENDERSIZE.xy / 2.0;
    
    vec4 col = vec4(0.0,0.0,0.0,1.0);
    
    float steps = 16.0;
    
    for(float x = -(steps/2.0); x < (steps/2.0); x++){
    	for(float y = -(steps/2.0); y < (steps/2.0); y++){
    		float dist = sqrt(sqrt((distance(gl_FragCoord.xy + vec2(x, y) / steps, centre))) * 100.0);
    		col += floor(mod(dist - TIME * 3.0, 2.0));
    	}
    }
    
    col /= steps * steps;
        
	gl_FragColor = col * (1.0 - (distance(gl_FragCoord.xy, centre) / RENDERSIZE.x));
}
