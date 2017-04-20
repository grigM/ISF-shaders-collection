/*{
	"DESCRIPTION": "Crosshatch FX",
	"CREDIT": "by IMIMOT (ported from https://github.com/BradLarson/GPUImage)",
	"CATEGORIES": [
		"Color Effect"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "intensity",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "filterColor",
			"TYPE": "color",
			"DEFAULT": [
				1.0,
				0.0,
				0.0,
				1.0
			]
		}
		
	]
}*/

const vec3 luminanceWeighting = vec3(0.2125, 0.7154, 0.0721);
 
void main()
{
	
	vec2 textureCoordinate = vv_FragNormCoord;
	
     //desat, then apply overlay blend
     vec4 textureColor = IMG_THIS_PIXEL(inputImage);
     float luminance = dot(textureColor.rgb, luminanceWeighting);
     
     vec4 desat = vec4(vec3(luminance), 1.0);
     
     //overlay
     vec4 outputColor = vec4(
                                  (desat.r < 0.5 ? (2.0 * desat.r * filterColor.r) : (1.0 - 2.0 * (1.0 - desat.r) * (1.0 - filterColor.r))),
                                  (desat.g < 0.5 ? (2.0 * desat.g * filterColor.g) : (1.0 - 2.0 * (1.0 - desat.g) * (1.0 - filterColor.g))),
                                  (desat.b < 0.5 ? (2.0 * desat.b * filterColor.b) : (1.0 - 2.0 * (1.0 - desat.b) * (1.0 - filterColor.b))),
                                  1.0
                                  );
     
     //which is better, or are they equal?
     gl_FragColor = vec4( mix(textureColor.rgb, outputColor.rgb, intensity), textureColor.a);
  
}
