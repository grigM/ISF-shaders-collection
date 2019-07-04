/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    
    
    {
			"NAME": "iterations",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 1.0,
			"MAX": 5.0
			
		},
		{
			"NAME": "iterations_2",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 1.0,
			"MAX": 5.0
			
		},
		{
			"NAME": "glow",
			"TYPE": "float",
			"DEFAULT": 0.01,
			"MIN": 0.0,
			"MAX": 0.1
			
		},
		{
			"NAME": "pos_x",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
			
		},
		{
			"NAME": "pos_y",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
			
		},
		{
			"NAME": "radius",
			"TYPE": "float",
			"DEFAULT": 0.4,
			"MIN": 0.0,
			"MAX": 1.0
			
		},
		{
			"NAME": "sin_p",
			"TYPE": "float",
			"DEFAULT": 0.4,
			"MIN": -1.0,
			"MAX": 1.0
			
		},
		
		{
			"NAME": "cos_p",
			"TYPE": "float",
			"DEFAULT": 0.4,
			"MIN": -1.0,
			"MAX": 1.0
			
		},
		
		{
			"NAME": "speed_cos",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 3.0
			
		}
		,
		{
			"NAME": "speed_sin",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 3.0
			
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#48857.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

//Water absorption and scattering of dirt inside of the water
#define PI 3.14159 
#extension GL_OES_standard_derivatives : enable
//precision mediump float;
vec3 color = vec3(0.722,0.544,0.888);
void main(void){
	vec2 m = vec2(pos_x * 2.0 - 1.0, pos_y * 2.0 - 1.0);
    	vec2 p = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
    	p.x += pos_x;
    	p.y += pos_y;
	vec3 t=vec3(0.0);
	for(int j=0;j<int(iterations_2);j++){
    		for(int i=0;i<int(iterations);i++){
			vec2 q=p+vec2(sin((TIME*speed_sin)+(float(i)*PI*sin_p))*(1.+float(j)),cos((TIME*speed_cos)+(float(i)*PI*cos_p)))*radius;
			t += glow/length(q);
		}
	}
	gl_FragColor = vec4(vec3(t), 1.0);
}