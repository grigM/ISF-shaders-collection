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
		"MAX": 4.0
	},
	{
		"NAME": "TIMELINE",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": -1.0,
		"MAX": 1.0
	},
	{
		"NAME": "count",
		"TYPE": "float",
		"DEFAULT": 32.0,
		"MIN": 8.0,
		"MAX": 64.0
	},
	{
		"NAME": "scale",
		"TYPE": "float",
		"DEFAULT": 5.0,
		"MIN": 0.0,
		"MAX": 10.0
	}
	
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#38776.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

#//umu


void main( void ) {

	vec2 uv= ( gl_FragCoord.xy / RENDERSIZE.xy ) *2.0 -1.0;
	uv *=scale;
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
	
	
	vec2 pos = uv;
	
	int step = 0;
	bool found;
	for (int i = 0;i<int(count);i++){
		found = true;
		if (pos.x<0.) {
			pos.x *= -1.;
			step ++;
			found = false;
		}
		if (pos.y<0.){
			pos.y *= -1.;
			step ++;
			found = false;
		}
		if (pos.x+pos.y>1.){
			pos = vec2(-pos.y+1.,-pos.x+1.);
			step ++;
			found = false;
		}
		
		
	}
	
	float ma = abs(sin(TIME*speed))*count;	
	vec3 col = vec3(step)/ma;
	
	gl_FragColor = vec4(col,1.);

	

}