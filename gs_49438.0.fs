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
			"MIN": -5.0,
			"MAX": 5.0
			
		},
		
		{
			"NAME": "rgb_offset",
			"TYPE": "float",
			"DEFAULT": -1000.0,
			"MIN": -1000.0,
			"MAX": -999
			
		},
		{
			"NAME": "anim_mod",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": -20.0,
			"MAX": 20.0
			
		},
		{
			"NAME": "center_pos_x",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -0.5,
			"MAX": 0.5
			
		},
		{
			"NAME": "center_pos_y",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -0.5,
			"MAX": 0.5
			
		},
		{
			"NAME": "glow",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": -0.8,
			"MAX": 1.1
			
		},
		{
      "LABELS" : [
        "both",
        "x",
        "y",
        "x_2"
      ],
      "NAME" : "mod_type",
      "TYPE" : "long",
      "DEFAULT" : 0,
      "VALUES" : [
        0,
        1,
        2,
        3
       ]
    },
		
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#49438.0"
}
*/


// Author: Deam
// Title: RGB Square Morphing
// Date: 27/9/2018

#ifdef GL_ES
//#define t TIME
//#define r RENDERSIZE
precision mediump float;
#endif


void main() {
	float t = TIME*speed;
		
		vec3 c;
	float l, z = t;
	for(int i=0; i < 3; i++) {
		vec2 uv, p = gl_FragCoord.xy/RENDERSIZE;
		uv = p;
		p.x -= (0.5+center_pos_x);
		p.y -= (0.5+center_pos_y);
		p.x *= RENDERSIZE.x/RENDERSIZE.y;
		z += rgb_offset; // RGB offset
		if(mod_type==0){
			p *= sin(p*float(anim_mod) - z*2.4);
			//p.x -= (sin(p.x*float(anim_mod) - z*2.4))/10.4;
		}else if(mod_type==1){
			p.x -= sin(p.x*float(anim_mod) - z*2.4);
		}else if(mod_type==2){
			p.y -= sin(p.y*float(anim_mod) - z*2.4);
		}else if(mod_type==3){
			p.x -= (sin(p.x*float(anim_mod) - z*2.4))/15.4;
		}
		c[i] =  0.01 / length(p);
	}
    
	gl_FragColor = vec4(c/(1.2-glow), 1.);
}