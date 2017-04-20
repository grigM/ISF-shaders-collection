/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "08b42b43ae9d3c0605da11d0eac86618ea888e62cdd9518ee8b9097488b31560.png"
    }
  ],
  "CATEGORIES" : [
    "font",
    "morphing",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ltcXzs by FabriceNeyret2.  trying the new font texture https:\/\/www.shadertoy.com\/view\/llcXRl",
  "INPUTS" : [

  ]
}
*/


 // --- access to the image of ascii code c
vec4 char(vec2 p, float c) {
    if (p.x<0.|| p.x>1. || p.y<0.|| p.y>1.) return vec4(0,0,0,1e5);
   
	return IMG_NORM_PIXEL( iChannel0, p/16. + fract( floor(vec2(c, 15.999-c/16.)) / 16. ) );
    // possible variants: (but better separated in an upper function) 
    //     - inout pos and include pos.x -= .5 + linefeed mechanism
    //     - flag for bold and italic 
}

void main() {

	vec4 fragCoordPos = gl_FragCoord;

    fragCoordPos.xy /= RENDERSIZE.y;
    float t = 3.*TIME;
    gl_FragColor = char(fragCoordPos.xy,t);     // try .xxxx for mask, .wwww for distance field.
 // return;            // uncomment to just see the letter count.
    
    vec4 O2 = char(fragCoordPos.xy,++t);
    gl_FragColor = mix(gl_FragColor,O2,fract(t));             // linear morphing 
 // gl_FragColor = sqrt(mix(gl_FragColor*gl_FragColor,O2*O2,fract(t)));  // quadratic morphing
    
    
    gl_FragColor =  smoothstep(.5,.49,gl_FragColor.wwww)
       * gl_FragColor.yzww;                        // comment for B&W
  // text
  
  //fragCoordPos.xy *= 8.; fragCoordPos.x -=9.;
  //gl_FragColor += char(fragCoordPos.xy,64.+13.    ).x; fragCoordPos.x-=.5;
  //gl_FragColor += char(fragCoordPos.xy,64.+15.+32.).x; fragCoordPos.x-=.5;
  //gl_FragColor += char(fragCoordPos.xy,64.+18.+32.).x; fragCoordPos.x-=.5;
  //gl_FragColor += char(fragCoordPos.xy,64.+16.+32.).x; fragCoordPos.x-=.5;
  //gl_FragColor += char(fragCoordPos.xy,64.+ 8.+32.).x; fragCoordPos.x-=.5;
  //gl_FragColor += char(fragCoordPos.xy,64.+ 9.+32.).x; fragCoordPos.x-=.5;
  //gl_FragColor += char(fragCoordPos.xy,64.+14.+32.).x; fragCoordPos.x-=.5;
  //gl_FragColor += char(fragCoordPos.xy,64.+ 7.+32.).x; fragCoordPos.x-=.5;
}
