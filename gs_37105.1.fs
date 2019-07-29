/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#37105.1"
}
*/


//------------------------------------------------------------------
// LightsShineOnSphere.glsl v2 
//------------------------------------------------------------------
#ifdef GL_ES
precision highp float;
#endif

#extension GL_OES_standard_derivatives : enable


float ratio = RENDERSIZE.x / RENDERSIZE.y;

#define hash(a) fract(sin(a)*12345.0) 
#define snoise(p) ((old_noise(p, 883.0, 971.0) + old_noise(p + 0.5, 113.0, 157.0)) * 0.5)
float old_noise(vec3 x, float c1, float c2) {
    vec3 p = floor(x);
    vec3 f = fract(x);
    f = f*f*(3.0-2.0*f);
    float n = p.x + p.y*c2+ c1*p.z;
    return mix(
        mix(
            mix(hash(n+0.0),hash(n+1.0),f.x),
            mix(hash(n+c2),hash(n+c2+1.0),f.x),
            f.y),
        mix(
            mix(hash(n+c1),hash(n+c1+1.0),f.x),
            mix(hash(n+c1+c2),hash(n+c1+c2+1.0),f.x),
            f.y),
        f.z);
}

struct sRay { vec3 origin; vec3 direction; };
struct sSphere { vec3 position; float radius; };
//------------------------------------------------------------------
// return distance of ray.origin to sphere surface 
// http://www.iquilezles.org/www/articles/simplegpurt/simplegpurt.htm
//------------------------------------------------------------------
float RaySphereIntersection( in sRay ray, in sSphere sphere )
{
    vec3  h = ray.origin - sphere.position;
    float b = dot(ray.direction, h);
    float c = dot(h,h) - sphere.radius*sphere.radius;
    float d = b*b-c;
    if( d < 0.0 ) return -1.0;
    return -b -sqrt(d);
}

//------------------------------------------------------------------
// return distance of ray.origin to sphere surface
// original version of this shader
//------------------------------------------------------------------
float RaySphereIntersection2(in sRay ray, in sSphere sphere)
{
    vec3 h = ray.origin - sphere.position;
    float b = dot(ray.direction, h);
    float c = dot(h,h) - sphere.radius*sphere.radius;
    float d = b * b - c;
    if (d < 0.0) return -1.0;  // no intersection!
    d = sqrt(d);
    float q = (b < 0.0) ? (-b -d) : (-b +d);
    float t0 = q;
    float t1 = c / q;
    if (t0 > t1) 
    {   t0 = t1;   // flip intersection points
        t1 = q;
    }
    if (t1 < 0.0) return -1.0;
    if (t0 < 0.0) return t1;
    return t0; 
}

struct sPlane { vec3 origin; vec3 normal; };
//------------------------------------------------------------------
// return intersection point distance from ray.origin to plane
//------------------------------------------------------------------
float RayPlaneIntersection(in sRay ray, in sPlane plane) 
{
  return dot(plane.origin - ray.origin, plane.normal) 
       / dot(ray.direction, plane.normal);     
}

vec3 getViewDirection(in vec2 uv)
{
	vec2 clip = uv * 2.0 - 1.0;
	clip.x *= ratio;
	return normalize(vec3(0.0, 0.0, -1.0) + vec3(clip.x, clip.y, 0.0) * 0.4);
}

vec3 shadePoint(vec3 n, vec3 fragmentpos, vec3 point, vec3 color)
{
	float dst = distance(fragmentpos, point);
	float attenuation = 1.0 / (dst * dst + 0.01);
	return color * 3.0 * attenuation * max(0.0, dot(n, normalize(point - fragmentpos)));
}

void main( void ) 
{
	vec2 position = gl_FragCoord.xy / RENDERSIZE.xy;

	float sphereradius = 10.0;
	vec3 spherepos = vec3(0.0, 0.0, -40.0);
	sSphere sphere = sSphere(spherepos, sphereradius);

	vec3 camera = vec3(0.0);
	vec3 dir = getViewDirection(position);
	sRay ray = sRay(camera, dir);
	
	float hitdist = RaySphereIntersection(ray, sphere);
	vec3 hitpos = camera + dir * hitdist;
	
	float ballslimit = 4.0;
	vec3 color = vec3(0.0);
	float seed = TIME * 0.04;
	float seed2 = TIME * 0.1;
	float add = hitdist <= 0.0 ? 1.0 : 0.0;
	//hitdist = hitdist + (step(0.0, hitdist) * 1000.0);
	vec3 hitdir = normalize(hitpos - spherepos);
	float aa = smoothstep(0.3, 0.4, hitdir.z);
	for(int i=0; i<22; i++)
	{
		vec3 particlepos = vec3(
			snoise(vec3(seed, -seed, seed *1.113)),
			snoise(vec3(-seed * 0.982, seed, seed *1.113)),
			snoise(vec3(seed, seed, -seed *1.113))
			) * 2.0 - 1.0;
		seed += 13.0;
		vec3 pcolor = vec3(
			snoise(vec3(seed2, -seed2, seed2 *1.113)),
			snoise(vec3(-seed2 * 0.982, seed2, seed2 *1.113)),
			snoise(vec3(seed2, seed2, -seed2 *1.113))
		);
		seed2 += 13.0;
		particlepos = spherepos + normalize(particlepos * 100.0) * (sphereradius + length(particlepos) * ballslimit );
		color += pcolor * 2.0 * (add + step(0.0, distance(camera, spherepos) - distance(camera, particlepos))) 
			 * smoothstep(0.99992, 1.0, max(0.0, dot(-dir, normalize(camera - particlepos))));
		color +=  aa * shadePoint(hitdir, hitpos, particlepos, pcolor);
	}	
	color += step(0.0, hitdist) * aa * max(0.0, dot(normalize(hitpos - spherepos), vec3(1.0, 1.0, -0.7))) * 0.6;
	
	float blueness = snoise(400.0 * vec3(position.x * ratio, position.y, 0.0));
	color += mix(vec3(1.0), vec3(0.6, 0.9, 1.5), blueness) * (1.0 - step(0.0, hitdist) * aa) 
		 * smoothstep(0.75, 0.8, 0.1 * snoise(  8.0 * vec3(position.x * 10.0 * ratio, position.y, TIME * 0.1)) 
			               + 0.9 * snoise(500.0 * vec3(position.x * ratio, position.y, 0.0)));
	gl_FragColor = vec4(color, 1.0 );
}