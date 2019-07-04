/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#31149.0"
}
*/


#ifdef GL_ES
precision highp float;
#endif

varying vec2 surfaceSize;

vec3   iResolution = vec3(RENDERSIZE, 1.0);
float  iGlobalTime = TIME;
vec4   iMouse = vec4(mouse, 0.0, 1.0);
uniform sampler2D iChannel0,iChannel1,bb;

// srtuss, 2015
// volumetric cloud tunnel, a single light source, lightning and something that is supposed
// to look like raindrops. :)
// visuals are inspired by a piece of music, 2d-texture based 3d noise function by iq.
// the code could need some cleaning, but i don't want to do that right now.


// srtuss: Absolutely spectacular sir! My hat off to you 

#define pi 3.1415926535897932384626433832795

struct ITSC
{
	vec3 p;
	float dist;
	vec3 n;
    vec2 uv;
};

ITSC raycylh(vec3 ro, vec3 rd, vec3 c, float r)
{
	ITSC i;
	i.dist = 1e38;
	vec3 e = ro - c;
	float a = dot(rd.xy, rd.xy);
	float b = 2.0 * dot(e.xy, rd.xy);
	float cc = dot(e.xy, e.xy) - r;
	float f = b * b - 4.0 * a * cc;
	if(f > 0.0)
	{
		f = sqrt(f);
		float t = (-b + f) / (2.0 * a);
		
		if(t > 0.001)
		{
			i.dist = t;
			i.p = e + rd * t;
			i.n = -vec3(normalize(i.p.xy), 0.0);
		}
	}
	return i;
}

void tPlane(inout ITSC hit, vec3 ro, vec3 rd, vec3 o, vec3 n, vec3 tg, vec2 si)
{
    vec2 uv;
    ro -= o;
    float t = -dot(ro, n) / dot(rd, n);
    if(t < 0.0)
        return;
    vec3 its = ro + rd * t;
    uv.x = dot(its, tg);
    uv.y = dot(its, cross(tg, n));
    if(abs(uv.x) > si.x || abs(uv.y) > si.y)
        return;
    
    //if(t < hit.dist)
    {
        hit.dist = t;
        hit.uv = uv;
    }
    return;
}



#define noTexture true

mat3 m = mat3( 0.00, 0.90, 0.60, 
-0.90, 0.36, -0.48, 
-0.60, -0.48, 0.34 );

//----------------------------------------------------------------------
float hsh( float n )
{
  return fract(sin(n)*43758.5453123);
}



float nseI(float x)
{
    float fl = floor(x);
    return mix(hsh(fl), hsh(fl + 1.0), smoothstep(0.0, 1.0, fract(x)));
}

vec2 rotate(vec2 p, float a)
{
	return vec2(p.x * cos(a) - p.y * sin(a), p.x * sin(a) + p.y * cos(a));
}

float nse3d(in vec3 x)
{
  vec3 p = floor(x);
  vec3 f = fract(x);
  f = f*f*(3.0-2.0*f);

  #ifdef noTexture
    float n = p.x + p.y*57.0 + 113.0*p.z;
    return mix(mix(mix( hsh(n+  0.0), hsh(n+  1.0), f.x), 
    mix( hsh(n+ 57.0), hsh(n+ 58.0), f.x), f.y), 
      mix(mix( hsh(n+113.0), hsh(n+114.0), f.x), 
      mix( hsh(n+170.0), hsh(n+171.0), f.x), f.y), f.z);
  #else
    vec2 uv = (p.xy + vec2(37.0, 17.0)*p.z) + f.xy;
    vec2 rg = IMG_NORM_PIXEL(texture,mod(mod((uv+ 0.5)/256.0, 1.0),1.0),-100.0).yx;
    return mix( rg.x, rg.y, f.z );  
  #endif
}
float nse(in vec2 x)
{
  vec2 p = floor(x);
  vec2 f = fract(x);
  f = f*f*(6.0-4.0*f);

  float n = p.x + p.y*57.0;
  float res = mix(mix( hsh(n+  0.0), hsh(n+  1.0), f.x), 
  mix( hsh(n+ 57.0), hsh(n+ 58.0), f.x), f.y);
  return res;
}
float density2(vec2 p, float z, float t)
{
    float v = 0.0;
    float fq = 1.0, am = 0.5, mvfd = 1.0;
    vec2 rnd = vec2(0.3, 0.7);
    for(int i = 0; i < 7; i++)
    {
        rnd = fract(sin(rnd * 14.4982) * 2987253.28612);
        v += nse(p * fq + t * (rnd - 0.5)) * am;
        fq *= 2.0;
        am *= 0.5;
        mvfd *= 1.3;
    }
    return v * exp(z * z * -2.0);
}

