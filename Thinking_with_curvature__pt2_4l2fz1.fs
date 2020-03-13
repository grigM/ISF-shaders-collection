/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4l2fz1 by mmerchante.  In contrast with pt1, if we just use curvature for the normal estimation, we lose interesting silhouettes but can make shading more interesting. Note that the normal estimation epsilon is bigger to smooth the result a bit.",
  "INPUTS" : [

  ]
}
*/


// Uncomment these defines to switch implementation

//#define NORMAL_CURVATURE_NONE

// BASIC CURVATURE: We only add the curvature to the sdf when evaluating the normal
#define NORMAL_CURVATURE_BASIC

// NORMAL OFFSETTED CURVATURE: Before evaluating the curvature, we move outside a bit on the direction of the normal
//#define NORMAL_CURVATURE_NORMAL_OFFSET

// SDF WIDTH: Same as CURVATURE, but the width of the curvature evaluation depends on the sdf of the scene
//#define NORMAL_CURVATURE_SDF_WIDTH

// Curvature is just "added" to the resulting normal. This is independent of the previous defines
#define NORMAL_CURVATURE_ADD


// If you don't like the rendering, uncomment this
//#define DIFFUSE_ONLY

// How vertical/horizontal the crystal is :)
#define CRYSTAL_SCALE .75
#define CRYSTAL_VERTICAL_ANISOTROPY 1.2

// ---------------------------------------------------------
#define saturate(x) clamp(x, 0.0, 1.0)
#define PI 3.14159265
#define TAU (2*PI)
#define PHI (sqrt(5)*0.5 + 0.5)

#define MAX_STEPS 75
#define MAX_STEPS_F float(MAX_STEPS)

#define MAX_DISTANCE 15.0
#define MIN_DISTANCE .5
#define EPSILON .01

#define MATERIAL_NONE -1
#define MATERIAL_CRYSTAL 1

// ---------------------------------------------------------

// hg
void pR(inout vec2 p, float a) {
	p = cos(a)*p + sin(a)*vec2(p.y, -p.x);
}

// hg
float vmax(vec3 v) {
	return max(max(v.x, v.y), v.z);
}

// hg
float fBox(vec3 p, vec3 b) {
	vec3 d = abs(p) - b;
	return length(max(d, vec3(0))) + vmax(min(d, vec3(0)));
} 

// hg
float vmax(vec2 v) {
	return max(v.x, v.y);
}

// hg
float fBox2Cheap(vec2 p, vec2 b) {
	return vmax(abs(p)-b);
}

float fBox2(vec2 p, vec2 b) {
	vec2 d = abs(p) - b;
	return length(max(d, vec2(0))) + vmax(min(d, vec2(0)));
}

// hg
float fCapsule(vec3 p, float r, float c) {
	return mix(length(p.xz) - r, length(vec3(p.x, abs(p.y) - c, p.z)) - r, step(c, abs(p.y)));
}

// hg
float fOpIntersectionRound(float a, float b, float r) {
	vec2 u = max(vec2(r + a,r + b), vec2(0));
	return min(-r, max (a, b)) + length(u);
}

// iq
vec3 palette( float t, vec3 a, vec3 b, vec3 c, vec3 d)
{
    return saturate(a + b * cos(6.28318 * (c * t + d)));
}

// iq
float length2( vec2 p )
{
	return sqrt( p.x*p.x + p.y*p.y );
}

// iq
float length6( vec2 p )
{
	p = p*p*p; p = p*p;
	return pow( p.x + p.y, 1.0/6.0 );
}

// iq
float length8( vec2 p )
{
	p = p*p; p = p*p; p = p*p;
	return pow( p.x + p.y, 1.0/8.0 );
}

// iq
float sdTorus( vec3 p, vec2 t )
{
  vec2 q = vec2(length(p.yz)-t.x,p.x);
  return length(q)-t.y;
}

// iq
float sdTorus82( vec3 p, vec2 t )
{
  vec2 q = vec2(length2(p.yz)-t.x,p.x);
  return length8(q)-t.y;
}

// iq
vec2 opU(vec2 d1, vec2 d2 )
{
    return d1.x < d2.x ? d1 : d2;
}

// iq
vec3 rotateY( in vec3 p, float t )
{
    float co = cos(t);
    float si = sin(t);
    p.xz = mat2(co,-si,si,co)*p.xz;
    return p;
}

// iq
vec3 rotateX( in vec3 p, float t )
{
    float co = cos(t);
    float si = sin(t);
    p.yz = mat2(co,-si,si,co)*p.yz;
    return p;
}

// iq
vec3 rotateZ( in vec3 p, float t )
{
    float co = cos(t);
    float si = sin(t);
    p.xy = mat2(co,-si,si,co)*p.xy;
    return p;
}

