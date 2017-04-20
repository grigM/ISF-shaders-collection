/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "basic",
    "reactive",
    "audio",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MdsXWM by airtight.  Super basic audio-reactive HSL light ring. My first shader here :)",
  "INPUTS" : [
    {
    	"NAME":"volInput",	
    	"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0.0,
		"MAX": 1.5
    },
	{
			"NAME": "dots",
			"TYPE": "float",
			"DEFAULT": 40.0,
			"MIN": 0.0,
			"MAX": 50.0
	},
	{
			"NAME": "radius",
			"TYPE": "float",
			"DEFAULT": 0.25,
			"MIN": 0.0,
			"MAX": 1.0
	},
	{
			"NAME": "brightness",
			"TYPE": "float",
			"DEFAULT": 0.02,
			"MIN": 0.0,
			"MAX": 0.2
	},
	{
			"NAME": "rot_speed",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": 0.0,
			"MAX": 100.0
	},
	{
			"NAME": "black_inner",
			"TYPE": "float",
			"DEFAULT": 0.26,
			"MIN": -0.1,
			"MAX": 0.8
	},{
			"NAME": "black_smoth",
			"TYPE": "float",
			"DEFAULT": 0.05,
			"MIN": 0.0,
			"MAX": 0.1
	}
  ]
}
*/




//const float dots = 40.; //number of lights
//const float radius = .25; //radius of light ring
//const float brightness = 0.02;

//convert HSV to RGB
vec3 hsv2rgb(vec3 c){
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
		
void main(){
	
	vec2 p = (gl_FragCoord.xy-.5*RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y);
    vec3 c=vec3(0,0,0.1); //background color
		
    for(float i=0.;i<dots; i++){
	
		//read frequency for this dot from audio input channel 
		//based on its index in the circle
		float vol =  volInput;//IMG_NORM_PIXEL(AudioWaveformImage,mod(vec2(i/dots, 0.0),1.0)).x;
		float b = vol * brightness;
		
		//get location of dot
        float x = radius*cos(2.*3.14*float(i)/dots);
        float y = radius*sin(2.*3.14*float(i)/dots);
        vec2 o = vec2(x,y);
	    
		//get color of dot based on its index in the 
		//circle + time to rotate colors
		vec3 dotCol = hsv2rgb(vec3((i + TIME*rot_speed)/dots,1.,1.0));
	    
        //get brightness of this pixel based on distance to dot
		c += b/(length(p-o))*dotCol;
    }
	
    //black circle overlay	   
	float dist = distance(p , vec2(0));  
	c = c * smoothstep(black_inner, black_inner+black_smoth,  dist);
	 
	gl_FragColor = vec4(c,1);
}