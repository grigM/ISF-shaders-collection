/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xl3Xzl by mbouchard.  Test WIP",
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    },
    {
		"NAME": "n_randParam",
		"TYPE": "float",
		"MIN": 0.0,
		"MAX": 1.0,
		"DEFAULT": 0.01
	},
    {
		"NAME": "desaturate_r",
		"TYPE": "float",
		"MIN": 0.0,
		"MAX": 1.0,
		"DEFAULT": 0.3
	},
    {
		"NAME": "desaturate_g",
		"TYPE": "float",
		"MIN": 0.0,
		"MAX": 1.0,
		"DEFAULT": 0.59
	},
    {
		"NAME": "desaturate_b",
		"TYPE": "float",
		"MIN": 0.0,
		"MAX": 1.0,
		"DEFAULT": 0.11
	},
    {
		"NAME": "desaturate_smothstep_1",
		"TYPE": "float",
		"MIN": 0.0,
		"MAX": 1.0,
		"DEFAULT": 0.2
	},
    {
		"NAME": "desaturate_smothstep_2",
		"TYPE": "float",
		"MIN": 0.0,
		"MAX": 1.0,
		"DEFAULT": 0.5
	}
  ]
}
*/




float nrand( vec2 n )
{
	return fract(sin(dot(n.xy, vec2(12.9898, 78.233)))* 43758.5453);
}

float n1rand( vec2 n )
{
	float t = fract( TIME );
    //t = 1.;
	float nrnd0 = nrand( n + n_randParam*t );
	return nrnd0;
}

void main()
{

    
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;

    float N 	= n1rand(uv * 2.3);
 
    vec4 ditheredTex = IMG_NORM_PIXEL(inputImage,mod(uv,1.0));
    float desaturateTex = dot(vec3(desaturate_r, desaturate_g, desaturate_b),vec3(ditheredTex));
    desaturateTex = pow(1. - desaturateTex,1.);
   	desaturateTex = smoothstep(desaturate_smothstep_1, desaturate_smothstep_2, desaturateTex);
    
    vec3 final = vec3(step(desaturateTex,N)); 
	gl_FragColor = vec4(final,1);
}