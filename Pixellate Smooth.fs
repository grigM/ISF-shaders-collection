/*
{
  "DESCRIPTION": "",
	"CATEGORIES": [
		"ripple distortion effect"
	],
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "cell_size",
      "TYPE" : "float",
      "MAX" : 0.5,
      "DEFAULT" : 0.125,
      "MIN" : 0.001
    },
    {
      "VALUES" : [
        0,
        1
      ],
      "NAME" : "shape",
      "TYPE" : "long",
      "DEFAULT" : 0,
      "LABELS" : [
        "Square",
        "Rectangle"
      ]
    },
    {
      "NAME" : "hSmooth",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0,
      "MIN" : 0,
      "IDENTITY" : 1
    },
    {
      "NAME" : "vSmooth",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 1,
      "MIN" : 0,
      "IDENTITY" : 1
    }
  ],
  "CREDIT" : "by VIDVOX"
}
*/

#ifndef GL_ES
float distance (vec2 center, vec2 pt)
{
	float tmp = pow(center.x-pt.x,2.0)+pow(center.y-pt.y,2.0);
	return pow(tmp,0.5);
}
#endif

void main()
{
	
	//	At 0.0, or full smoothness just do a pass-thru
	if ((cell_size == 0.0)||((hSmooth == 1.0)&&(vSmooth == 1.0)))	{
		gl_FragColor = IMG_THIS_PIXEL(inputImage);
	}
	// CALCULATE EDGES OF CURRENT CELL
	else	{
		// Position of current pixel
		vec2 xy = isf_FragNormCoord;

		// Left and right of tile
		float CellWidth = cell_size;
		float CellHeight = cell_size;
		if (shape==0)	{
			CellHeight = cell_size * RENDERSIZE.x / RENDERSIZE.y;
		}

		float x1 = floor(xy.x / CellWidth)*CellWidth;
		float x2 = clamp((ceil(xy.x / CellWidth)*CellWidth), 0.0, 1.0);
		// Top and bottom of tile
		float y1 = floor(xy.y / CellHeight)*CellHeight;
		float y2 = clamp((ceil(xy.y / CellHeight)*CellHeight), 0.0, 1.0);
		x1 = mix(x1,xy.x,hSmooth);
		y1 = mix(y1,xy.y,vSmooth);
		x2 = mix(x2,xy.x,hSmooth);
		y2 = mix(y2,xy.y,vSmooth);
		
		// GET AVERAGE CELL COLOUR
		vec2 pt = vec2(x1, (y1+y2)/2.0);
		vec4 lp = IMG_NORM_PIXEL(inputImage, pt);
		pt = vec2(x2, (y1+y2)/2.0);
		vec4 rp = (hSmooth == 1.0) ? lp : IMG_NORM_PIXEL(inputImage, pt);
		vec4 tp = IMG_NORM_PIXEL(inputImage, vec2((x1+x2)/2.0, y1));
		vec4 bp = (vSmooth == 1.0) ? tp : IMG_NORM_PIXEL(inputImage, vec2((x1+x2)/2.0, y2));
		vec4 cp = IMG_NORM_PIXEL(inputImage, vec2((x1+x2)/2.0, (y1+y2)/2.0));
		
		// Average left and right pixels
		vec4 avgX = (lp + rp) / 2.0;
		// Average top and bottom pixels
		vec4 avgY = (tp + bp) / 2.0;
		// Centre pixel
		vec4 avgC = cp;
		vec4 avgClr = (avgX+avgY+avgC) / 3.0;

		gl_FragColor = vec4(avgClr);
	}
}
