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
  ]
}
*/


void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	
    vec2 coords = uv * 2.0 - 1.0;						//Remap uv to cartesian coordinates. -1 to 1
    coords.x *= RENDERSIZE.x / RENDERSIZE.y;			//Account for aspect ratio

	gl_FragColor = vec4(0.0,1.0,0.0,1.0);					//Set the background to green

	float l_time = fract((TIME / 0.75)*speed);			//Calculate a 0-1 time ... In this case it repeats every 0.75 seconds
    float l_radius = 2.2;
    
	if(length(coords) > l_time * l_radius)  			//Set to black if less than 
    {
		gl_FragColor = vec4(0.0,0.0,0.0,1.0);
    }    
}