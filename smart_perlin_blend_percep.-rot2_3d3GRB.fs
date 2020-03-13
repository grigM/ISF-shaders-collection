/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "0c7bf5fe9462d5bffbd11126e82908e39be3ce56220d900f633d58fb432e56f5.png"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3d3GRB by FabriceNeyret2.  SPACE: switch   mode 0: radial gradient between still & rotating texture.   mode 1: 50-50.\nMouse.y : sharpness of transition.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


// variant of https://shadertoy.com/view/Wsc3zS
// PS: this is just a sketch of test: noise is too low quality as it is.

#define L  10.
#define rot(a) mat2(cos(a),sin(a),-sin(a),cos(a)) // rot
#define T(u) ( 2.* IMG_NORM_PIXEL(iChannel0,mod((u)/1e3,1.0)).r -1. )
#define B(T) ( 1. - abs( T ) )
#define R (RENDERSIZE.xy)
#define keyToggle(a)  ( texelFetch(iChannel3,ivec2(a,2),0).x > 0.)
#define hue(v)  ( .6 + .6 * cos( 6.3*(v)  + vec4(0,23,21,0)  ) )

float N(vec2 u0, vec2 u1) { // smart blend of 2 perlin noise 
	mat2 M = rot(1.7);                        // to decorelate layers
    float v = 0., S = 0., s=1.,
          a = keyToggle(32) ? .5 : clamp(length(u0)/.7,0.,1.);
    
    for (float k=0.; k<L; k++) {              // loop on harmonics
        vec2  x0 = 10.*M*u0/s,
              x1 = 10.*M*u1/s;
        float t0 = T(x0), t1 = T(x1),
              t = mix(t0,t1,a)/sqrt(a*a + (1.-a)*(1.-a)), // morph: blend random bases
              b0 = B(t0) -.5,
              b1 = B(t1) -.5,
              b = //u0.y > 0. ?
                        B(t) - .5                               // morphing
                 ;//  : mix(b0,b1,a)/sqrt(a*a + (1.-a)*(1.-a)); // blend+normalize
	    v += (.5 + b ) *s; 
        S += s;
		M *= M; s/=2.; 
	}
    return v/S;
}

void main() {

    //if (floor(gl_FragCoord.y)==floor(R.y/2.)) { gl_FragColor=vec4(1,0,0,0); return; }
	gl_FragCoord.xy = (gl_FragCoord.xy-.5*R)/R.y;
    float T = .5*TIME;
    float e = iMouse.y > 0. ? .5*iMouse.y/R.y: .2,                  // tunes transition
          v = N(gl_FragCoord.xy, gl_FragCoord.xy*rot(T)+10.);
    gl_FragColor = hue(.7*smoothstep(.7-e,.7+e,v));
  //gl_FragColor = vec4( pow(smoothstep(.7-e,.7+e, v ),1./2.2) );  // antialiasing + to SRGB
}                                               // .7 because of interp ( otherwise .5 )
