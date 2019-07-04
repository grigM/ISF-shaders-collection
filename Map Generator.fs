/*
{
  "CATEGORIES" : [
    "Generator"
  ],
  "DESCRIPTION" : "Generates a map",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "randomness",
      "TYPE" : "float",
      "MAX" : 0.9,
      "DEFAULT" : 0.5,
      "MIN" : 0
    },
    {
      "NAME" : "resetGrid",
      "TYPE" : "bool",
      "DEFAULT" : 0,
      "LABEL" : "Reset Grid"
    },
    {
      "NAME" : "fillAllNow",
      "TYPE" : "bool",
      "DEFAULT" : 0
    },
    {
      "NAME" : "seaLevel",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "MIN" : 0
    },
    {
      "NAME" : "temperature",
      "TYPE" : "float",
      "MAX" : 8,
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "NAME" : "elevation",
      "TYPE" : "float",
      "MAX" : 8,
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "NAME" : "vegetation",
      "TYPE" : "float",
      "MAX" : 8,
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "NAME" : "patternLevel",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "LABELS" : [
        "Map",
        "Elevation",
        "Temperature"
      ],
      "NAME" : "displayMode",
      "TYPE" : "long",
      "DEFAULT" : 0,
      "VALUES" : [
        0,
        1,
        2
      ]
    },
    {
      "LABELS" : [
        "8x8",
        "16x16",
        "32x32"
      ],
      "NAME" : "gridCount",
      "TYPE" : "long",
      "DEFAULT" : 8,
      "VALUES" : [
        8,
        16,
        32
      ]
    }
  ],
  "PASSES" : [
    {
      "TARGET": "nextPixel",
      "persistent": true,
      "WIDTH" : "1",
      "HEIGHT" : "1"
    },
    {
      "TARGET": "dataHistory",
      "persistent": true
    },
    {
    	
    }
  ],
  "CREDIT" : "VIDVOX"
}
*/


//	thanks to https://trasevol.dog/2017/09/01/di19/
//	(via https://twitter.com/Andy_Makes/status/976615859069300736)


//const float gridSize = 32.0;

const float tau = 6.28318530718;

const vec4 deepWaterColor = vec4(0.0,0.1,0.5,1.0);
const vec4 waterColor = vec4(0.3,0.4,0.9,1.0);
const vec4 sandColor = vec4(0.95,0.7,0.5,1.0);
const vec4 aridColor = vec4(0.75,0.8,0.5,1.0);
const vec4 grassColor = vec4(0.3,0.8,0.3,1.0);
const vec4 jungleColor = vec4(0.1,0.5,0.2,1.0);
const vec4 rockColor = vec4(0.75,0.75,0.75,1.0);
const vec4 snowColor = vec4(0.95,0.95,0.95,1.0);
const vec4 tundraColor = vec4(0.4,0.55,0.65,1.0);


float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}


vec4 randColor(vec2 co)	{
	vec4	c = vec4(1.0);
	c.r = rand(co * 0.2341);
	c.g = rand(co * 0.72843);
	c.b = rand(co * 0.89342);
	return c;
}

vec4 colorForTileTypeIndex(int t)	{
	vec4	c = vec4(0.0);

	//	water
	if (t == 0)
		c = deepWaterColor;
	else if (t == 1)
		c = waterColor;
	else if (t == 2)
		c = sandColor;	
	else if (t == 3)
		c = grassColor;	
	else if (t == 4)
		c = jungleColor;	
	else if (t == 5)
		c = rockColor;
	else if (t == 6)
		c = snowColor;
	
	return c;
}

int tileTypeIndexForColor(vec4 c)	{
	int		index = -1;
	
	if (c == deepWaterColor)
		index = 0;
	else if (c == waterColor)
		index = 1;
	else if (c == sandColor)
		index = 2;
	else if (c == grassColor)
		index = 3;
	else if (c == jungleColor)
		index = 4;
	else if (c == rockColor)
		index = 5;
	else if (c == snowColor)
		index = 6;
	
	return index;	
}

vec4 randTileColor(vec2 co)	{
	int		randIndex = int(6.25*rand(co));
	return colorForTileTypeIndex(randIndex);
}



