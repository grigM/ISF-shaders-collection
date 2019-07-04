/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wlj3zV by iq.  A sequence of connected torus sections (not a truchet or any other cell based acceleration structure). It's really a tested for the torus section SDF in [url]https:\/\/www.shadertoy.com\/view\/tl23RK[url]",
  "INPUTS" : [

  ]
}
*/


// Created by inigo quilez - iq/2019
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.


#define AA 1

// https://www.shadertoy.com/view/tl23RK
float sdSqCappedTorus(in vec3 p, in vec2 sc, in float ra)
{
    p.x = abs(p.x);
    float k = (sc.y*p.x>sc.x*p.y) ? dot(p.xy,sc) : length(p.xy);
    return dot(p,p) + ra*ra - 2.0*ra*k;
}

vec3 hash( float n )
{
    vec3 m = n*vec3(23.0,41.0,17.0) + vec3(9.0,1.0,31.0);
    return fract( m*fract( m*0.3183099 ) );
}

vec2 map( in vec3 pos )
{
    vec3 pp = vec3(0.0);
    vec3 ww = vec3(0.0,1.0,0.0);
    
    float d = length(pos-pp);
    
    vec4 data = vec4(0.0);
                   
    for( int i=0; i<32; i++ )
    {
        // segment parameters        
        vec3 ran = hash(float(i));
        float ra = 0.13 + 0.08*ran.x; // radius
        float ap = 1.10 + 0.80*ran.y; // aperture
        vec3  up = normalize( sin(75.0*ran.z+vec3(0.0,1.0,4.0))); // orientation

        // world to torus transformation
        vec2 sc = vec2(sin(ap),cos(ap));
        vec3 ou = normalize(cross(ww,up));
        vec3 vv = cross(ou,ww);
        vec3 uu =  sc.x*ou + sc.y*ww;
             ww = -sc.y*ou + sc.x*ww;
        vec3 cpos = (pos-pp)*mat3(uu,ww,vv) + ra*vec3(-sc.x,sc.y,0.0);
        
        // distance evaluation        
        float tmp = sdSqCappedTorus(cpos, sc, ra );
        if( tmp<d )
        {
            d = tmp;
            data = vec4( float(i), cpos.xy, ap );
        }
        
        // prepare next segment        
        pp += 2.0*ra*sc.x*uu;
        ww = sc.y*uu - sc.x*ww;
    }
    
    return vec2(sqrt(d) - 0.035, // distance
                data.x + 0.5+0.5*(atan(data.y,data.z))/data.w // u
               );
}

// http://iquilezles.org/www/articles/normalsSDF/normalsSDF.htm
vec3 calcNormal( in vec3 pos )
{
    vec2 e = vec2(1.0,-1.0)*0.5773;
    const float eps = 0.001;
    return normalize( e.xyy*map( pos + e.xyy*eps ).x + 
					  e.yyx*map( pos + e.yyx*eps ).x + 
					  e.yxy*map( pos + e.yxy*eps ).x + 
					  e.xxx*map( pos + e.xxx*eps ).x );
}

#define ZERO min(FRAMEINDEX,0)

float calcAO( in vec3 pos, in vec3 nor )
{
	float ao = 0.0;

	vec3 v = normalize(vec3(0.7,0.5,0.2));
	for( int i=0; i<12; i++ )
	{
		float h = abs(sin(float(i)));
		vec3 kv = v + 2.0*nor*max(0.0,-dot(nor,v));
		ao += clamp( map(pos+nor*0.01+kv*h*0.2).x*3.0, 0.0, 1.0 );
		v = v.yzx; 
		//if( (i & 2)==2) v.yz *= -1.0;
	}
	ao /= 12.0;
	ao = ao + 2.0*ao*ao;
	return clamp( ao*2.5, 0.0, 1.0 );
}

void main() {



    // camera movement	
	float an = 0.15*(TIME-8.0);
    vec3 ta = vec3( 0.25, -0.06, -0.75 );
	vec3 ro = ta + vec3( 1.7*cos(an), 0.6, 1.7*sin(an) );
    // camera matrix
    vec3 ww = normalize( ta - ro );
    vec3 uu = normalize( cross(ww,vec3(0.0,1.0,0.0) ) );
    vec3 vv =          ( cross(uu,ww));
    
    // render
    vec3 tot = vec3(0.0);
    
    #if AA>1
    for( int m=ZERO; m<AA; m++ )
    for( int n=ZERO; n<AA; n++ )
    {
        // pixel coordinates
        vec2 o = vec2(float(m),float(n)) / float(AA) - 0.5;
        vec2 p = (-RENDERSIZE.xy + 2.0*(gl_FragCoord.xy+o))/RENDERSIZE.y;
        #else    
        vec2 p = (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy)/RENDERSIZE.y;
        #endif
	    // create view ray
        vec3 rd = normalize( p.x*uu + p.y*vv + 1.6*ww );
        // raymarch
        const float tmax = 3.5;
        float t = 0.5;
        float m = 0.0;
        for( int i=0; i<256; i++ )
        {
            vec3 pos = ro + t*rd;
            vec2 h = map(pos);
            m = h.y;
            if( h.x<0.001 || t>tmax ) break;
            t += h.x;
        }
    
        // shade/light
        vec3 col = vec3(0.1 - 0.015*length(p) + 0.05*rd.y );
        if( t<tmax )
        {
            vec3 pos = ro + t*rd;
            vec3 nor = calcNormal(pos);
            float fre = clamp(1.0+dot(nor,rd),0.0,1.0);
			float occ = calcAO(pos, nor);
            float amb = 0.5 + 0.5*nor.y;
            // material
            vec3 mat = 0.5 + 0.5*cos( m*0.06 + vec3(0.00,1.0,1.8) + 1.0 );
            mat += 0.05*nor;
            // lighting
            col = mat*1.5*occ*vec3(amb+fre*fre*col*0.4);
            //col *= 4.0*smoothstep( 0.9, 1.0, sin(1.0*m+TIME*3.0) );
            col *= 1.0-smoothstep( 0.98, 1.0, sin(1.0*m+TIME*3.0) );
            col += fre*occ*0.5*vec3(0.5,0.7,1.0)*smoothstep(0.0,0.1,reflect(rd,nor).y);
        }
        // gamma        
        col = sqrt( col );
	    tot += col;
    #if AA>1
    }
    tot /= float(AA*AA);
    #endif
    // cheap dithering
    tot += sin(gl_FragCoord.x*114.0)*sin(gl_FragCoord.y*211.1)/512.0;
    
	gl_FragColor = vec4( tot, 1.0 );
}
