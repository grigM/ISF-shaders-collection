/*
{
	"CREDIT": "by misha from lunapark",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"ripple distortion effect"
	],
  
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
			"NAME": "RIPPLE_SPEED",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 2.0
			
		},
		{
			"NAME": "RIPPLE_MOVE_PHASE",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
			
		},
		{
			"NAME": "RIPPLE_AMOUNT",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": 0.0,
			"MAX": 10.0
			
		},
		{
			"NAME": "RIPPLE_MOD",
			"TYPE": "float",
			"DEFAULT": 0.80,
			"MIN": 0.80,
			"MAX": 1.10
			
		},
		{
			"NAME": "CENTER_X_POS",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
			
		},
		{
			"NAME": "CENTER_Y_POS",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
			
		},
		
	],
  "CATEGORIES" : [
    "distorionripple",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llGXzR by tkoram20.  Just Random UV distortion tests",
  
}
*/


float radial(vec2 pos, float radius)
{
    float result = length(pos)-radius;
    result = fract(result*1.0);
    float result2 = 1.0 - result;
    float fresult = result * result2;
    fresult = pow((fresult*5.5),RIPPLE_AMOUNT);
    //fresult = clamp(0.0,1.0,fresult);
    return fresult;
}




void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    vec2 c_uv = uv * 2.0 - 1.0;
    
    
    c_uv.x += CENTER_X_POS;
    c_uv.y += CENTER_Y_POS;
    
    
    vec2 o_uv = uv * RIPPLE_MOD;
    float gradient = radial(c_uv, (TIME*RIPPLE_SPEED)+RIPPLE_MOVE_PHASE);
    vec2 fuv = mix(uv,o_uv,gradient);
    vec3 col = IMG_NORM_PIXEL(inputImage,mod(fuv,1.0)).xyz;
	gl_FragColor = vec4(col,1.0);
}