float densA = 1.0, densB = 2.0;

float fbm(vec3 p)
{
    vec3 q = p;
    q.xy = rotate(p.xy, iGlobalTime);
    
    p += (nse3d(p * 3.0) - 0.5) * 0.3;
    
    //float v = nse3d(p) * 0.5 + nse3d(p * 2.0) * 0.25 + nse3d(p * 4.0) * 0.125 + nse3d(p * 8.0) * 0.0625;
    
    //p.y += iGlobalTime * 0.2;
    
    float mtn = iGlobalTime * 0.15;
    
    float v = 0.0;
    float fq = 1.0, am = .45;
    for(int i = 0; i < 6; i++)
    {
        v += nse3d(p * fq + mtn * fq) * am;
        fq *= 2.;
        am *= 0.5;
    }
    return v;
}

float fbmHQ(vec3 p)
{
    vec3 q = p;
    q.xy = rotate(p.xy, iGlobalTime);
    
    p += (nse3d(p * 3.0) - 0.5) * 0.4;
    
    //float v = nse3d(p) * 0.5 + nse3d(p * 2.0) * 0.25 + nse3d(p * 4.0) * 0.125 + nse3d(p * 8.0) * 0.0625;
    
    //p.y += iGlobalTime * 0.2;
    
    float mtn = iGlobalTime * 0.2;
    
    float v = 0.0;
    float fq = 1.0, am = 0.5;
    for(int i = 0; i < 9; i++)
    {
        v += nse3d(p * fq + mtn * fq) * am;
        fq *= 2.0;
        am *= 0.5;
    }
    return v;
}

float density(vec3 p)
{
    vec2 pol = vec2(atan(p.y, p.x), length(p.yx));
    
    float v = fbm(p);
    
    float fo = atan((pol.y - 1.5),(pol.y - 1.5)+(densA + densB) * 0.5);
    fo *= (densB - densA);
    v *= exp(fo * fo * -5.0);
    
    float edg = .323;
    return smoothstep(edg, edg + 0.05, v);
}

float densityHQ(vec3 p)
{
    vec2 pol = vec2(atan(p.y, p.x), length(p.yx));
    
    float v = fbmHQ(p);
    
    float fo = atan((pol.y - 1.5),(pol.y - 1.5)+(densA + densB) * 0.5);
    fo *= (densB - densA);
    v *= exp(fo * fo * -5.0);
    
    float edg = 0.3;
    return smoothstep(edg, edg + 0.1, v);
}

vec2 drop(inout vec2 p)
{
    vec2 mv = iGlobalTime * vec2(0.5, -1.0) * 0.15;
    
    float drh = 0.0;
    float hl = 0.0;
    
    vec4 rnd = vec4(0.1, 0.2, 0.3, 0.4);
    for(int i = 0; i < 20; i++)
    {
        rnd = fract(sin(rnd * 2.184972) * 190723.58961);
        float fd = fract(iGlobalTime * 0.2 + rnd.w);
        fd = exp(fd * -4.0);
        float r = 0.025 * (rnd.w * 1.5 + 1.0);
        float sz = 0.35;
        
        
        vec2 q = (fract((p - mv) * sz + rnd.xy) - 0.5) / sz;
        mv *= 1.06;
        
        q.y *= -1.0;
        float l = length(q + pow(abs(dot(q, vec2(1.0, 0.4))), 0.7) * (fd * 0.2 + 0.1));
        if(l < r)
        {
        	float h = sqrt(r * r - l * l);
        	drh = max(drh, h * fd);
        }
        hl += exp(length(q - vec2(-0.02, 0.01)) * -30.0) * 0.4 * fd;
    }
    p += drh * 5.0;
    return vec2(drh, hl);
}


float hash1(float p)
{
	return fract(sin(p * 172.435) * 29572.683) - 0.5;
}

