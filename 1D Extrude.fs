/*
{
  "CATEGORIES" : [
    "Dillon0s",
    "Glitch"
  ],
  "DESCRIPTION" : "1d extrude from a given coordinate, with optional symmetry",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "LABELS" : [
        "Vertical",
        "Horizontal"
      ],
      "NAME" : "mode",
      "TYPE" : "long",
      "DEFAULT" : 0,
      "LABEL" : "Mode",
      "VALUES" : [
        0,
        1
      ]
    },
    {
      "NAME" : "cutoff",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "LABEL" : "Sample Position",
      "MIN" : 0
    },
    {
      "NAME" : "flipCutoff",
      "TYPE" : "bool",
      "LABEL" : "Flip Sample Position"
    },
    {
      "NAME" : "flipDirection",
      "TYPE" : "bool",
      "DEFAULT" : false,
      "LABEL" : "Flip Bleed Direction"
    },
    {
      "NAME" : "fillScreen",
      "TYPE" : "bool",
      "DEFAULT" : false,
      "LABEL" : "Fill Screen"
    },
    {
      "NAME" : "symmetry",
      "TYPE" : "bool",
      "DEFAULT" : false,
      "LABEL" : "Toggle Symmetry"
    },
    {
      "LABELS" : [
        "In",
        "Out"
      ],
      "NAME" : "symmetryMode",
      "TYPE" : "long",
      "DEFAULT" : 0,
      "LABEL" : "Inner Symmetry mode",
      "VALUES" : [
        0,
        1
      ]
    }
  ],
  "CREDIT" : "by Dillon O'Sullivan"
}
*/

void main()	{

	vec2 pixelPos;
	pixelPos.x = isf_FragNormCoord[0];
	pixelPos.y = isf_FragNormCoord[1];
	
	
	if ((fillScreen == false) && (mode == 0))
	{	
		if ((!flipDirection) && (!symmetry))
		{
			if((pixelPos.y < cutoff) && (!flipCutoff))
			{
				pixelPos.y = (cutoff);
			} else if ((pixelPos.y > (1.0 - cutoff)) && (flipCutoff))
			{
				pixelPos.y = (1.0 - cutoff);
			}
		} else  if ((flipDirection) && (!symmetry))
		{
			if((pixelPos.y > cutoff) && (!flipCutoff))
			{
				pixelPos.y = (cutoff);
			} else if ((pixelPos.y < (1.0-cutoff)) && (flipCutoff))
			{
				pixelPos.y = (1.0-cutoff);
			}
		} else if ((!flipDirection) && (symmetry))
		{
			if(pixelPos.y < cutoff)
			{
				pixelPos.y = (cutoff);
			}
			
			if(pixelPos.y > (1.0 - cutoff))
			{
				pixelPos.y = (1.0 - cutoff);				
			}
		} else if ((flipDirection) && (symmetry))
		{
			if(symmetryMode == 0)
			{
				if((pixelPos.y > cutoff) && (pixelPos.y < (1.0 - cutoff)))
				{
					pixelPos.y = (cutoff);
				}
			} else {
				if((pixelPos.y > cutoff) && (pixelPos.y < (1.0 - cutoff)))
				{
					pixelPos.y = (1.0 - cutoff);
				}
			}
		}
	} else if ((fillScreen) && (mode == 0))
	{
		pixelPos.y = cutoff;
	} else if ((fillScreen == false) && (mode == 1))
	{	
		if ((!flipDirection) && (!symmetry))
		{
			if((pixelPos.x < cutoff) && (!flipCutoff))
			{
				pixelPos.x = (cutoff);
			} else if ((pixelPos.x > (1.0 - cutoff)) && (flipCutoff))
			{
				pixelPos.x = (1.0 - cutoff);
			}
		} else  if ((flipDirection) && (!symmetry))
		{
			if((pixelPos.x > cutoff) && (!flipCutoff))
			{
				pixelPos.x = (cutoff);
			} else if ((pixelPos.x < (1.0-cutoff)) && (flipCutoff))
			{
				pixelPos.x = (1.0-cutoff);
			}
		} else if ((!flipDirection) && (symmetry))
		{
			if(pixelPos.x < cutoff)
			{
				pixelPos.x = (cutoff);
			}
			
			if(pixelPos.x > (1.0 - cutoff))
			{
				pixelPos.x = (1.0 - cutoff);				
			}
		} else if ((flipDirection) && (symmetry))
		{
			if(symmetryMode == 0)
			{
				if((pixelPos.x > cutoff) && (pixelPos.x < (1.0 - cutoff)))
				{
					pixelPos.x = (cutoff);
				}
			} else {
				if((pixelPos.x > cutoff) && (pixelPos.x < (1.0 - cutoff)))
				{
					pixelPos.x = (1.0 - cutoff);
				}
			}
		}
	} else if ((fillScreen) && (mode == 1))
	{
		pixelPos.x = cutoff;
	}
	
	gl_FragColor = IMG_NORM_PIXEL(inputImage,pixelPos);
	
	
}