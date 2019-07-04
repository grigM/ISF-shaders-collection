/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#53456.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	vec4 color= vec4(0.0);
	
	float c = .0;
	c += smoothstep(0.71, 0.8, uv.y);
	c -= smoothstep(0.6, 0.69, uv.y);
	c *= sin(TIME * -1.0 * 25.0 + uv.x * 3.1415 * 2.0) - 0.5;
	
	c *= 0.5;
		
	color += mix(vec4(0.0), vec4(0.0, 1.0, 1.0, 1.0), c);
	
	gl_FragColor = color;
}