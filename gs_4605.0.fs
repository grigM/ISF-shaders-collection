/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#4605.0"
}
*/


// On a Clear Day You Can See Forever
// -gman
precision mediump float;


vec3 permute(vec3 x) { return mod(((x*34.0)+1.0)*x, 289.0); }

// Perlin simplex noise
float snoise(vec2 v) {
  const vec4 C = vec4(0.211324865405187, 0.366025403784439,
			-0.577350269189626, 0.024390243902439);
  vec2 i  = floor(v + dot(v, C.yy) );
  vec2 x0 = v -   i + dot(i, C.xx);
  vec2 i1;
  i1 = (x0.x > x0.y) ? vec2(1.0, 0.0) : vec2(0.0, 1.0);
  vec4 x12 = x0.xyxy + C.xxzz;
  x12.xy -= i1;
  i = mod(i, 289.0);
  vec3 p = permute( permute( i.y + vec3(0.0, i1.y, 1.0 ))
	+ i.x + vec3(0.0, i1.x, 1.0 ));
  vec3 m = max(0.5 - vec3(dot(x0,x0), dot(x12.xy,x12.xy),
    dot(x12.zw,x12.zw)), 0.0);
  m = m*m ;
  m = m*m ;
  vec3 x = 2.0 * fract(p * C.www) - 1.0;
  vec3 h = abs(x) - 0.5;
  vec3 ox = floor(x + 0.5);
  vec3 a0 = x - ox;
  m *= 1.79284291400159 - 0.85373472095314 * ( a0*a0 + h*h );
  vec3 g;
  g.x  = a0.x  * x0.x  + h.x  * x0.y;
  g.yz = a0.yz * x12.xz + h.yz * x12.yw;
  return 130.0 * dot(m, g);
}

void main( void ) {

	vec2 position = abs(( gl_FragCoord.xy / RENDERSIZE.xy ) * 2.0 - 1.0);
	float t = TIME * -0.5;
        vec2 m = log(position) + vec2(t, t);
	float size = 4.0;
	float b = max(m.x, m.y) * size;
	float n = floor(b) / (size * 0.5);
	vec3 c = vec3(snoise(vec2(n, 0.5)),
	              snoise(vec2(n, 0.0)),
	              snoise(vec2(n, 1.0))) * 0.5 + 0.5;


	float a = step(mod(b, 1.0), 0.80);
	c = c * a;
	gl_FragColor = vec4(c, 1);

}