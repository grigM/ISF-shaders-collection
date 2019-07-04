/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "minimal",
    "dot",
    "point",
    "composition",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llt3zj by EanJee.  2D composition practice",
  "INPUTS" : [
	{
			"NAME": "circleMaxSize",
			"TYPE": "float",
			"DEFAULT": 0.4,
			"MIN": 0.0,
			"MAX": 5.0
		},
		{
			"NAME": "repeatTime",
			"TYPE": "float",
			"DEFAULT": 32.0,
			"MIN": 0.0,
			"MAX": 64.0
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 2.5,
			"MIN": 0.0,
			"MAX": 50.0
		},
				
  ]
}
*/


#define PI 3.14159

vec3 drawCircleFill(vec2 st, vec2 resolution, float radius, float fade) {
    vec2 mappedSt = st * resolution;
    float maxRadius = min(resolution.x, resolution.y);
    float pct = smoothstep(radius * maxRadius, (radius + fade) * maxRadius, 
                           length(mappedSt - resolution * 0.5));
    return vec3(pct);
}

void main(){
    vec2 st = gl_FragCoord.xy / RENDERSIZE.xy;

    // find orientation
    float xCoef = RENDERSIZE.x > RENDERSIZE.y ? 1.0 : 0.0;
    vec2 coef = vec2(xCoef, 1.0 - xCoef);
    vec2 coefReversed = 1.0 - coef;
    
    // tile on longer one
    //float repeatTime = 32.0;
    vec2 repeat = vec2(1.0) + coef * (repeatTime - 1.0);
    vec2 stTiled = fract(st * repeat);
    
    // get right resolution for each slice
    vec2 repeatReversed = vec2(1.0) + coefReversed * (repeatTime - 1.0);
    vec2 resolution = vec2(RENDERSIZE.x / RENDERSIZE.y, 1.0) * repeatReversed;
    
    // transform
    vec2 sliceIndex = floor(st * repeat);
    float phase = max(sliceIndex.x, sliceIndex.y) * PI / repeatTime * 2.0;
    float offsetValue = sin(TIME * speed + phase);
    vec2 offset = offsetValue * coefReversed * 0.08;
    stTiled += offset;
  
    // draw circle
    vec3 color = drawCircleFill(stTiled, resolution, circleMaxSize * abs(offsetValue), 0.1);
    gl_FragColor = vec4(color, 1.0);
}