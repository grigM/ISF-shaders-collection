/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59492.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/3lc3WN
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
const float PI = 3.14159;

float rand(float n){return fract(sin(n) * 43758.5453123);}

vec2 force(vec2 p, vec2 a) {
  return normalize(p - a) / distance(p, a);
}

vec2 calcVelocity(vec2 p) {
  vec2 v = vec2(0);
  vec2 a;
  float o, r, s, m;
  const float limit = 100.;
  for (float i = 0.; i < limit; i++) {
    r = rand(i/limit)-.5;
    m = rand(i+1.)-.5;
    m *= (iTime+(23.78*1000.))*2.;
    o = i + r + m;
    a = vec2(
      sin(o / limit * PI * 2.),
      cos(o / limit * PI * 2.)
    );
    s = sign(mod(i, 2.)-.5);
    v -= force(p, a) * s;
  }  
  v = normalize(v);
  return v;
}

float calcDerivitive(vec2 v, vec2 p) {
    vec2 o = vec2(0,3.) / iResolution.x;
	return (
        length(v - calcVelocity(p + o.xy)) +
        length(v - calcVelocity(p + o.yx))
    ) / 2.;
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    // Normalized pixel coordinates (from 0 to 1)
    vec2 p = (-iResolution.xy + 2.0*fragCoord.xy)/iResolution.x;
	p *= 3.;
    vec2 v = calcVelocity(p);
    float a = atan(v.x, v.y);
    a = abs(a/PI/2.);
    float spacing = 1./2.;
    float lines = mod(a, spacing) / spacing;
    lines = min(lines * 2., 1.) - max(lines * 2. - 1., 0.);

    lines /= calcDerivitive(v, p) / spacing;
    lines -= iResolution.x * .0001;

    float b = length(p) - 1.;
    lines = mix(1. - lines*.75, lines, clamp(b/fwidth(b), 0., 1.));

    fragColor = 1.-vec4(lines);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}