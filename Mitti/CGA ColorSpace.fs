/*{
	"DESCRIPTION": "CGA style FX",
	"CREDIT": "by IMIMOT (ported from https://github.com/BradLarson/GPUImage)",
	"CATEGORIES": [
		"Stylize"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		}
	]
}*/

void main()
{
	
	 vec2 textureCoordinate = vv_FragNormCoord;
	
     vec2 sampleDivisor = vec2(1.0 / 200.0, 1.0 / 320.0);
     
     vec2 samplePos = textureCoordinate - mod(textureCoordinate, sampleDivisor);
     vec4 color = IMG_NORM_PIXEL(inputImage, samplePos);
     

     vec4 colorCyan = vec4(85.0 / 255.0, 1.0, 1.0, 1.0);
     vec4 colorMagenta = vec4(1.0, 85.0 / 255.0, 1.0, 1.0);
     vec4 colorWhite = vec4(1.0, 1.0, 1.0, 1.0);
     vec4 colorBlack = vec4(0.0, 0.0, 0.0, 1.0);
     
     vec4 endColor = vec4(0.0);
     float blackDistance = distance(color, colorBlack);
     float whiteDistance = distance(color, colorWhite);
     float magentaDistance = distance(color, colorMagenta);
     float cyanDistance = distance(color, colorCyan);
     
     vec4 finalColor = vec4(0.0);
     
     float colorDistance = min(magentaDistance, cyanDistance);
     colorDistance = min(colorDistance, whiteDistance);
     colorDistance = min(colorDistance, blackDistance);
     
     if (colorDistance == blackDistance) {
         finalColor = colorBlack;
     } else if (colorDistance == whiteDistance) {
         finalColor = colorWhite;
     } else if (colorDistance == cyanDistance) {
         finalColor = colorCyan;
     } else {
         finalColor = colorMagenta;
     }
        
     gl_FragColor = finalColor;
  
}
