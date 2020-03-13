/*
{
  "CATEGORIES" : [
    "Geometry Adjustment",
    "Stylize"
  ],
  "DESCRIPTION" : "",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "angle",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : -0.25,
      "MIN" : -1
    },
    {
      "NAME" : "centerPt",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
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
      "NAME" : "lineWidth",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.1,
      "MIN" : 0
    },
    {
      "NAME" : "flipH",
      "TYPE" : "bool",
      "DEFAULT" : 1
    },
    {
      "NAME" : "flipV",
      "TYPE" : "bool",
      "DEFAULT" : 1
    }
  ],
  "CREDIT" : "VIDVOX"
}
*/



const float pi = 3.14159265359;


//	returns the distance from pt0 to the line defined by pt1 and pt2
float distancePtToLine(vec2 pt1, vec2 pt2, vec2 pt0)	{
	return ((pt2.y-pt1.y)*pt0.x-(pt2.x-pt1.x)*pt0.y+pt2.x*pt1.y-pt2.y*pt1.x)/(sqrt(pow(pt2.y-pt1.y,2.0)+pow(pt2.x-pt1.x,2.0)));
}

void main()	{
	vec2		loc = isf_FragNormCoord;
	vec4		returnMe = vec4(vec3(0.0),1.0);
	vec2		p1 = centerPt;
	vec2		p2 = p1 + vec2(cos(angle*pi),sin(angle*pi));
	float		val = distancePtToLine(p1,p2,loc);
	vec2		flipLoc = loc;
	flipLoc.x = (flipH) ? 1.0 - flipLoc.x : flipLoc.x;
	flipLoc.y = (flipV) ? 1.0 - flipLoc.y : flipLoc.y;

	if (abs(val) < lineWidth)	{
		vec4	pix1 = IMG_NORM_PIXEL(inputImage,loc);
		vec4	pix2 = IMG_NORM_PIXEL(inputImage,flipLoc);
		returnMe = mix(pix1,pix2,((-val+lineWidth)) / (2.0*lineWidth));
		//returnMe.r = 2.0 * ((val+lineWidth)) / (2.0*lineWidth);
		//returnMe.g = 2.0 - 2.0 * ((val+lineWidth)) / (2.0*lineWidth);
	}
	else if (val > 0.0)	{
		returnMe = IMG_NORM_PIXEL(inputImage,loc);
	}
	else	{
		returnMe = IMG_NORM_PIXEL(inputImage,flipLoc);
	}
	gl_FragColor = returnMe;
}
