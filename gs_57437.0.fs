/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57437.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	vec2 position = gl_FragCoord.xy;

	gl_FragColor = vec4(0.0, 0.0, 0.0, 1.0);
	
	float x = floor(position.x / 5.);
	if (mod(position.x, 5.) < 2.) {
		return;
	}

	float y = floor(position.y / 5. + sin(-TIME*2. + x*0.9)*10.);
	
	float dist = (floor(RENDERSIZE.y/10.) - y);
	
	if (cos(-TIME*9. + x*-1.5) < 0.) {
		dist = -dist;
	}
	
	float c = 1./(1.+dist);
	c /= (1.+pow(sin(-TIME*2. + x*-1.5), 2.));
	gl_FragColor = vec4(c-0.07, c-0.05, c, 1.0);

}