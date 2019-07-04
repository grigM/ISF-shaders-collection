/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "3d",
    "fast",
    "cheap",
    "short",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MsjSW3 by nimitz.  A wild distance field in its natural habitat.",
  "INPUTS" : [
  	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 10
	},
	{
		"NAME": "TL",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": 0,
		"MAX": 6.3
	},
	{
		"NAME": "itCount",
		"TYPE": "float",
		"DEFAULT": 5,
		"MIN": 2,
		"MAX": 9
	},
	{
		"NAME": "camZ",
		"TYPE": "float",
		"DEFAULT": 5.0,
		"MIN": 0,
		"MAX": 5
	},
	{
		"NAME": "mapAmp",
		"TYPE": "float",
		"DEFAULT": 0.7,
		"MIN": 0,
		"MAX": 2.0
	},
	{
		"NAME": "mapMutate",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0,
		"MAX": 3.0
	},
	{
		"NAME": "mapSize",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": -2,
		"MAX": 4.0
	},
	{
		"NAME": "mapQ",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": -0.02,
		"MAX": 4.0
	},
	{
		"NAME": "mapX",
		"TYPE": "float",
		"DEFAULT": 0.4,
		"MIN": -0.4,
		"MAX": 0.8
	},
	{
		"NAME": "mapY",
		"TYPE": "float",
		"DEFAULT": 0.3,
		"MIN": -0.3,
		"MAX": 0.6
	},
	{
      "NAME": "desaturation",
      "TYPE": "float",
      "MIN": 0.0,
      "MAX": 1,
      "DEFAULT": 0.0
    }

  ]
}
*/


		

//Ether by nimitz (twitter: @stormoid)


mat2 m(float a){float c=cos(a), s=sin(a);return mat2(c,-s,s,c);}
float map(vec3 p){
    p.xz*= m(((TIME*speed)+TL)*mapX);
    p.xy*= m(((TIME*speed)+TL)*mapY);
    vec3 q = p*mapQ+((TIME*speed)+TL);
    return length(p+vec3(sin(((TIME*speed)+TL)*mapAmp)))*log(length(p)+mapSize) + sin(q.x+sin(q.z+sin(q.y)))*mapMutate - 1.;
}

void main(){	
	vec2 p = gl_FragCoord.xy/RENDERSIZE.y - vec2(.9,.5);
    vec3 cl = vec3(0.);
    float d = 2.5;
    for(int i=0; i<=int(itCount); i++)	{
		vec3 p = vec3(0,0,camZ) + normalize(vec3(p, -1.))*d;
        float rz = map(p);
		float f =  clamp((rz - map(p+.1))*0.5, -.1, 1. );
        vec3 l = vec3(0.1,0.3,.4) + vec3(5., 2.5, 3.)*f;
        cl = cl*l + (1.-smoothstep(0., 2.5, rz))*.7*l;
		d += min(rz, 1.);
	}
	
	
	
	vec3 grayXfer = vec3(0.3, 0.59, 0.11);
	vec3 gray = vec3(dot(grayXfer, cl));
		
    
  
  
    
    
    gl_FragColor = vec4(mix(cl, gray, desaturation), 1.);
}