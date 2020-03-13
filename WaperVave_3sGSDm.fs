/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3sGSDm by scry.  Things are as they are.\n\nBest in full-screen, lets the patterns really stretch out.",
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
			"MIN": 0.0,
			"MAX": 0.5,
			"DEFAULT": 0.05
		},
		
		{
			"NAME": "px_multypl",
			"TYPE": "float",
			"MIN": 1.0,
			"MAX": 20.0,
			"DEFAULT": 4.0
		},
		
		{
			"NAME": "g_mod_speed_in_rect",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1000.0,
			"DEFAULT": 500.0
		},
		
		{
			"NAME": "b_mod_speed_in_rect",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1000.0,
			"DEFAULT": 500.0
		},
		
		
		{
			"NAME": "r_mod_speed",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 0.3,
			"DEFAULT": 0.2
		},

		{
			"NAME": "b_mod_speed",
			"TYPE": "float",
			"MIN": 0.01,
			"MAX": 5.0,
			"DEFAULT": 0.01
		},
		
		{
			"NAME": "g_mod_speed",
			"TYPE": "float",
			"MIN": 0.01,
			"MAX": 5.0,
			"DEFAULT": 0.01,
		}

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
	    px *= (1.-c.r)*c.r*4.;
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
	        col.b = ((u.r-d.r))*(b_mod_speed_in_rect);
	    }
	    if (l.r > r.r || l.r > r.r) {
	        col.g = ((l.r-r.r))*(g_mod_speed_in_rect);
	    }
	    vec3 c2 = T((col.b-0.5)*px.x,(col.g-0.5)*px.y);
	    col.r += (u.r+d.r+l.r+r.r)/4.3;
	    col.r = mod(col.r+0.03,cos((TIME*speed)*0.1)*0.5+.75)*0.96;
	    col.r += c2.r*r_mod_speed;
	    col.b *= b_mod_speed;
	    col.g *= g_mod_speed;
	    
	    
	    col = mod(col*0.9,1.);
	    
	    col = mix(col,(udlr+c)/2.,col.b-col.r*0.95);
	    if(samp){
	    	col = vec3(IMG_NORM_PIXEL(sampImage,gl_FragCoord.xy/RENDERSIZE.xy));
	    }
	    if(blink){
	    	col.r = 1.;
	    	col.b = 1.;
	    	col.g = 1.;
	    }
	    //col = mix(col,c,0.9);
	    gl_FragColor = vec4(col, 1.);
	}
	else if (PASSINDEX == 1)	{


	    // Normalized pixel coordinates (from 0 to 1)
	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	
	    // Time varying pixel color
	    vec3 col = IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).rgb;
	    gl_FragColor = vec4(col,1.0);
	}
}