// iq
mat3 rotationAxisAngle( const vec3 v, float a )
{
    float si = sin( a );
    float co = cos( a );
    float ic = 1.0 - co;

    return mat3x3( v.x*v.x*ic + co,       v.y*v.x*ic - si*v.z,    v.z*v.x*ic + si*v.y,
                   v.x*v.y*ic + si*v.z,   v.y*v.y*ic + co,        v.z*v.y*ic - si*v.x,
                   v.x*v.z*ic - si*v.y,   v.y*v.z*ic + si*v.x,    v.z*v.z*ic + co );
}

// iq
float impulse( float k, float x )
{
    float h = k*x;
    return h*exp(1.0-h);
}

float longTailImpulse(float k, float x, float c)
{
    return mix(impulse(k, x), impulse(k, (x+1.0/k) * c), step(1.0/k, x));
}

// A single iteration of Bob Jenkins' One-At-A-Time hashing algorithm.
uint hash( uint x ) {
    x += ( x << 10u );
    x ^= ( x >>  6u );
    x += ( x <<  3u );
    x ^= ( x >> 11u );
    x += ( x << 15u );
    return x;
}

// Construct a float with half-open range [0:1] using low 23 bits.
// All zeroes yields 0.0, all ones yields the next smallest representable value below 1.0.
float floatConstruct( uint m ) {
    const uint ieeeMantissa = 0x007FFFFFu; // binary32 mantissa bitmask
    const uint ieeeOne      = 0x3F800000u; // 1.0 in IEEE binary32

    m &= ieeeMantissa;                     // Keep only mantissa bits (fractional part)
    m |= ieeeOne;                          // Add fractional part to 1.0

    float  f = uintBitsToFloat( m );       // Range [1:2]
    return f - 1.0;                        // Range [0:1]
}

float random(inout uint seed)
{
	seed = hash(seed);
	return floatConstruct(seed);
}

float hash31(vec3 uv) {
    float f = fract(sin(dot(uv, vec3(.09123898, .0231233, .0532234))) * 1e5);
    return f;
}

// ---------------------------------------------------------

struct Intersection
{
    float totalDistance;
    float sdf;
    float density;
    int materialID;
};
    
struct Camera
{
	vec3 origin;
    vec3 direction;
};

// ---------------------------------------------------------

float sdf_simple(vec3 p)
{
    float d = 0.0;
	uint seed = uint(14041956 + int(TIME * .5));
    
    float sides = 8.0;
    float sideAmpl = 1.0 / sides;

    // Side planes
	for(float i = 0.0; i < sides; i++)
	{
        float angle = mix(i, i+1.0, random(seed)) * sideAmpl * PI * 2.0;
        float verticalOffset = random(seed) * 2.0 - 1.0;
        vec3 offset = vec3(cos(angle), verticalOffset * .25, sin(angle));
		
		d = max(d, dot(p - offset * CRYSTAL_SCALE / CRYSTAL_VERTICAL_ANISOTROPY, normalize(offset)));
	}
    
    // Cap planes
	for(float i = 0.0; i < sides; i++)
	{
        float angle = mix(i, i+1.0, random(seed)) * sideAmpl * PI * 2.0;
        float verticalOffset = random(seed) * 2.0 - 1.0;
        vec3 offset = vec3(cos(angle), verticalOffset * 3.0, sin(angle));
		
		d = max(d, dot(p - offset * CRYSTAL_SCALE * CRYSTAL_VERTICAL_ANISOTROPY, normalize(offset)));
	}
    
	return d;
}

float curv_modifier(in vec3 p, in float w)
{
    vec2 e = vec2(-1., 1.) * w;   
    
    float t1 = sdf_simple(p + e.yxx), t2 = sdf_simple(p + e.xxy);
    float t3 = sdf_simple(p + e.xyx), t4 = sdf_simple(p + e.yyy);
    
    return (.25/e.y) * (t1 + t2 + t3 + t4 - 4.0 * sdf_simple(p));
}

vec3 sdfNormal_simple(vec3 p, float epsilon)
{
    vec3 eps = vec3(epsilon, -epsilon, 0.0);
    
	float dX = sdf_simple(p + eps.xzz) - sdf_simple(p + eps.yzz);
	float dY = sdf_simple(p + eps.zxz) - sdf_simple(p + eps.zyz);
	float dZ = sdf_simple(p + eps.zzx) - sdf_simple(p + eps.zzy); 

	return normalize(vec3(dX,dY,dZ));
}

