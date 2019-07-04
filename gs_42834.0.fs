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
		"MIN": 0.2,
		"MAX": 3
	},
	{
		"NAME": "iterators",
		"TYPE": "float",
		"DEFAULT": 10,
		"MIN": 1,
		"MAX": 20
	},
	{
		"NAME": "rgSpeed",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": 0,
		"MAX": 10.0
	},
	{
		"NAME": "amp",
		"TYPE": "float",
		"DEFAULT": 1.5,
		"MIN": 0,
		"MAX": 10.0
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#42834.0"
}
*/


#ifdef GL_ES
precision highp float;
#endif

#extension GL_OES_standard_derivatives : enable


float hash( vec2 p ) {
	float h = dot(p,vec2(127.1,311.7));	
    return fract(sin(h)*4758.5453123);
}

float noise( in vec2 p ) {
    vec2 i = floor( p );
    vec2 f = fract( p );	
	vec2 u = f*f*(3.0-2.0*f);
    return -1.0+2.0*mix( mix( hash( i + vec2(0.0,0.0) ), 
                     hash( i + vec2(1.0,0.0) ), u.x),
                mix( hash( i + vec2(0.0,1.0) ), 
                     hash( i + vec2(1.0,1.0) ), u.x), u.y);
}

void main() {

	vec2 position =2.0*(gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x,RENDERSIZE.y);

	float v=0.0;
	float ii=0.0;
	
	int f=0;
	
	for(int i=0;i<int(iterators);i++)
	{
		vec2 n1=rgSpeed*vec2(sin((TIME*speed)+ii),cos((TIME*speed)+ii));
		vec2 n2=rgSpeed*vec2(cos((TIME*speed)+ii),sin((TIME*speed)+ii));
		vec2 pos=vec2(noise(n1),noise(n2));
		float l1 = amp/length(pos - position);
		if( f==0 )
		{
			v+=l1;
			f=1;
		}
		else
		{
			v-=l1;
			f=0;
		}
		ii+=1.0;
	}
	
	//v=pow(v,1.5);
	gl_FragColor = vec4(vec3(v),1.0);
}