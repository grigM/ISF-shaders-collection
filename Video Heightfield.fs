/*{
	"CREDIT": "by nikharron",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Distortion Effect"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "MAXHEIGHT",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX":2.0
		},
		{
			"NAME": "OVERDRIVE",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 1.0,
			"MAX": 10.0
		}
	]
}*/

// Based on "video heightfield" "by simesgreen: https://www.shadertoy.com/view/Xss3zr#

const int _Steps = 20;
const vec3 lightDir = vec3(0.577, 0.577, 0.577);

// transforms
vec3 rotateX(vec3 p, float a)
{
    float sa = sin(a);
    float ca = cos(a);
    vec3 r;
    r.x = p.x;
    r.y = ca*p.y - sa*p.z;
    r.z = sa*p.y + ca*p.z;
    return r;
}

vec3 rotateY(vec3 p, float a)
{
    float sa = sin(a);
    float ca = cos(a);
    vec3 r;
    r.x = ca*p.x + sa*p.z;
    r.y = p.y;
    r.z = -sa*p.x + ca*p.z;
    return r;
}

bool
intersectBox(vec3 ro, vec3 rd, vec3 boxmin, vec3 boxmax, out float tnear, out float tfar)
{
	// compute intersection of ray with all six bbox planes
	vec3 invR = 1.0 / rd;
	vec3 tbot = invR * (boxmin - ro);
	vec3 ttop = invR * (boxmax - ro);
	// re-order intersections to find smallest and largest on each axis
	vec3 tmin = min (ttop, tbot);
	vec3 tmax = max (ttop, tbot);
	// find the largest tmin and the smallest tmax
	vec2 t0 = max (tmin.xx, tmin.yz);
	tnear = max (t0.x, t0.y);
	t0 = min (tmax.xx, tmax.yz);
	tfar = min (t0.x, t0.y);
	// check for hit
	bool hit;
	if ((tnear > tfar)) 
		hit = false;
	else
		hit = true;
	return hit;
}

float luminance(vec2 uv)
{
	vec3 c = IMG_NORM_PIXEL(inputImage, uv).rgb;
	return dot(c, vec3(0.33, 0.33, 0.33));
}

vec2 worldToTex(vec3 p)
{
	vec2 uv = p.xz*.5+.5;
	uv.y = 1.0 - uv.y;
	return uv;
}

float heightField(vec3 p)
{
	return luminance(worldToTex(p))*MAXHEIGHT*OVERDRIVE; //FACTOR IS SCALING in Z
}

bool traceHeightField(vec3 ro, vec3 rayStep, out vec3 hitPos)
{
	vec3 p = ro;
	bool hit = false;
	float pH = 0.0;
	vec3 pP = p;
	for(int i=0; i<_Steps; i++) {
		float h = heightField(p);
		if ((p.y < h) && !hit) {
			hit = true;
			//hitPos = p;
			// interpolate based on height
            hitPos = mix(pP, p, (pH - pP.y) / ((p.y - pP.y) - (h - pH)));
		}
		pH = h;
		pP = p;
		p += rayStep;
	}
	return hit;
}

vec3 background(vec3 rd)
{
     return mix(vec3(1.0, 1.0, 1.0), vec3(0.0, 0.5, 1.0), abs(rd.y));
}

#define TWOPI 6.28318530718

void main(void)
{
    vec2 pixel = (gl_FragCoord.xy / RENDERSIZE.xy)*2.0-1.0;

    // compute ray origin and direction
    float asp = 1.0; //RENDERSIZE.x / RENDERSIZE.y;
    vec3 rd = normalize(vec3(asp*pixel.x, pixel.y, -2.0));
    vec3 ro = vec3(0., 0., 2.);
		
	// rotate view
    float ax = TWOPI * 1.75;

    rd = rotateX(rd, ax);
    ro = rotateX(ro, ax);
		
	
	// intersect with bounding box
    bool hit;	
	vec3 boxMin = vec3(-1.0, -.001, -1.0);
	vec3 boxMax = vec3(1.0, MAXHEIGHT, 1.0); //FACTOR IS SCALING Z - 2nd term in vec3
	float tnear, tfar;
	hit = intersectBox(ro, rd, boxMin*OVERDRIVE, boxMax*OVERDRIVE, tnear, tfar);

	tnear -= 0.0001;
	vec3 pnear = ro + rd*tnear;
    vec3 pfar = ro + rd*tfar;
	
    float stepSize = length(pfar - pnear) / float(_Steps);
	
    vec4 col = vec4(0,0,0,0);
    if(hit)
    {
    	// intersect with heightfield
		ro = pnear;
		vec3 hitPos;
		hit = traceHeightField(ro, rd*stepSize, hitPos);
		if (hit) {
			
			vec2 uv = worldToTex(hitPos);
			col = IMG_NORM_PIXEL(inputImage, uv);
	}
     }

    gl_FragColor=vec4(col);
}