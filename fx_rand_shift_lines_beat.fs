/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	 {
      "TYPE" : "image",
      "NAME" : "inputImage"
    }
    ,
    
    
    {
      
      "NAME" : "horizontal",
      "TYPE" : "bool",
      "DEFAULT": 1
    },
    
    {
      
      "NAME" : "vertical",
      "TYPE" : "bool",
      "DEFAULT": 1
    },
    {
      
      "NAME" : "direction_switch",
      "TYPE" : "bool",
      "DEFAULT": 1
    },
    
    {
      		"NAME" : "speed",
      		"TYPE" : "float",
      		"DEFAULT" : 1.0,
      		"MAX" : 3,
      		"MIN" : 0.0
    	},
    
    {
      		"NAME" : "shift_amp",
      		"TYPE" : "float",
      		"DEFAULT" : 1.1	,
      		"MAX" : 4,
      		"MIN" : 0.0
    	},
    	
    	
    	
    	
    	{
      		"NAME" : "lines_x",
      		"TYPE" : "float",
      		"DEFAULT" : 20.0,
      		"MAX" : 180,
      		"MIN" : 2.0
    	},
    
    {
      		"NAME" : "lines_y",
      		"TYPE" : "float",
      		"DEFAULT" : 20.0	,
      		"MAX" : 180,
      		"MIN" : 2.0
    	},
        
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59821.0"
}
*/



//precision mediump float;


float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main()
{
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;//(gl_FragCoord.xy - RENDERSIZE / 2.0) / RENDERSIZE.y;
    
    if(horizontal){
    	if(direction_switch){
    		uv.x += rand(vec2(floor(uv.y * lines_x), 0.0)) * pow(1.0 - fract((TIME*speed)), 8.0) * shift_amp;
    	}else{
    		uv.x -= rand(vec2(floor(uv.y * lines_x), 0.0)) * pow(1.0 - fract((TIME*speed)), 8.0) * shift_amp;
    	}
    }
    if(vertical){
    	if(direction_switch){
    		uv.y += rand(vec2(floor(uv.x * lines_y), 0.0)) * pow(1.0 - fract((TIME*speed) + 0.5), 8.0) * shift_amp;
    	}else{
    		uv.y -= rand(vec2(floor(uv.x * lines_y), 0.0)) * pow(1.0 - fract((TIME*speed) + 0.5), 8.0) * shift_amp;
    	}
    }
    vec3 col = vec3(0.0);
    
    
    
    col = IMG_NORM_PIXEL(inputImage, uv).rgb;
     
    
    
    gl_FragColor = vec4(col, 1.0);
    
}