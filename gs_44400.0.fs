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
    },
    
    {
		
		"NAME": "ringsize",
		"TYPE": "float",
		"DEFAULT": 0.004,
		"MIN": 0.000000,
		"MAX": 0.06
		
	},
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#44400.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

//const float ringsize = 0.004;

float ring(vec2 screenpos, vec2 mousepos, vec2 ringcenter) {
	float sdist = length(screenpos - ringcenter);
	float mdist = length(mousepos - ringcenter);
	vec2 center = screenpos - ringcenter;
	float ang = atan(center.y, center.x);
	float intensity = (1.5 - sin(ang*245.34635 + TIME*0.78464) * sin(ang*285.34635 - TIME*0.5326) * sin(ang*295.34635));
	return 1.5-abs(sdist-mdist)/ringsize*0.35*intensity;
}

void main( void ) {
	vec2 screenpos = ( gl_FragCoord.xy / RENDERSIZE.xy ) - 0.5;
	screenpos.y *= RENDERSIZE.y/RENDERSIZE.x;
	vec2 mousepos = mouse - 0.5;
	mousepos.y *= RENDERSIZE.y/RENDERSIZE.x;

	gl_FragColor.r = ring(screenpos, mousepos, vec2(ringsize, -ringsize));
	gl_FragColor.g = ring(screenpos, mousepos, vec2(0.0, 0.0));
	gl_FragColor.b = ring(screenpos, mousepos, vec2(-ringsize, ringsize));
	gl_FragColor.a = 1.0;
}