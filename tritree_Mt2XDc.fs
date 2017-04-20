/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "short",
    "tritree",
    "quadtri",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Mt2XDc by FabriceNeyret2.  the tri-tree variant of quad-tree https:\/\/www.shadertoy.com\/view\/lljSDy\nindeed, this is another kind of 4trees ;-)\n(NB: subdivision if the object intersect the node bounding sphere).",
  "INPUTS" : [

  ]
}
*/


void main() {



    gl_FragColor = vec4(0.0);
    float r=.2, z=4., t=TIME, H = RENDERSIZE.y, uz;
    
    vec4 fragCoordPos = gl_FragCoord;
     
    fragCoordPos.xy /=  H;                              // object : disc(P,r)
    vec2 P = .5+.5*vec2(cos(t),sin(t*.7)), C=vec2(-.7,0), fU;  
    fragCoordPos.xy =(fragCoordPos.xy-C)/z; P=(P-C)/z; r/= z;         // unzoom for the whole domain falls within [0,1]^n
    
    mat2 M = mat2(1,0,.5,.87), IM = mat2(1,0,-.577,1.155);
    fragCoordPos.xy = IM*fragCoordPos.xy;         // goto triangular coordinates (there, regular orthonormal grid + diag )
    
    gl_FragColor.b = .25;                            // backgroud = cold blue
    for (int i=0; i<7; i++) {             // to the infinity, and beyond ! :-)
        fU = min(fragCoordPos.xy,1.-fragCoordPos.xy); uz = 1.-fragCoordPos.x-fragCoordPos.y;
        if (min(min(fU.x,fU.y),abs(uz)) < z*r/H) { gl_FragColor--; break; } // cell border
    	if (length(P-M*vec2(.5-sign(uz)/6.)) - r > .6) break;    // cell is out of the shape
                // --- iterate to child cell
        fU = step(.5,fragCoordPos.xy);                  // select grid-child
        fragCoordPos.xy = 2.*fragCoordPos.xy - fU;                    // go to new local frame
        P = 2.*P - M*fU;  r *= 2.;
        
        gl_FragColor += .13;                         // getting closer, getting hotter
    }
               
	gl_FragColor.gb *= smoothstep(.9,1.,length(P-M*fragCoordPos.xy)/r); // draw object
}
