/*{
	"DESCRIPTION": "MissEtikate FX",
	"CREDIT": "by IMIMOT (ported from https://github.com/BradLarson/GPUImage)",
	"CATEGORIES": [
		"Film"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		}, 
		{
			"NAME": "intensity",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
				
		}
	],
	"IMPORTED": {
		"lookup": {
			"PATH": "lookup_miss_etikate.png"
		}
	}
}*/

void main()
{
     vec4 textureColor = IMG_THIS_NORM_PIXEL(inputImage);
     
     float blueColor = textureColor.b * 63.0;
     
     vec2 quad1;
     quad1.y = floor(floor(blueColor) / 8.0);
     quad1.x = floor(blueColor) - (quad1.y * 8.0);
     
     vec2 quad2;
     quad2.y = floor(ceil(blueColor) / 8.0);
     quad2.x = ceil(blueColor) - (quad2.y * 8.0);
     
     vec2 texPos1;
     texPos1.x = (quad1.x * 0.125) + 0.5/512.0 + ((0.125 - 1.0/512.0) * textureColor.r);
     texPos1.y = (quad1.y * 0.125) + 0.5/512.0 + ((0.125 - 1.0/512.0) * textureColor.g);
     
     vec2 texPos2;
     texPos2.x = (quad2.x * 0.125) + 0.5/512.0 + ((0.125 - 1.0/512.0) * textureColor.r);
     texPos2.y = (quad2.y * 0.125) + 0.5/512.0 + ((0.125 - 1.0/512.0) * textureColor.g);
     
     vec4 newColor1 = IMG_NORM_PIXEL(lookup, texPos1);
     vec4 newColor2 = IMG_NORM_PIXEL(lookup, texPos2);
     
     vec4 newColor = mix(newColor1, newColor2, fract(blueColor));
     gl_FragColor = mix(textureColor, vec4(newColor.rgb, textureColor.w), intensity);
}
