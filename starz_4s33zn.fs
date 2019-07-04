/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "starfield",
    "2tweets",
    "short",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4s33zn by rcread.  moving starfield",
  "INPUTS" : [

  ]
}
*/


// Created by randy read - rcread/2015
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

void main() {

	vec4 fragCoordPos = gl_FragCoord;

	float f		=	.5,
    r			=	RENDERSIZE.x,
    t			=	DATE.w * .1,
    c			=	cos( t ),
    s			=	sin( t );
	fragCoordPos.xy			=	( fragCoordPos.xy / r - f ) * mat2( c, s, -s, c );
	vec3 d		=	floor( f * r * vec3( fragCoordPos.xy, f ) );
	gl_FragColor 			=	vec4( 1. / ( f + exp( 2e2 * fract( sin( dot( d * sin( d.x ) + f, vec3( 13, 78.2, 57.1 ) ) ) * 1e5 ) ) ) );
}
