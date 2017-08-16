/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#41346.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {
	
	vec2 sP = (gl_FragCoord.xy / RENDERSIZE.xy) - .5;
	
		
	float sPdp = dot(sP,sP);
	sP *= mix( (0.75-sPdp), sPdp, sin(TIME+sPdp*sPdp) );
		
	vec2 z = vec2(length(sP)*3.5, atan(sP.x, sP.y));
	z.y += TIME*1.5+z.x*14.0;
	z.x *= 2.-sin(z.y)*0.1;
	z = abs(z.x*vec2(cos(z.y), cos(z.y)));
	gl_FragColor = vec4(2.0-max(z.x, z.y)*5.);
	
}