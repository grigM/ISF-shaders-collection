/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "noise",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4ltXz2 by lherm.  Playing with raymarching and noise on an plane",
  "INPUTS" : [

  ]
}
*/


// Structure based on https://www.shadertoy.com/view/Xt2XDt by Cabbibo

#define MIRROR

const int S = 32; // steps
const float D = 100.; // max distance
const float P = 0.001; // intersection precision

// iq noise
float hash( float n )
{
    return fract(sin(n)*43758.5453123);
}

float noise( in vec3 x )
{
    vec3 p = floor(x);
    vec3 f = fract(x);
    f = f*f*(3.0-2.0*f);
	
    float n = p.x + p.y*157.0 + 113.0*p.z;
    return mix(mix(mix( hash(n+  0.0), hash(n+  1.0),f.x),
                   mix( hash(n+157.0), hash(n+158.0),f.x),f.y),
               mix(mix( hash(n+113.0), hash(n+114.0),f.x),
                   mix( hash(n+270.0), hash(n+271.0),f.x),f.y),f.z);
}

//-------------------
// Camera
//-------------------
mat3 calcLookAtMatrix( in vec3 ro, in vec3 ta, in float roll )
{
    vec3 ww = normalize( ta - ro );
    vec3 uu = normalize( cross(ww,vec3(sin(roll),cos(roll),0.0) ) );
    vec3 vv = normalize( cross(uu,ww));
    return mat3( uu, vv, ww );
}


vec2 opU( vec2 d1, vec2 d2 )
{
    
	return (d1.x<d2.x) ? d1 : d2;
    
}

float sdPlane(in vec3 p, in vec4 n)
{
    return dot(p, n.xyz) + n.w;
}


//-------------------
// Modelling
//-------------------

vec2 map(in vec3 p)
{
    vec3 ip = floor(p * 1.5);
    vec3 np = vec3(noise(ip), noise(ip), noise(ip));
    float s = sdPlane(vec3(p), vec4(0., 1., noise(np + TIME), 1.));
    vec2 res = vec2(s, 1.);
    return res;
}

vec2 trace(in vec3 ro, in vec3 rd)
{
    float h = P*2.;
    float t = 0.;
    float id = -1.;
    float res = -1.;
    
    for (int i = 0; i < S; i++)
    {
        if (h<P || t>D) break;
        vec3 r = ro + rd*t;
        h = map(r).x;
        t += h;
        id = map(r).y;
    }
    
    if (t < D) res = t;
    if (t > D) id = -1.;
    
    return vec2(res, id);
}

vec3 calcNormal( in vec3 pos ){
    
	vec3 eps = vec3( 0.001, 0.0, 0.0 );
	vec3 nor = vec3(
	    map(pos+eps.xyy).x - map(pos-eps.xyy).x,
	    map(pos+eps.yxy).x - map(pos-eps.yxy).x,
	    map(pos+eps.yyx).x - map(pos-eps.yyx).x );
	return normalize(nor);
}

float calcAO( in vec3 pos, in vec3 nor )
{
	float occ = 0.0;
    float sca = 1.0;
    for( int i=0; i<5; i++ )
    {
        float hr = 0.01 + 0.12*float(i)/4.0;
        vec3 aopos =  nor * hr + pos;
        float dd = map( aopos ).x;
        occ += -(dd-hr)*sca;
        sca *= 0.95;
    }
    return clamp( 1.0 - 3.0*occ, 0.0, 1.0 );    
}

vec3 render(in vec2 res, in vec3 ro, in vec3 rd)
{
    vec3 color = vec3(0.);
    vec3 lightPos = vec3( 1. , 4. , 3. );
    
    if (res.y > -.5)
    {
        vec3 p = ro + rd*res.x;
        vec3 norm = calcNormal(p);      
        vec3 lightDir = normalize(lightPos - p);
        float match = max( 0. , dot( lightDir , norm ));
        float occ = calcAO(p, norm);
        
        if (res.y == 1.)
        {
            color = vec3(1.) * match * occ * (1.0-calcAO(p, rd));
        }
    }
    
    return color;
}

void main()
{
	vec2 uv = (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy)/RENDERSIZE.y;
    
    #ifdef MIRROR
    uv = -abs(uv);
    #endif
    
    vec3 ro = vec3(0., 0., 4.);
    vec3 ta = vec3(0., 0., 0.);
    
    // Camera Matrix
    mat3 camMat = calcLookAtMatrix( ro, ta, 0. );  // 0.0 is the camera roll
    
    // Create view ray
	vec3 rd = normalize( camMat * vec3(uv.xy,2.0) ); // 2.0 is the lens length
    
    vec2 res = trace(ro, rd);
    
    vec3 color = render(res, ro, rd);
    
	gl_FragColor = vec4(color,1.0);
}