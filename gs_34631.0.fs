/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 2.3,
			"MIN": 0.0,
			"MAX": 6.0
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34631.0"
}
*/


// Warped Hex
// By: Brandon Fogerty
// xdpixel.com

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float hex( vec2 p )
{		
	p.y += mod( floor(p.x), 4.) * 0.5;
	p = abs( fract(p)- 0.5 );
	return  abs(sin(max(sin(p.x*10.5 + p.y*5.+TIME*speed), sin(p.y * 5.0+TIME*speed)))+0.5); //abs( max(p.x*1.5 + p.y, p.y * 2.0) - 1.0 ) ;
}

void main( void ) {

	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy ) * 2.0 - 1.0;
	uv.x *= RENDERSIZE.x / RENDERSIZE.y;	
	gl_FragColor = vec4( vec3(1.-hex(uv*1.)*5.+2.), 1.0 );// *(sin(uv.x+TIME)+1.))

}