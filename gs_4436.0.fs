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
		"MIN": -10.0,
		"MAX": 10.0
		
	},
	{
		"NAME": "grid_x",
		"TYPE": "float",
		"DEFAULT": 5.0,
		"MIN": 1,
		"MAX": 7
		
	},
	{
		"NAME": "grid_y",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0.5,
		"MAX": 1.0
		
	},
	{
		
		"NAME": "grid_mod_x",
		"TYPE": "float",
		"DEFAULT": 0.000003,
		"MIN": 0.000000,
		"MAX": 0.000010
		
	},
	{
		
		"NAME": "grid_mod_y",
		"TYPE": "float",
		"DEFAULT": 0.000001,
		"MIN": 0.000000,
		"MAX": 0.000005
		
	},
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#4436.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif



float noise(vec2 p) {
	p=(p);
	return fract(sin(p.x*45.11+p.y*97.23)*878.73+733.17)*2.0-1.0;
}

void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );
	
	float x = noise(vec2(position.x*grid_mod_x,int(grid_x)));
	float z = noise(vec2(position.y*grid_mod_y,grid_y));
	
	float color = abs(fract((x+position.x+z*(TIME*speed)))-.5)*2.;
	
	gl_FragColor = vec4( pow(vec3(color), vec3(1.0, 1.0, 1.0)), 1.0 );
}