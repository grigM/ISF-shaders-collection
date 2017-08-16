/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1000,
        1000
      ],
      "MIN" : [
        0,
        0
      ]
    },{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 10
	},
	{
		"NAME": "TL",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": 0,
		"MAX": 6.3
	},
	
		{
			"NAME": "fnc",
			"TYPE": "long",
			"VALUES": [
				0,
				1,
				2,
				3

			],
			"LABELS": [
				"sin",
				"cos",
				"tanX",
				"tanY"
		
			],
			"DEFAULT": 0
		},
	{
		"NAME": "amp",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0.5,
		"MAX": 10
	}, 
	{
		"NAME": "freq",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": -1.0,
		"MAX": 1
	},
	{
		"NAME": "blobCount",
		"TYPE": "float",
		"DEFAULT": 10,
		"MIN": 0,
		"MAX": 30
	},
	
	{
		"NAME": "bOfset",
		"TYPE": "float",
		"DEFAULT": 0.05,
		"MIN": -0.09,
		"MAX": 0.09
	},
	{
		"NAME": "gOfset",
		"TYPE": "float",
		"DEFAULT": 0.01,
		"MIN": -0.09,
		"MAX": 0.09
	},
	{
		"NAME": "smoothstepP1",
		"TYPE": "float",
		"DEFAULT": 0.49,
		"MIN": -1.0,
		"MAX": 1
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40159.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



#define PI  3.14159265358979323846
#define TAU 6.28318530717958647692

float random(in float val) {
    return fract(sin(val * 12.9898) * 43758.5453123);
}

float random(in vec2 st) {
    return fract(sin(dot(st.xy, vec2(12.9898, 78.233))) * 43758.5453123);
}

vec2 random2(in float val){
    vec2 st = vec2( dot(vec2(val), vec2(127.1, 311.7)), dot(vec2(val), vec2(269.5, 183.3)) );
    return -1.0 + 2.0 * fract(sin(st) * 43758.5453123);
}

vec2 random2(in vec2 st){
    st = vec2( dot(st, vec2(127.1, 311.7)), dot(st, vec2(269.5, 183.3)) );
    return -1.0 + 2.0 * fract(sin(st) * 43758.5453123);
}

void main( void )
{
	float d = min(RENDERSIZE.x, RENDERSIZE.y);
    vec2 st = (gl_FragCoord.xy - (RENDERSIZE.xy / 2.0)) / d;
    vec2 p =  (mouse.xy - (RENDERSIZE.xy / 2.0)) / d;
    vec3 color = vec3(0.0);
    float t = (TIME*speed)+TL;
    
    vec3 m_dist = vec3(0.0);
    
    for(int i=0; i<int(blobCount); i++){
        vec2 pos = random2(vec2(float(i), 0.2)) * 0.2;
        float angle  = mix(0.0,  TAU, random(float(i)));
        float radius = mix(0.01, 0.4, random(float(i)))*amp;
        
        vec2 offset;
        if (fnc == 0){
        	offset = radius*vec2(sin(freq*t + angle), cos(freq*t + angle));
        }else if (fnc == 1){
        	offset = radius*vec2(cos(freq*t + angle), cos(freq*t + angle));
        }else if (fnc == 2){
        	offset = radius*vec2(tan(freq*t + angle), cos(freq*t + angle));
        }else if (fnc == 3){
        	offset = radius*vec2(sin(freq*t + angle), tan(freq*t + angle));
        }
        
        m_dist.r += 1.0 / distance(st + vec2(0.00, 0.00), pos + offset);
        m_dist.g += 1.0 / distance(st + vec2(gOfset, 0.01), pos + offset);
        m_dist.b += 1.0 / distance(st + vec2(bOfset, 0.00), pos + offset);
    }
    m_dist.r -= 2.0 / distance(st + vec2(0.00, 0.00), p);
    m_dist.g -= 2.0 / distance(st + vec2(0.01, 0.01), p);
    m_dist.b -= 2.0 / distance(st + vec2(0.05, 0.00), p);

    color += smoothstep(smoothstepP1, 0.5, m_dist * 0.01);
    
    gl_FragColor = vec4(color, 1.0);
}