/*
{
  "CATEGORIES" : [
    "Generator"
  ],
  "DESCRIPTION" : "",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "pointCount",
      "TYPE" : "float",
      "MAX" : 20,
      "DEFAULT" : 5,
      "LABEL" : "Point Count",
      "MIN" : 3
    },
    {
      "NAME" : "pointRadius",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.125,
      "LABEL" : "Radius",
      "MIN" : 0
    },
    {
      "NAME" : "pointRotation",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.25,
      "LABEL" : "Rotation",
      "MIN" : 0
    },
    {
      "NAME" : "seed",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.1971,
      "MIN" : 0
    },
    {
      "NAME" : "randAmount",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.3971,
      "MIN" : 0
    }
  ],
  "CREDIT" : ""
}
*/

float sign(vec2 p1, vec2 p2, vec2 p3)
{
	return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
}

bool PointInTriangle(vec2 pt, vec2 v1, vec2 v2, vec2 v3)
{
	bool b1, b2, b3;

	b1 = sign(pt, v1, v2) < 0.0;
	b2 = sign(pt, v2, v3) < 0.0;
	b3 = sign(pt, v3, v1) < 0.0;

	return ((b1 == b2) && (b2 == b3));
}

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

const float pi = 3.14159265359;

void main()	{
	vec4		inputPixelColor = vec4(0.0);
	vec2		lastEndPoint = vec2(0.5);
	vec2		loc = isf_FragNormCoord;
	float		angleInc = 1.0 / pointCount;
	float		rotAmount = pointRotation * 2.0 * pi;
	vec2		cpt = vec2(0.5);
	cpt.x += rand(vec2(0.74234+seed,(1.9711)*seed+1.3651)) * randAmount - randAmount / 2.0;
	cpt.y += rand(vec2((1.654)*seed+0.97265,3.0383+seed)) * randAmount - randAmount / 2.0;
	
	for(float i = 0.0; i < 20.0; ++i)	{
		if (i > pointCount)
			break;
		vec2 pt1 = cpt;
		vec2 pt2 = (i != 0.0) ? lastEndPoint : vec2(0.5) + pointRadius * vec2(cos(rotAmount + i * angleInc * 2.0 * pi),sin(rotAmount + i * angleInc * 2.0 * pi));
		vec2 pt3 = vec2(0.5) + pointRadius * vec2(cos(rotAmount + (1.0+i) * angleInc * 2.0 * pi),sin(rotAmount + (1.0+i) * angleInc * 2.0 * pi));
		
		pt3.x += rand(vec2(i+0.34234+seed,(i+1.0)*seed+1.3651)) * randAmount - randAmount / 2.0;
		pt3.y += rand(vec2((i+1.0)*seed+0.97265,i+3.0383+seed)) * randAmount - randAmount / 2.0;
		
		if (PointInTriangle(loc,pt1,pt2,pt3))	{
			inputPixelColor = vec4(1.0);
			break;
		}
		lastEndPoint = pt3;
	}
	
	gl_FragColor = inputPixelColor;
}
