/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
		{
			"NAME": "cosTime",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.01,
			"MAX": 1.0
		},
		
		{
			"NAME": "sinTime",
			"TYPE": "float",
			"DEFAULT": 5.0,
			"MIN": 1.0,
			"MAX": 10.0
		},
		
		{
			"NAME": "iterCount",
			"TYPE": "float",
			"DEFAULT": 20.0,
			"MIN": 1.0,
			"MAX": 30.0
		},
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34774.0"
}
*/


//  Modded by @dennishjorth

#ifdef GL_ES
precision mediump float;
#endif


void main()
{
	vec2 p=(2.0*gl_FragCoord.xy-RENDERSIZE)/max(RENDERSIZE.x,RENDERSIZE.y);

	for(int i=1; i<int(iterCount); i++)
	{
		vec2 newp=p;
		float vv = (cos(TIME*cosTime)+1.0)*0.501;
		newp.x+=vv/float(i)*sin(float(i)*p.y+TIME/sinTime+0.5*float(i+ 0))+2.0;		
		newp.y+=vv/float(i)*sin(float(i)*p.x+TIME/sinTime+0.5*float(i+20))+1.0;
		p=newp;
	}
	vec3 col=vec3(0.5*sin(4.0*p.x)+0.5,0.5*sin(2.0*p.y)+0.5,sin(2.1*(p.x+p.y)));
	gl_FragColor=vec4(col, 1.0);
}