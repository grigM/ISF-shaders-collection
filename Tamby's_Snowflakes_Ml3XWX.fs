/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "3d",
    "bokeh",
    "star",
    "falling",
    "snow",
    "snowflakes",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Ml3XWX by TambakoJaguar.  A 3D, not really realistic but artistic presentation of falling snow... Please tell me what you think! :)\nUse the mouse to rotate around.\nI know if could be optimized to be faster...",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


/*
"Tamby's Snowflakes" by Emmanuel Keller aka Tambako - December 2016
License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
Contact: tamby@tambako.ch
*/

#define pi 3.14159265359

// Switches, you can play with them!
#define diffraction
#define sin_movement
#define star_flakes

// Campera parameters
vec3 campos;
vec3 camtarget = vec3(0., 0., 0.);
vec3 camdir = vec3(0., 0., 0.);
float fov = 3.8;

const vec3 skyColor = vec3(0.7, 0.73, 0.78);
const float fogdens = 0.08;

// Snow parameters
const vec3 snowColor = vec3(0.9, 0.95, 1.0);
const int nbFlakes = 1000;
const float flakeGlobalIntensity = 1.8;
const vec3 flakedomain = vec3(10., 7., 10.);
const float flakeMinSpeed = 1.7;
const float flakeMaxSpeed = 4.2;
const float flakeMinSinVariation = 0.02;
const float flakeMaxSinVariation = 0.07;
const float flakeMinFreq = 5.;
const float flakeMaxFreq = 12.;
const vec2 flakeWindFact = vec2(0.45, 0.08);

// Some parameters of the star of orbs
const float starNbBranches = 6.;
const float starPow = 1.5;
const float starStrength = 1.2;

vec2 rotateVec(vec2 vect, float angle)
{
    vec2 rv;
    rv.x = vect.x*cos(angle) - vect.y*sin(angle);
    rv.y = vect.x*sin(angle) + vect.y*cos(angle);
    return rv;
}

// 1D hash function
float hash(float n)
{
    return fract(sin(n)*753.5453123);
}

// From https://www.shadertoy.com/view/4sfGzS
float noise(vec3 x)
{
    //x.x = mod(x.x, 0.4);
    vec3 p = floor(x);
    vec3 f = fract(x);
    f = f*f*(3.0-2.0*f);
	
    float n = p.x + p.y*157.0 + 113.0*p.z;
    return mix(mix(mix(hash(n+  0.0), hash(n+  1.0),f.x),
                   mix(hash(n+157.0), hash(n+158.0),f.x),f.y),
               mix(mix(hash(n+113.0), hash(n+114.0),f.x),
                   mix(hash(n+270.0), hash(n+271.0),f.x),f.y),f.z);
}

vec3 colorRamp3(vec3 col1, vec3 col2, vec3 col3, float v)
{
   return mix(mix(col1, col2, smoothstep(0.0, 0.5, v)), col3, smoothstep(0.5, 1.0, v));   
}

// Gets the color of the sky
vec3 sky_color(vec3 ray)
{ 
    return skyColor*(1. + 0.35*ray.y);
}

float rand(float min, float max, float seed)
{
    return min + (max - min)*hash(seed);
}

vec3 getFlakePosition(int flakeNr, float t)
{
    float fn = float(flakeNr);
    float s = rand(flakeMinSpeed, flakeMaxSpeed, fn*348. + 173.);
    float posY = mod(-(t + 15.*hash(fn*1613. + 1354.))*s, flakedomain.y*2.) - flakedomain.y;
    float posX = rand(-flakedomain.x, flakedomain.x, fn*743. + 514.) + posY*flakeWindFact.x;
    float posZ = rand(-flakedomain.z, flakedomain.z, fn*284. + 483.) + posY*flakeWindFact.y;

    #ifdef sin_movement
    float sinvar = rand(flakeMinSinVariation, flakeMaxSinVariation, fn*842. + 951.);
    float sinfreq = rand(flakeMinFreq, flakeMaxFreq, fn*348. + 173.);
    float dd = hash(fn*235. + 934.);
    posX+= sinvar*sin(t*sinfreq)*dd;
    posZ+= sinvar*sin(t*sinfreq)*sqrt(1. - dd*dd);
    #endif
    
    vec3 pos = vec3(posX, posY, posZ);
    return pos;
}

float nppow(float x, float p)
{
    return sign(x)*pow(abs(x), p);   
}

