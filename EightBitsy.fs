/*{
	"CREDIT": "by mojovideotech",
"CATEGORIES" : [
    "generator"
  ],
  "INPUTS" : [
	{
			"NAME": "rate",
			"TYPE": "float",
			"DEFAULT": 0.05,
			"MIN": -2.0,
			"MAX": 2.0
		},
			{
			"NAME": "size",
			"TYPE": "float",
			"DEFAULT": 150.0,
			"MIN": 0.0,
			"MAX": 300.0
		},
		{
			"NAME": "seed1",
			"TYPE": "float",
			"DEFAULT": 0.05,
			"MIN": -0.25,
			"MAX": 0.5
		},
		{
			"NAME": "seed2",
			"TYPE": "float",
			"DEFAULT": 1,
			"MIN": -0.5,
			"MAX":  1.0
		},
			{
			"NAME": "phase1",
			"TYPE": "float",
			"DEFAULT": -10.0,
			"MIN": -100.0,
			"MAX": 100.0
		},		
		{
			"NAME": "phase2",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": -100.0,
			"MAX": 100.0
		},
		{
			"NAME": "freq",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "pulse",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": -2.0,
			"MAX": 2.0
		},
		{
			"NAME": "invert",
			"TYPE": "bool",
	        "DEFAULT": "FALSE"
		},
		{
			"NAME": "flip",
			"TYPE": "bool",
	        "DEFAULT": "FALSE"
		},
		{
			"NAME": "flop",
			"TYPE": "bool",
	        "DEFAULT": "FALSE"
		}
  ],
  "DESCRIPTION" : ""
}
*/


// EightBitsy by mojovideotech 
// based on:
// glslsandbox.com/e#30753.0

#ifdef GL_ES
precision mediump float;
#endif


#define PULSE(a,b,x) (step((a),(x)) - step((b),(x)))

float f(vec2 uv) {
	return PULSE(seed2, 0.1 + seed1, fract((17.77*phase1) * (uv.x / uv.y) + (21.11*phase2) * uv.x + freq * TIME));
}

void main(void)
{
	vec2 uv = ceil((302.-size) * gl_FragCoord.xy / (RENDERSIZE.y));
	float T = TIME * rate;
	gl_FragColor = vec4(0, 0, 0, 1);
	float light = f(uv) + f(uv + vec2(0.1, 0.1));
	gl_FragColor += vec4(light, 0.01 * light, 0.01, 1.0);
	float back = 0.2 * sin(11.71 * uv.x + T) + mix(0.01, 0.2 + sin(pulse * TIME), cos(55.21 * dot(uv.y, uv.x) + 3.0 + 0.2 * T)); 
	vec4 fx = vec4(back, back, back, 1.0);
	vec4 gfc = fx;
	gl_FragColor /= vec4(gfc);
	if (flip) gl_FragColor = vec4(gl_FragColor.grba);
	if (invert) gl_FragColor /= vec4 (1.0-gl_FragColor.rgb,1.0);
	if (flop) gl_FragColor = gl_FragColor.brga;
}