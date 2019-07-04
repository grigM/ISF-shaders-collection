/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
		{
      "NAME" : "speed",
      "TYPE" : "float",
      "DEFAULT" : 1.0,
      "MIN" : -8.0,
      "MAX" : 8.0
    },
    {
      "NAME" : "num",
      "TYPE" : "float",
      "DEFAULT" : 16,
      "MIN" : 8.0,
      "MAX" : 164.0
    },
    {
      "NAME" : "size",
      "TYPE" : "float",
      "DEFAULT" : 0.22,
      "MIN" : 0.1,
      "MAX" : 2.0
    },
    {
      "NAME" : "rad",
      "TYPE" : "float",
      "DEFAULT" : 1.22,
      "MIN" : 1.0,
      "MAX" : 3.0
    },
     {
      "NAME" : "out_line_alpha",
      "TYPE" : "float",
      "DEFAULT" : 0.5,
      "MIN" : 0.0,
      "MAX" : 1.0
    },
    {
      "NAME" : "out_line_size",
      "TYPE" : "float",
      "DEFAULT" : 0.97,
      "MIN" : 0.0,
      "MAX" : 1.0
    },
    {
      "NAME" : "color_shift",
      "TYPE" : "float",
      "DEFAULT" : 0.1,
      "MIN" : 0.08,
      "MAX" : 0.1
    }
	
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#24537.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

// dashxdr 20150417


void main()
{
	//vec2 pos = vv_FragNormCoord;
	
	 vec2 pos = 2.0*( gl_FragCoord.xy / RENDERSIZE.xy )-1.0;
	pos.x *= RENDERSIZE.x/RENDERSIZE.y; 




	vec3 color = vec3(0.0, 0.0, 0.0);
	float alpha = 0.0;
	
//#define NUM 16
	float numf = float(num);
	int besti = -1;
	float bestd = 5000.0;
	//float size = 0.22;
	float at = mod(TIME*speed, 3.1415926*2.0);
	float r = size*rad;
	for(int i=0;i<int(num);++i)
	{
		float a = at + 3.1415926*2.0*float(i)/numf;


		vec2 xy = r*vec2(cos(a), sin(a));
		float d = length(xy - pos);
		if(d<size && (besti<0 || mod(float(int(num)+i-besti), numf) > .5*numf))
		{
			bestd = d;
			besti = i;
		}
	}
	if(bestd<size*out_line_size)
	{
		float rand = sin(float(besti)+color_shift);
		color = fract(rand*vec3(10.0, 1000.0, 100000.0));
		alpha = 1.0;
	} else if(bestd<size){
		color = vec3(0.0);
		alpha = out_line_alpha;
	}
	gl_FragColor =  vec4(color, alpha);
}