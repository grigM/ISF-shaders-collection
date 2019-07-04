/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#1389.0"
}
*/


// @ahnqqq
// unko now

# ifdef GL_ES
precision mediump float;
# endif


float b( vec2 p, float s )
{
	return ( length( p ) < s ) ? 1. : 0.;
}

float f( vec2 p )
{
	float t = TIME * 5.0;
	float c = 0.0;
	vec2 q;

	q = p + vec2( +.9 + sin( t * .9 ), -.5 + cos( t * .2 ) ) * .8;
	c += b( q, .4 );

	q = p + vec2( +.7 + sin( t * .6 ), -.4 + cos( t * .7 ) ) * .9;
	c += b( q, .4 );
	
	q = p + vec2( +.6 + sin( t * .3 ), -.2 + cos( t * .6 ) ) * .3;
	c += b( q, .3 );

	q = p + vec2( -.2 + sin( t * .7 ), +.3 + cos( t * .1 ) ) * .7;
	c += b( q, .3 );

	q = p + vec2( -.7 + sin( t * .2 ), +.8 + cos( t * .9 ) ) * .5;
	c += b( q, .2 );

	q = p + vec2( -.1 + sin( t * .5 ), +.7 + cos( t * .3 ) ) * .2;
	c += b( q, .2 );
	
	return c;
}

# define TS 0.02

void main()
{
	float r = RENDERSIZE.x / RENDERSIZE.y;
	vec2 t, q, p = ( gl_FragCoord.xy / RENDERSIZE ) - .5, m = mouse - .5;
	vec3 c;
	p.x *= r,
	m.x *= r;
	
	q = p - mod( p + vec2( .024, .054 ), TS ) + TS*.5;
	c.r = f( m - q );
	
	q = p - mod( p + vec2( .033, .073 ), TS ) + TS*.5;
	c.g = f( m - q );

	q = p - mod( p + vec2( .035, .075 ), TS ) + TS*.5;
	c.b = f( m - q );
	
	gl_FragColor = vec4( c, 1.0 );
}
