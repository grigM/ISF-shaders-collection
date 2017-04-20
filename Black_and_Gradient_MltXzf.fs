/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex07.jpg"
    }
  ],
  "CATEGORIES" : [
    "2d",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MltXzf by Zebbeni.  First attempt at making a shader",
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
    vec4 movie = IMG_NORM_PIXEL(inputImage,mod(uv,1.0));
    vec4 img = IMG_NORM_PIXEL(iChannel0,mod(uv,1.0));
    if (movie.g + movie.b + movie.r < 1.0) {
    	gl_FragColor = vec4(0.0 - movie.g/3.0,0.0 - movie.g/3.0,0.0 - movie.g/3.0,0.0);
    }
    else if (movie.g + movie.b + movie.r < 1.1) {
    	gl_FragColor = vec4(0.2 - movie.g/3.0,0.2 - movie.g/3.0,0.2 - movie.g/3.0,0.1);
    }
    else {
		gl_FragColor = vec4(uv,0.5+0.5*sin(TIME),1.0);
    }
}