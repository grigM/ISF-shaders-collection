/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "postprocessing",
    "cv",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MdfczN by athlete.  Takes in images\/videos of greyscale Thermal camera and outputs a mapped version to RGB",
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    },
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


const bool darkIsHot = true;

void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	vec3 texColor = IMG_THIS_PIXEL(inputImage,uv).rgb;
    
    if (gl_FragCoord.x < iMouse.x)
	{
		gl_FragColor = vec4(texColor, 1.0);
	}
    else
    {       
        float a = texColor.r;
        if(darkIsHot)
            a = 1.0 - a;
    	//fast shader version
        gl_FragColor.r = 1.0 - clamp(step(0.166, a)*a, 0.0, 0.333) - 0.667*step(0.333, a) + step(0.666, a)*a + step(0.833, a)*1.0;
        gl_FragColor.b = clamp(step(0.333, a)*a, 0.0, 0.5) + step(0.5, a)*0.5;
        gl_FragColor.g = clamp(a, 0.0, 0.166) + 0.834*step(0.166, a) - step(0.5, a)*a - step(0.666, a)*1.0;
        
        
        //human readable version
        //if(a<0.166)
        //{
            //mappedColor.r=1.0;
            //mappedColor.g=a;
            //mappedColor.b=0.0;
        //}
        //else if(a>=0.166 && a < 0.333)
        //{
            //mappedColor.r=1.0-a;
            //mappedColor.g=1.0;
            //mappedColor.b=0.0;
        //}
        //else if(a>=0.333 && a < 0.5)
        //{
            //mappedColor.r=0.0;
            //mappedColor.g=1.0;
            //mappedColor.b=a;
        //}
        //else if(a>=0.5 && a < 0.666)
        //{
            //mappedColor.r=0.0;
            //mappedColor.g=1.0-a;
            //mappedColor.b=1.0;
        //}
        //else if(a>=0.666 && a < 0.833)
        //{
            //mappedColor.r=a;
            //mappedColor.g=0.0;
            //mappedColor.b=1.0;
        //}
        //else
        //{
            //mappedColor.r=1.0;
            //mappedColor.g=0.0;
            //mappedColor.b=1.0;
        //}
    }
    
}
