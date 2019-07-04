/*
{
  "CATEGORIES" : [
    "Distortion Effect",
    "Geometry Adjustment"
  ],
  "DESCRIPTION" : null,
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "MAX" : 10,
      "NAME" : "zoomLevel",
      "TYPE" : "float",
      "DEFAULT" : 1,
      "MIN" : 1
    },
    {
      "NAME" : "zoomRandomize",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0,
      "MIN" : 0
    },
    {
      "MAX" : [
        1,
        1
      ],
      "NAME" : "center",
      "TYPE" : "point2D",
      "DEFAULT" : [
        0.5,
        0.5
      ],
      "MIN" : [
        0,
        0
      ]
    },
    {
      "NAME" : "centerRandomize",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0,
      "MIN" : 0
    },
    {
      "LABELS" : [
        "Random",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "10",
        "11",
        "12",
        "13",
        "14",
        "15",
        "16"
      ],
      "NAME" : "hRegionCount",
      "TYPE" : "long",
      "DEFAULT" : 4,
      "VALUES" : [
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13,
        14,
        15,
        16
      ]
    },
    {
      "LABELS" : [
        "Random",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "10",
        "11",
        "12",
        "13",
        "14",
        "15",
        "16"
      ],
      "NAME" : "vRegionCount",
      "TYPE" : "long",
      "DEFAULT" : 4,
      "VALUES" : [
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13,
        14,
        15,
        16
      ]
    },
    {
      "NAME" : "rSeed",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.45829999999999999,
      "MIN" : 0
    },
    {
      "NAME" : "zSeed",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.54290000000000005,
      "MIN" : 0
    },
    {
      "NAME" : "cSeed",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.28310000000000002,
      "MIN" : 0
    }
  ],
  "ISFVSN" : "2",
  "VSN" : null,
  "CREDIT" : "by VIDVOX"
}
*/


float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

vec2 rand2(vec2 co){
    return vec2(fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453),fract(sin(dot(co.xy ,vec2(13.9898,79.233))) * 43757.1413));
}


void main() {
	vec2		loc = isf_FragNormCoord;
	vec2		rDims = vec2(1.0);
	rDims.x = (hRegionCount == 0) ? floor(1.0+15.0*rand(vec2(13.5371*rSeed,0.4341))) : float(hRegionCount);
	rDims.y = (vRegionCount == 0) ? floor(1.0+15.0*rand(vec2(12.7653*rSeed,0.5937))) : float(vRegionCount);
	
	vec2		rCenter = mix(center, rand2(vec2(3.183,9.327*cSeed+0.12199)),centerRandomize);
	vec2		modifiedCenter = floor(isf_FragNormCoord * rDims) / rDims + rCenter / rDims;
	float		level = (zoomRandomize == 0.0) ? zoomLevel : mix(zoomLevel,1.0+5.0*rand(vec2(23.91*zSeed+0.129,0.3421)),zoomRandomize);
	
	loc.x = (loc.x - modifiedCenter.x)*(1.0/level) + modifiedCenter.x;
	loc.y = (loc.y - modifiedCenter.y)*(1.0/level) + modifiedCenter.y;

	if ((loc.x < 0.0)||(loc.y < 0.0)||(loc.x > 1.0)||(loc.y > 1.0))	{
		gl_FragColor = vec4(0.0);
	}
	else	{
		gl_FragColor = IMG_NORM_PIXEL(inputImage,loc);
	}
}
