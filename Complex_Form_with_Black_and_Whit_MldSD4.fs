/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "voronoi",
    "codevember",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MldSD4 by xorxor.  After Sol Lewitt\n\nfor Codevember 2016, day 21",
  "INPUTS" : [
	{
			"NAME": "scale",
			"TYPE": "float",
			"DEFAULT": 6.3,
			"MIN": 0.0,
			"MAX": 12.0
	},
	{
			"NAME": "lineRotation",
			"TYPE": "float",
			"DEFAULT": 5.28,
			"MIN": 0.0,
			"MAX": 12.0
	}
  ]
}
*/


// Created by XORXOR, 2016
// Attribution-NonCommercial-ShareAlike 4.0 International (CC BY-NC-SA 4.0)
// https://www.shadertoy.com/view/MldSD4
//
// Thanks to iq for voronoi
// https://www.shadertoy.com/view/ldl3W8

vec2 hash( vec2 p )
{
	return fract( sin( vec2( p.x * p.y, p.x + p.y ) ) * vec2( 234342.1459123, 373445.3490423 ) );
}

// iq's voronoi
// https://www.shadertoy.com/view/ldl3W8
vec3 voronoi( in vec2 x )
{
    vec2 n = floor(x);
    vec2 f = fract(x);

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
        o = 0.5 + 0.5*sin( TIME + 6.2831*o );
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
        o = 0.5 + 0.5*sin( TIME + 6.2831*o );
        #endif	
        vec2 r = g + o - f;

        if( dot(mr-r,mr-r)>0.00001 )
        md = min( md, dot( 0.5*(mr+r), normalize(r-mr) ) );
    }

    return vec3( md, mo );
}

void main() {


	vec4 fragCoordPos = gl_FragCoord;
	
	vec2 b = gl_FragCoord.xy / RENDERSIZE.x;
    fragCoordPos.xy /= RENDERSIZE.xy;
    float aspect = RENDERSIZE.x / RENDERSIZE.y;
    
    vec3 v = voronoi( b * scale );
    
    float rr = lineRotation * v.z;
    float c = cos( rr );
    float s = sin( rr );
    mat2 r = mat2( c, -s, s, c );
    vec2 q = b * r;
    
    float o = smoothstep( 0.25, 0.4, 0.48 + 0.5 * sin( 150.0 * q.y + 5.0 * TIME ) );
    o = mix( 0.0, o, smoothstep( .03, 0.06, v.x ) );
    
    vec2 border = vec2( 0.03 );
    border.x /= aspect;
    for ( float i = 3.0; i > 0.0; i -= 1.0 )
    {
        float c = mod( i + 1.0, 2.0 );
    	o = mix( c, o, step( i * border.x, fragCoordPos.x ) );
    	o = mix( c, o, step( i * border.y, fragCoordPos.y ) );
    	o = mix( c, o, 1.0 - step( 1.0 - i * border.x, fragCoordPos.x ) );
    	o = mix( c, o, 1.0 - step( 1.0 - i * border.y, fragCoordPos.y ) );
    }
	gl_FragColor = vec4( vec3( o ), 1.0 );
}
