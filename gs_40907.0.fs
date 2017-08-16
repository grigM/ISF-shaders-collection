/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40907.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



float rand(vec2 co)
{
	return fract(sin(dot(co.xy,vec2(1.,77))) * 4.5453);
}

float pattern (vec2 pos, float t)
{
	return step(rand(pos.xx + vec2(t / 100.)), pos.y);
}

void main( void ) {
	vec2 p = gl_FragCoord.xy / min(RENDERSIZE.x, RENDERSIZE.y);
	float t = TIME + p.x / -1.;
	float v = mix(pattern(p, floor(t)), pattern(p, ceil(t)), fract(t));
	gl_FragColor = vec4(mix(vec3(.0, 0., 0.), vec3(1., 1., 1.), p.y) * (1.-v), 111.0 );

}