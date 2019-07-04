/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/lt2SDD by eddietree.  rainbow core",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


// a study on raymarching, soft-shadows, ao, etc
// borrowed heavy from others, esp @cabbibo and @iquilezles and more
// by @eddietree

#define float3 vec3

const float INTERSECTION_PRECISION = 0.001;
const int NUM_OF_TRACE_STEPS = 50;

float distSphere(vec3 p, float radius) 
{
    return length(p) - radius;
}

float sdTorus( vec3 p, vec2 t )
{
  vec2 q = vec2(length(p.xy)-t.x,p.z);
  return length(q)-t.y;
}

float opS( float d1, float d2 )
{
    return max(-d1,d2);
}

float opU( float d1, float d2 )
{
    return min(d1,d2);
}

float opI( float d1, float d2 )
{
    return max(d1,d2);
}

// by shane : https://www.shadertoy.com/view/4lSXzh
float Voronesque( in vec3 p )
{
    vec3 i  = floor(p + dot(p, vec3(0.333333)) );  p -= i - dot(i, vec3(0.166666)) ;
    vec3 i1 = step(0., p-p.yzx), i2 = max(i1, 1.0-i1.zxy); i1 = min(i1, 1.0-i1.zxy);    
    vec3 p1 = p - i1 + 0.166666, p2 = p - i2 + 0.333333, p3 = p - 0.5;
    vec3 rnd = vec3(7, 157, 113); // I use this combination to pay homage to Shadertoy.com. :)
    vec4 v = max(0.5 - vec4(dot(p, p), dot(p1, p1), dot(p2, p2), dot(p3, p3)), 0.);
    vec4 d = vec4( dot(i, rnd), dot(i + i1, rnd), dot(i + i2, rnd), dot(i + 1., rnd) ); 
    d = fract(sin(d)*262144.)*v*2.; 
    v.x = max(d.x, d.y), v.y = max(d.z, d.w), v.z = max(min(d.x, d.y), min(d.z, d.w)), v.w = min(v.x, v.y); 
    return  max(v.x, v.y) - max(v.z, v.w); // Maximum minus second order, for that beveled Voronoi look. Range [0, 1].
    
}

mat3 calcLookAtMatrix( in vec3 ro, in float3 ta, in float roll )
{
    vec3 ww = normalize( ta - ro );
    vec3 uu = normalize( cross(ww,vec3(sin(roll),cos(roll),0.0) ) );
    vec3 vv = normalize( cross(uu,ww));
    return mat3( uu, vv, ww );
}

void doCamera( out vec3 camPos, out vec3 camTar, in float time, in vec2 mouse )
{
    float radius = 6.0;
    float theta = 9.6 + 5.0*mouse.x;// + TIME*0.5;
    float phi = 3.14159*0.4;//5.0*mouse.y;
    
    float pos_x = radius * cos(theta) * sin(phi);
    float pos_z = radius * sin(theta) * sin(phi);
    float pos_y = radius * cos(phi);
    
    camPos = vec3(pos_x, pos_y, pos_z);
    camTar = vec3(0.0,0.0,0.0);
}

float smin( float a, float b, float k )
{
    float res = exp( -k*a ) + exp( -k*b );
    return -log( res )/k;
}

// noise func
float hash( float n ) { return fract(sin(n)*753.5453123); }
float noise( in vec3 x )
{
    vec3 p = floor(x);
    vec3 f = fract(x);
    f = f*f*(3.0-2.0*f);
	
    float n = p.x + p.y*157.0 + 113.0*p.z;
    return mix(mix(mix( hash(n+  0.0), hash(n+  1.0),f.x),
                   mix( hash(n+157.0), hash(n+158.0),f.x),f.y),
               mix(mix( hash(n+113.0), hash(n+114.0),f.x),
                   mix( hash(n+270.0), hash(n+271.0),f.x),f.y),f.z);
}

// checks to see which intersection is closer
// and makes the y of the vec2 be the proper id
vec2 opU( vec2 d1, vec2 d2 ){
	return (d1.x<d2.x) ? d1 : d2; 
}


vec3 hash( vec3 x )
{
	x = vec3( dot(x,vec3(127.1,311.7, 74.7)),
			  dot(x,vec3(269.5,183.3,246.1)),
			  dot(x,vec3(113.5,271.9,124.6)));

	return fract(sin(x)*43758.5453123);
}

