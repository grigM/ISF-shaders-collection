/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "procedural",
    "3d",
    "noise",
    "sun",
    "clouds",
    "ice",
    "snow",
    "lake",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MtlSz8 by Xor.  Use the mouse to move scene.\nA simple 3D scene. The code definitely could be clean up.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


#define time TIME
float rand(float n)
{
 	return fract(abs(sin(n*5.3357))*256.75+0.325);   
}
float rand(vec2 n)
{
 	return fract(abs(sin(dot(n,vec2(5.3357,-5.8464))))*256.75+0.325);   
}
float noise(vec2 n)
{
    vec2 fn = floor(n);
    vec2 sn = smoothstep(vec2(0.0),vec2(1.0),fract(n));
    float h1 = mix(rand(fn),rand(fn+vec2(1.0,0.0)),sn.x);
    float h2 = mix(rand(fn+vec2(0.0,1.0)),rand(fn+vec2(1.0,1.0)),sn.x);
    float s1 = mix(h1,h2,sn.y);
    return s1;
}
float noisetile(float n, float s)
{
    return mix(rand(mod(floor(n),s)),rand(mod(ceil(n),s)),smoothstep(0.0,1.0,fract(mod(n,s))));
}
float noisetile(vec2 n, vec2 s)
{
    float n1 = noisetile(n.x,s.x);
    float n2 = noisetile(n.y+n.x,s.y);
    return mix(n1,n2,0.5);
}
void doCamera( out vec3 camPos, out vec3 camTar, in float time, in float mouseX )
{
    vec2 dir = vec2(0.5,0.5) * 6.2831;
    if (dot(iMouse.xy,vec2(1.0))>0.0)
    {
    	dir = ((iMouse.xy/RENDERSIZE.xy-0.5) * vec2(1.0,0.5)) * 6.2831;
    }
    vec3 pos = vec3(noise(vec2(0.02,0.01)*TIME)*20.0,0.5,TIME*0.2);
	camPos =  pos;
    camTar = pos+vec3(cos(dir.x)*cos(dir.y),sin(dir.y),sin(dir.x)*cos(dir.y));
}
vec3 doBackground( in vec3 dir)
{
    float sky = dot(dir,vec3(0.0,-1.0,0.0))*0.5+0.5;
    float sun = pow(dot(dir,normalize(vec3(0.2,0.2,0.8)))*0.5+0.5,64.0);
    vec2 p = dir.xz/dir.y+vec2(TIME*0.05,0.0);
    float clouds = (noise(p*6.0)*0.5+noise(p*16.0)*0.3+noise(p*32.0)*0.15+
                    noise(p*64.0)*0.05) * noise(p*2.0) * pow(1.0-sky,4.0);
    
    vec2 angle = vec2(atan(dir.z,dir.x),atan(dir.y,1.0)) / 3.14159 * 180.0;
    float mtn = noisetile(angle.x/12.0,30.0)*0.14+noisetile(angle.x/4.0,90.0)*0.03
        +noisetile(angle.x/2.0,180.0)*0.02+noisetile(angle.x,360.0)*0.01-dir.y;
    
    vec3 mtncol = mix(vec3(0.3,0.7,0.8),vec3(1.0),noisetile(angle/2.0,vec2(180.0))*0.3+
                      noisetile(360.0-angle,vec2(360.0))*0.2+0.5);
    
    
    vec3 above = vec3(sky*0.6+0.05+sun,sky*0.8+0.075+pow(sun,1.5),sky+0.2+pow(sun,4.0))+clouds;
    vec3 below = vec3(0.0,0.0,0.02);
    return mix(mix(above,mtncol,clamp(floor(mtn*32.0),0.0,1.0)),below,clamp((sky-0.6)*64.0,0.0,1.0));
}
    
