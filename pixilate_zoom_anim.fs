/*
{
  "CREDIT": "by misha from lunapark",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"pixilate distortion effect"
	],
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
	{
			"NAME": "min_pix_amount",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 128.0
			
		},
	{
			"NAME": "max_pix_amount",
			"TYPE": "float",
			"DEFAULT": 32.0,
			"MIN": 16.0,
			"MAX": 128.0
			
		},
		
		{
			"NAME": "pix_speed",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": 0.0,
			"MAX": 4.0
			
		},
		
		{
			"NAME": "rotate",
			"TYPE": "bool",
			"DEFAULT": 0.0
		},
		{
			"NAME": "rotate_speed",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": 0.0,
			"MAX": 4.0
			
		},

		
  ]
}
*/



#define PI 3.14159265359



mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

void main()	{
	float ss = min_pix_amount+abs((max_pix_amount*sin(TIME*pix_speed)));
	vec2 gg = gl_FragCoord.xy;
	gg = ceil(gg / ss) * ss;	
	
	
	//float mx = max(RENDERSIZE.x, RENDERSIZE.y);
	//vec2 p = 16.0 * (2.0 * gg - RENDERSIZE) / mx;
	
	
	
	
	
	vec2 p = gg / RENDERSIZE.xy;

	p -= vec2(0.5);
	if(rotate){
		p = rotate2d(cos(TIME*rotate_speed)*PI)*p;
	}
	p += vec2(0.5);
	p *= 1.;
	
	
	
	
	
	
	//vec2 p = gg / RENDERSIZE.xy; 
	//p.x *= RENDERSIZE.x/RENDERSIZE.y; 
	
	
	vec4		inputPixelColor;
	//	both of these are the same
	//inputPixelColor = IMG_THIS_PIXEL(inputImage);
	//inputPixelColor = IMG_PIXEL(inputImage, gl_FragCoord.xy);
	
	
	//if(COS_DEFORM){
		//p += cos(p.x * COS_X_DEFORM_PER + (TIME*COS_DEFORM_SPEED)) * COS_X_DEFORM_AMP;
		//p -=cos(p.y*COS_Y_DEFORM_PER +(TIME*COS_DEFORM_SPEED))*COS_Y_DEFORM_AMP;
    
	//}
	
	//	both of these are also the same
	//inputPixelColor = IMG_NORM_PIXEL(inputImage, isf_FragNormCoord.xy);
	//inputPixelColor = IMG_THIS_NORM_PIXEL(inputImage);
	
	
	inputPixelColor = IMG_NORM_PIXEL(inputImage, p);
	
	gl_FragColor = inputPixelColor;
}



