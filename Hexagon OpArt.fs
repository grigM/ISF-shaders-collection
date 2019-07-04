/*{
	"CREDIT": "by joshpbatty",
	"DESCRIPTION": "Hexagon Op Art",
	"CATEGORIES": [
		"Joshua Batty"
	],
	  "INPUTS": [
   	{
		"NAME": "stripes",
		"TYPE": "float",
		"DEFAULT": 32.0,
		"MIN": 0.0,
		"MAX": 64.0
	},
   	{
		"NAME": "octaves",
		"TYPE": "float",
		"DEFAULT": 13.0,
		"MIN": 8.0,
		"MAX": 80.0
	},
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 0.176,
		"MIN": 0.0,
		"MAX": 10.0
	},
 	{
		"NAME": "zoom",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 1.0
	},
	{
		"NAME": "soft_shade",
		"TYPE": "float",
		"DEFAULT": 0.01,
		"MIN": 0.0,
		"MAX": 1.0
	},
	{
		"NAME": "Closure",
		"TYPE": "float",
		"DEFAULT": 0.1,
		"MIN": 0.0,
		"MAX": 1.0
	},
	{
		"NAME": "squish",
		"TYPE": "float",
		"DEFAULT": 0.9,
		"MIN": 0.0,
		"MAX": 10.0
	}
  ]
}*/


#define _Smooth(p,r,s) smoothstep(-s, s, p-(r))
#define PI 3.141592
#define TPI 6.2831

// float stripes = 32.;
// float octaves = 13.0;
// float speed = 0.176;
// float zoom = 1.0;
// float soft_shade = 0.01;
// float Closure = 0.10;
// float squish = 0.90;// 0.866025;
float base_shape = abs(sin(TIME*0.1))*.5;


//--------------SYNTH
float df(vec2 pos)
{
    vec2 q = abs(pos);
	return max((q.x * squish +q.y*base_shape),q.y);
}


void main() {	
	vec2 uv = isf_FragNormCoord.xy; // fragCoord.xy / iResolution.xy;
    vec2 position = vec2(RENDERSIZE.x /RENDERSIZE.x * .5,.5);
    
    position = uv - position;
    
    float dist = 1. - df(position *zoom);
    
    float stripID = ceil(dist * stripes );
    float v = mod(stripID , 2.) * 2. - 1.; 
    v *= speed;
    
    float shape =  _Smooth(sin(dist * TPI * stripes), .1 - clamp(Closure * 3. -2.,0.,1.) * 1.2,.05);
    
    float angle = (atan(position.y,position.x)) ;
   	shape *= _Smooth(sin(angle * octaves + TIME * stripID * v ),0.1 - clamp(Closure * 1.5 ,0.,1.) * 1.5,soft_shade);
    shape *= _Smooth(dist,0.,.01);
    
    
    vec3 col = vec3(shape);
	gl_FragColor = vec4(col,1.0);
}