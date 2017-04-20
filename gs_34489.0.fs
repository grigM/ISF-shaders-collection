/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
	{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 10.0
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34489.0"
}
*/


// Created by inigo quilez - iq/2014
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

// Ray-Box intersection, by convertig the ray to the local space of the box.
//
// If this was used to raytace many equally orietned boxes (saym you are traversing a BVH,
// then the transformations in line 15 and 16, and the computations of m and n could be
// precomputed for the whole set of cubes. You probably wouldn't need line 31 and 35 either.
#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable




// returns t and normal
vec4 iBox( in vec3 ro, in vec3 rd, in mat4 txx, in mat4 txi, in vec3 rad )
{
    // convert from ray to box space
    vec3 rdd = (txx*vec4(rd,0.0)).xyz;
    vec3 roo = (txx*vec4(ro,1.0)).xyz;

    // ray-box intersection in box space
    vec3 m = 1.0/rdd;
    vec3 n = m*roo;
    vec3 k = abs(m)*rad;

    vec3 t1 = -n - k;
    vec3 t2 = -n + k;

    float tN = max( max( t1.x, t1.y ), t1.z );
    float tF = min( min( t2.x, t2.y ), t2.z );

    if( tN > tF || tF < 0.0) return vec4(-1.0);

    vec3 nor = -sign(rdd)*step(t1.yzx,t1.xyz)*step(t1.zxy,t1.xyz);

    // convert to ray space

    nor = (txi * vec4(nor,0.0)).xyz;

    return vec4( tN, nor );
}


float sBox( in vec3 ro, in vec3 rd, in mat4 txx, in vec3 rad )
{
    vec3 rdd = (txx*vec4(rd,0.0)).xyz;
    vec3 roo = (txx*vec4(ro,1.0)).xyz;

    vec3 m = 1.0/rdd;
    vec3 n = m*roo;
    vec3 k = abs(m)*rad;

    vec3 t1 = -n - k;
    vec3 t2 = -n + k;

    float tN = max( max( t1.x, t1.y ), t1.z );
    float tF = min( min( t2.x, t2.y ), t2.z );
    if( tN > tF || tF < 0.0) return -1.0;

    return tN;
}


//-----------------------------------------------------------------------------------------

mat4 rotationAxisAngle( vec3 v, float angle )
{
    float s = sin( angle );
    float c = cos( angle );
    float ic = 1.0 - c;

    return mat4( v.x*v.x*ic + c,     v.y*v.x*ic - s*v.z, v.z*v.x*ic + s*v.y, 0.0,
                 v.x*v.y*ic + s*v.z, v.y*v.y*ic + c,     v.z*v.y*ic - s*v.x, 0.0,
                 v.x*v.z*ic - s*v.y, v.y*v.z*ic + s*v.x, v.z*v.z*ic + c,     0.0,
                 0.0,                0.0,                0.0,                1.0 );
}

mat4 translate( float x, float y, float z )
{
    return mat4( 1.0, 0.0, 0.0, 0.0,
                 0.0, 1.0, 0.0, 0.0,
                 0.0, 0.0, 1.0, 0.0,
                 x,   y,   z,   1.0 );
}

mat4 inverse( in mat4 m )
{
    return mat4(
        m[0][0], m[1][0], m[2][0], 0.0,
        m[0][1], m[1][1], m[2][1], 0.0,
        m[0][2], m[1][2], m[2][2], 0.0,
        -dot(m[0].xyz,m[3].xyz),
        -dot(m[1].xyz,m[3].xyz),
        -dot(m[2].xyz,m[3].xyz),
        1.0 );
}

void main()
{
    vec2 p = (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy) / RENDERSIZE.y;

     // camera movement   
    float an = 0.4*TIME*speed;
    vec3 ro = vec3( 2.5*cos(an), 1.0, 2.5*sin(an) );
    vec3 ta = vec3( 0.0, 0.8, 0.0 );
    // camera matrix
    vec3 ww = normalize( ta - ro );
    vec3 uu = normalize( cross(ww,vec3(0.0,1.0,0.0) ) );
    vec3 vv = normalize( cross(uu,ww));
    // create view ray
    vec3 rd = normalize( p.x*uu + p.y*vv + 2.0*ww );

    // rotate and translate box   
    mat4 rot = rotationAxisAngle( normalize(vec3(1.0,1.0,0.0)), TIME*speed );
    mat4 tra = translate( 0.0, 1.0, 0.0 );
    mat4 txi = tra * rot;
    mat4 txx = inverse( txi );

    // raytrace
    float tmin = 10000.0;
    vec3  nor = vec3(0.0);
    vec3  pos = vec3(0.0);


    // raytrace box
    vec3 box = vec3(0.4,0.6,0.8) ;
    vec4 res = iBox( ro, rd, txx, txi, box);
    if( res.x>0.0 && res.x<tmin )
    {
        tmin = res.x;
        nor = res.yzw;
        //nor *= res.wzy;

    }

    // shading/lighting   
    vec3 col = vec3(0.0);
    float alpha = 0.0;
    if( tmin<100.0 )
    {
        vec3 lig = normalize(vec3(-0.8,0.4,0.1));
        pos = ro + tmin*rd;

        // material
        float occ = 1.0;
        vec3  mate = vec3(1.0);

            // recover box space data (we want to do shading in object space)           
            vec3 opos = (txx*vec4(pos,1.0)).xyz;
            vec3 onor = (txx*vec4(nor,0.0)).xyz;


            // wireframe
            mate *= 1.0 - (1.0-abs(onor.x))*smoothstep( box.x-0.04, box.x-0.02, abs(opos.x) );
            mate *= 1.0 - (1.0-abs(onor.y))*smoothstep( box.y-0.04, box.y-0.02, abs(opos.y) );
            mate *= 1.0 - (1.0-abs(onor.z))*smoothstep( box.z-0.04, box.z-0.02, abs(opos.z) );

            occ = 0.6 + 0.4*nor.y;

        mate = mate*mate*1.5;

        // lighting
        float dif = clamp( dot(nor,lig), 0.0, 1.0 );
        dif *= step( sBox( pos+0.01*nor, lig, txx, box ), 0.0 );
        col = vec3(0.13,0.17,0.2)*occ*3.0 + 1.5*dif*vec3(1.0,0.9,0.8);

        // material * lighting       
        col *= mate;
		alpha = 1.0;
        // fog
         col = mix( col, vec3(0.9), 1.0-exp( -0.003*tmin*tmin ) );
    }

     col = sqrt( col );

    gl_FragColor = vec4( col, alpha );
}