vec4 tileColorForSurroundingColors(vec4 u, vec4 b, vec4 l, vec4 r, vec2 co)	{
	vec4	c = vec4(0.0);
	float	alphaSum = u.a + b.a + l.a + r.a;
	if (alphaSum == 0.0)	{
		c = randColor(co);
		//c = randTileColor(co);
	}
	else if (alphaSum == 1.0)	{
		c = randColor(co);
		vec3	avg = (u.rgb * u.a + b.rgb * b.a + l.rgb * l.a + r.rgb * r.a) / (alphaSum);
		c.rgb = mix(avg,c.rgb,0.1);
		c.a = 1.0;
	}
	else	{
		c = randColor(co);
		vec3	avg = (u.rgb * u.a + b.rgb * b.a + l.rgb * l.a + r.rgb * r.a) / alphaSum;
		c.rgb = avg;
		c.rgb = mix(avg,c.rgb,0.1);
		c.a = 1.0;
	}
	return c;
}

vec4 tileColorForVariables(vec4 c, vec2 loc)	{
	vec4	modColor = c;
	float	distanceFromPole = (loc.y > 0.5) ? 1.0 - loc.y : loc.y;
	modColor.r *= temperature;
	modColor.b *= elevation;
	modColor.g *= vegetation;
	vec4	returnMe = modColor;
	//	elevation mode
	if (displayMode == 1)	{
		if ((seaLevel > 0.0)&&(returnMe.b < seaLevel))	{
			returnMe = vec4(0.0,0.0,0.25+0.75*returnMe.b / seaLevel,1.0);
		}
		else if (seaLevel < 1.0)	{
			float	val = 0.2 + ((returnMe.b - seaLevel) / (1.0 - seaLevel)) * 0.8;
			val = floor(val * 5.0) / 5.0;
			returnMe = vec4(val,val,val,1.0);
		}
	}
	//	temperature mode
	else if (displayMode == 2)	{
		if ((seaLevel > 0.0)&&(modColor.b < seaLevel))	{
			returnMe.b = returnMe.r;
			returnMe.r = 0.0;
			returnMe.g = 0.0;
			returnMe.rgb = 0.1 + floor(returnMe.rgb * 5.0) / 5.0;
		}
		else if (seaLevel < 1.0)	{
			if (returnMe.r < 0.333)	{
				returnMe.g = returnMe.r * 3.0;
				returnMe.r = 0.0;
			}
			else if (modColor.r < 0.667)	{
				returnMe.r = (modColor.r - 0.333) * 3.0;
				returnMe.g = 1.0;
			}
			else	{
				returnMe.g = 1.0 - (modColor.r - 0.677) * 3.0;
				returnMe.r = 1.0;
			}
			returnMe.b = 0.0;
			returnMe.rgb = floor(returnMe.rgb * 5.0) / 5.0;
		}
	}
	//	otherwise…
	//	if deep below sea level, deep water
	else if (modColor.b < seaLevel / 3.0)	{
		returnMe = deepWaterColor;	
	}
	//	if below sea level, water
	else if (modColor.b <= seaLevel)	{
		returnMe = waterColor;	
	}
	//	if very verry high up, either rock or snow
	else if (modColor.b > 0.9)	{
		if (modColor.r < modColor.b - 0.1)	{
			returnMe = snowColor;
		}
		else	{
			returnMe = rockColor;	
		}
	}
	//	if above the tree line, light grass / tundra or rock, or snow if very cold
	else if (modColor.b > 0.75)	{
		returnMe = (modColor.r > 0.75) ? aridColor : tundraColor;
		returnMe = (modColor.g > 0.5) ? returnMe : rockColor;
		returnMe = (modColor.r > 0.1) ? returnMe : snowColor;
	}
	//	if very hot, sand, unless the greenery is very high, in which case arid
	else if (modColor.r > 0.9)	{
		returnMe = (modColor.r > modColor.g) ? sandColor : aridColor;
	}
	//	if very cold, snow or tundra
	else if (returnMe.r < 0.1)	{
		returnMe = (1.0 - modColor.r > modColor.g) ? snowColor : tundraColor;
	}
	//	if warm
	else if (modColor.r > 0.5)	{
		returnMe = (modColor.g > 0.75) ? jungleColor : grassColor;
		returnMe = (modColor.g > 0.25) ? returnMe : aridColor;	
	}
	//	if cold
	else if (modColor.r <= 0.5)	{
		returnMe = (modColor.g > 0.75) ? grassColor : aridColor;
		returnMe = (modColor.g > 0.25) ? returnMe : tundraColor;
	}
	else	{
		//	error! unhandled case
		returnMe = vec4(1.0,0.0,0.0,1.0);	
	}
	return returnMe;
}




