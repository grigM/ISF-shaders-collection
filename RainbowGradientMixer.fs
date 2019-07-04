/*{
 "CREDIT": "by mojovideotech",
 "CATEGORIES" : [
    "color",
    "gradient",
    "rainbow"
  ],
  "DESCRIPTION" : "mod of https:\/\/www.shadertoy.com\/view\/ltVXW3 by Loeizd.  cheap rainbow using mix",
  "INPUTS" : [
   	{
		"NAME": 	"blend",
		"TYPE": 	"float",
		"DEFAULT":	0.33,
		"MIN": 		-0.5,
		"MAX": 		1.0
	},
	{
      	"NAME": 	"Rx",
      	"TYPE": 	"float",
      	"MIN": 		0,
      	"MAX": 		6,
      	"DEFAULT":	2
    },
    {
      	"NAME": 	"Gx",
      	"TYPE": 	"float",
      	"MIN": 		0,
      	"MAX": 		6,
      	"DEFAULT":	3
    },
    {
      	"NAME": 	"Bx",
      	"TYPE": 	"float",
      	"MIN": 		0,
      	"MAX": 		6,
      	"DEFAULT":	1
    },
    {
   		"NAME": 	"vertical",
     	"TYPE": 	"bool",
     	"DEFAULT": 	false
   	}
  ]
}
*/

////////////////////////////////////////////////////////////
// RainbowGradientMixer   by mojovideotech
//
// mod of 
// shadertoy.com\/ltVXW3  by Loeizd
//
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0
////////////////////////////////////////////////////////////


void main() 
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	if (vertical) uv.xy = uv.yx;
    float a = blend / 2.0 - 0.25;
    vec3 m = abs(uv.x * 2.0 - vec3(Rx,Gx,Bx) / 3.0);
    m.gb = 1.0 - m.gb;
    m += a;
    m = smoothstep(0.0, 1.0, m);
    
    gl_FragColor = sqrt(vec4(m, 1.0));
}
