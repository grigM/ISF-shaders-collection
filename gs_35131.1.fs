/*
{
  "CATEGORIES" : [
    "Automatically Converted"
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35131.1"
}
*/


// Ray Marching Tutorial (With Shading)
// By: Brandon Fogerty
// bfogerty at gmail dot com
// xdpixel.com

#ifdef GL_ES
precision mediump float;
#endif


float sphere( vec3 p, float radius )
{
    return length( p ) - radius;
}
float vmax(vec3 v) {
    return max(max(v.x, v.y), v.z);
}

float box(vec3 p, vec3 b) { //cheap box
    return vmax(abs(p) - b);
}

vec2 rotate (vec2 pos, float angle)
{
    float c = cos(angle);
    float s = sin(angle);
    return mat2 (c, s, -s, c) * pos; // matrice de rotation 2d
}

float fOpUnionRound(float a, float b, float r) {
	vec2 u = max(vec2(r - a,r - b), vec2(0));
	return max(r, min (a, b)) - length(u);
}

float map( vec3 p )
{
	//p.xz = rotate(p.xz, -mouse.x*8.5);
    p.yz = rotate(p.yz, mouse.y*10.);
	
	float b = box( p, vec3(1.,1.,1.) );
	float aa = b;
	//vec3 q = p+1.;
	//float c = box( q, vec3(1.,1.,1.) );
	
	for(int i=0; i<5; i++)
	{
	float off = float(i);
	vec3 q = p+off*.5;
	q.xz = rotate(q.xz, -mouse.x*off*12.);
	float c = box( q, vec3(off,1.,1.) );
	aa = fOpUnionRound(aa,c,.5);
	//aa = min(aa,c);
	}
    //p.x=mod(p.x+3.0,6.0)-3.0;
    //p.y=mod(p.y+3.0,6.0)-3.0;
	//p.z=mod(p.z+3.0,6.0)-3.0;
		
	//aa = min(b,c);
    return aa;
}

vec3 getNormal( vec3 p )
{
    vec3 e = vec3( 0.001, 0.00, 0.00 );

    float deltaX = map( p + e.xyy ) - map( p - e.xyy );
    float deltaY = map( p + e.yxy ) - map( p - e.yxy );
    float deltaZ = map( p + e.yyx ) - map( p - e.yyx );

    return normalize( vec3( deltaX, deltaY, deltaZ ) );
}

float trace( vec3 origin, vec3 direction, out vec3 p )
{
    float totalDistanceTraveled = 0.0;

    for( int i=0; i <2048; ++i)
    {

        p = origin + direction * totalDistanceTraveled * .95;

        float distanceFromPointOnRayToClosestObjectInScene = map( p );
        totalDistanceTraveled += distanceFromPointOnRayToClosestObjectInScene;

        if( distanceFromPointOnRayToClosestObjectInScene < 0.0001 )
        {
            break;
        }

        if( totalDistanceTraveled > 10000.0 )
        {
            totalDistanceTraveled = 0.0000;
            break;
        }
    }

    return totalDistanceTraveled;
}

vec3 calculateLighting(vec3 pointOnSurface, vec3 surfaceNormal, vec3 lightPosition, vec3 cameraPosition)
{
    vec3 fromPointToLight = normalize(lightPosition - pointOnSurface);
    float diffuseStrength = clamp( dot( surfaceNormal, fromPointToLight ), 0.0, 1.0 );

    vec3 diffuseColor = diffuseStrength * vec3( 1.0, 0.0, 0.0 );
    vec3 reflectedLightVector = normalize( reflect( -fromPointToLight, surfaceNormal ) );

    vec3 fromPointToCamera = normalize( cameraPosition - pointOnSurface );
    float specularStrength = pow( clamp( dot(reflectedLightVector, fromPointToCamera), 0.0, 1.0 ), 10.0 );

    specularStrength = min( diffuseStrength, specularStrength );
    vec3 specularColor = specularStrength * vec3( 1.0 );

    vec3 finalColor = diffuseColor + specularColor;

    return finalColor;
}

void main( void )
{

    vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy ) * 2.0 - 1.0;

    uv.x *= RENDERSIZE.x / RENDERSIZE.y;

    vec3 cameraPosition = vec3(0.0, 0.0, -10.0 );

    vec3 cameraDirection = normalize( vec3( uv.x, uv.y, 1.0) );

    vec3 pointOnSurface;
    float distanceToClosestPointInScene = trace( cameraPosition, cameraDirection, pointOnSurface );

    vec3 finalColor = vec3(0.0);
    if( distanceToClosestPointInScene > 0.0 )
    {
        vec3 lightPosition = vec3( 0.0, 4.5, -10.0 );
        vec3 surfaceNormal = getNormal( pointOnSurface );
        finalColor = calculateLighting( pointOnSurface, surfaceNormal, lightPosition, cameraPosition );
    }

    gl_FragColor = vec4( finalColor, 1.0 );
}