/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3ll3z2 by FabriceNeyret2.  Testing various modulations to convey illusion of motion.\ninspired from paper [url]http:\/\/www.cse.yorku.ca\/~kosta\/Motion_Without_Movement\/Motion_Without_Movement.html[\/url]",
  "INPUTS" : [

  ]
}
*/


void main() {



    vec2 R = RENDERSIZE.xy, 
         U = 2.* gl_FragCoord.xy / R.y,
         D = cos(.3*TIME-vec2(0,1.57));           // custom dir
    if (U.x>3.) { gl_FragColor-=gl_FragColor; return; }
    int i = int(U.x)+3*int(U.y);                   // pannel id
    U = 2.*fract(U)-1.;                            // pannel coords
    
    float t = 10.*TIME,
          kl = 50.,                                // wavelenght
          kr = .1,                                 // border width
          l = length(U), r = .5,
          m = smoothstep(-kr,kr,r-l),              // disk mask
          b = .5-.5*cos(6.28*m),                   // border zone (where effect occurs)
          x = dot(U,D),                            // pos along D
          d = x*x+r*r-l*l, s = -x+sign(x)*sqrt(d), // distance to disk in dir D
        phi =   i==0 ? kl*l                        // radial
              : i==1 ? kl*x                        // axial
              : i==2 ? d > 0. ? kl*s : t           // axial from disk
              : i==3 ? d > 0. && x < 0. ? kl*s : t // axial left to disk
              : i==4 ? -kl*l
              : i==5 ? d > 0. && x < 0. ? -kl*s : t
              : t;
    
    gl_FragColor = vec4( m + .25*b*sin(phi-t) );              // magic draw
}
