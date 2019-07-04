/*
{
  "CREDIT": "by misha from lunapark",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"ripple distortion effect"
	],
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
	{
			"NAME": "COS_DEFORM_SPEED",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 2.0
			
		},
	{
			"NAME": "COS_X_DEFORM_PER",
			"TYPE": "float",
			"DEFAULT": 6.0,
			"MIN": 0.0,
			"MAX": 8.0
			
		},
		{
			"NAME": "COS_Y_DEFORM_PER",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 8.0
			
		},
		
		{
			"NAME": "COS_X_DEFORM_AMP",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": -1.0,
			"MAX": 1.0
			
		},
		{
			"NAME": "COS_Y_DEFORM_AMP",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": -1.0,
			"MAX": 1.0
			
		}
  ]
}
*/





/*
vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    vec4 movie = IMG_NORM_PIXEL(inputImage,mod(uv,1.0));
    vec4 img = IMG_NORM_PIXEL(iChannel0,mod(uv,1.0));
    if (movie.g + movie.b + movie.r < 1.0) {
    	gl_FragColor = vec4(0.0 - movie.g/3.0,0.0 - movie.g/3.0,0.0 - movie.g/3.0,0.0);
    }
    else if (movie.g + movie.b + movie.r < 1.1) {
    	gl_FragColor = vec4(0.2 - movie.g/3.0,0.2 - movie.g/3.0,0.2 - movie.g/3.0,0.1);
    }
    else {
		gl_FragColor = vec4(uv,0.5+0.5*sin(TIME),1.0);
    }
*/


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



