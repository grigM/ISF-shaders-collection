/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "stars",
    "flare",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ltj3DK by rcread.  modified bsdbeard\/nimitz shader to be all procedural and added my own starfield",
  "INPUTS" : [

  ]
}
*/


// Created by randy read - rcread/2015
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

//	modified to be all procedural and added my own starfield
//		bsdbeard -- https://www.shadertoy.com/view/4d2XR1
//		nimitz -- https://www.shadertoy.com/view/lsSGzy

//	hash22() from Dave_Hoskins -- https://www.shadertoy.com/view/ltfGDs
vec2 hash22( vec2 p ) {
	p		= fract( p * vec2( 5.3983, 5.4427 ) );
    p		+= dot( p.yx, p.xy + vec2( 21.5351, 14.3137 ) );
	return fract( vec2( p.x * p.y * 95.4337, p.x * p.y * 97.597 ) );
}
float sum( vec2 p ) { return p.x + p.y; }
float stars( vec2 p, float density, float brightness ) {
	return 1. / ( 1. + brightness * exp( pow( 2., density ) * sum( hash22( p ) ) ) );
}
float noise( vec2 p ) {
	return .5 + ( sin( p.x ) + sin( p.y  ) ) / 4.;
}
mat2 m2 = mat2( 0.80,  0.60, -0.60,  0.80 ) * 2.;
float fbm( in vec2 p ) {
	float z		=2.;
	float rz	= -0.05;
	p			*= 0.25;
	for (int i= 0 ; i < 4 ; i++) {
		rz		+= abs( ( noise( p * vec2( .741, .0217 ) ) - 0.5 ) * 2. ) / z;
		z		= z * 2.;
		p		= p * 2. * m2;
	}
	return rz;
}
void main(){
	float t		= -TIME * .2; 
	vec2 p		= gl_FragCoord.xy;
	vec2 uv		= p / RENDERSIZE.xy - 0.5;
	uv.x		*= RENDERSIZE.x / RENDERSIZE.y;
	uv			*= 15. * 0.1;
	
	float r		= length( uv );
	float x		= dot( normalize( uv ), vec2( .5, 0. ) ) + t;
	float y		= dot( normalize( uv ), vec2( 0., .5 ) ) + t;
 
    float val;
    val			= fbm( vec2( r + y * 4.5, r + x * 4.5 ) );
	val			= smoothstep( 5. * .02 - .1, 10. + ( 5. * 0.02 - .1 ) + .001, val );
	val			= sqrt( val );
	
	vec3 col	= val / vec3( 4., 1., .3 );
	col			= 1. - col;
	col			= mix( col, vec3( 1. ), 5. - 266.667 * r );
	float a		= col.x;
    a			= clamp( a, 0., 1. );	//	comment out for smoky effect
	col			= clamp( col, 0., 1. );

	//gl_FragColor.rgb = mix( vec3( stars( p, 5., .25 ) ), col, a );
    gl_FragColor.rgb = mix( vec3( clamp( pow( 1.3 * stars( p, 5., .25 ), 3. ), 0., 1. ) ), col, a );
}