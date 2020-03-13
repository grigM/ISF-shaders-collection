/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel1",
      "PATH" : "fb918796edc3d2221218db0811e240e72e340350008338b0c07a52bd353666a6.jpg"
    },
    {
      "NAME" : "iChannel0",
      "PATH" : [
        "550a8cce1bf403869fde66dddf6028dd171f1852f4a704a465e1b80d23955663.png",
        "550a8cce1bf403869fde66dddf6028dd171f1852f4a704a465e1b80d23955663_1.png",
        "550a8cce1bf403869fde66dddf6028dd171f1852f4a704a465e1b80d23955663_2.png",
        "550a8cce1bf403869fde66dddf6028dd171f1852f4a704a465e1b80d23955663_3.png",
        "550a8cce1bf403869fde66dddf6028dd171f1852f4a704a465e1b80d23955663_4.png",
        "550a8cce1bf403869fde66dddf6028dd171f1852f4a704a465e1b80d23955663_5.png"
      ],
      "TYPE" : "cube"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llSBRD by mmerchante.  Currently building a soulstone, but thought this crystal looks nice enough :)",
  "INPUTS" : [

  ]
}
*/


// Uncomment these defines to switch implementation

// BASIC CURVATURE: We only add the curvature to the sdf when evaluating the normal
#define NORMAL_CURVATURE_BASIC

// How vertical/horizontal the crystal is :)
#define CRYSTAL_SCALE 1.0
#define CRYSTAL_VERTICAL_ANISOTROPY 1.3

// ---------------------------------------------------------
#define saturate(x) clamp(x, 0.0, 1.0)
#define PI 3.14159265
#define TAU (2*PI)
#define PHI (sqrt(5)*0.5 + 0.5)

#define MAX_STEPS 50
#define MAX_STEPS_F float(MAX_STEPS)

#define FIXED_STEP_SIZE .05

