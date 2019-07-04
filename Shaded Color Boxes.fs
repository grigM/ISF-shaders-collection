/*
{
  "CATEGORIES" : [
    "Generator"
  ],
  "DESCRIPTION" : "Draws some colored squares",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "startColor",
      "TYPE" : "color",
      "DEFAULT" : [
        0.66604596376419067,
        0.39758062362670898,
        0.12855997681617737,
        1
      ]
    },
    {
      "NAME" : "endColor",
      "TYPE" : "color",
      "DEFAULT" : [
        0.9623754620552063,
        0.71732699871063232,
        0.41749665141105652,
        1
      ]
    },
    {
      "NAME" : "xShift",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 0,
      "MIN" : -2
    },
    {
      "NAME" : "yShift",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 0,
      "MIN" : -2
    },
    {
      "NAME" : "divisionColor",
      "TYPE" : "color",
      "DEFAULT" : [
        0.12271291762590408,
        0.077580191195011139,
        0.040417216718196869,
        1
      ]
    },
    {
      "LABELS" : [
        "0",
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
      "NAME" : "majorDivisions",
      "TYPE" : "long",
      "DEFAULT" : 3,
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
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8"
      ],
      "NAME" : "minorHDivisions",
      "TYPE" : "long",
      "DEFAULT" : 2,
      "VALUES" : [
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8
      ]
    },
    {
      "LABELS" : [
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8"
      ],
      "NAME" : "minorVDivisions",
      "TYPE" : "long",
      "DEFAULT" : 2,
      "VALUES" : [
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8
      ]
    },
    {
      "NAME" : "majorDivisionLineWidth",
      "TYPE" : "float",
      "MAX" : 5,
      "DEFAULT" : 3,
      "MIN" : 1
    },
    {
      "NAME" : "square",
      "TYPE" : "bool",
      "DEFAULT" : true
    }
  ],
  "CREDIT" : "VIDVOX"
}
*/


const float minorDivisionLineWidth = 1.0;


void main()	{
	vec4		inputPixelColor = startColor;
	vec2		loc = gl_FragCoord.xy;
	vec2		divisionSize = (square) ? vec2(max(RENDERSIZE.x,RENDERSIZE.y)) : RENDERSIZE;
	divisionSize = (divisionSize - majorDivisionLineWidth) / (1.0 + float(majorDivisions));
	vec2		modLoc = mod(loc,divisionSize);
	if ((modLoc.x < majorDivisionLineWidth)||(modLoc.y < majorDivisionLineWidth))	{
		inputPixelColor = divisionColor;
	}
	if (minorHDivisions > 0)	{
		vec2	locDivisionSize = (divisionSize) / (1.0+float(minorHDivisions));
		modLoc = mod(loc,locDivisionSize);
		if (modLoc.x < minorDivisionLineWidth)	{
			inputPixelColor = divisionColor;
		}
	}
	if (minorVDivisions > 0)	{
		vec2	locDivisionSize = (divisionSize) / (1.0+float(minorVDivisions));
		modLoc = mod(loc,locDivisionSize);
		if (modLoc.y < minorDivisionLineWidth)	{
			inputPixelColor = divisionColor;
		}
	}
	if (inputPixelColor == startColor)	{
		vec2	majorIndex = floor(loc / divisionSize);
		majorIndex += vec2(xShift,yShift);
		float	val = (majorIndex.x+majorIndex.y) / (2.0*float(2+majorDivisions));
		vec2	locDims = vec2((1.0+float(minorHDivisions)),(1.0+float(minorVDivisions)));
		vec2	locDivisionSize = divisionSize / locDims;
		vec2	minorIndex = floor((loc - majorIndex * divisionSize) / locDivisionSize);
		val += (minorIndex.x+minorIndex.y) / (2.0+float(minorVDivisions+minorHDivisions));
		inputPixelColor = mix(startColor,endColor,val);
	}

	gl_FragColor = inputPixelColor;
}
