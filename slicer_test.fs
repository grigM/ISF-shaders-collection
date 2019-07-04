


/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "noise",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Mdlyz2 by Jops.  noise",
  "INPUTS" : [
    
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    
    	
		{
			"NAME": "speed",
			"TYPE": "float",
			"MIN": -4.0,
			"MAX": 4.0,
			"DEFAULT": 1.0
		},
		{
			"NAME": "ofset",
			"TYPE": "float",
			"MIN": -1.0,
			"MAX": 1.0,
			"DEFAULT": 0.0
		},

		{
			"NAME": "fract_count",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 100.0,
			"DEFAULT": 10.0
		},
		{
			"NAME": "fract_speed",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 15.0,
			"DEFAULT": 2.5
		},
		{
			"NAME": "HORIZONTAL",
			"TYPE": "bool",
			"DEFAULT": 0.0
		}
		

  ]
}
*/


#define PI 3.14159265359
#define TWO_PI 6.28318530718
float rand(vec2 uv)
{
    //return fract(sin(dot(uv, vec2(12.9898,78.233)))*10000.*TIME);
	//return fract(sin(dot(uv, iMouse.xy))*10000.);
	//return (fract(sin(dot(uv, vec2(12., 70.)))*100000.));
    return (fract(sin(dot(uv, vec2(12., 70.)))*43758.5453123));
}


void main() {



    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    //uv.x *= RENDERSIZE.x/RENDERSIZE.y;
    
    
    vec4		inputPixelColor;
    
  
    
    vec2 pos = vec2(0.5 * (RENDERSIZE.x/RENDERSIZE.y), 0.0);
    //float offset = noise(uv * 6. + (TIME)) * noise_amp;
    //vec3 randc = vec3(uv * 0.2-0.9*sin(TIME), value) ;
    
  

    float value;
        
    float id;
    float r; 
    
    if(HORIZONTAL){
    	
    	id = floor(uv.y*fract_count)+10.0;
    	r = rand(vec2(id));
    
    	uv.x += r*fract_speed * -((TIME*speed)+ofset) / 2. ;
   		uv.x = fract(uv.x);
   		
    }else{
    	
    	
    
    	id = floor(uv.x*fract_count)+10.0;
    	r = rand(vec2(id));
    
    	uv.y += r*fract_speed * -((TIME*speed)+ofset) / 2. ;
   		uv.y = fract(uv.y);
    }
    
       
    inputPixelColor = IMG_NORM_PIXEL(inputImage, uv);
	gl_FragColor = inputPixelColor;
	
	
}



/*
void main()	{
	
	vec2 p = gl_FragCoord.xy / RENDERSIZE.xy; 
	//p.x *= RENDERSIZE.x/RENDERSIZE.y; 
	
	
	vec4		inputPixelColor;
	//	both of these are the same
	//inputPixelColor = IMG_THIS_PIXEL(inputImage);
	//inputPixelColor = IMG_PIXEL(inputImage, gl_FragCoord.xy);
	
	
	//if(COS_DEFORM){
		p += cos(p.x * COS_X_DEFORM_PER + (TIME*COS_DEFORM_SPEED)) * COS_X_DEFORM_AMP;
		p -=cos(p.y*COS_Y_DEFORM_PER +(TIME*COS_DEFORM_SPEED))*COS_Y_DEFORM_AMP;
    
	//}
	
	//	both of these are also the same
	//inputPixelColor = IMG_NORM_PIXEL(inputImage, isf_FragNormCoord.xy);
	//inputPixelColor = IMG_THIS_NORM_PIXEL(inputImage);
	
	
	inputPixelColor = IMG_NORM_PIXEL(inputImage, p);
	
	gl_FragColor = inputPixelColor;
}
*/