//	based on https://github.com/hughsk/glsl-dither

float luma(vec3 color)	{
	return (color.r + color.g + color.b) / 3.0;	
}

float luma(vec4 color)	{
	return color.a * (color.r + color.g + color.b) / 3.0;	
}

float dither8x8(vec2 position, float brightness) {
  int x = int(mod(position.x, 8.0));
  int y = int(mod(position.y, 8.0));
  int index = x + y * 8;
  float limit = 0.0;

  if (x < 8) {
    if (index == 0) limit = 0.015625;
    if (index == 1) limit = 0.515625;
    if (index == 2) limit = 0.140625;
    if (index == 3) limit = 0.640625;
    if (index == 4) limit = 0.046875;
    if (index == 5) limit = 0.546875;
    if (index == 6) limit = 0.171875;
    if (index == 7) limit = 0.671875;
    if (index == 8) limit = 0.765625;
    if (index == 9) limit = 0.265625;
    if (index == 10) limit = 0.890625;
    if (index == 11) limit = 0.390625;
    if (index == 12) limit = 0.796875;
    if (index == 13) limit = 0.296875;
    if (index == 14) limit = 0.921875;
    if (index == 15) limit = 0.421875;
    if (index == 16) limit = 0.203125;
    if (index == 17) limit = 0.703125;
    if (index == 18) limit = 0.078125;
    if (index == 19) limit = 0.578125;
    if (index == 20) limit = 0.234375;
    if (index == 21) limit = 0.734375;
    if (index == 22) limit = 0.109375;
    if (index == 23) limit = 0.609375;
    if (index == 24) limit = 0.953125;
    if (index == 25) limit = 0.453125;
    if (index == 26) limit = 0.828125;
    if (index == 27) limit = 0.328125;
    if (index == 28) limit = 0.984375;
    if (index == 29) limit = 0.484375;
    if (index == 30) limit = 0.859375;
    if (index == 31) limit = 0.359375;
    if (index == 32) limit = 0.0625;
    if (index == 33) limit = 0.5625;
    if (index == 34) limit = 0.1875;
    if (index == 35) limit = 0.6875;
    if (index == 36) limit = 0.03125;
    if (index == 37) limit = 0.53125;
    if (index == 38) limit = 0.15625;
    if (index == 39) limit = 0.65625;
    if (index == 40) limit = 0.8125;
    if (index == 41) limit = 0.3125;
    if (index == 42) limit = 0.9375;
    if (index == 43) limit = 0.4375;
    if (index == 44) limit = 0.78125;
    if (index == 45) limit = 0.28125;
    if (index == 46) limit = 0.90625;
    if (index == 47) limit = 0.40625;
    if (index == 48) limit = 0.25;
    if (index == 49) limit = 0.75;
    if (index == 50) limit = 0.125;
    if (index == 51) limit = 0.625;
    if (index == 52) limit = 0.21875;
    if (index == 53) limit = 0.71875;
    if (index == 54) limit = 0.09375;
    if (index == 55) limit = 0.59375;
    if (index == 56) limit = 1.0;
    if (index == 57) limit = 0.5;
    if (index == 58) limit = 0.875;
    if (index == 59) limit = 0.375;
    if (index == 60) limit = 0.96875;
    if (index == 61) limit = 0.46875;
    if (index == 62) limit = 0.84375;
    if (index == 63) limit = 0.34375;
  }

  return brightness < limit ? 0.0 : 1.0;
}

bool isWater(vec4 c)	{
	return ((c == deepWaterColor)||(c == waterColor));	
}

vec4 noisyEdge(vec2 pos, float amp, vec4 c1, vec4 c2)	{
	//float	val = (isWater(c1)) ? rand(pos + TIME) : rand(pos);
	float	val = rand(pos);
	if (val < 0.5)
		return c1;
	else
		return c2;
}

