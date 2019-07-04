/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#46050.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {
	vec2 p = (2.0 * gl_FragCoord.xy - RENDERSIZE.xy) / min(RENDERSIZE.x, RENDERSIZE.y);
	for(int i = 1; i < 7; i++) {
		p += sin(p.yx * 3.14 + sin(TIME * float(i) / 10.0) * 12.0);
	}
	float c = pow(max(0.0, p.x / 2.0 - 0.7), 2.0);
	gl_FragColor = vec4(vec3(c), 1.0);
}