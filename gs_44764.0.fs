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
		"MIN": -2.1,
		"MAX": 2.0
		
	},
	{
		"NAME": "iter",
		"TYPE": "float",
		"DEFAULT": 15.0,
		"MIN": 0,
		"MAX": 40.0
		
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#44764.0"
}
*/


#ifdef GL_ES
precision highp float;
#endif


float rand(vec3 r) { return fract(sin(dot(r.xy,vec2(1.38984*sin(r.z),1.13233*cos(r.z))))*653758.5453); }

vec2 threshold(vec2 threshold,vec2 x,vec2 low,vec2 high)
{
	return low+step(threshold,x)*(high-low);
}

void main(void)
{
	vec2 position=(2.0*gl_FragCoord.xy-RENDERSIZE)/min(RENDERSIZE.x,RENDERSIZE.y);

	vec2 topleft=vec2(-1.0);
	vec2 bottomright=vec2(1.0);
	float col=1.0;

	for(int i=0;i<int(iter);i++)
	{
		vec2 midpoint=(topleft+bottomright)/2.0;
		vec2 diagonal=bottomright-topleft;

		//if(position.x>bottomright.x || position.y>bottomright.y) break;
		//if(position.x<topleft.x || position.y<topleft.y) break;

		if(rand(vec3(topleft,floor(position.x+position.y+TIME/speed)+1.0))<0.7)
		{
			if(length(position-midpoint)>length(diagonal)*0.35) break;
			topleft+=diagonal*0.15;
			bottomright-=diagonal*0.15;
			col*=-1.0;
		}
		else
		{
			topleft=threshold(midpoint,position,topleft,midpoint);
			bottomright=threshold(midpoint,position,midpoint,bottomright);
		}
	}

	gl_FragColor=vec4(vec3(col*0.5+0.5),1.0);
}