/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "glitch",
    "blend",
    "icosahedron",
    "dodecahedron",
    "transform",
    "twist",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtyXzW by tdhooper.  The glitch effect moves with the model, pause and pan around to see.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


#define MODEL_ROTATION vec2(.5, .5)
#define LIGHT_ROTATION vec2(.3, .8)
#define CAMERA_ROTATION vec2(.5, .67)

// Mouse control
// 0: Defaults
// 1: Model
// 2: Lighting
// 3: Camera
#define MOUSE_CONTROL 1

// Debugging
//#define NORMALS
//#define NO_GLITCH
//#define GLITCH_MASK


float time;

float round(float n) {
    return floor(n + .5);
}

vec2 round(vec2 n) {
    return floor(n + .5);
}

// --------------------------------------------------------
// HG_SDF
// https://www.shadertoy.com/view/Xs3GRB
// --------------------------------------------------------

#define PI 3.14159265359
#define PHI (1.618033988749895)
#define TAU 6.283185307179586

float vmax(vec3 v) {
    return max(max(v.x, v.y), v.z);
}

// Rotate around a coordinate axis (i.e. in a plane perpendicular to that axis) by angle <a>.
// Read like this: R(p.xz, a) rotates "x towards z".
// This is fast if <a> is a compile-time constant and slower (but still practical) if not.
void pR(inout vec2 p, float a) {
    p = cos(a)*p + sin(a)*vec2(p.y, -p.x);
}

// Plane with normal n (n is normalized) at some distance from the origin
float fPlane(vec3 p, vec3 n, float distanceFromOrigin) {
    return dot(p, n) + distanceFromOrigin;
}

// Box: correct distance to corners
float fBox(vec3 p, vec3 b) {
    vec3 d = abs(p) - b;
    return length(max(d, vec3(0))) + vmax(min(d, vec3(0)));
}


#define GDFVector3 normalize(vec3(1, 1, 1 ))
#define GDFVector4 normalize(vec3(-1, 1, 1))
#define GDFVector5 normalize(vec3(1, -1, 1))
#define GDFVector6 normalize(vec3(1, 1, -1))

#define GDFVector7 normalize(vec3(0, 1, PHI+1.))
#define GDFVector8 normalize(vec3(0, -1, PHI+1.))
#define GDFVector9 normalize(vec3(PHI+1., 0, 1))
#define GDFVector10 normalize(vec3(-PHI-1., 0, 1))
#define GDFVector11 normalize(vec3(1, PHI+1., 0))
#define GDFVector12 normalize(vec3(-1, PHI+1., 0))

#define GDFVector13 normalize(vec3(0, PHI, 1))
#define GDFVector14 normalize(vec3(0, -PHI, 1))
#define GDFVector15 normalize(vec3(1, 0, PHI))
#define GDFVector16 normalize(vec3(-1, 0, PHI))
#define GDFVector17 normalize(vec3(PHI, 1, 0))
#define GDFVector18 normalize(vec3(-PHI, 1, 0))

#define fGDFBegin float d = 0.;
#define fGDF(v) d = max(d, abs(dot(p, v)));
#define fGDFEnd return d - r;

float fDodecahedron(vec3 p, float r) {
    fGDFBegin
    fGDF(GDFVector13) fGDF(GDFVector14) fGDF(GDFVector15) fGDF(GDFVector16)
    fGDF(GDFVector17) fGDF(GDFVector18)
    fGDFEnd
}

float fIcosahedron(vec3 p, float r) {
    fGDFBegin
    fGDF(GDFVector3) fGDF(GDFVector4) fGDF(GDFVector5) fGDF(GDFVector6)
    fGDF(GDFVector7) fGDF(GDFVector8) fGDF(GDFVector9) fGDF(GDFVector10)
    fGDF(GDFVector11) fGDF(GDFVector12)
    fGDFEnd
}


// --------------------------------------------------------
// Rotation
// --------------------------------------------------------