float doModel( vec3 p )
{
    float snow = min(pow(noise(p.xz)*0.6+noise(p.xz*2.0)*0.25+noise(p.xz*4.0)*0.15+0.4,64.0),1.0)*0.5+0.5;
    
    float height = 0.1*pow(noise(p.xz)*0.5+noise(p.xz*8.0)*0.32+noise(p.xz*16.0)*0.1+
    noise(p.xz*32.0)*0.05+noise(p.xz*64.0)*0.02+noise(p.xz*128.0)*0.01,4.0)*noise(p.xz/64.0);
    
    float model = p.y-height/snow;
    return model;
}
vec3 doMaterial(in vec3 p, in vec3 rd, in vec3 nor )
{
    float snow = smoothstep(0.48,0.52,noise(p.xz)*0.6+noise(p.xz*2.0)*0.25+noise(p.xz*4.0)*0.15);
    float v = noise(p.xz*2.5)*0.5+0.5;
    vec3 ref = doBackground(reflect(rd,nor));
    vec3 ice = mix(vec3(0.2,0.5,0.6) * noise(p.xz),vec3(0.9-noise(p.xz*16.0)*0.06-noise(p.xz*32.0)*0.04)*snow,snow);
    return mix(ice,ref,clamp(dot(ref,vec3(1.0/3.0))*1.5,0.0,(1.0-snow)*v));
}
float calcSoftshadow( in vec3 ro, in vec3 rd );

vec3 doFog( in vec3 rd, in float dis, in vec3 mal )
{
    vec3 col = mal;
	col = mix(doBackground(rd),col,1.0-clamp(dis*dis/90.0,0.0,1.0));

    return col;
}

float calcIntersection( in vec3 ro, in vec3 rd )
{
	const float maxd = 10.0;           // max trace distance
	const float precis = 0.001;        // precission of the intersection
    float h = precis*2.0;
    float t = 0.0;
	float res = -1.0;
    for( int i=0; i<90; i++ )          // max number of raymarching iterations is 90
    {
        if( h<precis||t>maxd ) break;
	    h = doModel( ro+rd*t );
        t += h*.8;
    }

    if( t<maxd ) res = t;
    return res;
}

vec3 calcNormal( in vec3 pos )
{
    const float eps = 0.002;             // precision of the normal computation

    const vec3 v1 = vec3( 1.0,-1.0,-1.0);
    const vec3 v2 = vec3(-1.0,-1.0, 1.0);
    const vec3 v3 = vec3(-1.0, 1.0,-1.0);
    const vec3 v4 = vec3( 1.0, 1.0, 1.0);

	return normalize( v1*doModel( pos + v1*eps ) + 
					  v2*doModel( pos + v2*eps ) + 
					  v3*doModel( pos + v3*eps ) + 
					  v4*doModel( pos + v4*eps ) );
}

float calcSoftshadow( in vec3 ro, in vec3 rd )
{
    float res = 1.0;
    float t = 0.5;                 // selfintersection avoidance distance
	float h = 1.0;
    for( int i=0; i<40; i++ )         // 40 is the max numnber of raymarching steps
    {
        h = doModel(ro + rd*t);
        res = min( res, 64.0*h/t );   // 64 is the hardness of the shadows
		t += clamp( h, 0.02, 2.0 );   // limit the max and min stepping distances
    }
    return clamp(res,0.0,1.0);
}

mat3 calcLookAtMatrix( in vec3 ro, in vec3 ta, in float roll )
{
    vec3 ww = normalize( ta - ro );
    vec3 uu = normalize( cross(ww,vec3(sin(roll),cos(roll),0.0) ) );
    vec3 vv = normalize( cross(uu,ww));
    return mat3( uu, vv, ww );
}

void main() {



    vec2 p = (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy)/RENDERSIZE.y;
    vec2 m = iMouse.xy/RENDERSIZE.xy;
    
    // camera movement
    vec3 ro, ta;
    doCamera( ro, ta, TIME, m.x );
    // camera matrix
    mat3 camMat = calcLookAtMatrix( ro, ta, 0.0 );  // 0.0 is the camera roll
    
	// create view ray
	vec3 rd = normalize( camMat * vec3(p.xy,2.0) ); // 2.0 is the lens length
	vec3 col = doBackground(rd);
	// raymarch
    float t = calcIntersection( ro, rd );
    if( t>-0.5 )
    {
        // geometry
        vec3 pos = ro + t*rd;
        vec3 nor = calcNormal(pos);
        // materials
        vec3 mal = doMaterial(pos, rd, nor );
        col = doFog( rd, t, mal );
	}
    // gamma
	col = pow( clamp(col,0.0,1.0), vec3(0.4545) );
	   
    gl_FragColor = vec4( col, 1.0 );
}
