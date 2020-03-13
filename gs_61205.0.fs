/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#61205.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/MsGyWK
 */

#extension GL_OES_standard_derivatives : enable

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE

// --------[ Original ShaderToy begins here ]---------- //
// Inspired by another tuto by Etienne Jacob https://necessarydisorder.wordpress.com/2017/11/15/drawing-from-noise-and-then-making-animated-loopy-gifs-from-there/
// closer (expensive) version here: https://www.shadertoy.com/view/MdyyWK

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

    for (int i = 0; i < 2; i++) 
        p.xy *= R, p.yz *= R,
        v += a * noise3(p), p*=2., a/=2.;

    return v;
}
// -------------------------------------

void mainImage( out vec4 O, vec2 U )
{
    vec2 R = iResolution.xy;
    U = ( U+U - R ) / R.y;
    O -= O;
    
    float t = 2.*iTime;
    U += .5*max(1.-.5*length(U),0.)
         * vec2( fbm3(vec3(U, t)), fbm3(vec3(U+5., t)) );
    U = sin(60.*U); U = smoothstep(1.5,0.,abs(U)/fwidth(U));
    O += U.x+U.y;  // * if you want points
    
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}