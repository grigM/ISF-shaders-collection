/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
  		
		{
			"NAME": "posX",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "speedx",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 6.0
		},
		{
			"NAME": "speedy",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 6.0
		},
		{
			"NAME": "param1",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "param2",
			"TYPE": "float",
			"DEFAULT": 0.4,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "param3",
			"TYPE": "float",
			"DEFAULT": 50.0,
			"MIN": 1.0,
			"MAX": 50.0
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34654.3"
}
*/


// Warped Hex
// By: Brandon Fogerty
// xdpixel.com

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float c( vec2 p ) {		
	//p.y += mod( floor(p.x), 4.0) * 0.5;
	//p = abs( fract(p)- 0.5 );
	//return 1.-abs( max(p.x*1.5 + p.y, p.y * 2.0) - 1.0 )*10. ;
	return (length(fract(p)-param1)-param2)*param3;
}

void main( void ) {
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy ) * 2.0 - posX;
	uv.x *= RENDERSIZE.x / RENDERSIZE.y;	
	uv/=dot(uv,uv);
	uv.x+=TIME*speedx/1.5;uv.y+=TIME*speedy;
	gl_FragColor = vec4( vec3(c(uv)), 1.0 );
}