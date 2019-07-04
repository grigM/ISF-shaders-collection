/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "test",
    "epilepsy",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MlffDN by Janbg.  This is not a actual test for epilepsy.",
  "INPUTS" : [
	{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 5.0
		}
  ]
}
*/


const vec3 backgroundColor = vec3(0.0, 0.0, 0.0);
const vec3 RED 	= vec3(1.0, 0.0, 0.0);
const vec3 GREEN 	= vec3(0.0, 1.0, 0.0);
const vec3 BLUE 	= vec3(0.0, 0.0, 1.0);

float multiplierTime = 2.0;


vec3 Circle(vec2 uv, vec3 colorCircle) {
    
    float d = length(uv); // length(uv) == sqrt(uv.x*uv.x + uv.y*uv.y)
    vec3 color = backgroundColor;
    float radiusCircle = 0.2+0.2*sin((TIME*multiplierTime)*speed);
    
    if(d <= radiusCircle) 
        color = colorCircle;

    return color;
}

vec3 Rectangle(vec2 uv, vec3 colorRectangle, bool position) {
    
    float d = uv.x;
    vec3 color = backgroundColor;
    float medium = 0.0;
    float radiusCircle = 0.2-0.2*sin((TIME*multiplierTime)*speed);
    if(!position)
    {
		if(d < medium && length(uv) > radiusCircle) 
        	color = colorRectangle;
    }
    else
    {
        if(d > medium && length(uv) > radiusCircle) 
        	color = colorRectangle;
    }
    
    return color;
}

void main() {



	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy ) - 0.5;  // -0.5 <> 0.5
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    
    bool Left = false; 
    bool Right = true;
    
    vec3 c = backgroundColor;
    
 
    float temp = 0.5+0.5*sin(1.0*(TIME*multiplierTime)*speed);
    vec3 hameleon = vec3(temp, temp, temp);
    c += Circle(uv, hameleon);
 
    temp = 0.5-0.5*sin(1.0*(TIME*multiplierTime)*speed);
    hameleon = vec3(temp, temp, temp);
	c += Rectangle(uv, hameleon, Left);
    
    temp = 0.5-0.5*sin(1.0*(TIME*multiplierTime)*speed);
    hameleon = vec3(temp, temp, temp);
    c += Rectangle(uv, hameleon, Right);
    
    
    
    gl_FragColor = vec4(c, 1.0);
    
}
