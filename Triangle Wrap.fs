/*
{
  "CATEGORIES" : [
    "Geometry Adjustment"
  ],
  "DESCRIPTION" : "Wraps the input image into a triangle shape",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "MAX" : 1,
      "NAME" : "preRotateAngle",
      "TYPE" : "float",
      "DEFAULT" : 0,
      "MIN" : 0
    },
    {
      "MAX" : 2,
      "NAME" : "triangleSize",
      "TYPE" : "float",
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "MAX" : 1,
      "NAME" : "triangleAngle",
      "TYPE" : "float",
      "DEFAULT" : 0.5,
      "MIN" : 0
    },
    {
      "MAX" : 2,
      "NAME" : "baseWidth",
      "TYPE" : "float",
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "MAX" : 1,
      "NAME" : "angleShift",
      "TYPE" : "float",
      "DEFAULT" : 0,
      "MIN" : 0
    },
    {
      "LABELS" : [
        "None",
        "Repeat",
        "Reflect"
      ],
      "NAME" : "repeatStyle",
      "TYPE" : "long",
      "DEFAULT" : 0,
      "VALUES" : [
        0,
        1,
        2
      ]
    }
  ],
  "ISFVSN" : "2",
  "VSN" : null,
  "CREDIT" : "VIDVOX"
}
*/

const float pi = 3.14159265359;


//	note that this works on normalized points, but respects aspect ratio
vec2 rotatePoint(vec2 pt, float rot)	{
	vec2 returnMe = pt * RENDERSIZE;

	float r = distance(RENDERSIZE/2.0, returnMe);
	float a = atan ((returnMe.y-RENDERSIZE.y/2.0),(returnMe.x-RENDERSIZE.x/2.0));

	returnMe.x = r * cos(a + 2.0 * pi * rot - pi) + 0.5;
	returnMe.y = r * sin(a + 2.0 * pi * rot - pi) + 0.5;
	
	returnMe = returnMe / RENDERSIZE + vec2(0.5);
	
	return returnMe;
}

vec2 rotatePointNorm(vec2 pt, float rot)	{
	vec2 returnMe = pt;

	float r = distance(vec2(0.50), returnMe);
	float a = atan((returnMe.y-0.5),(returnMe.x-0.5));

	returnMe.x = r * cos(a + 2.0 * pi * rot - pi) + 0.5;
	returnMe.y = r * sin(a + 2.0 * pi * rot - pi) + 0.5;
	
	returnMe = returnMe;
	
	return returnMe;
}

//	borrowed from pixel spirit deck!
float triSDF(vec2 st) {
    st = (st*2.-1.)*2.;
    return max(abs(st.x) * 0.866025 + st.y * 0.5, -st.y * 0.5);
}

void main()	{
	vec4 returnMe = vec4(0.0);
	float val = 0.0;
	vec2 u_resolution = RENDERSIZE;
    vec2 st = gl_FragCoord.xy/u_resolution;
    
    st = rotatePoint(st, triangleAngle);
    //	size
    st -= 0.5;
    st /= triangleSize;
    st.x /= baseWidth;
    st += 0.5;
   
    st = mix(vec2((st.x*u_resolution.x/u_resolution.y)-(u_resolution.x*.5-u_resolution.y*.5)/u_resolution.y,st.y), 
             vec2(st.x,st.y*(u_resolution.y/u_resolution.x)-(u_resolution.y*.5-u_resolution.x*.5)/u_resolution.x), 
             step(u_resolution.x,u_resolution.y));
	
    val += triSDF(st);

	float r = val;
	if (repeatStyle == 1)
		r = mod(r,1.0);
	else if (repeatStyle == 2)	{
		r = mod(r,2.0);
		r = (r > 1.0) ? 2.0 - r : r;	
	}
	
	float a = 0.0;
	if (r <= 1.0)	{
		vec2	cnt = vec2(0.5,0.5);
		a = (atan(cnt.y-st.y,cnt.x-st.x) + pi) / (2.0*pi);
		a = mod(a + angleShift, 1.0);
		vec2 pt = vec2(a,r);
		pt = rotatePointNorm(pt,preRotateAngle);
		//returnMe = vec4(r,a,0.0,1.0);
		returnMe = IMG_NORM_PIXEL(inputImage,pt);
	}

    gl_FragColor = returnMe;
}