void main()	{
	vec4		returnMe = vec4(0.0);
	float		gridSize = float(gridCount);
	if (PASSINDEX == 0)	{
		vec4		lastIndex = (resetGrid == true) ? vec4(0.0) : IMG_THIS_PIXEL(nextPixel);
		//if ((lastIndex.a != 1.0)&&(step == true))	{
		if (lastIndex.b != 1.0)	{
			//	pick a random tile to replace
			returnMe.a = lastIndex.a;
			returnMe.b = lastIndex.b;
			float		gridSquared = pow(gridSize,2.0);
			float		nextIndex = -1.0;
			//float		i = 0.0;
			float		offset = 1.0 / gridSize;
			float		maxCount = gridSquared;
			
			for (float i = 0.0;i < 1025.0 ;++i)	{
				nextIndex = (lastIndex.b > randomness) ? i : floor(gridSquared*rand(vec2(TIME,i)));
				//nextIndex = i;
				
				vec2	loc = RENDERSIZE * vec2(offset + mod(nextIndex,gridSize),offset + floor(nextIndex / gridSize));
				loc = loc / gridSize;
				vec4	tmpPixel = IMG_PIXEL(dataHistory,loc);
				
				if (tmpPixel.a == 0.0)	{
					float	colIndex = mod(nextIndex, gridSize);
					float	rowIndex = floor(nextIndex / gridSize);
					returnMe = vec4(colIndex/gridSize,rowIndex/gridSize,lastIndex.b+1.0/255.0,1.0);
					break;
				}
				if (i > maxCount)	{
					break;
				}
			}
			
		}
		else	{
			returnMe = lastIndex;
		}
		//vec4	tmpPixel = IMG_PIXEL(nextPixel,vec2(0.0,0.0));
		//float	nextIndex = 63.0*tmpPixel.r + 1.0;
		//returnMe = (resetGrid) ? vec4(0.0) : vec4(nextIndex / 63.0);
		//returnMe = vec4(1.0);
	}
	else if (PASSINDEX == 1)	{
		if (resetGrid == true)	{
			returnMe = vec4(0.0,0.0,0.0,0.0);
		}
		else	{
			vec4	oldPixel = IMG_THIS_PIXEL(dataHistory);
			if ((oldPixel.r == 0.0)&&(oldPixel.b== 0.0))	{
				float	gridSquared = pow(gridSize,2.0);
				vec4	tmpPixel = IMG_PIXEL(nextPixel,vec2(0.0,0.0));
				float	nextIndex = gridSquared*tmpPixel.g + gridSize*tmpPixel.r;
				float	colIndex = floor(isf_FragNormCoord.x*gridSize);
				float	rowIndex = floor(isf_FragNormCoord.y*gridSize);
				vec2	loc = vec2(colIndex,rowIndex) / gridSize;
				bool	doAllFill = ((fillAllNow == true)||(tmpPixel.b >= randomness)) ? true : false;
				if ((doAllFill == true)||(floor(nextIndex) == floor(rowIndex * gridSize + colIndex)))	{
					float		offset = 1.0 / gridSize;
					vec4		u = IMG_NORM_PIXEL(dataHistory,isf_FragNormCoord + vec2(0.0,-offset));
					vec4		b = IMG_NORM_PIXEL(dataHistory,isf_FragNormCoord + vec2(0.0,offset));
					vec4		l = IMG_NORM_PIXEL(dataHistory,fract(isf_FragNormCoord + vec2(-offset,0.0)));
					vec4		r = IMG_NORM_PIXEL(dataHistory,fract(isf_FragNormCoord + vec2(offset,0.0)));
					//returnMe = tileColorForSurroundingColors(u,b,l,r,vec2(TIME,rowIndex*colIndex+rowIndex+colIndex+1.232));
					returnMe = returnMe = randTileColor(loc+vec2(TIME,1.232));
				}
				else	{
					returnMe = oldPixel;
				}
			}
			else	{
				returnMe = oldPixel;
			}
		}
	}
	else if (PASSINDEX == 2)	{
		vec4	tmp = IMG_NORM_PIXEL(dataHistory,isf_FragNormCoord);
		//	based on the location and the randomized data, fill in this color
		if (tmp.a > 0.0)	{
			float	colIndex = floor(isf_FragNormCoord.x*gridSize);
			float	rowIndex = floor(isf_FragNormCoord.y*gridSize);
			vec2	loc = vec2(colIndex,rowIndex) / gridSize;
			
			returnMe = tileColorForVariables(tmp,isf_FragNormCoord);

			if (patternLevel > 0.0)	{
				float	b = 1.0-tmp.b*elevation;
				vec4	patColor = returnMe;
				patColor.rgb = patColor.rgb * (1.0 - 0.2 * patternLevel);
				b = (b < 0.0) ? 1.0 + b : b;
				float	pat = dither8x8((isf_FragNormCoord)*RENDERSIZE,b);
				returnMe = mix(returnMe,patColor,pat);
			}
		}
	}
	gl_FragColor = returnMe;
}
