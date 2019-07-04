/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "2tweets",
    "short",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tlfRB by FabriceNeyret2.  .",
  "INPUTS" : [

  ]
}
*/


#define r(a) mat2(cos(a),-sin(a),sin(a),cos(a))

void main() {



    vec2 R = RENDERSIZE.xy,
	     U = r(TIME) * ( gl_FragCoord.xy+gl_FragCoord.xy - R ) / R.y,
         S = sign(U) * r(.1);  // wave train direction ( 4 sectors, tilted by .1 or .28 ). 
    gl_FragColor = vec4( sin( 8.* log( dot( U, S ) ) + atan(S.y,S.x) -10.*TIME) );
	gl_FragColor = smoothstep( .7, .5, abs(gl_FragColor) );                                       // bands
  //gl_FragColor = smoothstep( 1.,0.,(abs(gl_FragColor)-.6)/fwidth(gl_FragColor) );                          // better antialiasing
  //gl_FragColor = sqrt(smoothstep( 0., 50./R.y/(abs(U.x) + abs(U.y)), .7 - abs(gl_FragColor) )); // Shane variant
}