mat3 sphericalMatrix(float theta, float phi) {
    float cx = cos(theta);
    float cy = cos(phi);
    float sx = sin(theta);
    float sy = sin(phi);
    return mat3(
        cy, -sy * -sx, -sy * cx,
        0, cx, sx,
        sy, cy * -sx, cy * cx
    );
}

mat3 mouseRotation(bool enable, vec2 xy) {
    if (enable) {
        vec2 mouse = iMouse.xy / RENDERSIZE.xy;

        if (mouse.x != 0. && mouse.y != 0.) {
            xy.x = mouse.x;
            xy.y = mouse.y;
        }
    }
    float rx, ry;
    
    rx = (xy.y + .5) * PI;
    ry = (-xy.x) * 2. * PI;
    
    return sphericalMatrix(rx, ry);
}

mat3 modelRotation() {
    mat3 m = mouseRotation(MOUSE_CONTROL==1, MODEL_ROTATION);
    return m;
}

mat3 lightRotation() {
    mat3 m = mouseRotation(MOUSE_CONTROL==2, LIGHT_ROTATION);
    return m;
}

mat3 cameraRotation() {
    mat3 m = mouseRotation(MOUSE_CONTROL==3, CAMERA_ROTATION);
    return m;
}


// --------------------------------------------------------
// Modelling 
// --------------------------------------------------------

struct Material {
    vec3 albedo;
};

struct Model {
    float dist;
    Material material;
};

Material defaultMaterial = Material(
    vec3(.5)
);  

Model newModel() {
    return Model(
        10000.,
        defaultMaterial
    );
}

const float modelSize = 1.2;

float blend(float y, float blend, float progress) {
    float a = (y / modelSize) + .5;
    a -= progress * (1. + blend) - blend * .5;
    a += blend / 2.;
    a /= blend;
    a = clamp(a, 0., 1.);
    a = smoothstep(0., 1., a);
    a = smoothstep(0., 1., a);
    return a;
}

float ShapeBlend(float y, float progress) {
    float shapeProgress = clamp(progress * 2. - .5, 0., 1.);
    float shapeBlend = blend(y, .8, shapeProgress);
    return shapeBlend;
}

float SpinBlend(float y, float progress) {
    return blend(y, 1.5, progress);
}

float Flip() {
	return round(mod(time, 1.));
}

float Progress() {
    float progress = mod(time*2., 1.);
    //progress = smoothstep(0., 1., progress);
    //progress = sin(progress * PI - PI/2.) * .5 + .5;
    return progress;
}

Model mainModel(vec3 p) {
    Model model = newModel();
    
	float progress = Progress();
    float flip = Flip();
    
    float spinBlend = SpinBlend(p.y, progress);
    pR(p.xz, spinBlend * PI / 2.);
    pR(p.xz, PI * -.5 * flip);
    
    float part1 = fDodecahedron(p, modelSize * .5);
    pR(p.xz, PI/2.);
    float part2 = fIcosahedron(p, modelSize * .5);
    
	float shapeBlend = ShapeBlend(p.y, progress);
    shapeBlend = mix(shapeBlend, 1. - shapeBlend, flip);    
    float d = mix(part1, part2, shapeBlend);

    model.dist = d;
    model.material.albedo = mix(vec3(.03), vec3(.8), 1. - shapeBlend);

    return model;
}

Model glitchModel(vec3 p) {
    Model model = newModel();
    float progress = Progress();
	float band = ShapeBlend(p.y, progress);
    band = sin(band * PI);    

   	float fadeBottom = clamp(1. - dot(p, vec3(0,1,0)), 0., 1.);
    band *= fadeBottom;

    float radius = modelSize / 2. + band * .2;
    model.dist = length(p) - radius;
    model.material.albedo = vec3(band);
    
    return model;
}

Model map( vec3 p , bool glitchMask){
    mat3 m = modelRotation();
    p *= m;
    pR(p.xz, -time*PI);
    if (glitchMask) {
    	return glitchModel(p);
    }
    Model model = mainModel(p);
    return model;
}


