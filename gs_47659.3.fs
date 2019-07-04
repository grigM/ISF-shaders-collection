/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#47659.3"
}
*/


// configuring float specificity (you're required to do this or this code won't work)
#ifdef GL_ES
precision mediump float;
#endif

// inputs given by GLSL sandbox


vec3 colorQuadrants(vec2 currentPixelLocation) {
	float currentPixelX = currentPixelLocation.x;
	float currentPixelY = currentPixelLocation.y;
	
	float currentMouseX = mouse.x;
	float currentMouseY = mouse.y;
	
	
	
	// transitions from 1 to 0 as current pixel moves left to right to left of mouse
	float willBePositiveOnLeftSide= (currentPixelX - currentMouseX) / -currentMouseX;
	
	// transitions from 0 to 1 as current pixel moves left to right to right of mouse
	float willBePositiveOnRightSide = (currentPixelX - currentMouseX) / (1. -  currentMouseX);
	
	// pick positive value such that red will fade out until mouse is hit on X axis, then in once mouse is passed
	float positiveHorizontalValue = max(willBePositiveOnRightSide, willBePositiveOnLeftSide);
	
	
	
	// transitions from 1 to 0 as current pixel moves top to bottom above mouse
	float willBePositiveOnTop = (currentPixelY - currentMouseY) / -currentMouseY;
	
	// transitions from 0 to 1 as current pixel moves top to bottom below mouse
	float willBePositiveOnBottom = (currentPixelY - currentMouseY) / (1. - currentMouseY);
	
	// pick positive value such that blue will fade out until mouse is hit on Y axis, then in once mouse is passed
	float positiveVerticalValue = max(willBePositiveOnTop, willBePositiveOnBottom);
	
	
	vec3 returnedColor = vec3(.5, .5, .5); // initialize red, green, and blue as .5, values are between 0 and 1
	
	// stark quadrant definition 
	// only modifying red if the X distance from the mouse is greater than Y distance
	// vice versa for modifying blue
	if(positiveHorizontalValue > positiveVerticalValue) {
		returnedColor.r = positiveHorizontalValue;
	} else {
		returnedColor.b = positiveVerticalValue;
	}
	
	return returnedColor;
}

// this "main" function will get run for every single pixel
void main( void ) {
	// "gl_FragCoord" is special!  It is a vector containing the coordinates of the pixel we are currently determining the color of
	vec2 currentPixelPosition = gl_FragCoord.xy;
	
	// GLSL (this language, a superset of C), conveniently provides a vector operators, the below just does simple division value-by-value
	vec2 normalizedPosition = currentPixelPosition / RENDERSIZE.xy;
	
	// **Note that we are able to create a vector of two values from a larger vector using the property "xy" 
	// **This is actually would also work for "xyz" or "rgba"
	// **https://www.khronos.org/opengl/wiki/Data_Type_(GLSL)#Swizzling
	
	vec3 currentPixelColor = colorQuadrants(normalizedPosition);
	
	// "gl_FragCoord" is also special!  It sets the color of the current pixel via a 4-value vector of the format "r,g,b,a"
	// given that this "main" function runs for every pixel, this draws the output onto the canvas
	gl_FragColor = vec4(currentPixelColor, 1.);
}