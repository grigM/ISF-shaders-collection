/*
{
  "IMPORTED" : [
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
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XljXWD by eddietree.  A study on gold materials",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


// a study on raymarching, soft-shadows, ao, etc
// borrowed heavy from others, esp @cabbibo and @iquilezles and more
// by @eddietree

#define float3 vec3

const float INTERSECTION_PRECISION = 0.001;
const int NUM_OF_TRACE_STEPS = 40;

float distSphere(vec3 p, float radius) 
{
    return length(p) - radius;
}

// by shane : https://www.shadertoy.com/view/4lSXzh
float Voronesque( in vec3 p )
{
    vec3 i  = floor(p + dot(p, vec3(0.333333)) );  p -= i - dot(i, vec3(0.166666)) ;
    vec3 i1 = step(0., p-p.yzx), i2 = max(i1, 1.0-i1.zxy); i1 = min(i1, 1.0-i1.zxy);    
    vec3 p1 = p - i1 + 0.166666, p2 = p - i2 + 0.333333, p3 = p - 0.5;
    vec3 rnd = vec3(7, 157, 113); // I use this combination to pay homage to Shadertoy.com. :)
    vec4 v = max(0.5 - vec4(dot(p, p), dot(p1, p1), dot(p2, p2), dot(p3, p3)), 0.);
    vec4 d = vec4( dot(i, rnd), dot(i + i1, rnd), dot(i + i2, rnd), dot(i + 1., rnd) ); 
    d = fract(sin(d)*262144.)*v*2.; 
    v.x = max(d.x, d.y), v.y = max(d.z, d.w), v.z = max(min(d.x, d.y), min(d.z, d.w)), v.w = min(v.x, v.y); 
    return  max(v.x, v.y) - max(v.z, v.w); // Maximum minus second order, for that beveled Voronoi look. Range [0, 1].
    
}

mat3 calcLookAtMatrix( in vec3 ro, in float3 ta, in float roll )
{
    vec3 ww = normalize( ta - ro );
    vec3 uu = normalize( cross(ww,vec3(sin(roll),cos(roll),0.0) ) );
    vec3 vv = normalize( cross(uu,ww));
    return mat3( uu, vv, ww );
}

void doCamera( out vec3 camPos, out vec3 camTar, in float time, in vec2 mouse )
{
    float radius = 7.0;
    float theta = 0.3 + 5.0*mouse.x - TIME*0.5;
    float phi = 3.14159*0.4;//5.0*mouse.y;
    
    float pos_x = radius * cos(theta) * sin(phi);
    float pos_z = radius * sin(theta) * sin(phi);
    float pos_y = radius * cos(phi);
    
    camPos = vec3(pos_x, pos_y, pos_z);
    camTar = vec3(0.0,0.0,0.0);
}

float smin( float a, float b, float k )
{
    float res = exp( -k*a ) + exp( -k*b );
    return -log( res )/k;
}

float opS( float d1, float d2 )
{
    return max(-d1,d2);
}

float opU( float d1, float d2 )
{
    return min(d1,d2);
}

// noise func
float hash( float n ) { return fract(sin(n)*753.5453123); }
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

// checks to see which intersection is closer
// and makes the y of the vec2 be the proper id
vec2 opU( vec2 d1, vec2 d2 ){
	return (d1.x<d2.x) ? d1 : d2; 
}

float opI( float d1, float d2 )
{
    return max(d1,d2);
}

//--------------------------------
// Modelling 
//--------------------------------
vec2 map( vec3 pos )
{  
    float t1 = distSphere( pos, 2.0 + Voronesque(pos*0.5) );// + noise(pos * 1.0 + TIME*0.75);   
    t1 = min( t1, distSphere(pos + vec3(4.0,0.0,0.0), 0.3) );
    t1 = min( t1, distSphere(pos + vec3(-4.0,0.0,0.0), 0.3) );
   
   	return vec2( t1, 1.0 );
}

float shadow( in vec3 ro, in vec3 rd )
{
    const float k = 2.0;
    
    const int maxSteps = 50;
    float t = 0.0;
    float res = 1.0;
    
    for(int i = 0; i < maxSteps; ++i) {
        
        float d = map(ro + rd*t).x;
            
        if(d < INTERSECTION_PRECISION) {
            
            return 0.0;
        }
        
        res = min( res, k*d/t );
        t += d;
    }
    
    return res;
}


float ambientOcclusion( in vec3 ro, in vec3 rd )
{
    const int maxSteps = 7;
    const float stepSize = 0.05;
    
    float t = 0.0;
    float res = 0.0;
    
    // starting d
    float d0 = map(ro).x;
    
    for(int i = 0; i < maxSteps; ++i) {
        
        float d = map(ro + rd*t).x;
		float diff = max(d-d0, 0.0);
        
        res += diff;
        
        t += stepSize;
    }
    
    return res;
}

vec3 calcNormal( in vec3 pos )
{
	vec3 eps = vec3( 0.001, 0.0, 0.0 );
	vec3 nor = vec3(
	    map(pos+eps.xyy).x - map(pos-eps.xyy).x,
	    map(pos+eps.yxy).x - map(pos-eps.yxy).x,
	    map(pos+eps.yyx).x - map(pos-eps.yyx).x );
	return normalize(nor);
}

vec3 renderColor( vec3 ro , vec3 rd, in vec3 color, vec3 currPos )
{
    //vec3 normal = calcNormal( currPos );
    //vec3 normal = calcNormal( currPos - rd * Voronesque(currPos*35.0)*0.2 );
    vec3 normal = calcNormal( currPos - rd * noise(currPos*70.0)*0.1 );

    //float shadowVal = shadow( currPos - rayDirection* 0.03, lightDir  );
    float ao = ambientOcclusion( currPos - normal*0.01, normal );
    float ndotl = abs(dot( -rd, normal ));
    float rim = pow(1.0-ndotl, 2.5);
    //float specular = pow( dot( lightDir, normal ), 3.0);

    vec3 reflectionColor = textureCube(iChannel0,reflect( rd, normal )).xyz;

    //color = vec3(0.2);
    color = reflectionColor;
    color *= vec3(1.0,1.0,0.5);
    //color = mix( color, normal*0.5+vec3(0.5), rim_distorted+0.1 );

    //color = normal;

    //color = normal;
    //color *= vec3(mix(0.25,1.0,shadowVal));
    color *= vec3(mix(0.6,1.0,ao));
    color += rim * 0.3;
        
    return color;
}

bool renderRayMarch(vec3 ro, vec3 rd, inout vec3 color ) {
    const int maxSteps = NUM_OF_TRACE_STEPS;
        
    float t = 0.0;
    float d = 0.0;
    
    vec3 lightDir = normalize(vec3(1.0,0.4,0.0));
    
    for(int i = 0; i < maxSteps; ++i) 
    {
        vec3 currPos = ro + rd * t;
        d = map(currPos).x;
        if(d < INTERSECTION_PRECISION) 
        {
        	break;
        }
        
        t += d;
    }
    
    if(d < INTERSECTION_PRECISION) 
    {
	    vec3 currPos = ro + rd * t;
    	color = renderColor( ro, rd, color, currPos );
    }
    
    return true;
}


void main() {



	vec2 p = (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy)/RENDERSIZE.y;
    vec2 m = iMouse.xy/RENDERSIZE.xy;
    
    // camera movement
    vec3 ro, ta;
    doCamera( ro, ta, TIME, m );
    // camera matrix
    mat3 camMat = calcLookAtMatrix( ro, ta, 0.0 );  // 0.0 is the camera roll
    
	// create view ray
	vec3 rd = normalize( camMat * vec3(p.xy,2.0) ); // 2.0 is the lens length
    
    // calc color
    //vec3 col = vec3(0.9);
    vec3 col = textureCube(iChannel0,rd).xyz;
    renderRayMarch( ro, rd, col );
    
    // vignette, OF COURSE
    float vignette = 1.0-smoothstep(1.0,2.5, length(p));
    col.xyz *= mix( 0.5, 1.0, vignette);
        
    gl_FragColor = vec4( col , 1. );
}
