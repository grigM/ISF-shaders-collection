/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#55898.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main( void ) {
	vec2 pos = gl_FragCoord.xy / RENDERSIZE.x;
	
	vec4 c = vec4(vec3(0.),1.);
	if(distance(vec2(0.5),mod(pos*20.,1.))<abs(0.5-fract(pos.x+TIME/4.))*sqrt(2.)) {
		c = vec4(vec3(1.),1.);
	}
	gl_FragColor = c;
}