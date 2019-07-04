/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : [
        "488bd40303a2e2b9a71987e48c66ef41f5e937174bf316d3ed0e86410784b919.jpg",
        "488bd40303a2e2b9a71987e48c66ef41f5e937174bf316d3ed0e86410784b919_1.jpg",
        "488bd40303a2e2b9a71987e48c66ef41f5e937174bf316d3ed0e86410784b919_2.jpg",
        "488bd40303a2e2b9a71987e48c66ef41f5e937174bf316d3ed0e86410784b919_3.jpg",
        "488bd40303a2e2b9a71987e48c66ef41f5e937174bf316d3ed0e86410784b919_4.jpg",
        "488bd40303a2e2b9a71987e48c66ef41f5e937174bf316d3ed0e86410784b919_5.jpg"
      ],
      "TYPE" : "cube"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/lljSDW by eddietree.  same as other, but applying a cubemap",
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

const float MAX_TRACE_DISTANCE = 10.0;
const float INTERSECTION_PRECISION = 0.001;
const int NUM_OF_TRACE_STEPS = 50;

float distSphere(vec3 p, float radius) 
{
    return length(p) - radius;
}

mat3 calcLookAtMatrix( in vec3 ro, in vec3 ta, in float roll )
{
    vec3 ww = normalize( ta - ro );
    vec3 uu = normalize( cross(ww,vec3(sin(roll),cos(roll),0.0) ) );
    vec3 vv = normalize( cross(uu,ww));
    return mat3( uu, vv, ww );
}

void doCamera( out vec3 camPos, out vec3 camTar, in float time, in vec2 mouse )
{
    float radius = 4.0;
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
vec2 map( vec3 pos ){  
   
    float sphere = distSphere(pos, 1.75) + noise(pos * 1.0 + TIME*0.75);   
    float t1 = sphere;
    
    t1 = smin( t1, distSphere( pos + vec3(1.8,0.0,0.0), 0.2 ), 2.0 );
    t1 = smin( t1, distSphere( pos + vec3(-1.8,0.0,-1.0), 0.2 ), 2.0 );
   
   	return vec2( t1, 1.0 );
    
}

vec2 map2( vec3 pos ){  
   
    //float sphere = distSphere(pos, 1.0) + noise(pos * 1.2 + vec3(-0.3) + TIME*0.2);
    float sphere = distSphere(pos, 0.45);
    
    sphere = smin( sphere, distSphere( pos + vec3(-0.4,0.0,-1.0), 0.04 ), 5.0 );
    sphere = smin( sphere, distSphere( pos + vec3(-0.5,-0.75,0.0), 0.05 ), 50.0 );
    sphere = smin( sphere, distSphere( pos + vec3(0.5,0.7,0.5), 0.1 ), 5.0 );

   	return vec2( sphere, 1.0 );
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

vec3 calcNormal( in vec3 pos ){
    
	vec3 eps = vec3( 0.001, 0.0, 0.0 );
	vec3 nor = vec3(
	    map(pos+eps.xyy).x - map(pos-eps.xyy).x,
	    map(pos+eps.yxy).x - map(pos-eps.yxy).x,
	    map(pos+eps.yyx).x - map(pos-eps.yyx).x );
	return normalize(nor);
}


vec3 calcNormal2( in vec3 pos ){
    
	vec3 eps = vec3( 0.001, 0.0, 0.0 );
	vec3 nor = vec3(
	    map2(pos+eps.xyy).x - map2(pos-eps.xyy).x,
	    map2(pos+eps.yxy).x - map2(pos-eps.yxy).x,
	    map2(pos+eps.yyx).x - map2(pos-eps.yyx).x );
	return normalize(nor);
}

void renderColor2( vec3 ro , vec3 rd, inout vec3 color, vec3 currPos )
{
    //vec3 lightDir = normalize(vec3(1.0,0.4,0.0));
    vec3 normal = calcNormal2( currPos );
    vec3 normal_distorted = calcNormal2( currPos +  rd*noise(currPos*2.5 + TIME*2.0)*0.75 );

    float ndotl = abs(dot( -rd, normal ));
    float ndotl_distorted = (dot( -rd, normal_distorted ))*0.5+0.5;
    float rim = pow(1.0-ndotl, 3.0);
    float rim_distorted = pow(1.0-ndotl_distorted, 6.0);

    //color = mix( color, normal*0.5+vec3(0.5), rim_distorted+0.15 );
    //color = mix( vec3(0.0,0.1,0.6), color, rim*1.5 );
    color = mix( refract(normal, rd, 0.5)*0.5+vec3(0.5), color, rim );
    //color = mix( vec3(0.1), color, rim );
    color += rim*0.6;
}

// for inside ball
bool renderRayMarch2(vec3 ro, vec3 rd, inout vec3 color ) {
    
    float t = 0.0;
    float d = 0.0;
    
    for(int i = 0; i < NUM_OF_TRACE_STEPS; ++i) 
    {
        vec3 currPos = ro + rd*t;
        d = map2(currPos).x;
        if(d < INTERSECTION_PRECISION) 
        {
            renderColor2( ro, rd, color, currPos );
            return true;
        }
        
        t += d;
    }
    
    if(d < INTERSECTION_PRECISION) 
    {
        vec3 currPos = ro + rd*t;
        renderColor2( ro, rd, color, currPos );
        return true;
    }

    return false;
}

void renderColor( vec3 ro , vec3 rd, inout vec3 color, vec3 currPos )
{
    vec3 lightDir = normalize(vec3(1.0,0.4,0.0));
    vec3 normal = calcNormal( currPos );
    vec3 normal_distorted = calcNormal( currPos +  noise(currPos*1.5 + vec3(0.0,0.0,sin(TIME*0.75))) );
    float shadowVal = shadow( currPos - rd* 0.01, lightDir  );
    float ao = ambientOcclusion( currPos - normal*0.01, normal );

    float ndotl = abs(dot( -rd, normal ));
    float ndotl_distorted = abs(dot( -rd, normal_distorted ));
    float rim = pow(1.0-ndotl, 6.0);
    float rim_distorted = pow(1.0-ndotl_distorted, 6.0);

	color = textureCube(iChannel0,refract( rd, normal, 0.85 )).xyz;
    color = mix( color, normal*0.5+vec3(0.5), rim_distorted+0.1 );
    //color += rim;
    color += textureCube(iChannel0,reflect( rd, normal )).xyz * rim + vec3(rim*0.6);
    //color = normal;

    // refracted ray-march into the inside area
    vec3 color2 = vec3(0.5);
    renderRayMarch2( currPos, refract(rd, normal, 0.85), color );
    //renderRayMarch2( currPos, rayDirection, color2 );

    //color = color2;
    //color = normal;
    //color *= vec3(mix(0.25,1.0,shadowVal));

    color *= vec3(mix(0.8,1.0,ao));
}

bool renderRayMarch(vec3 ro, vec3 rd, inout vec3 color ) 
{
    const int maxSteps = NUM_OF_TRACE_STEPS;
        
    float t = 0.0;
    float d = 0.0;
    
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
        renderColor( ro, rd, color, currPos );
        return true;
    }
    
    return false;
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
    col.xyz *= mix( 0.7, 1.0, vignette);
        
    gl_FragColor = vec4( col , 1. );
}