// --------------------------------------------------------
// LIGHTING
// https://www.shadertoy.com/view/Xds3zN
// --------------------------------------------------------

float softshadow( in vec3 ro, in vec3 rd, in float mint, in float tmax )
{
    float res = 1.0;
    float t = mint;
    for( int i=0; i<16; i++ )
    {
        float h = map( ro + rd*t, false ).dist;
        res = min( res, 8.0*h/t );
        t += clamp( h, 0.02, 0.10 );
        if( h<0.00001 || t>tmax ) break;
    }
    return clamp( res, 0.0, 1.0 );
}

float calcAO( in vec3 pos, in vec3 nor )
{
    float occ = 0.0;
    float sca = 1.0;
    for( int i=0; i<5; i++ )
    {
        float hr = 0.01 + 0.12*float(i)/4.0;
        vec3 aopos =  nor * hr + pos;
        float dd = map( aopos, false ).dist;
        occ += -(dd-hr)*sca;
        sca *= 0.95;
    }
    return clamp( 1.0 - 3.0*occ, 0.0, 1.0 );    
}

vec3 doLighting(Material material, vec3 pos, vec3 nor, vec3 ref, vec3 rd) {
    vec3 lightPos = vec3(0,0,-1);
    vec3 backLightPos = normalize(vec3(0,-.3,1));
    vec3 ambientPos = vec3(0,1,0);

    mat3 m = lightRotation();
    lightPos *= m;
    backLightPos *= m;
        
    float occ = calcAO( pos, nor );
    vec3  lig = lightPos;
    float amb = clamp((dot(nor, ambientPos) + 1.) / 2., 0., 1.);
    float dif = clamp((dot(nor, lig) + 1.) / 3., 0.0, 1.0 );
    float bac = pow(clamp(dot(nor, backLightPos), 0., 1.), 1.5);
    float fre = pow( clamp(1.0+dot(nor,rd),0.0,1.0), 2.0 );
    
    dif *= softshadow( pos, lig, 0.01, 2.5 ) * .5 + .5;

    vec3 lin = vec3(0.0);
    lin += 1.20*dif*vec3(.95,0.80,0.60);
    lin += 0.80*amb*vec3(0.50,0.70,.80)*occ;
    lin += 0.30*bac*vec3(0.25,0.25,0.25)*occ;
    lin += 0.20*fre*vec3(1.00,1.00,1.00)*occ;
    vec3 col = material.albedo*lin;
    
	float spe = clamp(dot(ref, lightPos), 0., 1.);
    spe = pow(spe, 2.) * .1;
    col += spe;

    return col;
}   


// --------------------------------------------------------
// Ray Marching
// Adapted from: https://www.shadertoy.com/view/Xl2XWt
// --------------------------------------------------------

const float MAX_TRACE_DISTANCE = 30.; // max trace distance
const float INTERSECTION_PRECISION = .001; // precision of the intersection
const int NUM_OF_TRACE_STEPS = 100;
const float FUDGE_FACTOR = .9; // Default is 1, reduce to fix overshoots

struct CastRay {
    vec3 origin;
    vec3 direction;
    bool glitchMask;
};

struct Ray {
    vec3 origin;
    vec3 direction;
    float len;
};

struct Hit {
    Ray ray;
    Model model;
    vec3 pos;
    bool isBackground;
    vec3 normal;
    vec3 color;
};

vec3 calcNormal( in vec3 pos ){
    vec3 eps = vec3( 0.001, 0.0, 0.0 );
    vec3 nor = vec3(
        map(pos+eps.xyy, false).dist - map(pos-eps.xyy, false).dist,
        map(pos+eps.yxy, false).dist - map(pos-eps.yxy, false).dist,
        map(pos+eps.yyx, false).dist - map(pos-eps.yyx, false).dist );
    return normalize(nor);
}
    
