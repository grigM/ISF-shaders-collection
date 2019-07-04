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
		"DEFAULT": 20,
		"MIN": 1,
		"MAX": 50
	},
	{
		"NAME": "gridF",
		"TYPE": "float",
		"DEFAULT": 5,
		"MIN": 1,
		"MAX": 20
	},
	{
		"NAME": "rowF",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 1,
		"MAX": 4
	},
	{
		"NAME": "border",
		"TYPE": "float",
		"DEFAULT": 0.01,
		"MIN": 0,
		"MAX": 0.2
	},
	{
		"NAME": "margX",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": 0,
		"MAX": 1
	},
	{
		"NAME": "margY",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": 0,
		"MAX": 1
	},
	{
		"NAME": "square",
		"TYPE": "bool",
		"DEFAULT": 1
	},
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39890.0"
}
*/


// Author @patriciogv - 2015
// Title: Matrix


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float random(in float x){ return fract(sin(x)*43758.5453); }
float random(in vec2 st){ return fract(sin(dot(st.xy ,vec2(12.9898,78.233))) * 43758.5453); }

float randomChar(vec2 outer,vec2 inner){
    float grid = float(int(gridF));
    vec2 margin = vec2(margX,margY);
    vec2 borders = step(margin,inner)*step(margin,1.-inner);
    vec2 ipos = floor(inner*grid);
    vec2 fpos = fract(inner*grid);
    return step(.5,random(outer*64.+ipos)) * (borders.x) * (borders.y) * step(border,fpos.x) * step(border,fpos.y);
}

void main(){
    vec2 st = gl_FragCoord.st/RENDERSIZE.xy;
    if(square){
 	   st.y *= RENDERSIZE.y/RENDERSIZE.x;
    }
    vec3 color = vec3(0.0);

    float rows = float(int(rowF));
    
    vec2 ipos = floor(st*rows);
    vec2 fpos = fract(st*rows);

    ipos += vec2(0.,floor(TIME*speed*random(ipos.x+1.)));

    float pct = 1.0;
    pct *= randomChar(ipos,fpos);
    // pct *= random(ipos);

    color = vec3(pct);

    gl_FragColor = vec4( color , 1.0);
}