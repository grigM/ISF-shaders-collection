/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "spiral",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MslyWB by nomadiclizard.  The simplest spiral. Things can only get more complex from here by adding nonlinearity to the theta, r and iGlobalTime terms.",
  "INPUTS" : [

  ]
}
*/


const float pi = 3.141592654;
const vec4 white = vec4(1.0);
const vec4 black = vec4(0.0);

void main() {



	vec2 uv = (gl_FragCoord.xy - 0.5 * RENDERSIZE.xy) / RENDERSIZE.yy;
	float r = length(uv);
    float theta = atan(uv.y, uv.x);   
    gl_FragColor = fract(2.5 * theta / pi - 15.0 * pow(r, 0.25) + 1.5 * (1.0 + 0.35 * pow(r, 5.5)) * TIME) < 0.5 ? white : black;
}
