/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tlXSR8 by qat.  Ported from Max\/MSP.",
  "PASSES" : [
    {
      "TARGET" : "BufferA",
      "PERSISTENT" : true,
      "FLOAT" : true
    }
  ],
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    },
    {
			"NAME": "amt_p",
			"TYPE": "float",
			"DEFAULT": 0.31,
			"MIN": -2.0,
			"MAX": 2.0
		},
		 {
			"NAME": "sin_speed",
			"TYPE": "float",
			"DEFAULT": 0.75,
			"MIN": 0.0,
			"MAX": 3.0
		},
		{
			"NAME": "fnc",
			"TYPE": "long",
			"VALUES": [
				1,
				2,
				3,
				4,
				5
			],
			"LABELS": [
				"sin",
				"cos",
				"tan",
				"frct",
				"sign"
			],
			"DEFAULT": 1
		}
    
  ]
}
*/


void main() {
	if (PASSINDEX == 0)	{
		
		float amt;
		
		if(fnc==1){
	    	amt =  sin(TIME*sin_speed) * amt_p;
		}
		if(fnc==2){
	    	amt =  cos(TIME*sin_speed) * amt_p;
		}
		if(fnc==3){
	    	amt =  tan(TIME*sin_speed) * amt_p;
		}
		if(fnc==4){
	    	amt =  fract(TIME*sin_speed) * amt_p;
		}
		if(fnc==5){
	    	amt =  sign(TIME*sin_speed) * amt_p;
		}
		
	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	    
	    vec3 c = IMG_NORM_PIXEL(inputImage,mod(uv,1.0)).rgb;
	    
	    vec3 d = IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).rgb;
	 	d *= amt;
	    
		vec3 e = IMG_NORM_PIXEL(inputImage,mod(uv + d.r,1.0)).rgb;
	    vec3 f = IMG_NORM_PIXEL(inputImage,mod(uv + d.g,1.0)).rgb;
	    vec3 g = IMG_NORM_PIXEL(inputImage,mod(uv + d.b,1.0)).rgb;
	    vec3 h = vec3(e.r,f.g,g.b);
	    
	    gl_FragColor = vec4(h,1.);
	}
	else if (PASSINDEX == 1)	{


	    vec3 c = IMG_NORM_PIXEL(BufferA,mod(gl_FragCoord.xy / RENDERSIZE.xy,1.0)).rgb;
	  
	    gl_FragColor = vec4(c,1.);
	}
}
