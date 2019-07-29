/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/wtfGR8 by TekF.  Inspired by this: https://twitter.com/sasj_nl/status/1118844071756357633",
    "IMPORTED": {
    },
    "INPUTS": [
    ]
}

*/


// Created by Hazel Quantock 2019
// This work is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. http://creativecommons.org/licenses/by-nc-sa/4.0/


const int numCubes = 32;
const float twistStep = .06;
const float scaleStep = 0.96;
const float zoom = 3.7;
const float lineThickness = 1.2; // in pixels

/*
// alternative settings for full-screen
const int numCubes = 64;
const float twistStep = .03;
const float scaleStep = 0.98;
const float zoom = 2.8;
const float lineThickness = .8; // in pixels
*/


// input in range [-1,1] to span RENDERSIZE.y pixels
float RenderLine( vec2 a, vec2 b, vec2 fragCoord )
{
    a = (RENDERSIZE.y*a + RENDERSIZE.xy)*.5;
    b = (RENDERSIZE.y*b + RENDERSIZE.xy)*.5;
    
    const float halfThickness = lineThickness*.5; 

    const float halfAASoftening = .7; // in pixels (don't change this much)
    
    float t = dot(fragCoord-a,b-a);
    t /= dot(b-a,b-a);
    t = clamp( t, 0., 1. );
    return smoothstep( halfThickness-halfAASoftening, halfThickness+halfAASoftening, length(fragCoord - mix(a,b,t)) );
}

    
float RenderLine3D( vec3 a, vec3 b, vec2 fragCoord )
{
    vec3 camPos = vec3(0,0,-5);
    
    a -= camPos;
    b -= camPos;
    
    // todo: transform by camera matrix

    a.z /= zoom;
    b.z /= zoom;
    
    // perspective projection
    return RenderLine( a.xy/a.z, b.xy/b.z, fragCoord );
}


// combine 2 anti-aliased values
float BlendAA( float a, float b )
{
    // a and b values represent what proportion of the pixel is covered by each line,
    // but they don't contain enough information to accurately combine them!
    // if both lines are covering the same part of the pixel the result should be min(a,b)
    // if they cover non-overlapping parts of the pixel the result is a-(1-b)
	// a*b assumes the proportion of overlap is the same in the solid and clear regions
    // this is the safest assumption given the lack of any other info

    // but, tune it until it looks good
    return mix( min(a,b), a*b, .5 );
}


void main() {



    gl_FragColor.rgb = vec3(.8);
    
    vec3 a = vec3(twistStep*cos(TIME*3./vec3(11,13,17)+1.5));
    mat3 stepTransform =
        scaleStep *
        mat3( cos(a.z), sin(a.z), 0,
             -sin(a.z), cos(a.z), 0,
              0, 0, 1 ) *
        mat3( cos(a.y), 0, sin(a.y),
             0, 1, 0,
             -sin(a.y), 0, cos(a.y) ) *
        mat3( 1, 0, 0,
              0, cos(a.x), sin(a.x),
              0,-sin(a.x), cos(a.x) );
    vec3 b = vec3(.7+TIME/6.,.7+TIME/6.,.6);
    mat3 transform = //mat3(1,0,0,0,1,0,0,0,1); // identity
        mat3( cos(b.z), sin(b.z), 0,
             -sin(b.z), cos(b.z), 0,
              0, 0, 1 ) *
        mat3( cos(b.y), 0, sin(b.y),
             0, 1, 0,
             -sin(b.y), 0, cos(b.y) ) *
        mat3( 1, 0, 0,
              0, cos(b.x), sin(b.x),
              0,-sin(b.x), cos(b.x) );
    float line = 1.;
    #define DrawLine(a,b) line = BlendAA( line, RenderLine3D(a,b,gl_FragCoord.xy) );
    
    for ( int cube=0; cube < numCubes; cube++ )
    {
        vec3 vertices[8];
        for ( int i=0; i < 8; i++ )
        {
            vertices[i] = transform*(vec3(i>>2,(i>>1)&1,i&1)*2.-1.);
        }
        
        DrawLine( vertices[0], vertices[1] );
        DrawLine( vertices[2], vertices[3] );
        DrawLine( vertices[4], vertices[5] );
        DrawLine( vertices[6], vertices[7] );
        DrawLine( vertices[0], vertices[2] );
        DrawLine( vertices[1], vertices[3] );
        DrawLine( vertices[4], vertices[6] );
        DrawLine( vertices[5], vertices[7] );
        DrawLine( vertices[0], vertices[4] );
        DrawLine( vertices[1], vertices[5] );
        DrawLine( vertices[2], vertices[6] );
        DrawLine( vertices[3], vertices[7] );
        
        transform *= stepTransform;
    }
    
    gl_FragColor.rgb = mix( vec3(.02), vec3(.8), line );
    
    gl_FragColor.rgb = pow(gl_FragColor.rgb,vec3(1./2.2));
    gl_FragColor.a = 1.;
}