float hash2(vec2 p)
{
	vec2 r = (456.789 * sin(789.123 * p.xy));
	return fract(r.x * r.y * (1.0 + p.x));
}

float ns(float p)
{
	float fr = fract(p);
	float fl = floor(p);
	return mix(hash1(fl), hash1(fl + 1.0), fr);
}

float fbm(float p)
{
	return (ns(p) * 0.4 + ns(p * 2.0 - 10.0) * 0.125 + ns(p * 8.0 + 10.0) * 0.025);
}

float fbmd(float p)
{
	float h = 0.01;
	return atan(fbm(p + h) - fbm(p - h), h);
}

float arcsmp(float x, float seed)
{
	return fbm(x * 3.0 + seed * 1111.111) * (1.0 - exp(-x * 5.0));
}

float arc(vec2 p, float seed, float len)
{
	p *= len;
	//p = rotate(p, iGlobalTime);
	float v = abs(p.y - arcsmp(p.x, seed));
	v += exp((2.0 - p.x) * -4.0);
	v = exp(v * -60.0) + exp(v * -10.0) * 0.6;
	//v += exp(p.x * -2.0);
	v *= smoothstep(0.0, 0.05, p.x);
	return v;
}

float arcc(vec2 p, float sd)
{
	float v = 0.0;
	float rnd = fract(sd);
	float sp = 0.0;
	v += arc(p, sd, 1.0);
	for(int i = 0; i < 4; i ++)
	{
		sp = rnd + 0.01;
		vec2 mrk = vec2(sp, arcsmp(sp, sd));
		v += arc(rotate(p - mrk, fbmd(sp)), mrk.x, mrk.x * 0.4 + 1.5);
		rnd = fract(sin(rnd * 195.2837) * 1720.938);
	}
	return v;
}

void main(void)
{
	vec2 uv = gl_FragCoord.xy / iResolution.xy;
    
    uv = 2.0 * uv - 1.0;
    uv.x *= iResolution.x / iResolution.y;
    
    vec2 drh = vec2(0,0);//drop(uv);
    
    float camtm = iGlobalTime * 1.55;
    vec3 ro = vec3(0., 0.0, camtm);
    vec3 rd = normalize(vec3(uv, 1.2));
   // rd.xz = rotate(rd.xz, sin(camtm) * 0.4);
   // rd.yz = rotate(rd.yz, sin(camtm * 1.3) * 0.4);
    
    vec3 sun = normalize(vec3(0.2, 1.0, 0.1));
    
    float sd = sin(gl_FragCoord.x * 0.01 + gl_FragCoord.y * 3.333333333 + iGlobalTime) * 1298729.146861;
    
    vec3 col;
    float dacc = 0.0, lacc = 0.0;
    
    vec3 light = vec3(cos(iGlobalTime * 8.0) * 0.5, sin(iGlobalTime * 4.0) * 0.5, ro.z + 4.0 + sin(iGlobalTime));
    
    ITSC tunRef;
    #define STP 10
    for(int i = 0; i < STP; i++)
    {
        ITSC itsc = raycylh(ro, rd, vec3(0.0), densB + float(i) * (densA - densB) / float(STP) + fract(sd) * 0.07);
        float d = density(itsc.p);
        vec3 tol = light - itsc.p;
        float dtol = length(tol);
        tol = tol * 0.1 / dtol;
        
        float dl = density(itsc.p + tol);
        lacc += max(d - dl, 0.0) * exp(dtol * -0.2);
        dacc += d;
        tunRef = itsc;
    }
    dacc /= float(STP);
    ITSC itsc = raycylh(ro, rd, vec3(0.0), 4.0);
    vec3 sky = vec3(0.6, 0.3, 0.2);
    sky *= 0.9 * pow(fbmHQ(itsc.p), 2.0);
    lacc = max(lacc * 0.3 + 0.3, 0.0);
    vec3 cloud = pow(vec3(lacc), vec3(0.5, 0.0, 0.0) * 1.0);
    col = mix(sky, cloud, dacc);
    col *= exp(tunRef.dist * -0.1);
    col += drh.y;
    
   
    col = pow(col, vec3(1.0, 0.9, 0.6) * 1.4) * 1.1;
    col = pow(col, vec3(1.0 / 2.2));
	gl_FragColor = vec4(col, 1.0);
}