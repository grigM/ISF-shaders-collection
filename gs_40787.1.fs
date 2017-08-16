/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40787.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define iGlobalTime TIME

// https://www.shadertoy.com/view/MdlBW7
// "Volatile Sphere of Cubes" by Krzysztof Narkowicz @knarkowicz
// License: Public Domain

const float MATH_PI = float( 3.14159265359 );

float saturate( float x )
{
    return clamp( x, 0.0, 1.0 );
}

float Sphere( vec3 p, float s )
{
	return length( p ) - s;
}

float RoundBox( vec3 p, vec3 b, float r )
{
	return length( max( abs( p ) - b, 0.0 ) ) - r;
}

float Union( float a, float b )
{
    return min( a, b );
}

float RepeatAngle( inout vec2 p, float n ) 
{
	float angle = 2.0 * MATH_PI / n;
	float a = atan( p.y, p.x ) + angle / 2.0;
	float r = length( p );
	float c = floor( a / angle );
	a = mod( a, angle ) - angle / 2.;
	p = vec2( cos( a ), sin( a ) ) * r;
	return c;
}

void Rotate( inout vec2 p, float a ) 
{
    p = cos( a ) * p + sin( a ) * vec2( p.y, -p.x );
}

float Scene( vec3 p )
{
    float a = p.x;
    p.x = -p.y;
    p.y = a;
    
    Rotate( p.yz, iGlobalTime );    
    
    // sphere
    float ret = Sphere( p, 0.6 );
	
    float s = abs(sin(TIME*0.125))*0.115+0.1;
	
    for ( float i = -3.0; i < 3.0; ++i )
    {
        float theta   = ( (i+s*222.0) ) * MATH_PI * 0.5;
        float boxNum  = 3.0 - sin( i * acos(s) ) * 2.0;
        float radius  = cos( i + theta ) * 1.1;
        float boxSize = abs( radius ) * 0.12;
        float boxOffset = -sin( theta ) * 0.7;
                
        Rotate( p.yz, MATH_PI / ( 2.0 * boxNum ) );
        
        vec3 t = p;		
		float boxLen = 0.15 + sin( iGlobalTime * 2.0 ) * 0.1;        
    	t = p + vec3( boxOffset, 0.0, 0.0 );
    	RepeatAngle( t.yz, boxNum );
    	t.y -= radius;
        Rotate( t.xy, -theta );
    	ret = Union( ret, RoundBox( t, vec3( boxSize, boxLen, boxSize ), 0.02 ) );
        
		t = p;
		boxLen = 0.15 + sin( iGlobalTime * 3.0 ) * 0.1;        
		Rotate( p.yz, MATH_PI / boxNum );
    	t = p + vec3( boxOffset, 0.0, 0.0 );
    	RepeatAngle( t.yz, boxNum );
    	t.y -= radius;
        Rotate( t.xy, -theta );
    	ret = Union( ret, RoundBox( t, vec3( boxSize, boxLen, boxSize ), 0.02 ) );
    }
    
    // debris
    vec3 t = p;
    t.y = abs( t.y + 0.3 * sin( 1.7 * iGlobalTime ) );
    t.z = abs( t.z );
    t -= vec3( 0.8, 1.0, 1.0 );
    Rotate( t.xz, iGlobalTime );    
	Rotate( t.xy, -0.25 );
    ret = Union( ret, RoundBox( t, vec3( 0.01 ), 0.005 ) );
    
    Rotate( p.xy, 0.1 * iGlobalTime );
    Rotate( p.yz, MATH_PI * 0.25 );
    t = p;
    t.y = abs( t.y + 0.3 * sin( 1.7 * iGlobalTime ) );
    t.z = abs( t.z );
    t -= vec3( 0.8, 1.0, 1.0 );
    Rotate( t.xz, iGlobalTime );    
	Rotate( t.xy, -0.25 );    
    ret = Union( ret, RoundBox( t, vec3( 0.01 ), 0.005 ) );    

	return ret;
}

