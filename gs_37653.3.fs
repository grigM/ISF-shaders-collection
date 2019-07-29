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
		"MIN": -3.0,
		"MAX": 3.0
	},
	{
		"NAME": "item_count",
		"TYPE": "float",
		"DEFAULT": 15,
		"MIN": 5,
		"MAX": 100.0
	},
	{
		"NAME": "circ_distamce",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0,
		"MAX": 5.0
	},
	{
		"NAME": "y_start_pos",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": -2.0,
		"MAX": 2.0
	},
	{
		"NAME": "y_step",
		"TYPE": "float",
		"DEFAULT": 0.3,
		"MIN": -1.0,
		"MAX": 1.0
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#37653.3"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



//forked this for my cat

void main( void ) {
	
	vec2 p=(gl_FragCoord.xy*2.-RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y); 
	
	//vec2 p = vv_FragNormCoord;

	vec3 color = vec3(0.0);
	
	// color = 1.0 - length(p) * 3.0;
	
	float k = circ_distamce;
	float t = TIME * speed;
	

	
	for(float i=1.; i<item_count; i++)
	{
		t += i*.1;
		
		float fx = sin(t+sin(t*3.)) * k;	
		float fy = -sin(t/2.)+cos(sin(t*1.2)+.1);
		fy -= y_start_pos;
		fy*=y_step;
			
		
		if((p.x+fx)*(p.x+fx)+(p.y+fy)*(p.y+fy) <i*0.001){
		       color.x+=1.;
		       color.y+=abs(fx/fy);
		       color.z+=abs(fx/fy);
		}
	}
	

	gl_FragColor = vec4( color, 1.0 );

}