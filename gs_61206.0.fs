/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#61206.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/MsycWK
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE

// --------[ Original ShaderToy begins here ]---------- //
// #4b: https://shadertoy.com/view/MsGcWK
// #4: https://shadertoy.com/view/lsGcWK
// #3: https://shadertoy.com/view/MdGcWK
// shaking grid: https://www.shadertoy.com/view/MdyyWK
// #2: https://shadertoy.com/view/lsGyWK
// #1: https://shadertoy.com/view/MdGyWK

// http://www.thisiscolossal.com/2018/04/animation-of-sinusoidal-waves-in-gifs-by-etienne-jacob
// EJ's tutos: https://necessarydisorder.wordpress.com/

// --- pseudo perlin noise 3D

int MOD = 1;  // type of Perlin noise
#define rot(a) mat2(cos(a),-sin(a),sin(a),cos(a))

#define hash31(p) fract(sin(dot(p,vec3(127.1,311.7, 74.7)))*43758.5453123)
float noise3(vec3 p) {
    vec3 i = floor(p);
    vec3 f = fract(p); f = f*f*(3.-2.*f); // smoothstep

    float v= mix( mix( mix(hash31(i+vec3(0,0,0)),hash31(i+vec3(1,0,0)),f.x),
                       mix(hash31(i+vec3(0,1,0)),hash31(i+vec3(1,1,0)),f.x), f.y), 
                  mix( mix(hash31(i+vec3(0,0,1)),hash31(i+vec3(1,0,1)),f.x),
                       mix(hash31(i+vec3(0,1,1)),hash31(i+vec3(1,1,1)),f.x), f.y), f.z);
	return   MOD==0 ? v
	       : MOD==1 ? 2.*v-1.
           : MOD==2 ? abs(2.*v-1.)
                    : 1.-abs(2.*v-1.);
}

float fbm3(vec3 p) {
    float v = 0.,  a = .5;
    mat2 R = rot(.37);

    for (int i = 0; i < 1; i++) 
        p.xy *= R, p.yz *= R,
        v += a * noise3(p), p*=2., a/=2.;

    return v/.5;
}
// -------------------------------------


#define S(D,e)     smoothstep( 3., 0. ,  length(D)*R.y -e )
vec2 A,B,R; float l;
#define line(p,a,b) ( l = dot(B=b-a, A=p-a)/dot(B,B), clamp(l, 0., 1.) == l ? S( A - B * l ,0.) : 0.)
//float line(vec2 p, vec2 a, vec2 b) {      // draw a segment without round ends
//    b -= a; p -= a;
//    float l = dot(b,p)/dot(b,b);
//    return clamp(l, 0., 1.) == l ? S( p - b * l ,0.) : 0.;
//}

void mainImage( out vec4 O, vec2 U )
{
    R = iResolution.xy;
    U = ( U+U - R ) / R.y;
    O -= O;
    if ( abs(U.x)-(U.y+1.)/1.2 > 0. ) return;    // optim
    
    float t = 2.*iTime, T = 10., S=1., K = .2;
    
#define  P0(t) vec2(0,.5) + K* vec2( fbm3(vec3(i,0, t)), fbm3(vec3(i,2, t)) ) / S

    vec2 P1 = vec2(0,-.9), _P, P, X;
    for ( float i = 0.; i < 15.; i++ ) {
        X = vec2((i/7.5-1.),.3*sin(1e4*i));
        _P = P0(t)-X;
        for (float s=0.; s<1.03; s+=.04) {       // draw curve
            P = mix( P0(t-T*s)-X, P1, s ); 
            O += line(U,_P,P) *.03/ (.01+length(P-_P));
            _P = P;
       }
       O += S(P0(t)-X -U, 3.);
    }
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
    gl_FragColor.a = 1.0;
}