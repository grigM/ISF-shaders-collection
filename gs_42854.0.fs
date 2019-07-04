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
		"MAX": 3.0
		
	},
	{
		"NAME": "ofset",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 3.0
		
	},
	{
		"NAME": "mod_x_1",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 2.0
		
	},
	{
		"NAME": "mod_y_1",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 6.3
		
	},
	{
		"NAME": "iter",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": 1.0,
		"MAX": 4.0
		
	},
        
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#42854.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define TRI(S)(  max(-2.*S.y, max(S.y-sqrt(3.)*S.x, S.y+sqrt(3.)*S.x))  )

void main( void ) {
	gl_FragColor = vec4( 1.0 );
	
	#define mult_TRI(S){  gl_FragColor *= vec4( vec3( TRI(S) ), 1.0 );  }
	
	
	
	vec2 position = ( gl_FragCoord.xy * 2. - RENDERSIZE.xy ) / min(RENDERSIZE.x, RENDERSIZE.y);
	mult_TRI(position);
	//return;
	for(int i = 0; i <= int(iter); i++){
		mult_TRI(position);
		position *= (1.+(-0.5+mod_x_1)*10.0*dot(position, position));	
		
		position += vec2(sin(mod_y_1), cos(mod_y_1))*cos(((TIME*speed)-ofset)+dot(position, position));
	}
	
	
}