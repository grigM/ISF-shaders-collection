/*{
	"CREDIT": "by mojovideotech",
  "CATEGORIES": [
    "noise",
    "refraction",
    "lens",
    "perlin",
    "fresnel",
    "magnifying",
    "Automatically Converted"
  ],
  "DESCRIPTION": "Automatically converted.  A shiny (spherical) lens which is magnifying a moving backdrop of Perlin noise.",
  "IMPORTED": [
    
  ],
  "INPUTS": [
         {
            "NAME": "ambientAmount",
            "TYPE": "float",
           "DEFAULT": 0.1,
            "MIN": 0.0,
            "MAX": 0.5
      },
      {
            "NAME": "specularAmount",
            "TYPE": "float",
           "DEFAULT": 0.0,
            "MIN": 0.0,
            "MAX": 0.5
      },
       {
            "NAME": "refractionIn",
            "TYPE": "float",
           "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0
        },
         {
            "NAME": "refractionOut",
            "TYPE": "float",
           "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0
        },
         {
            "NAME": "shininess",
            "TYPE": "float",
           "DEFAULT": 500,
            "MIN": 50.0,
            "MAX": 900
        },
          {
            "NAME": "rate",
            "TYPE": "float",
           "DEFAULT": 0.1,
            "MIN": 0.05,
            "MAX": 1.5
        }
  ]
}
*/


// MagnifyingMarble by mojovideotech
// copied from :
// https://www.shadertoy.com/view/ldfSDN

#define M_PI 3.1415926535

float TT = TIME * rate;

//Light setup
vec3 light = vec3(5.0, 5.0, 20.0);

//Object setup
vec4 sph1 = vec4( 0.0, 0.0, 0.0, 1.0);
//Functions 

vec2 iSphere(in vec3 ro, in vec3 rd, in vec4 sph)
{
	//sphere at origin has equation |xyz| = r
	//sp |xyz|^2 = r^2.
	//Since |xyz| = ro + t*rd (where t is the parameter to move along the ray),
	//we have ro^2 + 2*ro*rd*t + t^2 - r2. This is a quadratic equation, so:
	vec3 oc = ro - sph.xyz; //distance ray origin - sphere center
	
	float b = dot(oc, rd);
	float c = dot(oc, oc) - sph.w * sph.w; //sph.w is radius
	float h = b*b - c; //Commonly known as delta. The term a is 1 so is not included.
	
	vec2 t;
	if(h < 0.0) 
		t = vec2(-1.0);
	else  {
		float sqrtH = sqrt(h);
		t.x = (-b - sqrtH); //Again a = 1.
		t.y = (-b + sqrtH);
	}
	return t;
}

//Get sphere normal.
vec3 nSphere(in vec3 pos, in vec4 sph )
{
	return (pos - sph.xyz)/sph.w;
}

float intersect(in vec3 ro, in vec3 rd, out vec2 resT)
{
	resT = vec2(1000.0);
	float id = -1.0;
	vec2 tsph = iSphere(ro, rd, sph1); //Intersect with a sphere.
	
	if(tsph.x > 0.0 || tsph.y > 0.0)
	{
		id = 1.0;
		resT = tsph;
	}
	return id;
}

//
// GLSL textureless classic 2D noise "cnoise",
// with an RSL-style periodic variant "pnoise".
// Author:  Stefan Gustavson (stefan.gustavson@liu.se)
// Version: 2011-08-22
//
// Many thanks to Ian McEwan of Ashima Arts for the
// ideas for permutation and gradient selection.
//
// Copyright (c) 2011 Stefan Gustavson. All rights reserved.
// Distributed under the MIT license. See LICENSE file.
// https://github.com/ashima/webgl-noise
//

vec4 mod289(vec4 x)
{
  return x - floor(x * (1.0 / 289.0)) * 289.0;
}

vec4 permute(vec4 x)
{
  return mod289(((x*34.0)+1.0)*x);
}

vec4 taylorInvSqrt(vec4 r)
{
  return 1.79284291400159 - 0.85373472095314 * r;
}

vec2 fade(vec2 t) {
  return t*t*t*(t*(t*6.0-15.0)+10.0);
}