Hit raymarch(CastRay castRay){

    float currentDist = INTERSECTION_PRECISION * 2.0;
    Model model;
    
    Ray ray = Ray(castRay.origin, castRay.direction, 0.);

    for( int i=0; i< NUM_OF_TRACE_STEPS ; i++ ){
        if (currentDist < INTERSECTION_PRECISION || ray.len > MAX_TRACE_DISTANCE) {
            break;
        }
        model = map(ray.origin + ray.direction * ray.len, castRay.glitchMask);
        currentDist = model.dist;
        ray.len += currentDist * FUDGE_FACTOR;
    }
    
    bool isBackground = false;
    vec3 pos = vec3(0);
    vec3 normal = vec3(0);
    vec3 color = vec3(0);
    
    if (ray.len > MAX_TRACE_DISTANCE) {
        isBackground = true;
    } else {
        pos = ray.origin + ray.direction * ray.len;
        normal = calcNormal(pos);
    }

    return Hit(ray, model, pos, isBackground, normal, color);
}


// --------------------------------------------------------
// Rendering
// Refraction from https://www.shadertoy.com/view/lsXGzH
// --------------------------------------------------------

void shadeSurface(inout Hit hit){

    vec3 color = vec3(.25);
    
    if (hit.isBackground) {
        hit.color = color;
        return;
    }

    #ifdef NORMALS
        color = hit.normal * 0.5 + 0.5;
    #else
    	vec3 ref = reflect(hit.ray.direction, hit.normal);
        color = doLighting(
            hit.model.material,
            hit.pos,
            hit.normal,
            ref,
            hit.ray.direction
        );
    #endif

    hit.color = color;
}


vec3 render(Hit hit){
    
    shadeSurface(hit);
    
    if (hit.isBackground) {
        return hit.color;
    }
    
    return hit.color;
}


// --------------------------------------------------------
// Camera
// https://www.shadertoy.com/view/Xl2XWt
// --------------------------------------------------------

mat3 calcLookAtMatrix( in vec3 ro, in vec3 ta, in float roll )
{
    vec3 ww = normalize( ta - ro );
    vec3 uu = normalize( cross(ww,vec3(sin(roll),cos(roll),0.0) ) );
    vec3 vv = normalize( cross(uu,ww));
    return mat3( uu, vv, ww );
}

void doCamera(out vec3 camPos, out vec3 camTar, out float camRoll, in float time, in vec2 mouse) {
    float dist = 3.;
    camRoll = 0.;
    camTar = vec3(0,0,0);
    camPos = vec3(0,0,-dist);
    camPos *= cameraRotation();
    camPos += camTar;
}

Hit raymarchPixel(vec2 p, bool glitchPass) {
    vec2 m = iMouse.xy / RENDERSIZE.xy;

    vec3 camPos = vec3( 0., 0., 2.);
    vec3 camTar = vec3( 0. , 0. , 0. );
    float camRoll = 0.;
    
    // camera movement
    doCamera(camPos, camTar, camRoll, TIME, m);
    
    // camera matrix
    mat3 camMat = calcLookAtMatrix( camPos, camTar, camRoll );  // 0.0 is the camera roll
    
    // create view ray
    float focalLength = 3.;
    vec3 rd = normalize( camMat * vec3(p.xy, focalLength) );
    
    Hit hit = raymarch(CastRay(camPos, rd, glitchPass));
    
    return hit;
}


// --------------------------------------------------------
// Gamma
// https://www.shadertoy.com/view/Xds3zN
// --------------------------------------------------------

const float GAMMA = 2.2;

vec3 gamma(vec3 color, float g) {
    return pow(color, vec3(g));
}

vec3 linearToScreen(vec3 linearRGB) {
    return gamma(linearRGB, 1.0 / GAMMA);
}


// --------------------------------------------------------
// Glitch core
// --------------------------------------------------------


float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

const float glitchScale = .5;

vec2 glitchCoord(vec2 p, vec2 gridSize) {
	vec2 coord = floor(p / gridSize) * gridSize;;
    coord += (gridSize / 2.);
    return coord;
}


struct GlitchSeed {
    vec2 seed;
    float prob;
};
    
