/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "voronoi",
    "spiral",
    "codevember",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MtcXD7 by xorxor.  After Sol Lewitt's Complex Form with Black and White Bands.\n\nfor Codevember 2016, day 22.",
  "INPUTS" : [
  
    
    {
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 6.2831,
			"MIN": 0.0,
			"MAX": 20.0
	}
  ]
}
*/


// Complex Form with Black and White Spirals
// Created by XORXOR, 2016
// Attribution-NonCommercial-ShareAlike 4.0 International (CC BY-NC-SA 4.0)
//
// Thanks to iq for voronoi
// https://www.shadertoy.com/view/ldl3W8

#define ANIMATE

vec2 hash( vec2 p )
{
    return fract( sin( vec2( p.x * p.y, p.x + p.y ) ) * vec2( 234342.1459123, 373445.3490423 ) );
}

// iq's voronoi
// https://www.shadertoy.com/view/ldl3W8
vec4 voronoi( in vec2 x )
{
    vec2 n = floor( x );
    vec2 f = fract( x );

    //----------------------------------
    // first pass: regular voronoi
    //----------------------------------
    vec2 mg, mr, mo;

    float md = 8.0;
    for( int j=-1; j<=1; j++ )
    for( int i=-1; i<=1; i++ )
    {
        vec2 g = vec2(float(i),float(j));
        vec2 o = hash( n + g );
        #ifdef ANIMATE
        o = 0.5 + 0.3*sin( TIME + speed*o );
        #endif
        vec2 r = g + o - f;
        float d = dot(r,r);

        if( d<md )
        {
            md = d;
            mr = r;
            mg = g;
            mo = o;
        }
    }

    //----------------------------------
    // second pass: distance to borders
    //----------------------------------
    md = 8.0;
    for( int j=-2; j<=2; j++ )
    for( int i=-2; i<=2; i++ )
    {
        vec2 g = mg + vec2(float(i),float(j));
        vec2 o = hash( n + g );
        #ifdef ANIMATE
        o = 0.5 + 0.3*sin( TIME + speed*o );
        #endif
        vec2 r = g + o - f;

        if( dot(mr-r,mr-r)>0.00001 )
        md = min( md, dot( 0.5*(mr+r), normalize(r-mr) ) );
    }

    return vec4( md, mr, mo.x + mo.y );
}

void main() {



    vec2 b = 6.0 * gl_FragCoord.xy / RENDERSIZE.x;
    vec4 v = voronoi( b );
    vec2 q = v.yz;
    float a = TIME + atan( sign( v.w - 1.0 ) * q.y, q.x );
    float l = length( q * 5.0 / ( sqrt( v.x ) ) ) + 0.319 * a;
    float m = mod( l, 2.0 );
#if 1 // sharpening by s23b
    float w = min( fwidth( mod( l + 1.5, 2.0 ) ), fwidth( mod( l + 0.5, 2.0 ) ) ) / 2.0;
	float o = ( 1.0 - smoothstep( 1.85 - w, 1.85 + w, m ) ) * smoothstep( 1.15 - w, 1.15 + w, m );
#else
    float o = ( 1.0 - smoothstep( 1.7, 2.0, m ) ) * smoothstep( 1.0, 1.3, m );
#endif
    o = mix( 0.0, o, smoothstep( 0.04, 0.07, v.x ) );
    gl_FragColor = vec4( vec3( o ), 1.0 );
}
