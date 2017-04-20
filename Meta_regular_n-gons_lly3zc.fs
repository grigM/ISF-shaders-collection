/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "metaball",
    "learning",
    "gooey",
    "metacube",
    "metapolygon",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/lly3zc by twitchingace.  Mucking about, trying to implement regular meta n-gons.",
  "INPUTS" : [

  ]
}
*/


/////////////////////////////////////////////////////////////////////////////
// Set COL to 0. to see the shapes colorized.
// Use ISSOLID in conjunction with ISTHRESHOLDto determine if 
// the shape should be filled in.
/////////////////////////////////////////////////////////////////////////////

#define COL 1.
#define ISSOLID 0
#define ISTHRESHOLD 1
#define PI 3.14159
#define MAXSIDES 10

struct metaball{
    vec2 pos;
    vec3 colour;
    float radius;
    
};    
struct metangon{
	vec2 pos;
    int numSides;
    vec3 colour;
    float radius;
};
    
vec2 rotate(in vec2 vec, in float rot){
    vec2 newVec;
	newVec.x = vec.x * cos(rot) - vec.y * sin(rot);
	newVec.y = vec.x * sin(rot) + vec.y * cos(rot); 
    return newVec;
}

vec3 doMetangon(in metangon mngon, in vec2 testPoint, in float rot){
    // Basically, we want to fake a "distance" to use by finding the projection
    // of the vector from the center of the shape to the testPoint onto the
    // normal (scaled by "radius") of each side, and take the biggest one.
    
    vec2 testVec = testPoint - mngon.pos;   
    vec2 sideNormal = vec2(0., mngon.radius);
    sideNormal = rotate(sideNormal, rot);    
    float maxDist = dot(testVec, normalize(sideNormal));    
    for (int i = 1; i < MAXSIDES; i++){
        // A silly hack to get around the need for constant loop iterations
        if (i >= mngon.numSides){
            break;
        }            
        sideNormal = rotate(sideNormal, radians(360./ float(mngon.numSides))); 
        maxDist = max(maxDist, dot(testVec, normalize(sideNormal)));
    }
        
    return mngon.colour * mngon.radius / maxDist;
}

vec3 doMetaball(in metaball mball, in vec2 testPoint){
    vec2 pos = mball.pos;
	return mball.colour *  mball.radius/length(pos - testPoint);
}

void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xx;
    
    metaball ball1 = metaball(vec2(0.5+ .5*cos(TIME + 0.4), 0.25 + .2*sin(TIME)),
                              vec3(COL,1.,1.), .1);
    metangon cube1 = metangon(vec2(0.5 + .5 * sin(TIME), 0.15), 4,
                              vec3(1.,1., COL), .07);
    metangon tri1 = metangon(vec2(abs(0.3 + .5 * sin(TIME)), 0.4 + .2 * cos(TIME)), 3,
                              vec3(1.,COL, 1.), .07);
    metaball ball2 = metaball(vec2(abs(0.1 + sin(TIME)), 0.2 + 0.2 * cos(TIME)), 
                              vec3(1., COL, COL), 0.05);    
    metangon pentagon1 = metangon(vec2(0.5, abs(0.3 + .2 * sin(TIME * 0.7 + 5.))), 5,
                              vec3(COL, 1., COL), .075);    
    metangon septagon1 = metangon(vec2(abs(0.4 + sin(TIME)), 0.4 - 0.2 * cos(TIME)), 7,
                              vec3(1., 1., 1.), 0.06);
    
    vec3 rgb = vec3(0.);
    rgb += doMetaball(ball2, uv);
    rgb += doMetaball(ball1, uv);
    rgb += doMetangon(cube1, uv, TIME * 1.1 + 0.3);
	rgb += doMetangon(tri1, uv, TIME);
    rgb += doMetangon(pentagon1, uv, -TIME);
    rgb += doMetangon(septagon1, uv, 0.);                            
    rgb /= 1.75;
    
    #if ISTHRESHOLD == 1
    float threshold = 1.;
    if (rgb.x <= threshold){
        rgb.x = 0.;
    }
    if (rgb.y <= threshold){
        rgb.y = 0.;
    }
    if (rgb.z <= threshold){
        rgb.z = 0.;
    }    
    #if ISSOLID == 0
    if (rgb.x > threshold + 0.1 || 
        	rgb.y > threshold + 0.1 ||
        	rgb.z > threshold + 0.1){
        rgb = vec3(0.);
    }
    #endif
    #endif
    
	gl_FragColor = vec4(rgb ,1.0);
}