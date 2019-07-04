/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "grid",
    "warp",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MlVSDW by cacheflowe.  Warping a grid with noise",
  "INPUTS" : [
  {
      "NAME" : "color_invert",
      "TYPE" : "bool",
      "DEFAULT" : 1
    },
	
	{
			"NAME": "divisor",
			"TYPE": "float",
			"DEFAULT": 12.0,
			"MIN": 5,
			"MAX": 30
		},
		{
			"NAME": "distortion_amnt",
			"TYPE": "float",
			"DEFAULT": 0.15,
			"MIN": 0,
			"MAX": 2
		}
  ]
}
*/


#define PI     3.14159265358
#define TWO_PI 6.28318530718

// 2D Random
float random (in vec2 st) { 
    return fract(sin(dot(st.xy,vec2(12.9898,78.233))) * 43758.5453123);
}

// 2D Noise based on Morgan McGuire @morgan3d
// https://www.shadertoy.com/view/4dS3Wd
float noise (in vec2 st) {
    vec2 i = floor(st);
    vec2 f = fract(st);
    float a = random(i);
    float b = random(i + vec2(1.0, 0.0));
    float c = random(i + vec2(0.0, 1.0));
    float d = random(i + vec2(1.0, 1.0));
    vec2 u = f*f*(3.0-2.0*f);
    return mix(a, b, u.x) + 
            (c - a)* u.y * (1.0 - u.x) + 
            (d - b) * u.x * u.y;
}

void main()
{
    float time = TIME * 1.;									// adjust time
    vec2 uv = (2. * gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.y;	// center coordinates
    float dist = pow(length(uv), 0.5);
    float uvDeformMult = 1. + dist * cos(noise(uv * 2.) + 2. * noise(uv * 3.) + time);
    uv *= 1. + distortion_amnt * sin(time) * uvDeformMult;
    //float divisor = 12.;
    float col = min(
        smoothstep(0.1, 0.25, abs(sin(uv.x * divisor))),
        smoothstep(0.1, 0.25, abs(sin(uv.y * divisor)))
    );
    
    vec3 inv_color = vec3(1.0-vec3(col).r, 1.0-vec3(col).g, 1.0-vec3(col).b);
	if(color_invert){
		gl_FragColor = vec4( inv_color, 1.0 );
	}else{
		gl_FragColor = vec4(vec3(col),1.0);
	}
}