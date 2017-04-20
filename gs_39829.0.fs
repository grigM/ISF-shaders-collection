/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "SPEED_RATIO",
		"TYPE": "float",
		"DEFAULT": 0.2,
		"MIN": -1.0,
		"MAX": 1.0
	},
	{
		"NAME": "NUMBER_OF_CIRCLES",
		"TYPE": "float",
		"DEFAULT": 9.0,
		"MIN": 1,
		"MAX": 16.0
	},
	
	{
		"NAME": "BAR_WIDTH",
		"TYPE": "float",
		"DEFAULT": 0.33,
		"MIN": 0.2,
		"MAX": 2.0
	},
	
	{
		"NAME": "MARGIN",
		"TYPE": "float",
		"DEFAULT": 0.111,
		"MIN": 0,
		"MAX": 0.5
	},
	{
      "NAME" : "color_1",
      "TYPE" : "color",
      "DEFAULT" : [
        1.0,
        1.0,
        1.0,
        1
      ],
      "LABEL" : ""
    },
    {
      "NAME" : "color_2",
      "TYPE" : "color",
      "DEFAULT" : [
        0.0,
        0.0,
        0.0,
        0.0
      ],
      "LABEL" : ""
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39829.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

//#extension GL_OES_standard_derivatives : enable



//vec3 COLOR_FG;
//const vec3 COLOR_BG = vec3(0.0);

//const float NUMBER_OF_CIRCLES = 9.0;
//const float SPEED_RATIO = 0.2;




const float PI = 3.14159265359;

float aastep(float threshold, float value)
{
    float aaf = fwidth(value) * 0.5;
    return smoothstep(threshold-aaf, threshold+aaf, value);
}

void main(  )
{
	float BAR_MARGIN = (1.0 - BAR_WIDTH) * 0.5;
	//COLOR_FG = vec3(color_1);
	
    vec2 uv = 2.0 * gl_FragCoord.xy / RENDERSIZE.xy - vec2(1.0);
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    
    float a = 1.0-mod(0.25+0.5+atan(uv.y, uv.x)*0.5/PI,1.0);
    float d = length(uv);
    
    float c = 0.0;
    
    if (d > MARGIN && d < (1.0 - MARGIN))
    {
        float dd = (d - MARGIN) / (1.0 - MARGIN * 2.0);
        float qq = floor(dd * NUMBER_OF_CIRCLES) / NUMBER_OF_CIRCLES;
        float qq0 = floor(dd * NUMBER_OF_CIRCLES) / (NUMBER_OF_CIRCLES-1.0);        
        float rr = fract(dd * NUMBER_OF_CIRCLES);
        if (rr > BAR_MARGIN && rr < (1.0-BAR_MARGIN))
        {
            float rrr = 1.0-abs(2.0*(rr - BAR_MARGIN)/BAR_WIDTH-1.0);
            float speed = (11.0 - qq0*10.0) * SPEED_RATIO;
            float aa = mod(a - TIME * speed - TIME * 0.05, 1.0); 
            if (aa > 0.5)
            {
        		c = aastep(1.0-aa,rrr*0.5);
            }
        }
    }
    
	gl_FragColor = vec4(mix(vec3(color_2), vec3(color_1), c), 0.9);
}

// version history
// 1.0 - original version [tpen]
// 1.1 - timing fix [void room]