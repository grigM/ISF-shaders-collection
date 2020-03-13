/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/WsKXRc by skaplun.  Siri-like animation. It's quite heavy but still....",
    "IMPORTED": {
        "iChannel0": {
            "NAME": "iChannel0",
            "PATH": [
                "793a105653fbdadabdc1325ca08675e1ce48ae5f12e37973829c87bea4be3232.png",
                "793a105653fbdadabdc1325ca08675e1ce48ae5f12e37973829c87bea4be3232_1.png",
                "793a105653fbdadabdc1325ca08675e1ce48ae5f12e37973829c87bea4be3232_2.png",
                "793a105653fbdadabdc1325ca08675e1ce48ae5f12e37973829c87bea4be3232_3.png",
                "793a105653fbdadabdc1325ca08675e1ce48ae5f12e37973829c87bea4be3232_4.png",
                "793a105653fbdadabdc1325ca08675e1ce48ae5f12e37973829c87bea4be3232_5.png"
            ],
            "TYPE": "cube"
        }
    },
    "INPUTS": [
        {
            "NAME": "iMouse",
            "TYPE": "point2D"
        }
    ],
    "PASSES": [
        {
        },
        {
        }
    ]
}

*/


#define MIN_FLOAT 1e-6
#define MAX_FLOAT 1e6

struct Sphere{vec3 origin;float rad;};
struct Ray{ vec3 origin, dir;};
struct HitRecord{ float t; vec3 p;};

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
    vec2 xy = fragCoord - size / 2.0;
    float z = size.y / tan(radians(fieldOfView) / 2.0);
    return normalize(vec3(xy, -z));
}

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
    vec3 f = normalize(center - eye);
    vec3 s = normalize(cross(f, up));
    vec3 u = cross(s, f);
    return mat3(s, u, -f);
}
vec3 hash(vec3 x){
	x = vec3( dot(x,vec3(127.1,311.7, 74.7)),
			  dot(x,vec3(269.5,183.3,246.1)),
			  dot(x,vec3(113.5,271.9,124.6)));

	return fract(sin(x)*43758.5453123);
}

bool sphere_hit(const in Sphere sphere, const in Ray inray, float t_min, float t_max, inout HitRecord rec) {
    vec3 oc = inray.origin - sphere.origin;
    float a = dot(inray.dir, inray.dir);
    float b = dot(oc, inray.dir);
    float c = dot(oc, oc) - sphere.rad*sphere.rad;
    float discriminant = b*b - a*c;
    if (discriminant > 0.) {
        float temp = (-b - sqrt(discriminant))/a;
        if (temp < t_max && temp > t_min) {
            rec.t = temp;
            rec.p = inray.origin + inray.dir * rec.t;
            return true;
        }
    }
    return false;
}

float noise( in vec3 p ){
    vec3 i = floor( p );
    vec3 f = fract( p );
	
	vec3 u = f*f*(3.0-2.0*f);

    return mix( mix( mix( dot( hash( i + vec3(0.0,0.0,0.0) ), f - vec3(0.0,0.0,0.0) ), 
                          dot( hash( i + vec3(1.0,0.0,0.0) ), f - vec3(1.0,0.0,0.0) ), u.x),
                     mix( dot( hash( i + vec3(0.0,1.0,0.0) ), f - vec3(0.0,1.0,0.0) ), 
                          dot( hash( i + vec3(1.0,1.0,0.0) ), f - vec3(1.0,1.0,0.0) ), u.x), u.y),
                mix( mix( dot( hash( i + vec3(0.0,0.0,1.0) ), f - vec3(0.0,0.0,1.0) ), 
                          dot( hash( i + vec3(1.0,0.0,1.0) ), f - vec3(1.0,0.0,1.0) ), u.x),
                     mix( dot( hash( i + vec3(0.0,1.0,1.0) ), f - vec3(0.0,1.0,1.0) ), 
                          dot( hash( i + vec3(1.0,1.0,1.0) ), f - vec3(1.0,1.0,1.0) ), u.x), u.y), u.z );
}

vec3 hsv2rgb(vec3 c) {
  // Íñigo Quílez
  // https://www.shadertoy.com/view/MsS3Wc
  vec3 rgb = clamp(abs(mod(c.x*6.+vec3(0.,4.,2.),6.)-3.)-1.,0.,1.);
  rgb = rgb * rgb * (3. - 2. * rgb);
  return c.z * mix(vec3(1.), rgb, c.y);
}

float fbm1x(float x, float time){
	float amplitude = 1.;
    float frequency = 1.;
    float y = sin(x * frequency);
    float t = 0.01*(-time * 130.0);
    y += sin(x*frequency*2.1 + t)*4.5;
    y += sin(x*frequency*1.72 + t*1.121)*4.0;
    y += sin(x*frequency*2.221 + t*0.437)*5.0;
    y += sin(x*frequency*3.1122+ t*4.269)*2.5;
    y *= amplitude*0.06;
    return y;
}
#define MAX_MARCHING_STEPS 128

float map(vec3 p){
    return noise(p + vec3(0., TIME, 0.));
}

vec3 render(in vec2 fragCoord){
	vec3 color = vec3(0.);
    float a = (RENDERSIZE.x - iMouse.x) * .05;
    vec3 eye = vec3(4.5 * sin(a), 3., 4.5 * cos(a));
    vec3 viewDir = rayDirection(45., RENDERSIZE.xy, fragCoord);
    vec3 worldDir = viewMatrix(eye, vec3(0., 0., 0.), vec3(0., 1., 0.)) * viewDir;
	
    Ray camRay = Ray(eye, worldDir);
    HitRecord rec;
    if(sphere_hit(Sphere(vec3(0.), 1.00001), camRay, MIN_FLOAT, MAX_FLOAT, rec)){
		vec3 sp;
        float t=rec.t, layers=0., d, aD;
        for(int i=0; i<MAX_MARCHING_STEPS; i++)	{
            sp = eye + worldDir * t;
            d = map(sp);
            if(abs(pow(d, .5) - min(pow(sp.y, .75), (.75 - pow(length(sp.xz), 64.)))) <= .05)
            	color += hsv2rgb(vec3(fbm1x(d, TIME * .1 + 100.) * 3.1415, 1., 1.)) * .015;
            t += .01;
        }
        
        vec3 nrm = normalize(rec.p);
        float dt = pow(1. - abs(dot(worldDir, nrm)), 2.);
        eye = rec.p;
        worldDir = -nrm;
        t = .1;
        vec3 fresCol = vec3(0.);
        for(int i=0; i<MAX_MARCHING_STEPS; i++)	{
            sp = eye + worldDir * t;
            d = map(sp);
            if(abs(pow(d, .5) - min(pow(sp.y, .75), (.75 - pow(length(sp.xz), 64.)))) <= .05)
            	fresCol += .05 * hsv2rgb(vec3(fbm1x(d, TIME * .1 + 100.) * .5415, 1., 1.));
            t += .0075;
        }
        color += fresCol * dt;
        color += pow(textureCube(iChannel0,reflect(worldDir, nrm)).rgb, vec3(4.)) * .1;
    }
    return color;
}

#define AA 1
void main() {
	if (PASSINDEX == 0)	{
	}
	else if (PASSINDEX == 1)	{
	    gl_FragColor -= gl_FragColor;
	    for(int y = 0; y < AA; ++y)
	        for(int x = 0; x < AA; ++x){
	            gl_FragColor.rgb += clamp(render(gl_FragCoord.xy + vec2(x, y) / float(AA)), 0., 1.);
	        }
	    
	    gl_FragColor.rgb /= float(AA * AA);
	}

}
