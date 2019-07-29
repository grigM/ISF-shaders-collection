/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#56026.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/ldBXz3
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime length(vv_FragNormCoord)
//dot(vv_FragNormCoord,vv_FragNormCoord)
#define iResolution RENDERSIZE

// --------[ Original ShaderToy begins here ]---------- //
#define ttime iTime*0.5

float grid(vec2 p) {
  vec2 orient = normalize(vec2(1.0,3.0));
  vec2 perp = vec2(orient.y, -orient.x);
  float g = mod(floor(1. * dot(p, orient)) + floor(1. * dot(p, perp)), 2.);
  return g;
}

#define samp 3.
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
  vec2 p = fragCoord.xy / 50. + vec2(-ttime, ttime);
  vec2 q = (fragCoord.xy - (iResolution.xy / 2.)) / iResolution.x / 1.5 ;
  vec4 c = vec4(grid(p));
  if (q.x + 0.1 * q.y > 100.) {
    fragColor = c;
  }
  else {
    vec4 cc = vec4(0.0);
    float total = 0.0;
    
    float radius = length(q) * 100.;
    for (float t = -samp; t <= samp; t++) {
      float percent = t / samp;
      float weight = 1.0 - abs(percent);
	  float u = t / 100.;
      vec2 dir = vec2(fract(sin(537.3 * (u + 0.5)) ) , fract(sin(523.7 * (u + 0.25)) ));
      dir = normalize(dir) * 0.01;
      float skew = percent * radius;
      vec4 samplev = vec4(
          grid(vec2(0.03,0.) + p +  dir * skew),
          grid(radius * vec2(0.005,0.00) + p +  dir * skew),
          grid(radius * vec2(0.007,0.00) + p +  dir * skew),
          1.0);
      cc += samplev * weight;
      total += weight;
    }


    fragColor = cc / total - length(q ) * vec4(1.,1.,1.,1.) * 1.5;
	  
	  fragColor = fract(fragColor+TIME);
  }
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
    gl_FragColor.a = 1.0;
}