/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : [
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_1.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_2.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_3.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_4.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_5.png"
      ],
      "TYPE" : "cube"
    }
  ],
  "CATEGORIES": [
		"filter"
	],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MsdyW2 by felipunkerito.  Trying some reflections based on iq's Cubemaps, please help me get it right as it is not photoreal as it should",
  "INPUTS" : [
  
  	{
			"NAME": "inputImage",
			"TYPE": "image"
		},
  
    {
      "NAME" : "iChannel1",
      "TYPE" : "audio"
    }
  ]
}
*/


#define EPS 0.0001
#define STEPS 1028
#define FAR 1000.0

float hash( float n )
{

    return fract( sin( n ) * 45843.349 );
    
}

float noise( in vec3 x )
{

    vec3 p = floor( x );
    vec3 k = fract( x );
    
    k *= k * k * ( 3.0 - 2.0 * k );
    
    float n = p.x + p.y * 57.0 + p.z * 113.0; 
    
    float a = hash( n );
    float b = hash( n + 1.0 );
    float c = hash( n + 57.0 );
    float d = hash( n + 58.0 );
    
    float e = hash( n + 113.0 );
    float f = hash( n + 114.0 );
    float g = hash( n + 170.0 );
    float h = hash( n + 171.0 );
    
    float res = mix( mix( mix ( a, b, k.x ), mix( c, d, k.x ), k.y ),
                     mix( mix ( e, f, k.x ), mix( g, h, k.x ), k.y ),
                     k.z
    				 );
    
    return res;
    
}

float fbm( in vec3 p )
{

    float f = 0.0;
    f += 0.5000 * noise( p ); p *= 2.02;
    f += 0.2500 * noise( p ); p *= 2.03;
    f += 0.1250 * noise( p ); p *= 2.01;
    f += 0.0625 * noise( p );
    f += 0.0125 * noise( p );
    return f / 0.9375;
    
}

float pla( vec3 p )
{

    return p.y + 1.0;
    
}

float sph( vec3 p, float r )
{

    float wav = IMG_NORM_PIXEL(iChannel1,mod(vec2( 0.0, 0.25 ),1.0)).x;
    r += r * 0.15 * ( fbm( p * fbm( p + wav + TIME ) ) );
    return length( p ) - r;
    
}

vec2 map( vec3 p )
{

    vec2 sp = vec2( sph( p, 1.0 ), 0.0 );
    vec2 pla = vec2( pla( p ), 1.0 );
    /*if( sp.x < pla.x ) pla = sp;
    return pla;*/
    return sp;
    
}

vec3 grad( vec3 p )
{

    vec2 e = vec2( 0.0, EPS );
    return vec3( map( p + e.xyy ).x -  map( p - e.xyy ).x,
                 map( p + e.yxy ).x -  map( p - e.yxy ).x,
                 map( p + e.yyx ).x -  map( p - e.yyx ).x 
                );
    
}

/*float softShadows( in vec3 ro, in vec3 rd )
{

    float res = 1.0;
    for( float t = 0.1; t < 8.0; ++t )
    {
    
        float h = map( ro + rd * t ).x;
        if( h < EPS ) return 0.0;
        res = max( res, 8.0 * h / t );
        
    }
    
    return res;
    
}*/

vec3 rend( vec3 ro, vec3 rd, float t )
{

    /*vec3 p = ro + rd * t;
    vec3 n = normalize( grad( p ) );
    vec3 col;
    //vec3 lig = vec3( 1.0, 0.8, 0.6 );
    vec3 lig = 2.0 * normalize( vec3( sin( TIME ), 0.6, cos( TIME ) ) );
    vec3 ref = reflect( rd, p );
    vec3 refN = reflect( rd, n );
    vec3 con = vec3( 1.0 );
    vec3 cub = textureCube(iChannel0,ref).xyz;
    
    float amb = 0.5 + 0.5 * n.y;
    float dif = max( 0.0, dot( n, lig ) );
    float spe = pow( clamp( dot( lig, ref ), 0.0, 1.0 ), 6.0 );
    float rim = pow( clamp( 1.0 + dot( n, rd ), 0.0, 1.0 ), 4.0 );
    float fre = 0.3 + 0.7*pow( clamp( 1.0 + dot( rd, p ), 0.0, 1.0 ), 5.0 );
    float sha = softShadows( p, lig );
    
    col = con * vec3( 1.0 );
    col += vec3( 1.0 ) * amb;
    //col += dif * vec3( 0.45 );
    col += 0.5 * rim;
    col += 0.3 * spe;
    col += 0.1 * sha;
    col += 2.0 * dot( cub, vec3( 3.0 ) ) * fre;
    //col = sqrt( col );
    col *= 0.1;
    
    
    vec3 tex = IMG_NORM_PIXEL(iChannel1,mod(p.xz,1.0)).xyz;
    
    //if( map( p ).y == 0.0 ) col *= tex * 2.0;
    
    if( map( p ).y == 1.0 ) col *= vec3( 0.0, 0.5, 0.8 );
    
    return col;*/
    
    vec3 n = normalize( grad( ro + rd * t ) );
    vec3 lig = normalize( vec3( sin( TIME ), 0.8, cos( TIME ) ) );
    vec3 ref = reflect( rd, ro + rd * t );
    vec3 col;
    vec3 tex = textureCube(iChannel0,ref).rgb;
    
    //float rim = pow( clamp( 1.0 + dot( rd, ref ), 0.0, 1.0 ), 4.0 );
    float spe = pow( clamp( dot( ref, lig ), 0.0, 1.0 ), 16.0 );
    col += tex;
    //col += 0.2 * rim;
    col += 1.0 * spe;
    
    return col;
    
}

void main() {



    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = ( -RENDERSIZE.xy + 2.0 * gl_FragCoord.xy ) / RENDERSIZE.y;
    
    vec3 ro = 2.0 * vec3( sin( TIME * 0.2 ), 0.0, cos( TIME * 0.2 ) );
    //vec3 ro = vec3( 0.0, 0.0, 2.0 );
    vec3 ww = normalize( vec3( 0.0 ) - ro );
    vec3 uu = normalize( cross( vec3( 0.0, 1.0, 0.0 ), ww ) );
    vec3 vv = normalize( cross( ww, uu ) );
    vec3 rd = normalize( uv.x * uu + uv.y * vv + 1.5 * ww );
    float t = 0.0, d = EPS;
    for( int i = 0; i < STEPS; ++i )
    {
        
    	d = map( ro + rd * t ).x;
        if( d < EPS || t > FAR ) break;
        
        t += d;
        
    
    }
    
    // Time varying pixel color
    
    vec3 tex = textureCube(iChannel0,rd).xyz;
    
    vec3 col = d < EPS ? rend( ro, rd, t ) : vec3( tex );
    // Output to screen
    gl_FragColor = vec4(col,1.0);
}
