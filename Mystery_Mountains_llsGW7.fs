/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "e6e5631ce1237ae4c05b3563eda686400a401df4548d0f9fad40ecac1659c46c.jpg"
    }
  ],
  "CATEGORIES" : [
    "3d",
    "raymarching",
    "mountain",
    "2tc15",
    "mystery",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llsGW7 by Dave_Hoskins.  A fractal mountain range in 264 chars.",
  "INPUTS" : [
    {
      "NAME" : "iChannel1",
      "TYPE" : "audio"
    },
    {
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 20.0,
		"MIN": 0.0,
		"MAX": 200.0
	},
    {
		"NAME": "camrotYang",
		"TYPE": "float",
		"DEFAULT": 0.4,
		"MIN": 0.2,
		"MAX": 2.0
	},
	{
		"NAME": "camrotXang",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": -0.5,
		"MAX": 0.5
	},
    {
		"NAME": "bright",
		"TYPE": "float",
		"DEFAULT": 1.5,
		"MIN": 0.5,
		"MAX": 4.0
	},
	{
		"NAME": "noise1",
		"TYPE": "float",
		"DEFAULT": 0.007,
		"MIN": 0.003,
		"MAX": 0.05
	},
	{
		"NAME": "camYpos",
		"TYPE": "float",
		"DEFAULT": 1.3,
		"MIN": 0.5,
		"MAX": 2.0
	},
	{
		"NAME": "camXpos",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": -200.0,
		"MAX": 200.0
	},
	
	{
		"NAME": "camXposSinSpeed",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 5.0
	},
	{
		"NAME": "camXposSinAmp",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 200.0
	},
	
	{
			"NAME": "lightMovePattern",
			"TYPE": "long",
			"VALUES": [
				0,
				1,
				2,
				3

			],
			"LABELS": [
				"stil",
				"sin",
				"crazy",
				"tan_scan_crazy"
		
			],
			"DEFAULT": 0
		},
		{
			"NAME": "rstIncr",
			"TYPE": "event"
		}
	

  ]
}
*/


//// [2TC 15] Mystery Mountains.
// David Hoskins.

// Add layers of the texture of differing frequencies and magnitudes...
#define F +IMG_NORM_PIXEL(iChannel0,mod(.3+p.xz*s/3e3,1.0))/(s+=s) 

vec4 incr =vec4(0,0,0,0);
float timline = 0.0;
void main() {

	

    vec4 p=vec4(gl_FragCoord.xy/RENDERSIZE.xy,1,1)-.5,d=p,t;
    p.z += (TIME*speed);
    p.x = camXpos+cos(TIME*camXposSinSpeed)*camXposSinAmp;
    d.x-=camrotXang;
    d.y-=camrotYang;
    for(float i=bright;i>0.;i-=.002)
    {
        float s=.5;
        t = F F F F F F;
        
        if (lightMovePattern == 0){
        	gl_FragColor = vec4(1,1.,.9,9)+d.x-t*i;
        }else if (lightMovePattern == 1){
        	gl_FragColor = vec4(1,1.,.9,9)+d.x-sin((t*i)+=(TIME/5.0));
        }else if (lightMovePattern == 2){
        	
        	
        	if(rstIncr==true){
        		timline = 0.0;
        	}else{
        		timline+=TIMEDELTA;
        		
        	}
        	
        	
        	incr=+cos(timline*(d.x-t*i));

        	gl_FragColor = vec4(1,1.,.9,9)+incr;
        }else if (lightMovePattern == 3){
        	gl_FragColor = vec4(1,1.,.9,9)+tan(TIME*(d.x-t*i));
        }
        if(t.x>p.y*noise1+camYpos)break;
        p += d;
    }
    
    
}