float getSnowProfile(float val, float dist, vec3 fpos, vec3 ray, int flakeNr)
{
    float val2 = -log(1. - val);
    
    #ifdef star_flakes
    // Complicated stuff to calculate the star shape of the snow flakes by making a 3D to 2D projection
    // From: http://stackoverflow.com/questions/23472048/projecting-3d-points-to-2d-plane
    if (dist<1.2)
    {
        vec3 v3 = (fpos - campos) - dot((fpos - campos), ray) * ray;
        vec3 vx = vec3(1., 0., 0.);
        vx.xy = rotateVec(vx.xy, 2.*float(flakeNr)*152.5 + TIME*0.4);
        vx = normalize(vx - dot(vx, ray)*ray);
        vec3 vy = vec3(ray.y*vx.z - ray.z*vx.y, ray.z*vx.x - ray.x*vx.z, ray.x*vx.y - ray.y*vx.x);

        float a = atan(dot(v3, vx)/dot(v3, vy));

        float spp = 1. + starStrength*nppow(sin(a*starNbBranches), starPow);
        val2+= 1.3*spp*pow(smoothstep(1.6, 0.1, dist), 2.0);
    }
    #endif  
    
    float delta = 1.5 - 0.9/pow(dist + 1., 0.3);
    float midpoint = 10./pow(dist + 0.1, 0.3);
    float pr = smoothstep(midpoint - delta*.5, midpoint + delta*.5, val2);
    
    float d = 1. - pow(abs(1. - 2.*pr), 2.);
    float f = 1.3/(pow(dist + .8, 2.5));
    
    #ifdef diffraction
    if (val2<8.)
       pr+= 32.*pow(f, 1.5)*max(0., dist - 2.)*d*(0.5 + sin(val2*230./(3.8 + dist) - midpoint*90.)*0.5);
    #endif
    
    return pr*f*flakeGlobalIntensity;
}

vec3 getFlakes(vec3 ray)
{
	vec3 rc1 = vec3(0.);
    vec3 rc2 = vec3(0.);
    float lintensity;
    vec3 fpos;
    float lp;
    
    for (int l=0; l<nbFlakes; l++)
    {
        fpos = getFlakePosition(l, TIME);
        
        float val = max(0.0, dot(ray, normalize(fpos - campos)));
        if (val>0.996)
        {
            float dist1 = distance(camtarget, fpos);
            float dist2 = distance(campos, fpos);
            float dist = max(5.2*pow(dist1 / dist2, 1.7), 0.32);
            lp = getSnowProfile(val, dist, fpos, ray, l);

            // Fog
            lp*= clamp(exp(-pow(fogdens*dist2, 2.)), 0., 1.);
        
            // Flakes appear progressively in the domain along the y axis
            lp*= smoothstep(-flakedomain.y, -flakedomain.y*0.75, fpos.y);
            lp*= smoothstep(flakedomain.y, flakedomain.y*0.75, fpos.y);

            rc1+= clamp(normalize(mix(snowColor, vec3(1.), 0.55*lp))*lp, 0., 1.);
            rc2 = max(rc2, clamp(normalize(mix(snowColor, vec3(1.), 0.55*lp))*lp, 0., 1.));
        }
        else
           lp = 0.;
    }
    return mix(rc1, rc2, 0.7);
}

// From https://www.shadertoy.com/view/lsSXzD, modified
vec3 GetCameraRayDir(vec2 vWindow, vec3 vCameraDir, float fov)
{
	vec3 vForward = normalize(vCameraDir);
	vec3 vRight = normalize(cross(vec3(0.0, 1.0, 0.0), vForward));
	vec3 vUp = normalize(cross(vForward, vRight));
    
	vec3 vDir = normalize(vWindow.x * vRight + vWindow.y * vUp + vForward * fov);

	return vDir;
}

// Sets the position of the camera with the mouse and calculates its direction
const float axm = 4.;
const float aym = 1.5;
void setCamera()
{
   vec2 iMouse2;
   if (iMouse.x==0. && iMouse.y==0.)
      iMouse2 = vec2(0.5, 0.5);
   else
      iMouse2 = iMouse.xy/RENDERSIZE.xy;
   
   campos = vec3(8.5, 0., 0.);
   campos.xy = rotateVec(campos.xy, -iMouse2.y*aym + aym*0.5);
   campos.yz = rotateVec(campos.yz, -iMouse2.y*aym + aym*0.5);
   campos.xz = rotateVec(campos.xz, -iMouse2.x*axm);

   camtarget = vec3(0.);
   camdir = camtarget - campos;   
}

void main() {

   

    setCamera();
    
  	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy; 
  	uv = uv*2.0 - 1.0;
  	uv.x*= RENDERSIZE.x / RENDERSIZE.y;
  	vec3 ray = GetCameraRayDir(uv, camdir, fov);
    
    vec3 col = sky_color(ray);
    col+= getFlakes(ray);
  	gl_FragColor = vec4(col, 1.0);
}
