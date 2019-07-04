/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtGyzD by nabr.  what should i say i the king of colors ....",
  "INPUTS" : [

  ]
}
*/


// nabr
// https://www.shadertoy.com/view/XtGyzD
// License: public

// see also: http://www.iquilezles.org/www/articles/palettes/palettes.htm


#define pi  3.14159265359
#define tau 6.28318530718

#define R(a) mat2(cos(a), sin(a), -sin(a), cos(a))


// smoothfactor
#define sf 0.7


void main() {



	// vec2 uv = ( 2.0 * gl_FragCoord.xy - RENDERSIZE.xy )/min(RENDERSIZE.x,RENDERSIZE.y);
    // uv *= R( pi ); // -uv.y becourse it's fun
	
    // changed after comment from FabriceNeyret2
    vec2 uv = (RENDERSIZE.xy - 2.0 * gl_FragCoord.xy)/min(RENDERSIZE.x,RENDERSIZE.y);
    
    
    vec4 finalColor = vec4(0, 0, 0, 1);
    
    vec3 color = uv.y + 0.5 * cos(TIME - (tau * acos(uv.x)) + vec3(0.0, pi * 0.5, pi));
	
    finalColor.rgb = pow( sf - sqrt(color), vec3(1.0/2.2));
    gl_FragColor = finalColor; 
	
	
    // -------- FabriceNeyret2
    
	vec2 u = gl_FragCoord.xy;
    vec2 R = RENDERSIZE.xy ,             
         U = ( R - u-u ) / min(R.x,R.y);  
    finalColor = sf - sqrt( U.y + .5 * cos(TIME - 6.28 *( acos(U.x) + vec4(0, .25, .5, 0))));
	if(U.x > 0.001)
      gl_FragColor = finalColor; 
	
}
