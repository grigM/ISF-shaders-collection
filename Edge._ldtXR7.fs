/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "sin",
    "cos",
    "animation",
    "smoothstep",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ldtXR7 by IO.  The result of last night. \n\nThx to all.\nHave a inspiring day.\nIO_",
  "INPUTS": [
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 9.0
		}

  ]
}
*/


void main()
{
	
	float alpha = 0.0;

	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv = 2.0 * (uv) - 1.0;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    
    vec3 fColor = vec3(0.0);
    
    fColor.r = smoothstep(0.0,0.018,(sin(TIME*speed)*uv.y)+(cos(TIME*speed)*uv.x));
    fColor.g = smoothstep(0.0,0.018,(sin(TIME*speed+0.01)*uv.y)+(cos((TIME*speed)+0.01)*uv.x));
    fColor.b = smoothstep(0.0,0.020,(sin((TIME*speed)+0.02)*uv.y)+(cos((TIME*speed)+0.02)*uv.x));
    
    if(fColor.r==0.0 && fColor.g== 0.0 && fColor.b == 0.0 ){
    	alpha = 0.0;
    }
	gl_FragColor = vec4(fColor,1.0);
}