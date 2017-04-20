/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
  	{
		"NAME": "rotate",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": -1,
		"MAX": 1
	},
	{
		"NAME": "powAbs",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0,
		"MAX": 2
	},
	{
		"NAME": "pow",
		"TYPE": "float",
		"DEFAULT": 0.1,
		"MIN": 0.005,
		"MAX": 0.2
	},
	{
		"NAME": "sinRate",
		"TYPE": "float",
		"DEFAULT": 5.0,
		"MIN": 0.0,
		"MAX": 100.0
	},
	{
		"NAME": "sinAmp",
		"TYPE": "float",
		"DEFAULT": 100,
		"MIN": 0.0,
		"MAX": 100.0
	},
	{
		"NAME": "sinStepDif",
		"TYPE": "float",
		"DEFAULT": 0.2,
		"MIN": 0.0,
		"MAX": 2.0
	},
	{
		"NAME": "colorAmount",
		"TYPE": "float",
		"DEFAULT": 1.4,
		"MIN": 0.0,
		"MAX": 2.0
	}
	
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39393.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define iGlobalTime TIME
#define iMouse (mouse.xy/RENDERSIZE)
#define iResolution RENDERSIZE

// https://www.shadertoy.com/view/XslcD2

#define PI 3.14159265359
mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

void mainImage( out vec4 f, vec2 g )
{
    g -= f.xy=iResolution.xy/2.;
   	
   	//vec2 p = 
    
	
    g = rotate2d(rotate*PI ) * g;
    
     
    g /= f.y;
    
    float d = pow(abs(powAbs - max(abs(g.x),abs(g.y))), pow);
        
    g += d;
    
    g *= g;
    
    f = vec4(g,d,1) * d * (colorAmount + sinStepDif * sin(sinAmp * d+iGlobalTime*sinRate));
}

void main( void ) {
	
	mainImage( gl_FragColor, gl_FragCoord.xy );

}