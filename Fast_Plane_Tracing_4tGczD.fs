/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tGczD by yx.  Raytracing the floor plane instead of marching it - fewer iterations to reach the SDF surface, even for the other objects in the scene.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


#define STEPS 32
#define MAX_DIST 100.

vec2 rotate(vec2 p,float a){return cos(a)*p+sin(a)*vec2(-p.y,p.x);}

vec3 spin(vec3 p)
{
   	p.xy = rotate(p.xy, TIME);
   	p.xz = rotate(p.xz, TIME);
	return p;
}

float sdFloor(vec3 p, float y)
{
    return p.y-y;
}

float sdFloorFast(vec3 p, vec3 d, float y)
{
    float t = (y-p.y)/d.y;
    return t >= 0. ? t * .999 : MAX_DIST;
    
    // the .999 is a shitty hack so that we don't overshoot through the floor (yay floating point)
}

float sdBox(vec3 p, vec3 b)
{
	vec3 d = abs(p) - b;
	return min(max(d.x,max(d.y,d.z)),0.0) + length(max(d,0.0));
}

float scene(vec3 p)
{
    return min(
        sdBox(spin(p),vec3(1.)),
        sdFloor(p,-1.)
    );
}

float sceneFast(vec3 p, vec3 d)
{
    return min(
        sdBox(spin(p),vec3(1.)),
        sdFloorFast(p,d,-1.)
    );
}

void main() {



    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy-.5;
    uv.x *= RENDERSIZE.x/RENDERSIZE.y;
    vec3 cam = vec3(0,0,-5);
    vec3 dir = normalize(vec3(uv,1));
    
    int i;
    vec3 p = cam;
    for(i=0;i<STEPS;++i)
    {
    	float k;
        if (gl_FragCoord.x < (iMouse.y > 0. ? iMouse.x : RENDERSIZE.x * .5))
        	k = scene(p);
        else
            k = sceneFast(p,dir);
        if (k < .001 || k > MAX_DIST)
            break;
    	p += dir * k;
    }
    
    const vec2 o = vec2(.001,0);
    vec3 n = normalize(vec3(
		scene(p+o.xyy)-scene(p-o.xyy),
		scene(p+o.yxy)-scene(p-o.yxy),
		scene(p+o.yyx)-scene(p-o.yyx)
    ));
    
    // alternative normal calculation
   	//n = normalize(cross(dFdy(p),dFdx(p)));
    
    // brighter means fewer iterations
    
    float cost = float(i)/float(STEPS);
	gl_FragColor = vec4(1.-cost);
    //gl_FragColor = vec4(fract(p*4.+.5),1);
	//gl_FragColor *= vec4(n*.5+.5,1);
}
