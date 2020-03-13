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
		"DEFAULT": 2,
		"MIN": 0,
		"MAX": 10
	},
	{
		"NAME": "move_ofset",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": -2,
		"MAX": 2
	},
	{
		"NAME": "rowCount",
		"TYPE": "float",
		"DEFAULT": 1,
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
	},
	{
		"NAME": "tileParam1",
		"TYPE": "float",
		"DEFAULT": 2,
		"MIN": 0,
		"MAX": 3.99
	},
	{
		"NAME": "smoooth",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": 0,
		"MAX": 1
	}
	
	

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39883.0"
}
*/



// Title: Zigzag


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable




vec2 mirrorTile(vec2 _st, float _zoom){
    _st *= _zoom/2.0;
    
    if (fract(_st.y * 0.5) > 0.5){
        _st.x = _st.x+0.5;
        _st.y = 1.0-_st.y;
    }
    return fract(_st);
}

float fillY(vec2 _st, float _pct,float _antia){
  return  smoothstep( _pct-_antia, _pct, _st.y);
}



#define PI 3.14159265359
#define HALF_PI 1.57079632679

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}


void main() {
	vec2 st = 2.0*vec2(gl_FragCoord.xy-0.5*RENDERSIZE.xy)/RENDERSIZE.y;
	
    
    //rotate canvas
 	st = rotate2d(rotate*PI ) * st;
 	st *= zoom;
 	
  vec3 color = vec3(0.0);

  st = mirrorTile(st*vec2(1.,2.),rowCount);
  float x = st.x*tileParam1; 
  float a = floor(1.0+sin(x*PI));
  float b = floor(1.0+sin((x+1.)*PI));
  float f = fract(x);
	
  st.y += sin((TIME*speed)+move_ofset);
  color = vec3( fillY(st,mix(a,b,f),smoooth) );

	//if(color>0){
	  gl_FragColor = vec4( color, 1.0 );
	//}
}