/*{
	"DESCRIPTION": "Fake floyd-steinberg dithering",
	"CREDIT": "RavenWorks, adapted by David Lublin",
	"CATEGORIES": [
		"Stylize"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "errorCarry",
			"LABEL": "Error Carry",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "colorize",
			"LABEL": "Colorize",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "lookupSize",
			"LABEL": "Quality",
			"TYPE": "long",
			"VALUES": [
				8,
				16,
				32,
				64
			],
			"LABELS": [
				"Very Low",
				"Low",
				"Mid",
				"High"
			],
			"DEFAULT": 32
		}
	]
	
}*/


//	Fake floyd-steinberg dithering
//	Adapted from https://www.shadertoy.com/view/4sjGRD#




float getGrayscale(vec2 coords){
	vec2 uv = coords / RENDERSIZE.xy;
	//uv.y = 1.0-uv.y;
	vec3 sourcePixel = IMG_NORM_PIXEL(inputImage, uv).rgb;
	//return (sourcePixel.r+sourcePixel.g+sourcePixel.b)/3.0;
	return length(sourcePixel*vec3(0.2126,0.7152,0.0722));
}

void main()	{
	vec4		fragColor = vec4(0.0);
	vec2		fragCoord = gl_FragCoord.xy;
	vec4		inputPixelColor = IMG_THIS_PIXEL(inputImage);
	
	int topGapY = int(RENDERSIZE.y - fragCoord.y);
	
	int cornerGapX = int((fragCoord.x < 10.0) ? fragCoord.x : RENDERSIZE.x - fragCoord.x);
	int cornerGapY = int((fragCoord.y < 10.0) ? fragCoord.y : RENDERSIZE.y - fragCoord.y);
	int cornerThreshhold = ((cornerGapX == 0) || (topGapY == 0)) ? 5 : 4;
	
	if (cornerGapX+cornerGapY < cornerThreshhold) {
				
		fragColor = vec4(0.0,0.0,0.0,1.0);
		
	} else if (topGapY < 20) {
			
			if (topGapY == 19) {
				
				fragColor = vec4(0.0,0.0,0.0,1.0);
				
			} else {
		
				fragColor = vec4(1.0,1.0,1.0,1.0);
				
			}
		
	} else {
		
		float xError = 0.0;
		for(int xLook=0; xLook<64; xLook++){
			if (xLook > lookupSize)
				break;
			float grayscale = getGrayscale(fragCoord.xy + vec2(-lookupSize+xLook,0));
			grayscale += xError;
			float bit = grayscale >= 0.5 ? 1.0 : 0.0;
			xError = (grayscale - bit)*errorCarry;
		}
		
		float yError = 0.0;
		for(int yLook=0; yLook<64; yLook++){
			if (yLook > lookupSize)
				break;
			float grayscale = getGrayscale(fragCoord.xy + vec2(0,-lookupSize+yLook));
			grayscale += yError;
			float bit = grayscale >= 0.5 ? 1.0 : 0.0;
			yError = (grayscale - bit)*errorCarry;
		}
		
		float finalGrayscale = getGrayscale(fragCoord.xy);
		finalGrayscale += xError*0.5 + yError*0.5;
		float finalBit = finalGrayscale >= 0.5 ? 1.0 : 0.0;
		
		fragColor = vec4(finalBit,finalBit,finalBit,1.0);
			
	}
	
	inputPixelColor = inputPixelColor * fragColor;

	gl_FragColor = mix(fragColor,inputPixelColor,colorize);
}
