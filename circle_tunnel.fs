/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Mlf3DB by hughsk.  A tunnel is of course obligatory :)",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    },
    
    {
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 3.0
		},
		{
			"NAME": "warp",
			"TYPE": "float",
			"DEFAULT": 0.25,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "x_shift",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -5.0,
			"MAX": 5.0
		},
		{
			"NAME": "y_shift",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -5.0,
			"MAX": 5.0
		},
		{
			"NAME": "torus_size",
			"TYPE": "float",
			"DEFAULT": 1.5,
			"MIN": 0.0,
			"MAX": 3.0
		},
		{
			"NAME": "torus_lenth",
			"TYPE": "float",
			"DEFAULT": 0.155,
			"MIN": 0.03,
			"MAX": 0.9
		},
		{
			"NAME": "light_fade",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": 0.0,
			"MAX": 0.9
		},
		{
			"NAME": "lens_length",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 1.0,
			"MAX": 10.0
		},
		
		
		{
			"NAME": "cam_y_rot",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -3.0,
			"MAX": 3.0
		},
		{
			"NAME": "cam_z_rot",
			"TYPE": "float",
			"DEFAULT": 3.5,
			"MIN": 0.2,
			"MAX": 3.5
		}
		 
		
		
  ]
}
*/


float sdTorus82( vec3 p, vec2 t )
{
  vec2 q = vec2(length(p.xz)-t.x,p.y);
  return length(q)-t.y;
}

float fogFactorExp2(
  const float dist,
  const float density
) {
  const float LOG2 = -1.442695;
  float d = density * dist;
  return 1.0 - clamp(exp2(d * d * LOG2), 0.0, 1.0);
}

//------------------------------------------------------------------------
// Camera
//
// Move the camera. In this case it's using time and the mouse position
// to orbitate the camera around the origin of the world (0,0,0), where
// the yellow sphere is.
//------------------------------------------------------------------------
void doCamera( out vec3 camPos, out vec3 camTar, in float time, in float mouseX )
{
    float an = 0.0;
	camPos = vec3(3.5*sin(an), cam_y_rot, cam_z_rot*cos(an));
    camTar = vec3(0.0,0.0,0.0);
}


//------------------------------------------------------------------------
// Background 
//
// The background color. In this case it's just a black color.
//------------------------------------------------------------------------
vec3 doBackground( void )
{
    return vec3(0.02, 0.01, 0.03);
}
    
//------------------------------------------------------------------------
// Modelling 
//
// Defines the shapes (a sphere in this case) through a distance field, in
// this case it's a sphere of radius 1.
//------------------------------------------------------------------------
float doModel( vec3 p )
{
    float d = 10000000.0;
    vec2 off = vec2(1.0, 0.0);
    vec3 origin = vec3(x_shift, 0.0, y_shift-0.5);
    float move = (TIME*speed) * -4.0;
    //float warp = 0.25;
    
    for (int i = 0; i < 15; i++) {
        float I = float(i) - 5.0;
        float J = I - floor(move);
        vec3 P = p.xzy;
        
        P += off.yxy * I;
        P += origin;
        P += vec3(sin(J * 0.5 + move) * warp, mod(move, 1.0), cos(J * 0.9 + move) * warp);
        
        d = min(d, sdTorus82(P, vec2(torus_size, torus_lenth)));
    }
    
    return d;
}

//------------------------------------------------------------------------
// Material 
//
// Defines the material (colors, shading, pattern, texturing) of the model
// at every point based on its position and normal. In this case, it simply
// returns a constant yellow color.
//------------------------------------------------------------------------
vec3 doMaterial( in vec3 pos, in vec3 nor )
{
    return vec3(0.5, 0.5, 0.5);
}

//------------------------------------------------------------------------
// Lighting
//------------------------------------------------------------------------
float calcSoftshadow( in vec3 ro, in vec3 rd );

vec3 doLighting( in vec3 pos, in vec3 nor, in vec3 rd, in float dis, in vec3 mal )
{
    vec3 lin = vec3(0.0);

    // key light
    //-----------------------------
    vec3  lig = normalize(vec3(0.0, 0.0, 1.5)-pos);
    float dif = max(dot(nor,lig),0.0);
    float sha = 0.0; if( dif>0.01 ) sha=calcSoftshadow( pos+0.01*nor, lig );
    lin += dif*vec3(4.00,4.00,4.00)*sha;

    // ambient light
    //-----------------------------
    lin += vec3(0.50,0.50,0.50);

    
    // surface-light interacion
    //-----------------------------
    vec3 col = mal*lin;

    
    // fog    
    //-----------------------------
	//col *= exp(-0.03*dis*dis);

    return col;
}

float calcIntersection( in vec3 ro, in vec3 rd )
{
	const float maxd = 25.0;           // max trace distance
	const float precis = 0.001;        // precission of the intersection
    float h = precis*2.0;
    float t = 0.0;
	float res = -1.0;
    for( int i=0; i<90; i++ )          // max number of raymarching iterations is 90
    {
        if( h<precis||t>maxd ) break;
	    h = doModel( ro+rd*t );
        t += h;
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
    float t = 0.0005;                 // selfintersection avoidance distance
	float h = 1.0;
    for( int i=0; i<8; i++ )         // 40 is the max numnber of raymarching steps
    {
        h = doModel(ro + rd*t);
        res = min( res, 8.0*h/t );   // 64 is the hardness of the shadows
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
    //-----------------------------------------------------
    // camera
    //-----------------------------------------------------
    
    // camera movement
    vec3 ro, ta;
    doCamera( ro, ta, (TIME*speed), m.x );
    // camera matrix
    mat3 camMat = calcLookAtMatrix( ro, ta, 0.0 );  // 0.0 is the camera roll
    
	// create view ray
	vec3 rd = normalize( camMat * vec3(p.xy,lens_length) ); // 2.0 is the lens length
    //-----------------------------------------------------
	// render
    //-----------------------------------------------------
	vec3 col = doBackground();
	// raymarch
    float t = calcIntersection( ro, rd );
    if( t>-0.5 )
    {
        // geometry
        vec3 pos = ro + t*rd;
        vec3 nor = calcNormal(pos);
        // materials
        vec3 mal = doMaterial( pos, nor );
        col = mix(col, doLighting( pos, nor, rd, t, mal ), 1.0 - fogFactorExp2(t, light_fade));
	}
	//-----------------------------------------------------
	// postprocessing
    //-----------------------------------------------------
    // gamma
	col = pow( clamp(col,0.0,1.0), vec3(0.4545) );
    col.r = smoothstep(0., 0.9, col.r);
    col.g = smoothstep(0., 0.9, col.g);
    col.b = smoothstep(0., 0.9, col.b);
	   
    gl_FragColor = vec4( col, 1.0 );
}
