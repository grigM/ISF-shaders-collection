/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex07.jpg"
    }
  ],
  "CATEGORIES" : [
    "chromaticaberration",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tySWh by DifferentName.  Modified from shaders by LordSk and Sintel, to add more color variety!",
  "INPUTS" : [
	{
			"NAME": "RADIUS",
			
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.75
		},
		{
			"NAME": "AB_SCALE",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 3.0,
			"DEFAULT": 0.75
		},
		{
			"NAME": "RED_SPEED",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 5.0,
			"DEFAULT": 1.0
		}
		,
		{
			"NAME": "GREEN_SPEED",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 5.0,
			"DEFAULT": 1.0
		}
		,
		{
			"NAME": "BLUE_SPEED",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 5.0,
			"DEFAULT": 1.0
		}
  ]
}
*/




float diskColorr(in vec2 uv, vec2 offset)
{
    uv = uv - smoothstep(0.01,1.8,IMG_NORM_PIXEL(iChannel0,mod((uv*1.0 - vec2(((TIME*RED_SPEED)+0.06) /3.6,((TIME*RED_SPEED)+0.06) /9.2)) + offset,1.0)).r) * 0.3;
    
    float d = length(uv)-RADIUS;
    return smoothstep(0.01,0.015,d);
}
float diskColorg(in vec2 uv, vec2 offset)
{
    uv = uv - smoothstep(0.01,1.8,IMG_NORM_PIXEL(iChannel0,mod((uv*1.0 - vec2((TIME*GREEN_SPEED) /3.0,((TIME*GREEN_SPEED)) /8.0)) + offset,1.0)).r) * 0.3;
    
    float d = length(uv)-RADIUS;
    return smoothstep(0.01,0.015,d);
}
float diskColorb(in vec2 uv, vec2 offset)
{
    uv = uv - smoothstep(0.01,1.8,IMG_NORM_PIXEL(iChannel0,mod((uv*1.0 - vec2(((TIME*BLUE_SPEED)-0.06) /2.65,((TIME*BLUE_SPEED)-0.06) /7.0)) + offset,1.0)).r) * 0.3;
    
    float d = length(uv)-RADIUS;
    return smoothstep(0.01,0.015,d);
}

void main()
{
	vec2 uv = (-RENDERSIZE.xy + 2.0 * gl_FragCoord.xy) / RENDERSIZE.y;
   	
    vec3 color = vec3(0);
    color.r+=diskColorr(uv, vec2(0.00, 0.00) * AB_SCALE);
    color.g+=diskColorg(uv, vec2(0.00, 0.00) * AB_SCALE);
    color.b+=diskColorb(uv, vec2(0.00, 0.00) * AB_SCALE);
    gl_FragColor = vec4(color, 1.0);
}
