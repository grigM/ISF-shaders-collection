/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XsjyRW by FabriceNeyret2.  some worms are drawing mazes under bark on in the silt above marine stones.\nThese correspond to very simple exploration rules. Here is an artificial example.\nChange parameters, try fullscreen.",
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
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


#define CS(a)  vec2(cos(a),sin(a))
#define rnd(x) ( 2.* fract(456.68*sin(1e4*x+mod(DATE.w,100.))) -1.) // NB: mod(t,1.) for less packed pattern
#define T(U) textureLod(BufferA, (U)/R, 0.)
const float r = 1.5, N = 100., da = .5; // width , number of worms , turn angle at hit

void main() {
	if (PASSINDEX == 0)	{


	    vec2 R = RENDERSIZE.xy;
	    
	    if (T(R).x==0.) { gl_FragCoord.xy = abs(gl_FragCoord.xy/R*2.-1.); gl_FragColor  = vec4(max(gl_FragCoord.x,gl_FragCoord.y)>1.-r/R.y); gl_FragColor.w=0.; return; }
	
	    if (gl_FragCoord.y==.5 && T(gl_FragCoord.xy).w==0.) {                           // initialize heads state: P, a, t
	        gl_FragColor = vec4( R/2. + R/2.4* vec2(rnd(gl_FragCoord.x),rnd(gl_FragCoord.x+.1)) , 3.14 * rnd(gl_FragCoord.x+.2), 1);
	        if (T(gl_FragColor.xy).x>0.) gl_FragColor.w = 0.;                        // invalid start position
	        return;
	    }
	    
	    gl_FragColor = T(gl_FragCoord.xy);
	//  vec2 M = iMouse.xy; if (length(M)>0.) gl_FragColor += smoothstep(r,0., length(M-gl_FragCoord.xy));
	    
	    for (float x=.5; x<N; x++) {                           // draw heads
	        vec4 P = T(vec2(x,.5));                            // head state: P, a, t
	        if (P.w>0.) gl_FragColor += smoothstep(r,0., length(P.xy-gl_FragCoord.xy))  // draw head if active
	                         *vec4(1.-.01*P.w,1,0,1);          // coloring scheme
	                      // *(.5+.5*sin(6.3*x/N+vec4(0,-2.1,2.1,1)));   // coloring scheme
	    }
	    
	    if (gl_FragCoord.y==.5) {                                         // head programms: worm strategy
	        vec4 P = T(gl_FragCoord.xy);                                     // head state: P, a, t
	        if (P.w>0.) {                                      // if active
	            float a = P.z;
	            while ( T(P.xy+(r+2.)*CS(a)).w > 0. && a < 13. )  a += da; // hit: turn
	            if (a>=13.) { gl_FragColor.w = 0.; return; }              // stop head
	            a += .004;
	            gl_FragColor = vec4(P.xy+CS(a),mod(a,6.2832),P.w+1.);     // move head
	        }
	    }
	}
	else if (PASSINDEX == 1)	{


	    gl_FragColor = IMG_NORM_PIXEL(BufferA,mod(gl_FragCoord.xy/RENDERSIZE.xy,1.0));
	}
}
