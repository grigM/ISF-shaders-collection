/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#45657.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/Mdc3zH
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy globals
float iTime;
vec3  iResolution;

// Protect glslsandbox uniform names
#define TIME        stemu_time
#define RENDERSIZE  stemu_resolution

// --------[ Original ShaderToy begins here ]---------- //
// a variant of https://www.shadertoy.com/view/Xs33RH


// reduction: 307
                                                 // draw segment [a,b]
#define D(m)  3e-3/length( m.x*v - u+a )
#define L   ; m.x= dot(u-a,v=b-a)/dot(v,v); o.z += D(m); o += D(clamp(m,0.,1.));
#define P     b=c= vec2(r.x,1)/(4.+r.y) L   b=a*I L   a=c*I L   a=c; r= I*r.yx;

void mainImage(out vec4 o, vec2 U)
{   vec2 v,m, I=vec2(1,-1), a,b,c=iResolution.xy, 
        u = (U+U-c)/c.y,
        r = log(iTime+.8*I); r += I*r.yx;
    P  o-=o;       // just to initialize a
	P P P P        // 4*3 segments

}





/* // 388
                                                  // draw segment [a,b]
// #define L(a,b)  o+=3e-3/length( clamp( dot(u-a,v=b-a)/dot(v,v), 0.,1. ) *v - u+a );
#define D(m,a)  3e-3/length( m*v - u+a )
#define L(a,b)  m=dot(u-a,v=b-a)/dot(v,v); o.z += D(m,a); o += D(clamp(m,0.,1.),a);

#define Z(x,y)  vec2( x,1 ) / ( 4.+y )           // perspective transform
#define P(A,B)  L(A,B)  L(A*I,B*I)  L(A,A*I)     // draw 1 top segment + 1 bottom + 1 vertical

void mainImage( out vec4 o, in vec2 u )
{
    o-=o;
    u = 2.*u/iResolution.y - vec2(1.8,1); 
    
    float m=iTime, l=sin(m); m=cos(m)-l; l+=m+l;
    vec2  v, I=vec2(1,-1), 
          a=Z(l,m), b=Z(m,-l), c=Z(-l,-m), d=Z(-m,l); // 4 top vertices screen coords

          P(a,b) P(b,c) P(c,d) P(d,a)                 // draw 4*3 segments
}
/**/
// --------[ Original ShaderToy ends here ]---------- //

#undef TIME
#undef RENDERSIZE

void main(void)
{
  iTime = TIME;
  iResolution = vec3(RENDERSIZE, 0.0);

  mainImage(gl_FragColor, gl_FragCoord.xy);
  gl_FragColor.a = 1.0;
}