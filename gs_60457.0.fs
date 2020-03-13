/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60457.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


mat2 rotate(float a) {
	float c = cos(a), s = sin(a);
	return mat2(c, -s, s, c);
}

void main() {
	vec2 uv = (2. * gl_FragCoord.xy - RENDERSIZE) / RENDERSIZE.y;
	vec3 color = vec3(0.9);
	uv.x *= dot(uv,uv)*2.2;
	uv *= rotate(uv.y+TIME * .1);
	uv = abs(uv);
	vec2 ar = vec2(sin(atan(uv.x, uv.y)), length(uv));
	color = mix(color, vec3(.4, 2., .7), sin(8. * (ar.y * 1. - TIME) + TIME) + (8. * ar.x - 8. * ar.y));
	color = mix(color, vec3(.7, .2, 5.), cos(8. ) * (5. * ar.y - 8. * ar.x));	
	color /= mix(color, vec3(.4, 4., .7), sin(8. * (ar.y * 1. - TIME) + TIME) + (8. * ar.x - 8. * ar.y));
	gl_FragColor = vec4(color, 1.);
}