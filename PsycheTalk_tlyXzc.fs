/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tlyXzc by Reva.  Ex04",
  "INPUTS" : [
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 2
	},
	{
		"NAME": "noise_p",
		"TYPE": "float",
		"DEFAULT": 0.248 ,
		"MIN": 0,
		"MAX": 1
	},
	{
		"NAME": "lines_p",
		"TYPE": "float",
		"DEFAULT": 0.5 ,
		"MIN": 0,
		"MAX": 1
	},
	{
		"NAME": "zoom",
		"TYPE": "float",
		"DEFAULT": 10.0 ,
		"MIN": 1,
		"MAX": 100
	}
  ]
}
*/


// Author Reva - 2020-03-03

vec2 random2(vec2 st){
    st = vec2( dot(st,vec2(127.1,311.7)),
              dot(st,vec2(269.5,183.3)) );
    return -1.0 + 2.0*fract(sin(st)*43758.5453123);
}

// Value noise by Inigo Quilez - iq/2013
// https://www.shadertoy.com/view/lsf3WH
float noise(vec2 st) {
    vec2 i = floor(st);
    vec2 f = fract(st);

    vec2 u = f*f*(3.0-2.0*f);

    return mix( mix( dot( random2(i + vec2(0.0,0.0) ), f - vec2(0.0,0.0) ),
                     dot( random2(i + vec2(1.0,0.0) ), f - vec2(1.0,0.0) ), u.x),
                mix( dot( random2(i + vec2(0.0,1.0) ), f - vec2(0.0,1.0) ),
                     dot( random2(i + vec2(1.0,1.0) ), f - vec2(1.0,1.0) ), u.x), u.y);
}
mat2 rotate2d(float angle){
    return mat2(cos(angle),-sin(angle),
                sin(angle),cos(angle));
}

float lines(in vec2 pos, float b){
    return smoothstep(0.0,
                      b*fract((TIME*speed)*0.6)*4.0,
                      abs((sin(pos.x*noise(vec2(abs(pos.x)-(TIME*speed)*0.3,pos.y*(mod((TIME*speed),100.0)+700.0)*0.024)))))*0.900);
}

void main() {

    vec2 R = RENDERSIZE.xy;
    vec2 pos = zoom*(gl_FragCoord.xy - .5*R)/R.y;
    pos *= rotate2d( noise(pos)*noise_p );
    float pattern = lines(pos,lines_p);  
    float col = mix(pattern, 1. - pattern, step(3.,mod((TIME*speed),6.)));
    gl_FragColor = vec4(col);
}