// from iq: https://www.shadertoy.com/view/ldl3Dl
vec3 voronoi( in vec3 x )
{
    vec3 p = floor( x );
    vec3 f = fract( x );

	float id = 0.0;
    vec2 res = vec2( 100.0 );
    for( int k=-1; k<=1; k++ )
    for( int j=-1; j<=1; j++ )
    for( int i=-1; i<=1; i++ )
    {
        vec3 b = vec3( float(i), float(j), float(k) );
        vec3 r = vec3( b ) - f + hash( p + b );
        float d = dot( r, r );

        if( d < res.x )
        {
			id = dot( p+b, vec3(1.0,57.0,113.0 ) );
            res = vec2( d, res.x );			
        }
        else if( d < res.y )
        {
            res.y = d;
        }
    }

    return vec3( sqrt( res ), abs(id) );
}

float sdCapsule( vec3 p, vec3 a, vec3 b, float r )
{
    vec3 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}


vec4 mod289(vec4 x) {
  return x - floor(x * (1.0 / 289.0)) * 289.0; }

float mod289(float x) {
  return x - floor(x * (1.0 / 289.0)) * 289.0; }

vec4 permute(vec4 x) {
     return mod289(((x*34.0)+1.0)*x);
}

float permute(float x) {
     return mod289(((x*34.0)+1.0)*x);
}

vec4 taylorInvSqrt(vec4 r)
{
  return 1.79284291400159 - 0.85373472095314 * r;
}

float taylorInvSqrt(float r)
{
  return 1.79284291400159 - 0.85373472095314 * r;
}

vec4 grad4(float j, vec4 ip)
  {
  const vec4 ones = vec4(1.0, 1.0, 1.0, -1.0);
  vec4 p,s;

  p.xyz = floor( fract (vec3(j) * ip.xyz) * 7.0) * ip.z - 1.0;
  p.w = 1.5 - dot(abs(p.xyz), ones.xyz);
  s = vec4(lessThan(p, vec4(0.0)));
  p.xyz = p.xyz + (s.xyz*2.0 - 1.0) * s.www; 

  return p;
  }
						
// (sqrt(5) - 1)/4 = F4, used once below
#define F4 0.309016994374947451

float snoise(vec4 v)
  {
  const vec4  C = vec4( 0.138196601125011,  // (5 - sqrt(5))/20  G4
                        0.276393202250021,  // 2 * G4
                        0.414589803375032,  // 3 * G4
                       -0.447213595499958); // -1 + 4 * G4

// First corner
  vec4 i  = floor(v + dot(v, vec4(F4)) );
  vec4 x0 = v -   i + dot(i, C.xxxx);

// Other corners

// Rank sorting originally contributed by Bill Licea-Kane, AMD (formerly ATI)
  vec4 i0;
  vec3 isX = step( x0.yzw, x0.xxx );
  vec3 isYZ = step( x0.zww, x0.yyz );
//  i0.x = dot( isX, vec3( 1.0 ) );
  i0.x = isX.x + isX.y + isX.z;
  i0.yzw = 1.0 - isX;
//  i0.y += dot( isYZ.xy, vec2( 1.0 ) );
  i0.y += isYZ.x + isYZ.y;
  i0.zw += 1.0 - isYZ.xy;
  i0.z += isYZ.z;
  i0.w += 1.0 - isYZ.z;

  // i0 now contains the unique values 0,1,2,3 in each channel
  vec4 i3 = clamp( i0, 0.0, 1.0 );
  vec4 i2 = clamp( i0-1.0, 0.0, 1.0 );
  vec4 i1 = clamp( i0-2.0, 0.0, 1.0 );

  //  x0 = x0 - 0.0 + 0.0 * C.xxxx
  //  x1 = x0 - i1  + 1.0 * C.xxxx
  //  x2 = x0 - i2  + 2.0 * C.xxxx
  //  x3 = x0 - i3  + 3.0 * C.xxxx
  //  x4 = x0 - 1.0 + 4.0 * C.xxxx
  vec4 x1 = x0 - i1 + C.xxxx;
  vec4 x2 = x0 - i2 + C.yyyy;
  vec4 x3 = x0 - i3 + C.zzzz;
  vec4 x4 = x0 + C.wwww;

// Permutations
  i = mod289(i); 
  float j0 = permute( permute( permute( permute(i.w) + i.z) + i.y) + i.x);
  vec4 j1 = permute( permute( permute( permute (
             i.w + vec4(i1.w, i2.w, i3.w, 1.0 ))
           + i.z + vec4(i1.z, i2.z, i3.z, 1.0 ))
           + i.y + vec4(i1.y, i2.y, i3.y, 1.0 ))
           + i.x + vec4(i1.x, i2.x, i3.x, 1.0 ));

// Gradients: 7x7x6 points over a cube, mapped onto a 4-cross polytope
// 7*7*6 = 294, which is close to the ring size 17*17 = 289.
  vec4 ip = vec4(1.0/294.0, 1.0/49.0, 1.0/7.0, 0.0) ;

  vec4 p0 = grad4(j0,   ip);
  vec4 p1 = grad4(j1.x, ip);
  vec4 p2 = grad4(j1.y, ip);
  vec4 p3 = grad4(j1.z, ip);
  vec4 p4 = grad4(j1.w, ip);

// Normalise gradients
  vec4 norm = taylorInvSqrt(vec4(dot(p0,p0), dot(p1,p1), dot(p2, p2), dot(p3,p3)));
  p0 *= norm.x;
  p1 *= norm.y;
  p2 *= norm.z;
  p3 *= norm.w;
  p4 *= taylorInvSqrt(dot(p4,p4));

// Mix contributions from the five corners
  vec3 m0 = max(0.6 - vec3(dot(x0,x0), dot(x1,x1), dot(x2,x2)), 0.0);
  vec2 m1 = max(0.6 - vec2(dot(x3,x3), dot(x4,x4)            ), 0.0);
  m0 = m0 * m0;
  m1 = m1 * m1;
  return 49.0 * ( dot(m0*m0, vec3( dot( p0, x0 ), dot( p1, x1 ), dot( p2, x2 )))
               + dot(m1*m1, vec2( dot( p3, x3 ), dot( p4, x4 ) ) ) ) ;

  }


