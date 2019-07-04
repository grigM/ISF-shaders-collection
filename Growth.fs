/*{
	"CREDIT": "by mojovideotech",
  	"CATEGORIES" : [
  		"Distortion Effect",
  		"Geometry Adjustment",
    	"spiral",
    	"logarithmic",
    	"coordinatetransform"
  ],
  "DESCRIPTION" : "Transformation from screen-coordinates to logarithmic spiral with golden angle.",
  "ISFVSN" : "2",
  "INPUTS" : [
   	{
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
		{
      "NAME": "bending",
      "TYPE": "float",
      "MAX": 1.0,
      "MIN": 0.0001,
      "DEFAULT": 0.2
    },
    {
      "NAME": "number",
      "TYPE": "float",
      "MAX": 18.0,
      "MIN": 1.0,
      "DEFAULT": 6.0
    }
  ]
}
*/

////////////////////////////////////////////////////////////
// LogTransWarpSpiral  by mojovideotech
//
// based on :
// shadertoy.com\/Msd3Dn
// Logarithmic Spiral Transform - 2015-12-02 by Jakob Thomsen
//
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0
////////////////////////////////////////////////////////////


#define 	twpi  	6.2831853  	// two pi, 2*pi


void main() {
	float T = TIME * 0.02;
    vec2 p = (gl_FragCoord.xy+gl_FragCoord.xy-RENDERSIZE.xy)/RENDERSIZE.x;
	p = vec2(0.0, T - log(length(p.xy))*bending) + atan(p.y,p.x ) / twpi; 
   	p.x = ceil(p.y) - p.x;
    p.x *= number;
    gl_FragColor = IMG_NORM_PIXEL(inputImage,fract(p.yx+T));
}
