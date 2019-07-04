/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#30962.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


struct Ray
{
	vec3 direction;
	vec3 orign;
};

struct AABB
{
	vec3 low;
	vec3 high;
};
struct Intersection
{
	float TIME;
	vec3 normal;
	float aabb_edge;
};

vec3 envmap(vec3 dir)
{
	vec3 ret = vec3(0);
//	ret += vec3(1,1,3) * pow(clamp( dot(normalize(vec3(1,0,0)), dir), 0.0, 1.0 ), 3.0);//
//	ret += vec3(5,1,1) * pow(clamp( dot(normalize(vec3(1,3,5)), dir), 0.0, 1.0 ), 3.0);//
//	ret += vec3(1,0,0.2) * pow(clamp( dot(normalize(vec3(1,0,0)), dir), 0.0, 1.0 ), 3.0);
	return dir;
}

bool intersectionAABB(AABB aabb, Ray r, inout Intersection isect)
{
	float tmin = -10000.0;
	float tmax =  10000.0;
	
	int n;
	for(int i=0;i<3;i++)
	{
		float tx1 = (aabb.low[i] - r.orign[i])/r.direction[i];
		float tx2 = (aabb.high[i] - r.orign[i])/r.direction[i];
		
		float tminold = tmin;
		float tmaxold = tmax;
		tmin = max(tmin, min(tx1, tx2));
		tmax = min(tmax, max(tx1, tx2));
		
		if(tmin!=tminold)
		{
			n = i;
		}		
	}
	
	if(tmax>=tmin && tmin < isect.TIME)
	{
		isect.TIME = tmin;
		vec3 lp = r.orign + r.direction * tmin;
		vec3 cen = (aabb.low + aabb.high)/2.0;
		vec3 hw = -(aabb.low - aabb.high)/2.0;
		vec3 s = sign(lp-cen);
		
		if(n==0)
		{
			isect.normal = s * vec3(1,0,0);
		}
		else if(n==1)
		{
			isect.normal = s * vec3(0,1,0);
		}
		else if(n==2)
		{
			isect.normal = s * vec3(0,0,1);
		}
		
		isect.aabb_edge = length((lp-cen)/hw);
		
		return true;
	}	
	else
	{
		return false;
	}
 

}

mat3 rotation(vec3 axis, float angle)
{
    axis = normalize(axis);
    float s = sin(angle);
    float c = cos(angle);
    float oc = 1.0 - c;
    
    return mat3(oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s, 
                oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s, 
                oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c     );
}
bool raycast(Ray r, float t, inout Intersection isect)
{
	bool ret = false;
		

	for(int i=-2;i<=2;i++) for(int j=-2;j<=2;j++) for(int k=-2;k<=2;k++)
	{
		vec3 noise = vec3(0);
		vec3 ofs = vec3(i,j,k) * 0.1;
		AABB aabb;
		aabb.low = vec3(-0.03) + ofs;
		aabb.high = vec3(0.03) + ofs;
		
	
		if(intersectionAABB(aabb, r, isect)){ ret = true; }
	}
	return ret;
}




float fresnel(float costh, float f0)
{
	return f0 + (1.0 - f0) * (1.0 - costh)*(1.0 - costh)*(1.0 - costh)*(1.0 - costh)*(1.0 - costh);
}

void main( void ) {

	Intersection isect;
	isect.TIME = 100000.0;
	
	vec2 tx = ( gl_FragCoord.xy / RENDERSIZE.xy )*2.0-vec2(1.0);
	vec2 noise = 0.01*vec2(sin(tx.x*20.0+TIME*15.0), sin(tx.y*6.0+TIME*10.0));
	
	float asp = RENDERSIZE.y / RENDERSIZE.x;
	Ray r;
	r.orign = vec3(0, 0, -4.0 + 0.0);
	r.direction = normalize(vec3(vec2(1, asp)*tx, 1.0) - r.orign);
	
	float ts = TIME - tx.y*0.5;
	
	mat3 rot = rotation(normalize(vec3(1,sin(ts),0)), ts);
	r.orign = rot * r.orign;
	r.direction = rot * r.direction;
	
	vec3 outcol = envmap(r.direction);
	
	bool hit = raycast(r, TIME, isect);
	
	if(hit)
	{
		float fres = fresnel(dot(-r.direction, isect.normal), 0.3);
		vec3 rfl = reflect(-r.direction, isect.normal);
		vec3 rfr = refract(-r.direction, isect.normal, 1.0/1.33);
		outcol = vec3(pow(isect.aabb_edge/1.5, 5.0)) +  (fres * envmap(rfl)  + (1.0-fres) * envmap(rfr)) ;//vec3(1,0,0);
	}
	//outcol = vec3(noise,1);
	
	gl_FragColor = vec4(fwidth(outcol.x),fwidth(outcol.y),fwidth(outcol.z),1);
}