//--------------------------------
// Modelling 
//--------------------------------
vec2 map( vec3 pos )
{  
    vec3 voro0 = voronoi(pos);
    
    float t1 = distSphere( pos, 2.0 + snoise( vec4(pos*0.75, cos(TIME*1.5))) * 0.1 );//
    
    float torus = sdTorus( pos, vec2(1.6, 0.2 + noise(pos + vec3(sin(pos.z+TIME*1.5),0.0,cos(pos.y*0.8+TIME*1.5) )) ));
    
    t1 = opS( torus,t1 );
    
    //float cap0 = sdCapsule( pos + vec3(0.0, sin(pos.x + TIME*3.5)*0.1,0.0), vec3(0.8,0.0,0.0), vec3(9.0,0.3,-0.2), 0.05 );
    //float cap1 = sdCapsule( pos + vec3(0.0, cos(pos.x + TIME*3.5)*0.1,0.0), vec3(0.8,0.0,0.0), vec3(9.0,-0.3,0.2), 0.05);
    //float tail = smin( cap0, cap1, 9.0 );
    //t1 = min( t1, tail);
    //t1 = capsule;
    
    //t1 = min( t1, distSphere( pos + vec3(3.0,0.0,0.0), 0.2 +voronoi(pos).x ) + voronoi(pos*9.0).x*0.01 );
    //t1 = min( t1, distSphere( pos + vec3(-3.0,0.0,0.0), 0.2 +voronoi(pos).x ) + voronoi(pos*9.0).x*0.01 );
    //t1 = vnoise(pos*1.0, 2.0 );
    //float radius = (sin(pos.x) * 0.5 + 0.5) * 2.0;
    //float t1 = distSphere( pos,  floor(radius) );
    
    //t1 = min( t1, distSphere(pos + vec3(4.0,0.0,0.0), 0.3) );
    //t1 = min( t1, distSphere(pos + vec3(-4.0,0.0,0.0), 0.3) );
   
   	return vec2( t1, 1.0 );
}

float shadow( in vec3 ro, in vec3 rd )
{
    const float k = 2.0;
    
    const int maxSteps = 10;
    float t = 0.0;
    float res = 1.0;
    
    for(int i = 0; i < maxSteps; ++i) {
        
        float d = map(ro + rd*t).x;
            
        if(d < INTERSECTION_PRECISION) {
            
            return 0.0;
        }
        
        res = min( res, k*d/t );
        t += d;
    }
    
    return res;
}

float ambientOcclusion( in vec3 ro, in vec3 rd )
{
    const int maxSteps = 11;
    const float stepSize = 0.01;
    
    float t = 0.0;
    float res = 0.0;
    
    // starting d
    float d0 = map(ro).x;
    
    for(int i = 0; i < maxSteps; ++i) {
        
        float d = map(ro + rd*t).x;
		float diff = max(d0-d, 0.0);
        
        res += diff;
        
        t += stepSize;
    }
    
    return 1.0-res;
}