// Classic Perlin noise
float cnoise(vec2 P)
{
  vec4 Pi = floor(P.xyxy) + vec4(0.0, 0.0, 1.0, 1.0);
  vec4 Pf = fract(P.xyxy) - vec4(0.0, 0.0, 1.0, 1.0);
  Pi = mod289(Pi); // To avoid truncation effects in permutation
  vec4 ix = Pi.xzxz;
  vec4 iy = Pi.yyww;
  vec4 fx = Pf.xzxz;
  vec4 fy = Pf.yyww;

  vec4 i = permute(permute(ix) + iy);

  vec4 gx = fract(i * (1.0 / 41.0)) * 2.0 - 1.0 ;
  vec4 gy = abs(gx) - 0.5 ;
  vec4 tx = floor(gx + 0.5);
  gx = gx - tx;

  vec2 g00 = vec2(gx.x,gy.x);
  vec2 g10 = vec2(gx.y,gy.y);
  vec2 g01 = vec2(gx.z,gy.z);
  vec2 g11 = vec2(gx.w,gy.w);

  vec4 norm = taylorInvSqrt(vec4(dot(g00, g00), dot(g01, g01), dot(g10, g10), dot(g11, g11)));
  g00 *= norm.x;  
  g01 *= norm.y;  
  g10 *= norm.z;  
  g11 *= norm.w;  

  float n00 = dot(g00, vec2(fx.x, fy.x));
  float n10 = dot(g10, vec2(fx.y, fy.y));
  float n01 = dot(g01, vec2(fx.z, fy.z));
  float n11 = dot(g11, vec2(fx.w, fy.w));

  vec2 fade_xy = fade(Pf.xy);
  vec2 n_x = mix(vec2(n00, n01), vec2(n10, n11), fade_xy.x);
  float n_xy = mix(n_x.x, n_x.y, fade_xy.y);
  return 2.3 * n_xy;
}

vec4 getFragColor(float noiseValue) {
	vec4 fragColor;
	fragColor.r = fract(noiseValue);
	fragColor.g = fract(2. * fragColor.r);
	fragColor.b = fract(3. * fragColor.g);
	fragColor.a = 1.0;
	return fragColor;
}

void main()
{
	//pixel coordinates from 0 to 1
	float aspectRatio = RENDERSIZE.x/RENDERSIZE.y;
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	
	//generate a ray with origin ro and direction rd
	vec4 ro = vec4(0.0, 0.0, 1.5, 1.0);
	vec3 rd = normalize(vec3( (-1.0+2.0*uv) * vec2(aspectRatio, 1.0), -1.0));
	
	//intersect the ray with scene
	vec2 t;
	float id = intersect(ro.xyz, rd, t);
	
	vec3 col;
	//If we hit the sphere...
	if(id > 0.5 && id < 1.5)
	{
		//1) do Schlick approx of Fresnel lighting
		vec3 E = normalize(ro.xyz + t.x*rd);
		vec3 N = normalize(nSphere(E, sph1));
		vec3 L = normalize(light);
		
		vec3 reflectColor = vec3(ambientAmount);
		float lambertTerm = dot(N, L);
		if (lambertTerm > 0.0) {
			float w = pow(1.0 - max(0.0, dot(normalize(L+E), E)), 5.0);
			
			reflectColor += (1.0-w)*pow(max(0.0, dot(reflect(-L, E), E)), shininess);
		}
		
		//2) do Fresnel refraction to look up the appropriate Perlin noise color
		//light goes in
		vec3 refractionVec = refract(rd, N, refractionIn);
		
		//light comes out
		float id2 = intersect(E, refractionVec, t);
		if (id2 > 0.5 && id2 < 1.5) {
			E += refractionVec * t.y;
			E = normalize(E);
			N = normalize(nSphere(E, sph1));
			refractionVec = refract(refractionVec, N, refractionOut);
		}
		
		vec3 noiseColor = getFragColor(cnoise(vec2(TT + refractionVec.x + uv.x, refractionVec.y + uv.y))).rgb;
		col = mix(noiseColor, reflectColor, reflectColor);
	}
	else
		col = getFragColor(cnoise(vec2(TT + uv.x, uv.y))).rgb;
	
	gl_FragColor = vec4(col,1.0);
}