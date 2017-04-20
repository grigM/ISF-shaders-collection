/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "noob",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtVGDy by hasnmrtn.   i made a thing!",
  "INPUTS" : [
	{
			"NAME": "amp",
			"TYPE": "float",
			"MIN": -1.0,
			"MAX": 2.1,
			"DEFAULT": 1.02
	},
	{
			"NAME": "ampTrace",
			"TYPE": "float",
			"MIN": -1.0,
			"MAX": 2.1,
			"DEFAULT": 1.02
	}

  ]
}
*/


float map( vec3 p )
{
    //float amp = 1.0;//texture2D( iChannel2, vec2( 0.01, 0.002 ) ).x * 1.0;
    vec3 q = fract( p ) * 2.0 - 1.0;
    return length( q ) - 0.7;
}

float trace (vec3 o, vec3 r)
{
    //float amp = 1.6324;//texture2D( iChannel2, vec2( 0.1, 0.0 ) ).x * 2.0;
    float t = 0.0;
    for ( int i = 0; i < 158; ++i ) {
        vec3 p = o + r *( 2.3 * ampTrace );
        float d = map( p );
        t += d * 0.5;
         }
         return t;
        
}
void main()
{
	
    vec2 R = RENDERSIZE.xy;
    
    vec2 uv = gl_FragCoord.xy / R;
   
    uv = (2.0 * gl_FragCoord.xy - R) / R.y; 
    
    uv.x *= R.x / R.y; 
    
    vec3 r = normalize( vec3 ( uv, 1.0 ) ); 
    
    float the = TIME * 0.2;
    
    r.xz *= mat2( cos( the ), -sin( the ), sin( the ), cos( the ) );
    
    vec3 o = vec3( 0.0, 0.0, TIME );
    
    float t = trace( o, r ); 
    
    float fog = 3.0 / ( 1.0 + t * t * 0.1 );
    
    vec3 fc = vec3( fog );
    
    gl_FragColor = vec4( fc, 1.0 );
}