vec3 hsv(float h, float s, float v)
{
  return mix( vec3( 1.0 ), clamp( ( abs( fract(
    h + vec3( 3.0, 2.0, 1.0 ) / 3.0 ) * 6.0 - 3.0 ) - 1.0 ), 0.0, 1.0 ), s ) * v;
}


vec3 calcNormal( in vec3 pos )
{
	vec3 eps = vec3( 0.0001, 0.0, 0.0 );
	vec3 nor = vec3(
	    map(pos+eps.xyy).x - map(pos-eps.xyy).x,
	    map(pos+eps.yxy).x - map(pos-eps.yxy).x,
	    map(pos+eps.yyx).x - map(pos-eps.yyx).x );
	return normalize(nor);
}


vec3 rayPlaneIntersection( vec3 ro, vec3 rd, vec4 plane )
{
	float t = -( dot(ro, plane.xyz) + plane.w) / dot( rd, plane.xyz );
	return ro + t * rd;
}


vec3 renderColor( vec3 ro , vec3 rd, in vec3 color, vec3 currPos )
{
    vec3 normal = calcNormal( currPos );
    
    vec3 lightPos = vec3(-1.5,0.0,0.0);
    vec3 lightColor = vec3(1.0,0.5,0.6);

    vec3 lightDir = normalize(vec3(1.0,0.0,0.0));
    float shadowVal = shadow( currPos - rd* 0.03, lightDir  );
    
    float ao = ambientOcclusion( currPos - normal*INTERSECTION_PRECISION*3.0, -normal );
    float ndotl = abs(dot( -rd, normal ));
    float rim = pow(1.0-ndotl, 3.5);

    
    // sphere
    float distToCenter = length(currPos);
    if ( abs(distToCenter - 2.0) < 0.13  )
    {
        vec4 pos = vec4( currPos*15.0, 1.0 );
        float noiseVal = snoise(pos);
        
        color = vec3(0.0);
        color += vec3(rim + noiseVal*0.1);
    }
    
    // inside
    else
    {
        vec4 pos = vec4( currPos*15.0, 1.0 );
        color = normal*0.5+vec3(0.5);
        color += smoothstep( 1.5, 2.0, distToCenter ) * 0.6;
    }
    
        
    return color;
}

bool renderRayMarch(vec3 ro, vec3 rd, inout vec3 color ) {
    const int maxSteps = NUM_OF_TRACE_STEPS;
        
    float t = 0.0;
    float d = 0.0;
    
    vec3 lightDir = normalize(vec3(1.0,0.4,0.0));
    
    for(int i = 0; i < maxSteps; ++i) 
    {
        vec3 currPos = ro + rd * t;
        d = map(currPos).x;
        if(d < INTERSECTION_PRECISION) 
        {
        	break;
        }
        
        t += d;
    }
       
    if(d < INTERSECTION_PRECISION) 
    {
	    vec3 currPos = ro + rd * t;
    	color = renderColor( ro, rd, color, currPos );
        return true;
    }
    
    
     vec3 planePoint = rayPlaneIntersection(ro, rd, vec4(0.0, 1.0, 0.0, 2.5));
	float shadowFloor = shadow( planePoint, vec3(0.0,1.0,0.0));
	color = color * mix( 0.6, 1.0, shadowFloor );
    
    return false;
}


void main() {



	vec2 p = (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy)/RENDERSIZE.y;
    vec2 m = iMouse.xy/RENDERSIZE.xy;
    
    // camera movement
    vec3 ro, ta;
    doCamera( ro, ta, TIME, m );
    // camera matrix
    mat3 camMat = calcLookAtMatrix( ro, ta, 0.0 );  // 0.0 is the camera roll
    
	// create view ray
	vec3 rd = normalize( camMat * vec3(p.xy,2.0) ); // 2.0 is the lens length
    
    // calc color
    vec3 col = vec3(0.9);
    //vec3 col = texture (iChannel0, rd).xyz;
    renderRayMarch( ro, rd, col );
    
    // vignette, OF COURSE
    float vignette = 1.0-smoothstep(1.0,2.5, length(p));
    col.xyz *= mix( 0.5, 1.0, vignette);
        
    gl_FragColor = vec4( col , 1. );
}
