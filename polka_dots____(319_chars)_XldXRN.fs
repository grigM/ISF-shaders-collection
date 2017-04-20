/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "pattern",
    "short",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XldXRN by FabriceNeyret2.  ref: [url]https:\/\/www.facebook.com\/TheScienceScoop\/posts\/1343039352396001[\/url]\n\nWill you guess what is the trajectory of a given dot ? :-)",
  "INPUTS" : [

  ]
}
*/


void main() {

	vec4 fragCoordPos = gl_FragCoord;

	fragCoordPos.xy = ( fragCoordPos.xy+fragCoordPos.xy - (gl_FragColor.xy=RENDERSIZE.xy) ) /gl_FragColor.y * 8.;  
    float Pi = 3.14159265359,
           t = 16.*TIME,  // DATE.w cause streching bug on some machines (AMD+windows?)
           e = 35./gl_FragColor.y, v;
  //       a = Pi/3.*floor(t/2./Pi);
  //gl_FragCoord.xy *= mat2(cos(a),-sin(a), sin(a), cos(a));              
    fragCoordPos.xy *= mat2(sin(Pi/3.*ceil(t/2./Pi) + Pi*vec4(.5,1,0,.5)));      // animation ( switch dir )
    
  	fragCoordPos.y /= .866; 
    fragCoordPos.xy -= .5;   
    v = ceil(fragCoordPos.y);
    fragCoordPos.x += .5*v;                                                   // hexagonal tiling
  //gl_FragCoord.x += sin(t) > 0. ? (.5-.5*cos(t)) * (2.*mod(v,2.)-1.) : 0.;  
    fragCoordPos.x += sin(t) > 0. ? (1.-cos(t)) * (mod(v,2.)-.5) : 0.;        // animation ( scissor )
  //gl_FragCoord.x += (1.-cos(t/2.)) * (mod(v,2.)-.5);                        // variant
    
    fragCoordPos.xy = 2.*fract(fragCoordPos.xy)-1.;                                            // dots
    fragCoordPos.y *= .866;
	gl_FragColor += smoothstep(e,-e, length(fragCoordPos.xy)-.6) -gl_FragColor;
}