float fBox2d(vec2 p, vec2 b) {
  vec2 d = abs(p) - b;
  return min(max(d.x, d.y), 0.0) + length(max(d, 0.0));
}

GlitchSeed glitchSeed(vec2 p, float speed) {
    float seedTime = floor(time * speed);
    vec2 seed = vec2(
        1. + mod(seedTime / 100., 100.),
        1. + mod(seedTime, 100.)
    ) / 100.;
    seed += p;
    
    float prob = 0.;
    Hit hit = raymarchPixel(p, true);
    if ( ! hit.isBackground) {
    	prob = hit.model.material.albedo.x;
    }
    
    return GlitchSeed(seed, prob);
}

float shouldApply(GlitchSeed seed) {
    return round(
        mix(
            mix(rand(seed.seed), 1., seed.prob - .5),
            0.,
            (1. - seed.prob) * .5
        )
    );
}


// --------------------------------------------------------
// Glitch effects
// --------------------------------------------------------

// Swap

vec4 swapCoords(vec2 seed, vec2 groupSize, vec2 subGrid, vec2 blockSize) {
    vec2 rand2 = vec2(rand(seed), rand(seed+.1));
    vec2 range = subGrid - (blockSize - 1.);
    vec2 coord = floor(rand2 * range) / subGrid;
    vec2 bottomLeft = coord * groupSize;
    vec2 realBlockSize = (groupSize / subGrid) * blockSize;
    vec2 topRight = bottomLeft + realBlockSize;
    topRight -= groupSize / 2.;
    bottomLeft -= groupSize / 2.;
    return vec4(bottomLeft, topRight);
}

float isInBlock(vec2 pos, vec4 block) {
    vec2 a = sign(pos - block.xy);
    vec2 b = sign(block.zw - pos);
    return min(sign(a.x + a.y + b.x + b.y - 3.), 0.);
}

vec2 moveDiff(vec2 pos, vec4 swapA, vec4 swapB) {
    vec2 diff = swapB.xy - swapA.xy;
    return diff * isInBlock(pos, swapA);
}

void swapBlocks(inout vec2 xy, vec2 groupSize, vec2 subGrid, vec2 blockSize, vec2 seed, float apply) {
    
    vec2 groupOffset = glitchCoord(xy, groupSize);
    vec2 pos = xy - groupOffset;
    
    vec2 seedA = seed * groupOffset;
    vec2 seedB = seed * (groupOffset + .1);
    
    vec4 swapA = swapCoords(seedA, groupSize, subGrid, blockSize);
    vec4 swapB = swapCoords(seedB, groupSize, subGrid, blockSize);
    
    vec2 newPos = pos;
    newPos += moveDiff(pos, swapA, swapB) * apply;
    newPos += moveDiff(pos, swapB, swapA) * apply;
    pos = newPos;
    
    xy = pos + groupOffset;
}


// Static

void staticNoise(inout vec2 p, vec2 groupSize, float grainSize, float contrast) {
    GlitchSeed seedA = glitchSeed(glitchCoord(p, groupSize), 5.);
    seedA.prob *= .5;
    if (shouldApply(seedA) == 1.) {
        GlitchSeed seedB = glitchSeed(glitchCoord(p, vec2(grainSize)), 5.);
        vec2 offset = vec2(rand(seedB.seed), rand(seedB.seed + .1));
        offset = round(offset * 2. - 1.);
        offset *= contrast;
        p += offset;
    }
}


// Freeze time

void freezeTime(vec2 p, inout float time, vec2 groupSize, float speed) {
    GlitchSeed seed = glitchSeed(glitchCoord(p, groupSize), speed);
    //seed.prob *= .5;
    if (shouldApply(seed) == 1.) {
        float frozenTime = floor(time * speed) / speed;
        time = frozenTime;
    }
}


// --------------------------------------------------------
// Glitch compositions
// --------------------------------------------------------

