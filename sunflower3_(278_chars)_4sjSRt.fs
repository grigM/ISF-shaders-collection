/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4sjSRt by FabriceNeyret2.  .",
  "INPUTS" : [
  	{
			"NAME": "rc",
			"TYPE": "float",
			"DEFAULT": 1.5,
			"MIN": 0.0,
			"MAX": 2
	},
	{
			"NAME": "gc",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 2
	},
	{
			"NAME": "bc",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 2
	},
	{
			"NAME": "N",
			"TYPE": "float",
			"DEFAULT": 10,
			"MIN": 2.0,
			"MAX": 15
	},
	{
			"NAME": "SPEED",
			"TYPE": "float",
			"DEFAULT": 1,
			"MIN": 0.0,
			"MAX": 4
	},
	{
			"NAME": "SIZE",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": -5.0,
			"MAX": 10
	},
	{
			"NAME": "SPERALIZE",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": -5.0,
			"MAX": 10
	},
	{
			"NAME": "ALPHA_TRANSITION",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": -5.0,
			"MAX": 10
	},

  ]
}
*/



void main() {


    vec2 uv = (gl_FragCoord.xy+gl_FragCoord.xy-(gl_FragColor.xy=RENDERSIZE.xy))/gl_FragColor.y;
    
    
    float t = TIME*SPEED,
          r = length(uv.xy), a = atan(uv.y,uv.x),
          i = floor(r*N);
    a *= floor(pow(128.,i/N)); 	 
    a += 20.*sin(.5*t)+123.34*i-100.*r*cos(.5*t); // (r-0.*i/N)
    r +=  (1.-SIZE+SPERALIZE*cos(a)) / N;    
    r = (floor(N*r)/N)*ALPHA_TRANSITION;
	gl_FragColor = (1.-r)*vec4(rc,gc,bc,1);
}