float sdf_modifier(vec3 p)
{
#ifdef NORMAL_CURVATURE_BASIC
    float d = curv_modifier(p, .15) * .2;
#elif defined(NORMAL_CURVATURE_NONE)
    float d = 0.0;
#elif defined(NORMAL_CURVATURE_NORMAL_OFFSET)
    vec3 normal = sdfNormal_simple(p, EPSILON * 4.0);
    float d = curv_modifier(p + normal * .1, .15) * .2;
#elif defined(NORMAL_CURVATURE_SDF_WIDTH)
   	float w = sdf_simple(p);
    vec3 normal = sdfNormal_simple(p, EPSILON * 4.0);
   	float d = curv_modifier(p + normal * .1, .15 + w) * .2; 
#endif
    
    return d * -.95;
}

float sdf_complex(vec3 p)
{
    return sdf_simple(p) + sdf_modifier(p);
}

// https://www.shadertoy.com/view/Xts3WM
float curv(in vec3 p, in float w)
{
    vec2 e = vec2(-1., 1.) * w;
    
    float t1 = sdf_simple(p + e.yxx), t2 = sdf_simple(p + e.xxy);
    float t3 = sdf_simple(p + e.xyx), t4 = sdf_simple(p + e.yyy);
    
    return .25/e.y*(t1 + t2 + t3 + t4 - 4.0 * sdf_simple(p));
}

vec3 sdfNormal(vec3 p, float epsilon)
{
    vec3 eps = vec3(epsilon, -epsilon, 0.0);
    
	float dX = sdf_complex(p + eps.xzz) - sdf_complex(p + eps.yzz);
	float dY = sdf_complex(p + eps.zxz) - sdf_complex(p + eps.zyz);
	float dZ = sdf_complex(p + eps.zzx) - sdf_complex(p + eps.zzy); 

	return normalize(vec3(dX,dY,dZ));
}

int SampleMaterial(vec3 p)
{
    // We only have one material
    return MATERIAL_CRYSTAL;
}

Camera GetCamera(vec2 uv, float zoom)
{
    float dist = 3.0 / zoom;
    float time = TIME;
    
    vec3 target = vec3(0.0, 0.0, 0.0);
    vec3 p = vec3(0.0, 1.5, 0.0) + vec3(cos(time), 0.0, sin(time)) * dist;
        
    vec3 forward = normalize(target - p);
    vec3 left = normalize(cross(forward, vec3(0.0, 1.0, 0.0)));
    vec3 up = normalize(cross(forward, left));

    Camera cam;   
    cam.origin = p;
    cam.direction = normalize(forward + left * uv.x * zoom - up * uv.y * zoom);
        
    return cam;
}

Intersection Raymarch(Camera camera)
{    
    Intersection outData;
    outData.sdf = 0.0;
    outData.materialID = MATERIAL_NONE;
    outData.density = 0.0;
    outData.totalDistance = MIN_DISTANCE;
        
	for(int j = 0; j < MAX_STEPS; j++)
	{
        vec3 p = camera.origin + camera.direction * outData.totalDistance;
		outData.sdf = sdf_simple(p) * .9;
        //outData.density += sdfDensity(p);
        
		if(outData.sdf < EPSILON)
            break;        

		outData.totalDistance += outData.sdf;
        
        if(outData.totalDistance > MAX_DISTANCE)
            break;
	}
    
    if(outData.sdf < EPSILON)
		outData.materialID = SampleMaterial(camera.origin + camera.direction * outData.totalDistance);
    
    return outData;
}

vec3 gradient(float factor)
{
	vec3 a = vec3(0.478, 0.500, 0.500);
	vec3 b = vec3(0.500);
	vec3 c = vec3(0.688, 0.748, 0.748);
	vec3 d = vec3(0.318, 0.588, 0.908);

	return palette(factor, a, b, c, d);
}

vec3 Render(Camera camera, Intersection isect)
{
    if(isect.materialID > 0)
    {        
        vec3 p = camera.origin + camera.direction * isect.totalDistance;
        vec3 normal = sdfNormal(p, EPSILON * 10.0);
        
#ifdef NORMAL_CURVATURE_ADD
        float c = curv(p, .15);
        normal = normalize(normal - vec3(c));
#endif
        
        float diffuse = dot(normal, -camera.direction);
        
#ifdef DIFFUSE_ONLY
        return vec3(diffuse * .5 + .5);
#else
        return gradient(diffuse);
#endif
    }
    
    return vec3(0.0);
}

void main() {



	vec2 uv = (-RENDERSIZE.xy + (gl_FragCoord.xy*2.0)) / RENDERSIZE.y;
        
    Camera camera = GetCamera(uv, .5);
    Intersection isect = Raymarch(camera);    
    vec3 color = Render(camera, isect);
    
	gl_FragColor = vec4(color, 1.0);
}
