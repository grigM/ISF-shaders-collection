/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4ddfzS by Markant.  deformer, bender, warper",
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    }
  ]
}
*/


void main() {

	

	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	float def = (sin((uv.x + 0.5*TIME) * 10.0)*0.01)-
                  (sin((uv.y + 0.5*TIME) * 10.00)*0.01);
	//gl_FragColor = IMG_NORM_PIXEL(inputImage,mod(uv + def,1.0));
	gl_FragColor = IMG_NORM_PIXEL(inputImage,uv + def);
}
