/*{
	"CREDIT": "by VIDVOX",
	"CATEGORIES": [
		"Glitch"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "altImage",
			"TYPE": "image"
		},
		{
			"NAME": "glitch_size",
			"LABEL": "Size",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 0.5,
			"DEFAULT": 0.1
		},
		{
			"NAME": "glitch_horizontal",
			"LABEL": "Horizontal Amount",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.2
		},
		{
			"NAME": "glitch_vertical",
			"LABEL": "Vertical Amount",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "randomize_size",
			"LABEL": "Randomize Size",
			"TYPE": "bool",
			"DEFAULT": 1.0
		},
		{
			"NAME": "auto_time",
			"LABEL": "Auto Time",
			"TYPE": "bool",
			"DEFAULT": 1.0
		},
		{
			"NAME": "manual_time",
			"LABEL": "Manual Time",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "randomize_zoom",
			"LABEL": "Randomize Zoom",
			"TYPE": "bool",
			"DEFAULT": 0.0
		},
		{
			"NAME": "use_alt_image",
			"LABEL": "Use Alt Image",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.25
		},
		{
			"NAME": "composition_mode",
			"LABEL": "Style",
			"VALUES": [
				0,
				1,
				2,
				3,
				4,
				5
			],
			"LABELS": [
				"Replace",
				"Add",
				"Min",
				"Max",
				"Difference",
				"Random"
			],
			"DEFAULT": 0,
			"TYPE": "long"
		},
		{
			"NAME": "offset",
			"LABEL": "Offset",
			"TYPE": "point2D",
			"DEFAULT": [
				0,
				0
			]
		}
	]
}*/

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main()
{
	vec2 xy;
	vec4 returnMe;
	bool shifted = false;
	float currentTime = (auto_time) ? TIME : manual_time;
	
	xy.x = vv_FragNormCoord[0];
	xy.y = vv_FragNormCoord[1];
	
	//	quantize the xy to the glitch_amount size
	//xy = floor(xy / glitch_size) * glitch_size;
	vec2 random;

	float local_glitch_size = glitch_size;
	float random_offset = 0.0;
	
	if (randomize_size)	{
		random_offset = mod(rand(vec2(currentTime, currentTime)), 1.0);
		local_glitch_size = random_offset * glitch_size;
	}
	
	if (local_glitch_size > 0.0)	{
		random.x = rand(vec2(floor(random_offset + xy.y / local_glitch_size) * local_glitch_size, currentTime));
		random.y = rand(vec2(floor(random_offset + xy.x / local_glitch_size) * local_glitch_size, currentTime));
	}
	else	{
		random.x = rand(vec2(xy.x, currentTime));
		random.y = rand(vec2(xy.y, currentTime));
	}
	
	if (randomize_zoom)	{
		if ((random.x < glitch_horizontal)&&(random.y < glitch_vertical))	{
			float level = rand(vec2(random.x, random.y)) / 5.0 + 0.90;
			xy = (xy - vec2(0.5))*(1.0/level) + vec2(0.5);
		}
		else if (random.x < glitch_horizontal)	{
			float level = (random.x) + 0.98;
			xy = (xy - vec2(0.5))*(1.0/level) + vec2(0.5);
		}
		else if (random.y < glitch_vertical)	{
			float level = (random.y) + 0.98;
			xy = (xy - vec2(0.5))*(1.0/level) + vec2(0.5);
		}
	}
	vec2 shift;
	//	if doing a horizontal glitch do a random shift
	if ((random.x < glitch_horizontal)&&(random.y < glitch_vertical))	{
		shift = (offset / RENDERSIZE - 0.5);
		shift = shift * rand(shift + random);
		xy.x = mod(xy.x + random.x, 1.0);
		xy.y = mod(xy.y + random.y, 1.0);
		xy = xy + shift;
		shifted = true;
	}
	else if (random.x < glitch_horizontal)	{
		shift = (offset / RENDERSIZE - 0.5);
		shift = shift * rand(shift + random);
		xy = mod(xy + vec2(0.0, random.x) + shift, 1.0);
		shifted = true;
	}
	else if (random.y < glitch_vertical)	{
		shift = (offset / RENDERSIZE - 0.5);
		shift = shift * rand(shift + random);
		xy = mod(xy + vec2(random.y, 0.0) + shift, 1.0);
		shifted = true;
	}
	
	if ((shifted) && (rand(vec2(currentTime, shift.x * shift.y)) < use_alt_image))
		returnMe = IMG_NORM_PIXEL(altImage, xy);
	else
		returnMe = IMG_NORM_PIXEL(inputImage, xy);
	
	//	if it isn't the replace mode, blend it!
	int comp_mode = composition_mode;
	if (composition_mode == 5)	{
		comp_mode = int(floor(6.0 * rand(vec2(currentTime, shift.x + shift.y))));
	}
	if (comp_mode == 1)	{
		vec4 original = IMG_THIS_NORM_PIXEL(inputImage);
		returnMe = returnMe + original;
	}
	else if (comp_mode == 2)	{
		vec4 original = IMG_THIS_NORM_PIXEL(inputImage);
		returnMe = min(returnMe, original);		
	}
	else if (comp_mode == 3)	{
		vec4 original = IMG_THIS_NORM_PIXEL(inputImage);
		returnMe = max(returnMe, original);		
	}
	else if (comp_mode == 4)	{
		vec4 original = IMG_THIS_NORM_PIXEL(inputImage);
		returnMe.rgb = abs(original.rgb - returnMe.rgb);
	}
	
	gl_FragColor = returnMe;
}
