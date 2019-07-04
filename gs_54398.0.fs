/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54398.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float rand(vec2 n) {
  return fract(cos(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}

float noise(vec2 n) {
  const vec2 d = vec2(0.0, 1.0);
  vec2 b = floor(n), f = smoothstep(vec2(0.0), vec2(1.0), fract(n));
  return mix(mix(rand(b), rand(b + d.yx), f.x), mix(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}

float fbm(vec2 n) {
  float total = 0.0, amplitude = 1.0;
  for (int i = 0; i < 4; i++) {
    total += noise(n) * amplitude;
    n += n;
    amplitude *= 0.6;
  }
  return total;
}

void main() {
  const vec3 c1 = vec3( 255.0/255.0,  255.0/255.0, 255.0/255.0);
  const vec3 c2 = vec3( 000.0/255.0,  000.0/255.0, 000.0/255.0);
  const vec3 c3 = vec3( 000.0/255.0,  000.0/255.0, 000.0/255.0);
  const vec3 c4 = vec3( 000.0/255.0,  000.0/255.0, 000.0/255.0);
  const vec3 c5 = vec3( 055.0/255.0,  055.0/255.0, 055.0/255.0);
  const vec3 c6 = vec3( 000.0/255.0,  000.0/255.0, 000.0/255.0);


  vec2 speed = vec2(0.1, 0.2);
  float shift = 0.0;

  vec2 p = gl_FragCoord.xy * 6.0 / RENDERSIZE.xx;
  float q = fbm(p - TIME * 0.1);
  vec2 r = vec2(fbm(p + q + TIME * speed.x - p.x - p.y), fbm(p + q - TIME * speed.y));
  vec3 c = mix(c1, c2, fbm(p + r)) + mix(c3, c4, r.x) - mix(c5, c6, r.y);
  gl_FragColor = vec4(c * cos(shift * gl_FragCoord.y / RENDERSIZE.y), 1.0);
  float grad = gl_FragCoord.y / RENDERSIZE.y;
  gl_FragColor.xyz *= 1.0-grad;
}