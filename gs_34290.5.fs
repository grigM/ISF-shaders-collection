/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34290.5",
  "INPUTS" : [
 		 {
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 6.0
		},
		{
			"NAME": "BALL_NUM",
			"TYPE": "float",
			"DEFAULT": 100.0,
			"MIN": 0.0,
			"MAX": 150.0
		},
		{
			"NAME": "BALL_DIST",
			"TYPE": "float",
			"DEFAULT": 12.7,
			"MIN": 0.0,
			"MAX": 40.0
		}
		,
		{
			"NAME": "AR_RAD",
			"TYPE": "float",
			"DEFAULT": 7.7,
			"MIN": 0.0,
			"MAX": 35.0
		}
		,
		{
			"NAME": "BALL_SIZE",
			"TYPE": "float",
			"DEFAULT": 12.0,
			"MIN": 0.0,
			"MAX": 50.0
		},
		{
			"NAME": "sincosIncr",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 5.0
		}

  ],
  "PERSISTENT_BUFFERS" : [
    "backbuffer"
  ],
  "PASSES" : [
    {
      "TARGET" : "backbuffer"
    }
  ]
}
*/


#ifdef GL_ES
precision mediump float;
#endif


#define PI 3.14


void main( void ) {
	vec2 uv2 = gl_FragCoord.xy / RENDERSIZE * 2.0 - 1.0;
	uv2.x *= RENDERSIZE.x/RENDERSIZE.y;
	
	float o = 0.0;
	vec3 color = vec3(.0);
	
	for (float i = 0.0; i < BALL_NUM; ++i) {
		vec2 uv = uv2;
		uv.x += sin(TIME * (speed/2.0) + o) + cos(TIME * speed + o) + sin(o * AR_RAD);
		uv.y += cos(TIME * (speed/2.0) + o) + sin(TIME * speed + o) + cos(o * BALL_DIST);
		float t = TIME*speed;
		float d = length(uv*10.) - 0.03 - pow(BALL_SIZE * 0.1, 2.0);
		color = mix(color, vec3(sin(o - t), sin(o*8.0+6.0 + t), cos(o*13.0*16.0 + t))*0.5+0.5, smoothstep(0.01, -0.01, d));
		color = mix(color, IMG_NORM_PIXEL(backbuffer,mod(gl_FragCoord.xy / RENDERSIZE,1.0)).rgb, 0.01);
		
		o += sincosIncr * PI / float(BALL_NUM);
	}
	
	gl_FragColor = vec4(color, 1.0 );
}