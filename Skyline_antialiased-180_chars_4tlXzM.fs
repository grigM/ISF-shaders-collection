/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "2tweets",
    "short",
    "2tc",
    "skyline",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tlXzM by FabriceNeyret2.  antialiased version of  GregRostami https:\/\/www.shadertoy.com\/view\/MtXSR7 variant of  gsingh93 shader  https:\/\/www.shadertoy.com\/view\/4tXSRM#  :-D\nNB: replace 15* by 5* if you want a motion-blur effect :-)",
  "INPUTS" : [

  ]
}
*/


void main() {
	vec4 fragCoordPos = gl_FragCoord;
	
    fragCoordPos.xy /= RENDERSIZE.xy; 
    float x,c;
    for (float i = 1.; i < 20.; i++)   
		gl_FragColor = fragCoordPos.y+.04*i < sin(c=floor(x= 2e2*fragCoordPos.x/i + 9.*i + DATE.w)) ? 
                             gl_FragColor + min(15.*((x-=c)-x*x),1.) *(i/20.-gl_FragColor)  : gl_FragColor; 
}
