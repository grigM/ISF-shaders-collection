/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "pyramid",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xl3Xzf by yduf.  scalespace",
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    }
  ]
}
*/


void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;

	float scale = 2.0;
    vec2 left  = vec2( 0.0, 0.0);

    if( uv.y < 0.5) {
       if( uv.x > 0.5) {
            left.x += 0.5;
            scale *= 2.0;
        }
       if( uv.x > 0.75) {
            left.x += 0.25;
            scale *= 2.0;
        }
       if( uv.x > 0.875) {
            left.x += 0.125;
            scale *= 2.0;
        } 
        
        if( ( 1.0 - uv.y) > uv.x)
	 		gl_FragColor = IMG_NORM_PIXEL(inputImage,mod((uv - left)*scale,1.0));
        else
             gl_FragColor = IMG_NORM_PIXEL(inputImage,mod(uv,1.0));
    }
    else {       
        gl_FragColor = IMG_NORM_PIXEL(inputImage,mod(uv,1.0));
    }
}