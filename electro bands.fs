/*{
	"CREDIT": "by echophons",
	"DESCRIPTION": "electro bands",
	"CATEGORIES": [ "generator"
	],
	"INPUTS": [
		{
			"NAME": "radius",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 0.1,
			"DEFAULT": 0.02
		},
		{
			"NAME": "amp",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 30.0,
			"DEFAULT": 11.54
		},
		{
			"NAME": "band",
			"TYPE": "float",
			"MIN": -1.0,
			"MAX": 1.0,
			"DEFAULT": 0.75
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.33
		},
		{
			"NAME": "thickness",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 10.0,
			"DEFAULT": 1.05
		},
		{
			"NAME": "size",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 100.0,
			"DEFAULT": 47.5
		}
	]
}*/

//
// a very "I'm still learning" edit of 
// electro-prim's by @AlexWDunn
// https://www.shadertoy.com/view/Mll3WS

vec3   iResolution = vec3(RENDERSIZE, 1.0);
float  iGlobalTime = TIME;



#define PI (3.1416)

float electro(vec2 pos, float d, float f, float o, float s, float a, float b)
{
    float ti = iGlobalTime * s * 0.5;
    
    float theta = atan(pos.y,pos.x);
    
    float amp = smoothstep(0.0, 1.0, (sin(theta+iGlobalTime*2.0)*0.5+0.5)-b)*a;
    float phase = d - sin(theta * f + o + ti *1.5) * amp*2.5;
    
    return sin(clamp(phase, 0.0, PI*size) + PI/size) + 0.9999999;
}

float circle(vec2 pos, float r, float a, float o, float s, float f, float b, float t)
{
    float d = length(pos); 
    return 1.0 - smoothstep(0.0, t, electro(pos, d/r, f, o, s, a, b));
}

float shape(vec2 pos, float r, float a, float o, float s, float f, float b, float t)
{
    float ci = circle(pos+vec2(0.0,-0.20),r,a,o,s,f,b,t);
    return ci;
}

void main() 
{
   //const float radius = 0.00950;
   //const float amp = 40.0; 
   const float freq = 4.0;
   //const float band = 0.175;
   //const float speed = 0.001;
   //const float thickness = 4.0;
    
   vec2 pos = gl_FragCoord.xy / max(iResolution.x, iResolution.y) * vec2(2.0) - vec2(1.0);
   pos -= vec2(0.0, -0.5);
    
    
   vec3 color = vec3(0.0);
   color.r = shape(pos, radius, amp, 0.0 *PI, speed*0.01, freq, band, thickness);
   color.g = shape(pos, radius, amp, 0.1 *PI, speed*0.02,   freq, band, thickness);
   color.b = shape(pos, radius, amp, 0.2 *PI, speed*0.03, freq, band, thickness);

   gl_FragColor = vec4(color,1.0);
}