#define MAX_DISTANCE 15.0
#define MIN_DISTANCE .5
#define EPSILON .01
#define EPSILON_NORMAL .1

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
mat3x3 rotationAxisAngle( const vec3 v, float a )
{
    float si = sin( a );
    float co = cos( a );
    float ic = 1.0f - co;

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

vec2 csqr( vec2 a )  { return vec2( a.x*a.x - a.y*a.y, 2.*a.x*a.y  ); }

// ---------------------------------------------------------

struct Intersection
{
    float totalDistance;
    float mediumDistance;
    float sdf;
    float density;
    int materialID;
};
    
struct Camera
{
	vec3 origin;
    vec3 direction;
    vec3 left;
    vec3 up;
};
    
// ---------------------------------------------------------

float density(vec3 p)
{
    vec3 p0 = p;
    vec3 pp = p + mod(TIME, 2.0) * .35;
    p *= .3;
    float res = 0.0;
    
    // credit to guil for this hybrid fractal:
    // https://www.shadertoy.com/view/MtX3Ws
    // Anything can work here, the idea is to warp the fracture cuts
	for (int i = 0; i < 4; ++i) 
    {
        p = .7 * abs(p) / dot(p,p) - .95;
        p.yz = csqr(p.yz);
        p = p.zxy;
	}    
   
    p = pp + p * .5;
    
    float d = 0.0;
	uint seed = uint(14041956 + int(TIME * .5));
    
    // The fractal warping now distorts the planar cuts in meaningful ways ;)
	for(int i = 0; i < 6; ++i)
	{
        // Folding
		p.yxz = clamp(p, -1.0, 1.0) * 2.0 - p;
        
        vec3 axis = normalize(vec3(random(seed), random(seed) * 2.0, random(seed)) * 2.0 - vec3(1.0));
        vec3 offset = vec3(0.0, random(seed) * 2.0 - 1.0, 0.0);
		
        float proj = dot(p - offset, axis);
		d += smoothstep(.1, .0, abs(proj));
	}
    
    d = d * .5 + saturate(1.0 - length(p0 * (1.0 + sin(TIME * 2.0) * .5))) * (.75 + d * .25);
    
	return d * d + .05;
}

float sdf_simple(vec3 p)
{
    float d = 0.0;
	uint seed = uint(14041956 + int(TIME * .5));
    
    float sides = 6.0;
    float sideAmpl = PI * 2.0 / sides;
    
    // Side planes
	for(float i = 0.0; i < sides; i++)
	{
        float angle = mix(i, i+1.0, random(seed)) * sideAmpl;
        float verticalOffset = 0.0; //random(seed) * 2.0 - 1.0;
        vec3 offset = vec3(cos(angle), verticalOffset * .25, sin(angle));
        vec3 axis = normalize(offset);
        offset = offset * CRYSTAL_SCALE / CRYSTAL_VERTICAL_ANISOTROPY;
		
		d = max(d, dot(p - offset, axis));
	}

    vec3 offset = vec3(0.0, 2.0, 0.0);
    
    // Cap planes
	for(float i = 0.0; i < sides; i++)
	{
        float angle = mix(i, i+1.0, random(seed)) * sideAmpl;
        vec3 axis = normalize(vec3(cos(angle), .5 + random(seed), sin(angle)));
		
        // UP
		d = max(d, dot(p - offset * CRYSTAL_SCALE * CRYSTAL_VERTICAL_ANISOTROPY, axis));
        
        // DOWN
        d = max(d, dot(p + offset * CRYSTAL_SCALE * CRYSTAL_VERTICAL_ANISOTROPY, -axis));
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

float sdf_modifier(vec3 p)
{
    return 0.0;// -curv_modifier(p, .15) * .1;
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
    
    vec3 target = vec3(0.0, sin(TIME * 2.0) * .25, 0.0);
    vec3 p = vec3(0.0, 1.5, 0.0) + vec3(cos(time), 0.0, sin(time)) * dist;
        
    vec3 forward = normalize(target - p);
    vec3 left = normalize(cross(forward, vec3(0.0, 1.0, 0.0)));
    vec3 up = normalize(cross(forward, left));

    Camera cam;   
    cam.origin = p;
    cam.direction = normalize(forward + left * uv.x * zoom - up * uv.y * zoom);
    cam.up = up;
    cam.left = left;
        
    return cam;
}

Intersection Raymarch(Camera camera)
{    
    Intersection outData;
    outData.sdf = 0.0;
    outData.materialID = MATERIAL_NONE;
    outData.density = 0.0;
    outData.totalDistance = MIN_DISTANCE;
        
	for(int j = 0; j < MAX_STEPS; ++j)
	{
        vec3 p = camera.origin + camera.direction * outData.totalDistance;
		outData.sdf = sdf_simple(p) * .9;
        //outData.density += sdfDensity(p);
        
		outData.totalDistance += outData.sdf;
        
		if(outData.sdf < EPSILON || outData.totalDistance > MAX_DISTANCE)
            break;
	}
    
    
    // INNER MEDIUM
    if(outData.sdf < EPSILON)
    {
        float t = FIXED_STEP_SIZE;
        float d = 0.0;
        
        vec3 hitPosition = camera.origin + camera.direction * (outData.totalDistance + FIXED_STEP_SIZE);
        
        vec3 normal = sdfNormal(hitPosition, 1.0);
        vec3 refr = refract(camera.direction, normal, .9);
        
        for(int i = 0; i < 50; ++i)
        {            
            vec3 p = hitPosition + refr * t;
            
            if(sdf_simple(p) > EPSILON)
                break;
            
            d += density(p);            
            t += FIXED_STEP_SIZE;
        }
        
        outData.density = d;
		outData.materialID = SampleMaterial(camera.origin + camera.direction * outData.totalDistance);
    	outData.totalDistance *= .99;
        outData.mediumDistance = t;
    }
    
    return outData;
}

vec3 gradient(float factor)
{
	vec3 a = vec3(0.478, 0.4500, 0.500);
	vec3 b = vec3(0.500);
	vec3 c = vec3(0.1688, 0.748, 0.1748);
	vec3 d = vec3(0.1318, 0.388, 0.1908);

	return palette(factor, a, b, c, d);
}

vec3 triplanar(vec3 P, vec3 N)
{   
    vec3 Nb = abs(N);
    
    float b = (Nb.x + Nb.y + Nb.z);
    Nb /= vec3(b);
    
    vec3 c0 = IMG_NORM_PIXEL(iChannel1,mod(P.xy,1.0)).rgb * Nb.z;
    vec3 c1 = IMG_NORM_PIXEL(iChannel1,mod(P.yz,1.0)).rgb * Nb.x;
    vec3 c2 = IMG_NORM_PIXEL(iChannel1,mod(P.xz,1.0)).rgb * Nb.y;
    
    return c0 + c1 + c2;
}

vec3 Render(Camera camera, Intersection isect, vec2 uv)
{
    vec3 p = camera.origin + camera.direction * isect.totalDistance;
    
    if(isect.materialID > 0)
    {        
        vec3 lPos = camera.origin - camera.left * 2.0 + camera.up * 2.0;
        vec3 normal = sdfNormal(p, EPSILON_NORMAL);
        vec3 toLight = normalize(lPos - p);
        
        vec3 tx = triplanar(p * .85 - p.zzz * .3, normal);
        float c = curv(p, .1 + tx.r * .85);        
        normal = normalize(normal - vec3(c * .3) + (tx * .25 - .125));
        
        float rim = pow(smoothstep(0.0, 1.0, 1.0 - dot(normal, -camera.direction)), 7.0);
        vec3 H = normalize(toLight - camera.direction);        
        float specular = pow(max(0.0, dot(H, normal)), tx.r * 5.0 + c * 25.0);        
        
        vec3 R = reflect(camera.direction, normal);
        vec3 refl = textureCube(iChannel0,R).rgb ;
                
        vec3 glow = mix(vec3(1.0, .15, .15), vec3(1.0, .45, .15), (isect.density) * .05) * (isect.density) * .04;        
        glow *= smoothstep(.5, 1.0, c) * 1.5 + 1.0;
        
        // Fake transmission
        glow *= 1.0 + pow(exp(-isect.mediumDistance), 2.0) * 4.0;
        
        return (refl + specular) * vec3(.15, .1, .1) * rim + rim * c * .15 * vec3(.1, .4, .8) + glow;
    }
    
    float vignette = 1.0 - pow(length(uv + hash31(p) * .2) / 2., 2.0);
    return vec3(.15, .025, .1) * vignette * vignette * .25;
}

void main() {



	vec2 uv = (-RENDERSIZE.xy + (gl_FragCoord.xy*2.0)) / RENDERSIZE.y;
        
    Camera camera = GetCamera(uv, .5);
    Intersection isect = Raymarch(camera);    
    vec3 color = Render(camera, isect, uv);
        
    uv.y += sin(TIME * 2.0) * .1; // synced to cam position, super fake
 	vec3 glowColor = vec3(1.0, .7, .15);
    uv *= .7;
    vec3 fx = glowColor * pow(saturate(1.0 - length(uv * vec2(.75, .9))), 2.0);
    fx += glowColor * pow(saturate(1.0 - length(uv * vec2(.5, 1.0))), 2.0);
    fx += glowColor * pow(saturate(1.0 - length(uv * vec2(.25, 7.0))), 2.0) * .25;
    fx += glowColor * pow(saturate(1.0 - length(uv * vec2(.1, 7.0))), 2.0) * .15;
    
    float intensity = pow(IMG_NORM_PIXEL(iChannel1,mod(vec2(TIME * .03),1.0)).r, 2.0);
    color += fx * fx * fx * intensity * .2;
    
	gl_FragColor = vec4(color, 1.0);
}
