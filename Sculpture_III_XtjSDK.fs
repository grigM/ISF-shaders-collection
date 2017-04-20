/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex03.jpg"
    }
  ],
  "CATEGORIES" : [
    "3d",
    "raymarching",
    "noise",
    "trigonometry",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtjSDK by iq.  Another sine\/cosine deformation of a sphere.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    },
    {
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 3.0,
			"MIN": 0.5,
			"MAX": 200.0
	},
	{
		"NAME": "intersect_maxd",
			"TYPE": "float",
			"DEFAULT": 7.0,
			"MIN": 2.0,
			"MAX": 11.0
	},
	{
		"NAME": "first_deform",
			"TYPE": "float",
			"DEFAULT": 1.000,
			"MIN": 0.000,
			"MAX": 4.000
	},
	{
		"NAME": "second_deform",
			"TYPE": "float",
			"DEFAULT": 0.500,
			"MIN": 0.000,
			"MAX": 1.000
	},
	{
		"NAME": "third_deform",
			"TYPE": "float",
			"DEFAULT": 0.250,
			"MIN": 0.000,
			"MAX": 1.000
	},
	{
		"NAME": "fourth_deform",
			"TYPE": "float",
			"DEFAULT": 0.050,
			"MIN": 0.000,
			"MAX": 0.100
	},{
		"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 1.5,
			"MIN": 0.5,
			"MAX": 10.0
	
	}
  ]
}
*/


// Created by inigo quilez - iq/2015
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.


float hash1( float n )
{
    return fract(sin(n)*43758.5453123);
}

float hash1( in vec2 f ) 
{ 
    return fract(sin(f.x+131.1*f.y)*43758.5453123); 
}


const float PI = 3.1415926535897932384626433832795;
const float PHI = 1.6180339887498948482045868343656;

vec3 forwardSF( float i, float n) 
{
    float phi = 2.0*PI*fract(i/PHI);
    float zi = 1.0 - (2.0*i+1.0)/n;
    float sinTheta = sqrt( 1.0 - zi*zi);
    return vec3( cos(phi)*sinTheta, sin(phi)*sinTheta, zi);
}

float sca = 0.5 + 0.15*sin(TIME-10.0);
vec4 grow = vec4(1.0);

vec3 mapP( vec3 p )
{
    p.xyz += first_deform*sin(  2.0*p.yzx )*grow.x;
    p.xyz += second_deform*sin(  4.0*p.yzx )*grow.y;
    p.xyz += third_deform*sin(  8.0*p.yzx )*grow.z;
    p.xyz += fourth_deform*sin( 16.0*p.yzx )*grow.w;
    return p;
}

float map( vec3 q )
{
    vec3 p = mapP( q );
    float d = length( p ) - 1.5;
	return d * 0.05;
}

float intersect( in vec3 ro, in vec3 rd )
{
	//const float maxd = intersect_maxd;

	float precis = 0.001;
    float h = 1.0;
    float t = 1.0;
    for( int i=0; i<1256; i++ )
    {
        if( (h<precis) || (t>intersect_maxd) ) break;
	    h = map( ro+rd*t );
        t += h;
    }

    if( t>intersect_maxd ) t=-1.0;
	return t;
}

vec3 calcNormal( in vec3 pos )
{
    vec3 eps = vec3(0.005,0.0,0.0);
	return normalize( vec3(
           map(pos+eps.xyy) - map(pos-eps.xyy),
           map(pos+eps.yxy) - map(pos-eps.yxy),
           map(pos+eps.yyx) - map(pos-eps.yyx) ) );
}

float calcAO( in vec3 pos, in vec3 nor, in vec2 pix )
{
	float ao = 0.0;
    for( int i=0; i<64; i++ )
    {
        vec3 ap = forwardSF( float(i), 64.0 );
		ap *= sign( dot(ap,nor) ) * hash1(float(i));
        ao += clamp( map( pos + nor*0.05 + ap*1.0 )*32.0, 0.0, 1.0 );
    }
	ao /= 64.0;
	
    return clamp( ao*ao, 0.0, 1.0 );
}

float calcAO2( in vec3 pos, in vec3 nor, in vec2 pix )
{
	float ao = 0.0;
    for( int i=0; i<32; i++ )
    {
        vec3 ap = forwardSF( float(i), 32.0 );
		ap *= sign( dot(ap,nor) ) * hash1(float(i));
        ao += clamp( map( pos + nor*0.05 + ap*0.2 )*100.0, 0.0, 1.0 );
    }
	ao /= 32.0;
	
    return clamp( ao, 0.0, 1.0 );
}

