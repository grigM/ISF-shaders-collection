/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wsjSzV by deerfeeder.  simple loopable collection ",
  "INPUTS" : [

  ]
}
*/


// Fork of "Stripes" by None. https://shadertoy.com/view/-1
// 2019-04-04 17:35:14

float stripes(vec2 p) {
	return (-1.+ p.y*2.) * sin(TIME);
}



void main() {



    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	uv = abs(uv-0.5);
    // Time varying pixel color
    float x = stripes(uv) * (50. * cos(uv.x* (10. * sin(TIME * 0.2))));
    vec3 col = vec3(log(x*x)*sin(x));
    // Output to screen
    gl_FragColor = vec4(col,1.0);
}
