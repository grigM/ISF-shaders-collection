/*
{
  
  "CATEGORIES" : [
    "splitshift",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tdXzj by panda1234lee.  Easy Split Shift [similar effect: https:\/\/www.shadertoy.com\/view\/MlcXRB]",
   "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    }
  ]
}
*/



void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;    
    //float ratio = RENDERSIZE.x / RENDERSIZE.y;
    float offset = 0.;
         
    
    float t1 = uv.x + uv.y;
    float band = .5;
    
    if(t1 > 0. && t1 < 1.*band)
    {
        offset = -1.*sin(TIME);
    }
    else if(t1 >band && t1 < 2.*band)
    {
    	offset = 1. * sin(TIME);
    }
    else if(t1 > 2.*band && t1 < 3.*band)
    {
    	offset = -1.*sin(TIME);
    }
    else if(t1 > 3.*band)
    {
    	offset = 1. * sin(TIME);
    }
    

	vec4 col0 = IMG_NORM_PIXEL(image,mod(uv + vec2(offset, - offset));
    
    vec4 col1 = vec4(smoothstep(0.,0.015, abs(t1 - 1.*band)));
    vec4 col2 = vec4(smoothstep(0.,0.015, abs(t1 - 2.*band)));
    vec4 col3 = vec4(smoothstep(0.,0.015, abs(t1 - 3.*band)));
    
    gl_FragColor = mix(col0, vec4(1.), (1.- col1)+(1.-col2)+(1.-col3));
      
}

// ----------------------------------------
// Benefited from gigatron, amazing effect!
// ----------------------------------------
void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;    
    float offset = 0.;
    float t1 =  uv.y;
     
    for (float ii=0.00;ii<50.;ii++){
    
    if(t1 >0.01+ii*0.02 && t1 <0.02+ii*0.02)
    	{
      	   offset = -1.*sin(TIME)*0.02;
   		}
            
     if(t1 >0.02+ii*0.02 && t1 <0.03+ii*0.02)
 	    {
        	offset = 1.*sin(TIME)*0.02;
    	}
        
  
  }    
      
    vec4 col = IMG_NORM_PIXEL(iChannel0,mod(uv + vec2(offset,0.),1.0));
	     
    if (uv.x < 0.0-offset || uv.x > 1.0-offset ){
      
      col=vec4(0.,0.,0.,0.);
    }
     
    
    gl_FragColor = vec4(col);
      
}
