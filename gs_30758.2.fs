/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "line_w",
			"TYPE": "float",
			"DEFAULT":0.1,
			"MIN": 0.01,
			"MAX": 1.0
		},
		{
			"NAME": "line_blur",
			"TYPE": "float",
			"DEFAULT":150,
			"MIN": 8.0,
			"MAX": 150.0
		},
		{
			"NAME": "corn_rad",
			"TYPE": "float",
			"DEFAULT":0.2,
			"MIN": 0.004,
			"MAX": 1.0
		},
		{
			"NAME": "size_W",
			"TYPE": "float",
			"DEFAULT":1.0,
			"MIN": -0.0,
			"MAX": 3.5
		},
		{
			"NAME": "sin_w_speed",
			"TYPE": "float",
			"DEFAULT":0,
			"MIN": 0.0,
			"MAX": 10.0
		},
		{
			"NAME": "size_H",
			"TYPE": "float",
			"DEFAULT":0.2,
			"MIN": 0.0,
			"MAX": 1.0
		},
		
		
			
	{
		"NAME": "COS_DEFORM",
		"TYPE": "bool",
		"DEFAULT": false,
	},
	{
			"NAME": "COS_DEFORM_SPEED",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 2.0
			
		},
	{
			"NAME": "COS_X_DEFORM_PER",
			"TYPE": "float",
			"DEFAULT": 6.0,
			"MIN": 0.0,
			"MAX": 8.0
			
		},
		{
			"NAME": "COS_Y_DEFORM_PER",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 8.0
			
		},
		
		{
			"NAME": "COS_X_DEFORM_AMP",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": -1.0,
			"MAX": 1.0
			
		},
		{
			"NAME": "COS_Y_DEFORM_AMP",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": -1.0,
			"MAX": 1.0
			
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#30758.2"
}
*/


// 2D Round box 

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

#define clamps(x) clamp(x,0.,1.)
void main( void ) {

	vec2 p = 2.0*( gl_FragCoord.xy / RENDERSIZE.xy ) -1.0; 
	p.x *= RENDERSIZE.x/RENDERSIZE.y; 
	
	if(COS_DEFORM){
		p += cos(p.x * COS_X_DEFORM_PER + (TIME*COS_DEFORM_SPEED)) * COS_X_DEFORM_AMP;
		p -=cos(p.y*COS_Y_DEFORM_PER +(TIME*COS_DEFORM_SPEED))*COS_Y_DEFORM_AMP;
    
	}
    
	vec2 b = vec2((sin(TIME*sin_w_speed)+size_W)/2., size_H); // size

	float d = length(max(abs(p),b)-b)-corn_rad; 
	
	vec3 col = vec3(clamps(d*line_blur)-clamps((d-line_w)*line_blur)); 
	
	gl_FragColor = vec4(col, 1.0); 
}