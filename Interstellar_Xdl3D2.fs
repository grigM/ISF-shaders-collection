/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex16.png"
    }
  ],
  "CATEGORIES" : [
    "starfield",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xdl3D2 by TekF.  March through a 2D grid, offsetting stars along z for each grid cell. This is much faster than doing a loop over all stars, but creates some artefacts.",
  "INPUTS": [
		
		{
			"NAME": "OFFSET_SPEED",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MAX": 1.0,
			"MIN": 0.0
		},
		{
			"NAME": "ACCELERATION_SPEED",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MAX": 3.0,
			"MIN": 0.5
		},{
			"NAME": "DOT_WARP",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.05,
			"MAX": 4.0
		},{
			"NAME": "SPEED",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 3.0
		},{
			"NAME": "DISTANCE_VAL",
			"TYPE": "float",
			"DEFAULT": 50.0,
			"MIN": 0.0,
			"MAX": 100.0
		}
      ]
}
*/


const float tau = 6.28318530717958647692;

// Gamma correction
#define GAMMA (2.2)

vec3 ToLinear( in vec3 col )
{
	// simulate a monitor, converting colour values into light values
	return pow( col, vec3(GAMMA) );
}

vec3 ToGamma( in vec3 col )
{
	// convert back into colour values, so the correct light will come out of the monitor
	return pow( col, vec3(1.0/GAMMA) );
}

vec4 Noise( in ivec2 x )
{
	return IMG_NORM_PIXEL(iChannel0,mod((vec2(x)+0.5)/256.0,1.0),-100.0);
}

vec4 Rand( in int x )
{
	vec2 uv;
	uv.x = (float(x)+0.5)/256.0;
	uv.y = (floor(uv.x)+0.5)/256.0;
	return IMG_NORM_PIXEL(iChannel0,mod(uv,1.0),-100.0);
}


void main()
{
	vec3 ray;
	ray.xy = 2.0*(gl_FragCoord.xy-RENDERSIZE.xy*.5)/RENDERSIZE.x;
	ray.z = 1.0;

	float offset = TIME*OFFSET_SPEED;	
	float speed2 = (cos(offset)+1.0)*ACCELERATION_SPEED;
	float speed = speed2+DOT_WARP;
	offset += sin(offset)*.96;
	offset *= SPEED;
	
	
	vec3 col = vec3(0);
	
	vec3 stp = ray/max(abs(ray.x),abs(ray.y));
	
	vec3 pos = 2.0*stp+.5;
	for ( int i=0; i < 20; i++ )
	{
		float z = Noise(ivec2(pos.xy)).x;
		z = fract(z-offset);
		float d = DISTANCE_VAL*z-pos.z;
		float w = pow(max(0.0,1.0-8.0*length(fract(pos.xy)-.5)),2.0);
		vec3 c = max(vec3(0),vec3(1.0-abs(d+speed2*.5)/speed,1.0-abs(d)/speed,1.0-abs(d-speed2*.5)/speed));
		col += 1.5*(1.0-z)*c*w;
		pos += stp;
	}
	
	gl_FragColor = vec4(ToGamma(col),1.0);
}