float CastRay( in vec3 ro, in vec3 rd )
{
    const float maxd = 5.0;
    
	float h = 1.0;
    float t = 0.0;
   
    for ( int i = 0; i < 150; ++i )
    {
        if ( h < 0.001 || t > maxd ) 
        {
            break;
        }
        
	    h = Scene( ro + rd * t );
        t += h;
    }

    if ( t > maxd )
    {
        t = -1.0;
    }
	
    return t;
}

vec3 SceneNormal( in vec3 pos )
{
	vec3 eps = vec3( 0.001, 0.0, 0.0 );
	vec3 normal = vec3(
	    Scene( pos + eps.xyy ) - Scene( pos - eps.xyy ),
	    Scene( pos + eps.yxy ) - Scene( pos - eps.yxy ),
	    Scene( pos + eps.yyx ) - Scene( pos - eps.yyx ) );
	return normalize( normal );
}

vec3 KeyColor = vec3( 1.0, 0.98, 0.94 );
vec3 FillColor = vec3( 0.09, 0.19, 0.25 ); 

vec3 Sky( vec3 rayDir )
{
    vec3 skyPos     = rayDir;
    vec2 skyAngle   = vec2( atan( skyPos.z, skyPos.x ), acos( skyPos.y ) );

    vec3 color = KeyColor * mix( 1.0, 0.4, smoothstep( 0.0, 1.0, saturate( 1.5 * skyPos.y + 0.1 ) ) );
    color = mix( color, FillColor, smoothstep( 0.0, 1.0, saturate( -1.5 * skyPos.y - 0.1 ) ) );
    return color;
}

void mainImage( out vec4 fragColor, in vec2 fragCoord, vec2 iResolution )
{
	vec2 q = fragCoord.xy / iResolution.xy;
    vec2 p = -1.0 + 2.0 * q;
	p.x *= iResolution.x / iResolution.y;

    vec3 rayOrigin	= vec3( 0.0, -0.2, -3.0 );
	vec3 rayDir 	= normalize( vec3( p.xy, 2.0 ) ); 

	vec3 color = Sky( rayDir );
	float t = CastRay( rayOrigin, rayDir );
    if ( t > 0.0 )
    {
        vec3 pos = rayOrigin + t * rayDir;
        vec3 normal = SceneNormal( pos );
        vec3 lightDir = normalize( vec3( 0.5, 0.5, -1.0 ) );
            
        // ambient, diffuse and specular
        color = 0.8 * mix( FillColor, KeyColor, 0.5 * normal.y + 0.5 );
        float fresnel = saturate( pow( 1.4 + dot( rayDir, normal ), 5.0 ) );        
        color += 0.5 * Sky( normal ) * mix( 0.1, 1.0, fresnel );
        color += 0.3 * KeyColor * saturate( dot( normal, lightDir ) );
        color *= 0.85;
    }
    else
    {
        // sphere shadow
        float planeT = -( rayOrigin.y + 1.2 ) / rayDir.y;
        if ( planeT > 0.0 )
        {
            vec3 p = rayOrigin + planeT * rayDir;
            
            float radius = max( 0.15 + sin( iGlobalTime * 2.0 ) * 0.1, 0.15 + sin( iGlobalTime * 3.0 ) * 0.1 );
            color *= 0.7 + 0.3 * smoothstep( 0.0, 1.0, saturate( length( p + vec3( 0.0, 1.0, 0.0 ) ) - radius ) );
        }		
    }
    
    float vignette = q.x * q.y * ( 1.0 - q.x ) * ( 1.0 - q.y );
    vignette = saturate( pow( 32.0 * vignette, 0.05 ) );
    color *= vignette;
        
    fragColor = vec4( color, 1.0 );
}

void main( void ) {

	mainImage( gl_FragColor, .5-vv_FragNormCoord, vec2(1.) );

}