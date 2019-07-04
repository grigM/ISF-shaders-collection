/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
		"NAME": "xP",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": 0,
		"MAX": 5
	},
	{
		"NAME": "yP",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": 0,
		"MAX": 5
	},
    {
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0,
		"MAX": 5
	},
	{
		"NAME": "lineW",
		"TYPE": "float",
		"DEFAULT": 5.0,
		"MIN": 1.0,
		"MAX": 40.0
	},
	{
		"NAME": "lineSmooth",
		"TYPE": "float",
		"DEFAULT": 0.00001,
		"MIN": 0.0000001,
		"MAX": 0.05
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#42256.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



#define DEBUG 1
#define TAU 6.28318530718

float segment(vec2 p, vec2 a, vec2 b) {
    vec2 ab = b - a;
    vec2 ap = p - a;
    float k = clamp(dot(ap, ab)/dot(ab, ab), 0.0, 1.0);
    return smoothstep(0.0, lineW/RENDERSIZE.y, length(ap - k*ab) - lineSmooth);
}

float shape(float u,vec2 p, float angle) {
    float d = 100.0;
    vec2 a = vec2(1.0, 0.0), b;
    vec2 rot = vec2(cos(angle), sin(angle)*u);
    
    for (int i = 0; i < 6; ++i) {
        b = a;
        for (int j = 0; j < 18; ++j) {
        	b = vec2(b.x*rot.x - b.y*rot.y, b.x*rot.y + b.y*rot.x);
        	d = min(d, segment(p,  a, b));
        }
        a = vec2(a.x*rot.x - a.y*rot.y, a.x*rot.y + a.y*rot.x);
    }
    return d;
}
void main(void)
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    vec2 cc = (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy) / RENDERSIZE.y;
        
    float numPhases=1114.5;
float u;
    float t;
    
   u=0.005+(yP*2.2);
    t = xP/RENDERSIZE.x * numPhases;

    float col = shape(u,abs(cc), t+cos(0.01*(TIME*speed))*TAU);
    col *= 0.5 + 1.5*pow(uv.x*uv.y*(1.0-uv.x)*(1.0-uv.y), 0.3);
    
    
	gl_FragColor = vec4(vec3(pow(1.0-col, 2.15)),1.0);
}