vec4 texCube( sampler2D sam, in vec3 p, in vec3 n, in float k )
{
	vec4 x = texture2D( sam, p.yz );
	vec4 y = texture2D( sam, p.zx );
	vec4 z = texture2D( sam, p.xy );
    vec3 w = pow( abs(n), vec3(k) );
	return (x*w.x + y*w.y + z*w.z) / (w.x+w.y+w.z);
}

void main()
{
    vec2 p = (-RENDERSIZE.xy+2.0*gl_FragCoord.xy)/RENDERSIZE.y;
	vec2 q = gl_FragCoord.xy/RENDERSIZE.xy;

    vec2 m = vec2(0.5);
	if( iMouse.x>0.0 ) m = iMouse.xy/RENDERSIZE.xy;


    
    grow = sin(smoothstep( 0.0, 1.0, (TIME-vec4(0.0,1.0,2.0,3.0))/speed )*TIME);

    
    //-----------------------------------------------------
    // camera
    //-----------------------------------------------------
	
	float an = 1.1 + 0.05*(TIME-10.0) - 7.0*m.x;

	vec3 ro = vec3(4.5*sin(an),1.0,4.5*cos(an));
    vec3 ta = vec3(0.0,0.2,0.0);
    // camera matrix
    vec3 ww = normalize( ta - ro );
    vec3 uu = normalize( cross(ww,vec3(0.0,1.0,0.0) ) );
    vec3 vv = normalize( cross(uu,ww));
	// create view ray
	vec3 rd = normalize( p.x*uu + p.y*vv + zoom*ww );


    //-----------------------------------------------------
	// render
    //-----------------------------------------------------
    
	vec3 col = vec3(0.07)*clamp(1.0-length(q-0.5),0.0,1.0);

	// raymarch
    float t = intersect(ro,rd);

    if( t>0.0 )
    {
        vec3 pos = ro + t*rd;
        vec3 nor = calcNormal(pos);
		vec3 ref = reflect( rd, nor );
        vec3 sor = nor;
        
        vec3 q = mapP( pos );
        float occ = calcAO( pos, nor, gl_FragCoord.xy ); occ = occ*occ;

        // materials
		col = vec3(0.04);
        float ar = clamp(1.0-0.7*length(q-pos),0.0,1.0);
        col = mix( col, vec3(2.1,2.0,1.2), ar);
        col  *= 0.3;          
        col *= mix(vec3(1.0,0.4,0.3), vec3(0.8,1.0,1.3), occ);
        float occ2 = calcAO2( pos, nor, gl_FragCoord.xy );
        
        
        col *= 1.0*mix( vec3(2.0,0.4,0.2), vec3(1.0), occ2*occ2*occ2 );
        float ks = texCube( iChannel0, pos*1.5, nor, 4.0 ).x;
        ks = 0.5 + 1.0*ks;
        ks *= (1.0-ar);
        
        // lighting
        float sky = 0.5 + 0.5*nor.y;
        float fre = clamp( 1.0 + dot(nor,rd), 0.0, 1.0 );
        float spe = pow(max( dot(-rd,nor),0.0),8.0);
		// lights
		vec3 lin  = vec3(0.0);
		     lin += 3.0*vec3(0.7,0.80,1.00)*sky*occ;
             lin += 1.0*fre*vec3(1.2,0.70,0.60)*(0.1+0.9*occ);
        col += 0.3*ks*4.0*vec3(0.7,0.8,1.00)*smoothstep(0.0,0.2,ref.y)*(0.05+0.95*pow(fre,5.0))*(0.5+0.5*nor.y)*occ;
        col += 4.0*ks*1.5*spe*occ*col.x;
        col += 2.0*ks*1.0*pow(spe,8.0)*occ*col.x;
        col = col * lin;

        // dust
        col = mix( col, 0.2*fre*fre*fre+0.6*vec3(0.6,0.55,0.5)*sky*(0.8+0.4*texCube( iChannel0, pos*8.0, nor, 4.0 ).xyz), 0.6*smoothstep(0.3,0.7,nor.y)*sqrt(occ) );
        
        col *= 2.6*exp(-0.2*t);
    }

	col = pow(col,vec3(0.4545));

    col = pow( col, vec3(1.0,1.0,1.4) ) + vec3(0.0,0.02,0.14);
    
    col += (1.0/255.0)*hash1( gl_FragCoord.xy );
    
    gl_FragColor = vec4( col, 1.0 );
}