/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "e6e5631ce1237ae4c05b3563eda686400a401df4548d0f9fad40ecac1659c46c.jpg"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MsKGWR by iq.  Doodle",
  "INPUTS" : [
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 3.0
	},
	{
		"NAME": "speed_2",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 3.0
	}
  ]
}
*/


// Created by inigo quilez - iq/2016
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy + 0.01*TIME;
    
    vec3 col = vec3(0.0);
    
    for( int i=0; i<35; i++ )
    {
        vec2 off = 0.04*cos( 8.0*uv + 0.07*float(i) + 3.0*TIME + vec2(0.0,1.0));
        vec3 tmp = IMG_NORM_PIXEL(iChannel0,mod(uv + off,1.0)).yzx;
        col += tmp*tmp*tmp;
    }
    
    col /= 5.0;
    
	gl_FragColor = vec4( col, 1.0 );
}
