/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "wipignacircle",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llGGWm by randallfoster.  Example for gamedev.net",
  "INPUTS" : [
	{
            "NAME": "speed",
            "TYPE": "float",
            "DEFAULT": 0.4,
            "MIN": 0.0,
            "MAX": 3.0
    },
    {
            "NAME": "rad_ofset",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": 0.0,
            "MAX": 3.0
    },
    {
            "NAME": "max_rad",
            "TYPE": "float",
            "DEFAULT": 0.4,
            "MIN": 0.05,
            "MAX": 3.0
    },
    {
     "NAME": "iMouse",
            "TYPE": "point2D",
             "DEFAULT": [
				0,
				0
			],
			"MAX": [
				1,
				1
			]

        }
  ]
}
*/


void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	
    vec2 coords = (uv-iMouse);//* 2.0 - 1.0;						//Remap uv to cartesian coordinates. -1 to 1
    coords.x *= RENDERSIZE.x / RENDERSIZE.y;			//Account for aspect ratio
	
	
	//vec2 offsetMoon = vec2(iMouse.x,iMouse.y);
	//color -= fill(circleSDF(st-offsetMoon),.5);
	
	

	gl_FragColor = vec4(0.0,1.0,0.0,1.0);					//Set the background to green

	float l_time = fract((TIME / 0.75)*speed);			//Calculate a 0-1 time ... In this case it repeats every 0.75 seconds
    
	if(length(coords) > (l_time * max_rad)+rad_ofset)  			//Set to black if less than 
    {
		gl_FragColor = vec4(0.0,0.0,0.0,1.0);
    }    
}