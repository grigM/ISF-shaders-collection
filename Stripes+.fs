/*{
  "CREDIT": "by pablo.riera",
  "DESCRIPTION": "",
  "CATEGORIES": [
    "XXX"
  ],
  "INPUTS": [
    {
      "NAME": "color",
      "TYPE": "color",
      "DEFAULT": [
        1,
        1,
        1,
        1
      ]
    },
    {
      "NAME": "sc",
      "TYPE": "float",
      "DEFAULT": 100,
      "MIN": 0,
      "MAX": 1000
    },
    {
      "NAME": "off",
      "TYPE": "float",
      "DEFAULT": 0,
      "MIN": -1,
      "MAX": 2
    },
    {
      "NAME": "rot",
      "TYPE": "float",
      "DEFAULT": 0,
      "MIN": 0,
      "MAX": 6.28
    },
    {
      "NAME": "ssca",
      "TYPE": "float",
      "DEFAULT": 1,
      "MIN": 0,
      "MAX": 2
    },
    {
      "NAME": "dir",
      "TYPE": "point2D",
      "DEFAULT": [
        0,
        0
      ]
    }
  ]
}*/

float bump(float w, float center, float x)
{
	float f = 1.0;
	return smoothstep( (center-w)/f , (center-w)*f , x)*smoothstep( (center+w)/f , (center+w)*f , x);
	
}

void main() {
	
	vec2 pos = isf_FragNormCoord.xy;
//	;
	
	vec3 color = vec3(color.rgb);
	
	float x = float(int(pos.x*4.0));
		
	float ph = rot;//+ 3.1415*sin(6.0*pos.x)*x;
	
	/*if(x==0.0)
		ph=0.0;
	else if(x==1.0)
	 	ph=+3.14/4.0;
	else if(x==2.0)
	 	ph=+3.14/2.0;
	else if(x==3.0)
	 	ph=3.14/4.0*3.0;*/
	 	
	for(int i=0;i<4;i++)
	{
		ph+=bump(0.25,float(i)/4.0,pos.x)*3.1415/4.0;
	}
	 	
	
	vec2 M = vec2(sin(ph),cos(ph))*sc;
	
	float shape = sin(dot(pos,M))*ssca+off;
	
	gl_FragColor = vec4(shape*color,1.0);
	
}