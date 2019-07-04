/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 10.0
		
	},
	{
		"NAME": "lines_scroll_ofset",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 10.0
		
	},
	{
		"NAME": "lines_period",
		"TYPE": "float",
		"DEFAULT": 4.0,
		"MIN": 0.0,
		"MAX": 20.0
		
	},
	{
		"NAME": "ring_size",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0,
		"MAX": 2
		
	},
	{
		"NAME": "ring_thick",
		"TYPE": "float",
		"DEFAULT": 50.0,
		"MIN": 0,
		"MAX": 75
		
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#44341.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	vec2 p = 2.0*( gl_FragCoord.xy / RENDERSIZE.xy )-1.0;
	p.x *= RENDERSIZE.x/RENDERSIZE.y; 
	vec3 col = vec3(0);
	
	
	col = vec3(0.98,0.99,1.00)*sin(p.y*lines_period+((TIME*speed)-lines_scroll_ofset)+clamp(1.0/(ring_thick*abs(length(p.xy)-ring_size)), 0.0, 1.0)*2.0);
	gl_FragColor = vec4(col, 1.0); 
}