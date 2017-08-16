/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 3
	},
	{
		"NAME": "TL",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": 0,
		"MAX": 2
	},
	{
		"NAME": "noiseU1",
		"TYPE": "float",
		"DEFAULT": 3,
		"MIN": 2,
		"MAX": 5
	},{
	
		"NAME": "noiseU2",
		"TYPE": "float",
		"DEFAULT": 2,
		"MIN": 1,
		"MAX": 4
	},
	{
		"NAME": "amp",
		"TYPE": "float",
		"DEFAULT": 2,
		"MIN": 0,
		"MAX": 15
	},
	{
		"NAME": "c1p",
		"TYPE": "float",
		"DEFAULT": 0.05,
		"MIN": -0.1,
		"MAX": 0.2
	},
	{
		"NAME": "c2p",
		"TYPE": "float",
		"DEFAULT": 0.05,
		"MIN": -0.1,
		"MAX": 0.2
	},
	{
		"NAME": "c3p",
		"TYPE": "float",
		"DEFAULT": 0.05,
		"MIN": -0.1,
		"MAX": 0.2
	},
	{
		"NAME": "smoooth",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": 0,
		"MAX": 1
	}
	
	

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#41585.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float random (in vec2 st) { 
    return fract(sin(dot(st.xy,vec2(12.9898,78.233)))
		 * 43758.5453123);
}

// The MIT License
// Copyright © 2013 Inigo Quilez
float noise( in vec2 p )
{
    vec2 i = floor( p );
    vec2 f = fract( p );
    
    vec2 u = f*f*(noiseU1-noiseU2*f);

    return mix( mix( random( i + vec2(0.0,0.0) ), 
                     random( i + vec2(1.0,0.0) ), u.x),
                mix( random( i + vec2(0.0,1.0) ), 
                     random( i + vec2(1.0,1.0) ), u.x), u.y);
}

float smoothMin(float d1, float d2, float k){
    float h = exp(-k * d1) + exp(-k * d2);
    return -log(h) / k;
}

void main( void ) {

	vec2 p = ( gl_FragCoord.xy * 2. - RENDERSIZE.xy ) / min(RENDERSIZE.x, RENDERSIZE.y);
	p = vec2(dot(p-noise(vec2((TIME*speed)+TL)), p)-noise(vec2((TIME*speed)+TL)), dot(p/noise(vec2((TIME*speed)+TL)), p)-noise(vec2((TIME*speed)+TL)));
	#define a p = abs(p) / dot(p, p)-noise(vec2((TIME*speed)+TL));
	a;
	vec2 q = vec2(amp * noise(vec2(p.x / cos( noise(vec2((TIME*speed)+TL)) ) )), p.y + amp * noise(vec2(p.y * sin( noise(vec2((TIME*speed)+TL)) ))));
	q = vec2(dot(q+noise(vec2(q+(TIME*speed))+TL), q));
	vec2 r = vec2(amp * noise(vec2(q.x / cos( noise(vec2((TIME*speed)+TL)) ) )), q.y + amp * noise(vec2(q.y * sin( noise(vec2((TIME*speed)+TL)) ))));

	float c1 = c1p/length(p);
	float c2 = c2p/length(q);
	float c3 = c1p/length(r);
	
	float d =  (c1 + c2 + c3);

	gl_FragColor = vec4( vec3( d ), 1.0 );

}