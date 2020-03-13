/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WdySDm by scry.  I know not what I have found but I really really like it.",
  "PASSES" : [
    {
      "TARGET" : "BufferA",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {

    }
  ],
  "INPUTS" : [
	{
			"NAME": "sampImage",
			"TYPE": "image"
		},
		{
			"NAME": "samp",
			"TYPE": "event",
			"DEFAULT": false
		},
		
	{
			"NAME": "speed",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.5,
			"DEFAULT": 1.0
		},
		{
			"NAME": "blink",
			"TYPE": "event",
			"DEFAULT": false
		},
		{
			"NAME": "rect_offset",
			"TYPE": "float",
			"MIN": 0.01,
			"MAX": 0.5,
			"DEFAULT": 0.05
		},
		
		{
			"NAME": "px_multypl",
			"TYPE": "float",
			"MIN": 1.0,
			"MAX": 20.0,
			"DEFAULT": 2.0
		},
		
		{
			"NAME": "px_multypl_2",
			"TYPE": "float",
			"MIN": 1.0,
			"MAX": 20.0,
			"DEFAULT": 2.0
		},
		
		{
			"NAME": "amp",
			"TYPE": "float",
			"MIN": 0.5,
			"MAX": 0.999,
			"DEFAULT": 0.997
		},
		
		
		
		{
			"NAME": "b_mod_speed",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 200.999,
			"DEFAULT": 20.0
		},
		
		{
			"NAME": "g_mod_speed",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 20.0,
			"DEFAULT": 20.0
		},
		
		
		
		
  ]
}
*/


#define T(x,y) IMG_NORM_PIXEL(BufferA,mod(tv+vec2(x,y),1.0)).rgb
//#define TIME (TIME)
void main() {
	if (PASSINDEX == 0)	{


	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	    uv = -1. + 2. * uv;
	    vec2 R = RENDERSIZE.xy;
	    vec2 tv = gl_FragCoord.xy/RENDERSIZE.xy;;
	    vec2 px = vec2(1./R.x,1./R.y)*px_multypl;
	    vec3 c = T(0.,0.);
	    px *= (1.-c.r)*(1.-c.r)*px_multypl_2;
	    vec2 fc = gl_FragCoord.xy;
	    vec3 col = vec3(0.);
	    vec3 u = T(0,px.y);
	    vec3 d = T(0.,-px.y);
	    vec3 l = T(px.x,0.);
	    vec3 r = T(-px.x,0.);
	
	    vec3 udlr = (u+d+l+r)/4.;
	    if (tv.x < rect_offset || tv.y < rect_offset || tv.x > (1.0-rect_offset) || tv.y > (1.0-rect_offset)) {
	        col.r = sin(uv.x+uv.y+(TIME*speed)*0.1)*.5+0.5;
	    }
	    
	    if (u.r > d.r || d.r > u.r) {
	        col.b = ((u.r-d.r))*(b_mod_speed);
	    }
	    if (l.r > r.r || l.r > r.r) {
	        col.g = ((l.r-r.r))*(g_mod_speed);
	    }
	    vec3 c2 = T((col.b)*px.x,(col.g)*px.y);
	
	    col.r += c2.r*amp;
	    col.r *= 0.99;
	    if (col.r > 1.) {
	    	col.r = 1.;
	    }
		if(samp){
	    	col = vec3(IMG_NORM_PIXEL(sampImage,gl_FragCoord.xy/RENDERSIZE.xy));
	    }
	    if(blink){
	    	col.r = 1.;
	    	col.b = 1.;
	    	col.g = 1.;
	    }
	    //col.r = mix(col.r,c.r,0.1);
	    gl_FragColor = vec4(col, 1.);
	}
	else if (PASSINDEX == 1)	{


	    // Normalized pixel coordinates (from 0 to 1)
	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	
	    // Time varying pixel color
	    vec3 col = IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).rgb;
		col *= vec3(col.b+col.g);
	    gl_FragColor = vec4(col,1.0);
	}
}
