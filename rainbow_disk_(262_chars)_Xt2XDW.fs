/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "rainbow",
    "short",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xt2XDW by FabriceNeyret2.  .",
  "INPUTS" : [

  ]
}
*/


void main() {


	vec4 fragCoordPos = gl_FragCoord;
	
    float C,S, t=(TIME-11.)/1e3;
    gl_FragColor-=gl_FragColor;
    vec2 R = RENDERSIZE.xy, p;
	fragCoordPos.xy = 6.3*(fragCoordPos.xy+fragCoordPos.xy-R)/R.y;
    
#define B(k) ceil( (p=cos(fragCoordPos.xy*=mat2(C=cos(t),S=-sin(t),-S,C))).x * p.y )  * (.5+.5*cos(k))
 
    for (float a=0.; a<6.3; a+=.1)
        gl_FragColor += vec4(B(a),B(a+2.1),B(a-2.1),1) / 31.;
}


/**  // expended version (283 chars)

float C,S, t=(TIME-11.)/1e3;
#define rot(a) mat2(C=cos(a),S=-sin(a),-S,C)

{
    o-=o;
    vec2 R = RENDERSIZE.xy, p;
	u = 6.3*(u+u-R)/R.y;
    
#define B(k) ceil( (p=cos(u*=rot(t))).x * p.y )  * (.5+.5*cos(k))
 
    for (float a=0.; a<6.3; a+=.1)
        o += vec4(B(a),B(a+2.1),B(a-2.1),1) / 31.;
}
/**/
