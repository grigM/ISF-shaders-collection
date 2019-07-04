/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "parallax",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ldsGDn by baldand.  Simple (but not cheap) snow made from multiple parallax layers with randomly positioned flakes and directions. \nAlso includes a DoF effect. Pan around with mouse.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


// Copyright (c) 2013 Andrew Baldwin (twitter: baldand, www: http://thndl.com)
// License = Attribution-NonCommercial-ShareAlike (http://creativecommons.org/licenses/by-nc-sa/3.0/deed.en_US)

// "Just snow"
// Simple (but not cheap) snow made from multiple parallax layers with randomly positioned 
// flakes and directions. Also includes a DoF effect. Pan around with mouse.

#define LIGHT_SNOW // Comment this out for a blizzard

#ifdef LIGHT_SNOW
	#define LAYERS 50
	#define DEPTH .5
	#define WIDTH .3
	#define SPEED .6
#else // BLIZZARD
	#define LAYERS 200
	#define DEPTH .1
	#define WIDTH .8
	#define SPEED 1.5
#endif

void main() {



	const mat3 p = mat3(13.323122,23.5112,21.71123,21.1212,28.7312,11.9312,21.8112,14.7212,61.3934);
	vec2 uv = iMouse.xy/RENDERSIZE.xy + vec2(1.,RENDERSIZE.y/RENDERSIZE.x)*gl_FragCoord.xy / RENDERSIZE.xy;
	vec3 acc = vec3(0.0);
	float dof = 5.*sin(TIME*.1);
	for (int i=0;i<LAYERS;i++) {
		float fi = float(i);
		vec2 q = uv*(1.+fi*DEPTH);
		q += vec2(q.y*(WIDTH*mod(fi*7.238917,1.)-WIDTH*.5),SPEED*TIME/(1.+fi*DEPTH*.03));
		vec3 n = vec3(floor(q),31.189+fi);
		vec3 m = floor(n)*.00001 + fract(n);
		vec3 mp = (31415.9+m)/fract(p*m);
		vec3 r = fract(mp);
		vec2 s = abs(mod(q,1.)-.5+.9*r.xy-.45);
		s += .01*abs(2.*fract(10.*q.yx)-1.); 
		float d = .6*max(s.x-s.y,s.x+s.y)+max(s.x,s.y)-.01;
		float edge = .005+.05*min(.5*abs(fi-5.-dof),1.);
		acc += vec3(smoothstep(edge,-edge,d)*(r.x/(1.+.02*fi*DEPTH)));
	}
	gl_FragColor = vec4(vec3(acc),1.0);
}
