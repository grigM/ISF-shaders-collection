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
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 10
	},
	{
		"NAME": "rowCount",
		"TYPE": "float",
		"DEFAULT": 3,
		"MIN": 1,
		"MAX": 10
	},
	{
		"NAME": "rotate",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": -1,
		"MAX": 1
	},
	
	{
		"NAME": "zoom",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 2
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39877.0"
}
*/




// Author: Patricio Gonzalez Vivo


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

//#define PI 3.1415926535
#define PI 3.14159265359
#define HALF_PI 1.57079632679



// Title: recoded Fractal Invaders by Jared Tarbell
// http://www.levitated.net/daily/levInvaderFractal.html



float random(in float x){ return fract(sin(x)*43758.5453); }
float random(in vec2 st){ return fract(sin(dot(st.xy ,vec2(12.9898,78.233))) * 43758.5453); }

float randomChar(vec2 outer,vec2 inner){
    float grid = 7.;
    vec2 margin = vec2(.15,.15);
    vec2 borders = step(margin,inner)*step(margin,1.-inner);
    vec2 ipos = floor(inner*grid);
    ipos = abs(ipos-vec2(3.,0.));
    return step(.5, random(outer*64.+ipos)) * borders.x * borders.y;
}


mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}


void main() {
	//vec2 st = 2.0*vec2(gl_FragCoord.xy-0.5*RENDERSIZE.xy)/RENDERSIZE.y;
	
    
 	
    vec2 st = gl_FragCoord.st/RENDERSIZE.xy;
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    
    st.x *= ratio;
    
    //rotate canvas
 	st = rotate2d(rotate*PI ) * st;
 	st *= zoom;
 	
 	
    vec3 color = vec3(0.0);

    float rows = rowCount;
    rows += floor(mod((TIME*speed)*0.01,24.));
    
    float t_p = 1.+(TIME*speed)*0.3;
    vec2 vel = vec2(0.,-floor((t_p*speed)));
    
    vec2 ipos = floor(st*rows);
    vec2 fpos = fract(st*rows);
    
    float pct = 1.0;
    pct *= randomChar(mod(ipos + vel,vec2(999.)),fpos);
    if (ipos.y > 0.0 || ipos.x < fract((TIME*speed))*rows*ratio) {
        color = vec3(pct);
    }  

    gl_FragColor = vec4(color , 1.0);
}