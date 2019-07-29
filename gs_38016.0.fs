/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
   
    
    {
		"NAME": "scale",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": 0.3,
		"MAX": 5.0
	},
	 {
		"NAME": "deform",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": -5.0,
		"MAX": 5.0
	},
	{
		"NAME": "lw",
		"TYPE": "float",
		"DEFAULT": 0.05,
		"MIN": 0.01,
		"MAX": 0.4
	},
	{
		"NAME": "circle_size",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.2,
		"MAX": 1.9
	},
	{
		"NAME": "line_size",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0.2,
		"MAX": 0.8
	},
	
	
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#38016.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


//const float lw = 0.05;
void main( void ) {
	//vec2 p = 3.*vv_FragNormCoord;
	
	vec2 p=((gl_FragCoord.xy*2.-RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y)*scale); 
	
	
	float lp = length(p);
	gl_FragColor = vec4(1);
	if(abs(circle_size-lp) < lw) gl_FragColor = vec4(0);
	
	if(lp < circle_size){
		p.y *= mix(1.5-deform,1., lp);
	}
	
	if(abs(p.y) < 1.){
		if(fract(-.25+p.y/(lw*4.)) < line_size) gl_FragColor = vec4(0.);
	}

}