void glitchSwap(inout vec2 p) {

    vec2 pp = p;
    
    float scale = glitchScale;
    float speed = 5.;
    
    vec2 groupSize;
    vec2 subGrid;
    vec2 blockSize;    
    GlitchSeed seed;
    float apply;
    
    groupSize = vec2(.6) * scale;
    subGrid = vec2(2);
    blockSize = vec2(1);

    seed = glitchSeed(glitchCoord(p, groupSize), speed);
    apply = shouldApply(seed);
    swapBlocks(p, groupSize, subGrid, blockSize, seed.seed, apply);
    
    groupSize = vec2(.8) * scale;
    subGrid = vec2(3);
    blockSize = vec2(1);
    
    seed = glitchSeed(glitchCoord(p, groupSize), speed);
    apply = shouldApply(seed);
    swapBlocks(p, groupSize, subGrid, blockSize, seed.seed, apply);

    groupSize = vec2(.2) * scale;
    subGrid = vec2(6);
    blockSize = vec2(1);
    
    seed = glitchSeed(glitchCoord(p, groupSize), speed);
    float apply2 = shouldApply(seed);
    swapBlocks(p, groupSize, subGrid, blockSize, (seed.seed + 1.), apply * apply2);
    swapBlocks(p, groupSize, subGrid, blockSize, (seed.seed + 2.), apply * apply2);
    swapBlocks(p, groupSize, subGrid, blockSize, (seed.seed + 3.), apply * apply2);
    swapBlocks(p, groupSize, subGrid, blockSize, (seed.seed + 4.), apply * apply2);
    swapBlocks(p, groupSize, subGrid, blockSize, (seed.seed + 5.), apply * apply2);
    
    groupSize = vec2(1.2, .2) * scale;
    subGrid = vec2(9,2);
    blockSize = vec2(3,1);
    
    seed = glitchSeed(glitchCoord(p, groupSize), speed);
    apply = shouldApply(seed);
    swapBlocks(p, groupSize, subGrid, blockSize, seed.seed, apply);
}



void glitchStatic(inout vec2 p) {

    // Static
    //staticNoise(p, vec2(.25, .25/2.) * glitchScale, .005, 5.);
    
    // 8-bit
    staticNoise(p, vec2(.5, .25/2.) * glitchScale, .2 * glitchScale, 2.);
}

void glitchTime(vec2 p, inout float time) {
   freezeTime(p, time, vec2(.5) * glitchScale, 2.);
}

void glitchColor(vec2 p, inout vec3 color) {
    vec2 groupSize = vec2(.75,.125) * glitchScale;
    vec2 subGrid = vec2(0,6);
    float speed = 5.;
    GlitchSeed seed = glitchSeed(glitchCoord(p, groupSize), speed);
    seed.prob *= .3;
    if (shouldApply(seed) == 1.) {
        vec2 co = mod(p, groupSize) / groupSize;
        co *= subGrid;
        float a = max(co.x, co.y);
        //color.rgb *= vec3(
        //  min(floor(mod(a - 0., 3.)), 1.),
        //    min(floor(mod(a - 1., 3.)), 1.),
        //    min(floor(mod(a - 2., 3.)), 1.)
        //);
        
        color *= min(floor(mod(a, 2.)), 1.) * 10.;
    }
}

void main()
{
    time = TIME;
    time /= 3.;
    time = mod(time, 1.);
    
    vec2 p = (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy)/RENDERSIZE.y;
    
    vec3 color;
    
    #ifdef GLITCH_MASK
    	float prob = glitchSeed(p, 10.).prob;
    	color = vec3(prob);
   	#else

        #ifndef NO_GLITCH
            glitchSwap(p);
            glitchTime(p, time);
            glitchStatic(p);
        #endif

        Hit hit = raymarchPixel(p, false);
        color = render(hit);

        #ifndef NO_GLITCH
            glitchColor(p, color);
        #endif
    
        #ifndef NORMALS
           color = linearToScreen(color);
        #endif

    #endif

    gl_FragColor = vec4(color,1.0);
}