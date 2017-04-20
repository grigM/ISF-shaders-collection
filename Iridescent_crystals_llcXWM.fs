/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "dodecahedron",
    "iridescence",
    "facet",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llcXWM by tdhooper.  I originally planed to add a radiolaria model in the middle, and experiment with refraction, but this looks pretty nice so those plans can wait until next time.\n\nI've had to limit the step distance as the chamfers are causing a lot of overshoots :(",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


#define MODEL_ROTATION vec2(.122, .5)

// 0: Defaults
// 1: Model
#define MOUSE_CONTROL 1

// Comment out for faster rendering
#define ENABLE_CHAMFER


// --------------------------------------------------------
// HG_SDF
// https://www.shadertoy.com/view/Xs3GRB
// --------------------------------------------------------

#define PI 3.14159265359
#define PHI (1.618033988749895)
#define TAU 6.283185307179586

// Plane with normal n (n is normalized) at some distance from the origin
float fPlane(vec3 p, vec3 n, float distanceFromOrigin) {
	return dot(p, n) + distanceFromOrigin;
}

// Rotate around a coordinate axis (i.e. in a plane perpendicular to that axis) by angle <a>.
// Read like this: R(p.xz, a) rotates "x towards z".
// This is fast if <a> is a compile-time constant and slower (but still practical) if not.
void pR(inout vec2 p, float a) {
    p = cos(a)*p + sin(a)*vec2(p.y, -p.x);
}


// Repeat around the origin by a fixed angle.
// For easier use, num of repetitions is use to specify the angle.
float pModPolar(inout vec2 p, float repetitions) {
	float angle = 2.*PI/repetitions;
	float a = atan(p.y, p.x) + angle/2.;
	float r = length(p);
	float c = floor(a/angle);
	a = mod(a,angle) - angle/2.;
	p = vec2(cos(a), sin(a))*r;
	// For an odd number of repetitions, fix cell index of the cell in -x direction
	// (cell index would be e.g. -5 and 5 in the two halves of the cell):
	if (abs(c) >= (repetitions/2.)) c = abs(c);
	return c;
}


// Intersection has to deal with what is normally the inside of the resulting object
// when using union, which we normally don't care about too much. Thus, intersection
// implementations sometimes differ from union implementations.
float fOpIntersectionChamfer(float a, float b, float r) {
	float m = max(a, b);
	if (r <= 0.) return m;
	if (((-a < r) && (-b < r)) || (m < 0.)) {
		return max(m, (a + r + b)*sqrt(0.5));
	} else {
		return m;
	}
}


// --------------------------------------------------------
// https://github.com/stackgl/glsl-inverse
// --------------------------------------------------------

mat3 inverse(mat3 m) {
  float a00 = m[0][0], a01 = m[0][1], a02 = m[0][2];
  float a10 = m[1][0], a11 = m[1][1], a12 = m[1][2];
  float a20 = m[2][0], a21 = m[2][1], a22 = m[2][2];

  float b01 = a22 * a11 - a12 * a21;
  float b11 = -a22 * a10 + a12 * a20;
  float b21 = a21 * a10 - a11 * a20;

  float det = a00 * b01 + a01 * b11 + a02 * b21;

  return mat3(b01, (-a22 * a01 + a02 * a21), (a12 * a01 - a02 * a11),
              b11, (a22 * a00 - a02 * a20), (-a12 * a00 + a02 * a10),
              b21, (-a21 * a00 + a01 * a20), (a11 * a00 - a01 * a10)) / det;
}


// --------------------------------------------------------
// http://math.stackexchange.com/a/897677
// --------------------------------------------------------

mat3 orientMatrix(vec3 A, vec3 B) {
    mat3 Fi = mat3(
        A,
        (B - dot(A, B) * A) / length(B - dot(A, B) * A),
        cross(B, A)
    );
    mat3 G = mat3(
        dot(A, B),              -length(cross(A, B)),   0,
        length(cross(A, B)),    dot(A, B),              0,
        0,                      0,                      1
    );
    return Fi * G * inverse(Fi);
}


// --------------------------------------------------------
// IQ
// https://www.shadertoy.com/view/ll2GD3
// --------------------------------------------------------

vec3 pal( in float t, in vec3 a, in vec3 b, in vec3 c, in vec3 d ) {
    return a + b*cos( 6.28318*(c*t+d) );
}

vec3 spectrum(float n) {
    return pal( n, vec3(0.5,0.5,0.5),vec3(0.5,0.5,0.5),vec3(1.0,1.0,1.0),vec3(0.0,0.33,0.67) );
}


// --------------------------------------------------------
// MAIN
// --------------------------------------------------------

float time;

struct Model {
    float dist;
    vec3 colour;
    float id;
};

    
float quadrant(float a, float b) {
	return ((sign(a) + sign(b) * 2.) + 3.) / 2.;
}

// Nearest icosahedron vertex and id
vec4 icosahedronVertex(vec3 p) {
    vec3 v1, v2, v3, result, plane;
    float id;
    float idMod = 0.;
	v1 = vec3(
    	sign(p.x) * PHI,
        sign(p.y) * 1.,
        0
    );
	v2 = vec3(
    	sign(p.x) * 1.,
        0,
        sign(p.z) * PHI
    );
	v3 = vec3(
    	0,
        sign(p.y) * PHI,
        sign(p.z) * 1.
    );
    plane = normalize(cross(
        mix(v1, v2, .5),
        cross(v1, v2)
    ));
    if (dot(p, plane) > 0.) {
    	result = v1;
        id = quadrant(p.y, p.x);
    } else {
    	result = v2;
        id = quadrant(p.x, p.z) + 4.;
    }
    plane = normalize(cross(
        mix(v3, result, .5),
        cross(v3, result)
    ));
    if (dot(p, plane) > 0.) {
    	result = v3;
        id = quadrant(p.z, p.y) + 8.;
    }
    return vec4(normalize(result), id);
}

vec3 rand(vec3 seed){
    return fract(mod(seed, 1.) * 43758.5453);
}

vec3 jitterOffset(float seed) {
	return normalize(rand(vec3(seed, seed + .2, seed + .8)) - .5);
}

vec3 jitterVec(vec3 v, float seed, float magnitude) {
	return normalize(v + jitterOffset(seed) * magnitude);
}

float alias(float t, float resolution) {
	return floor(t * resolution) / resolution;
}

#ifdef ENABLE_CHAMFER
	const float chamfer = .003;
#else
	const float chamfer = .0;
#endif

float fCrystalShard(vec3 p, float size) {
    float d;
    float width = size * .04 + .07;
    vec3 o = normalize(vec3(1,0,-.04));

    pModPolar(p.xy, 5.);
    float part1, part2;
    
    p.y = abs(p.y);

    part1 = fPlane(p, vec3(1,0,-.04), -width);

    pR(p.xy, TAU/5.);
    part2 = fPlane(p, vec3(1,0,-.04), -width);
    
    d = max(part1, part2);
	d = fOpIntersectionChamfer(part1, part2, chamfer);

    return d;
}

float fCrystalCap(vec3 p, float id, float side) {
    float jitter = id + side * .1;
    vec3 o = normalize(vec3(1,0,.55));
	float angle = TAU / 3.;
    float d, part;

    d = fPlane(p, jitterVec(o, jitter + .3, .1), 0.);
    
    pR(p.xy, angle);
    part = fPlane(p, jitterVec(o, jitter + .5, .1), 0.);
    d = fOpIntersectionChamfer(d, part, chamfer);

    pR(p.xy, angle);
    part = fPlane(p, jitterVec(o, jitter + .9, .1), 0.);
    d = fOpIntersectionChamfer(d, part, chamfer);
    
    return d;
}

float fCrystal(vec3 p, float id, float focus) {
    
    float size = sin(time * TAU * 4. + focus * 5. + id) * .5 + .5;
    float size2 = cos(time * TAU * 2. + focus * 5. + id) * .5 + .5;
    
    size = alias(size, 2.);
    size2 = alias(size2, 2.);
    
    float height = size2 * .1 + .35;
    float offset = .9;
    float d;

    float shard = fCrystalShard(p, size);
    
    p.z -= offset;
    float side = sign(p.z) * .5 + .5;
    p.z = abs(p.z);
    p.z -= height;

    float cap = fCrystalCap(p, id, side);
    d = fOpIntersectionChamfer(shard, cap, chamfer);

	return d;
}

Model model(vec3 p) {    
	float d = 1000.;
   	vec3 col = vec3(0);
	vec3 dir = normalize(vec3(
    	0,
        -PHI,
        -1
    ));
    
    vec4 iv = icosahedronVertex(p);
    vec3 v = iv.xyz;
    float id = iv[3] / 12.;
    
    p *= orientMatrix(v, vec3(0,0,1));
    pR(p.xy, id);
    
    pR(p.xy, time * TAU);
    
    float focus = dot(v, dir) * .5 + .5;

    d = fCrystal(p, id, focus);
        
    return Model(d, col, 1.);
}



// The MINIMIZED version of https://www.shadertoy.com/view/Xl2XWt

const float MAX_TRACE_DISTANCE = 30.0;           // max trace distance
const float INTERSECTION_PRECISION = 0.001;        // precision of the intersection
const int NUM_OF_TRACE_STEPS = 100;

// Default is 1, reduce to fix overshoots
#ifdef ENABLE_CHAMFER
	const float FUDGE_FACTOR = .5;
#else
	const float FUDGE_FACTOR = 1.;
#endif

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


// checks to see which intersection is closer
Model opU( Model m1, Model m2 ){
    if (m1.dist < m2.dist) {
    	return m1;
    } else {
    	return m2;
    }
}


//--------------------------------
// Modelling 
//--------------------------------


Model map( vec3 p ){
    Model res = Model(1000000., vec3(0), 0.);
    p *= modelRotation();
    res = opU(res, model(p));
   	return res;
}


const float GAMMA = 2.2;

vec3 gamma(vec3 color, float g)
{
    return pow(color, vec3(g));
}

vec3 linearToScreen(vec3 linearRGB)
{
    return gamma(linearRGB, 1.0 / GAMMA);
}


struct Hit {
    float len;
    vec3 colour;
    float id;
};

Hit calcIntersection( in vec3 ro, in vec3 rd ){

    float h =  INTERSECTION_PRECISION*2.0;
    float t = 0.0;
    float res = -1.0;
    float id = -1.;
    vec3 colour;

    for( int i=0; i< NUM_OF_TRACE_STEPS ; i++ ){

        if( h < INTERSECTION_PRECISION || t > MAX_TRACE_DISTANCE ) break;
        Model m = map( ro+rd*t );
        h = m.dist;
        t += h * FUDGE_FACTOR;
        id = m.id;
        colour = m.colour;
    }

    if( t < MAX_TRACE_DISTANCE ) res = t;
    if( t > MAX_TRACE_DISTANCE ) id =-1.0;

    return Hit( res , colour , id );
}


//----
// Camera Stuffs
//----
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
    camTar = vec3(0);
    camPos = vec3(0,.1,-dist);
    pR(camPos.yx, time*TAU*1.);
    camPos += camTar;
}

// Calculates the normal by taking a very small distance,
// remapping the function, and getting normal for that
vec3 calcNormal( in vec3 pos ){
    vec3 eps = vec3( 0.001, 0.0, 0.0 );
    vec3 nor = vec3(
        map(pos+eps.xyy).dist - map(pos-eps.xyy).dist,
        map(pos+eps.yxy).dist - map(pos-eps.yxy).dist,
        map(pos+eps.yyx).dist - map(pos-eps.yyx).dist );
    return normalize(nor);
}


vec3 render( Hit hit , vec3 ro , vec3 rd ){

    vec3 bg = vec3(0.);
    vec3 color = bg;

    if (hit.id == 1.) {
    	vec3 pos = ro + rd * hit.len;
        vec3 norm = calcNormal( pos );
        vec3 ref = reflect(rd, norm);
        vec3 lig = normalize(vec3(.5,1,-.5));
        vec3 dome = vec3(0,1,0);
        vec3 eye = vec3(0,0,-1);

        vec3 perturb = sin(pos * 10.);
        color = spectrum( dot(norm + perturb * .05, eye) * 2.);

        float specular = clamp(dot(ref, lig), 0., 1.);
        specular = pow((sin(specular * 20. - 3.) * .5 + .5) + .1, 32.) * specular;
        specular *= .1;
        specular += pow(clamp(dot(ref, lig), 0., 1.) + .3, 8.) * .1;

        float shadow = pow(clamp(dot(norm, dome) * .5 + 1.2, 0., 1.), 3.);
        color = color * shadow + specular;

        float near = 2.8;
        float far = 4.;
        float fog = (hit.len - near) / (far - near);
        fog = clamp(fog, 0., 1.);
        color = mix(color, bg, fog);
    }

	return color;
}


void main()
{
    time = TIME;
    time /= 3.;
    time = mod(time, 1.);   
    
    
    vec2 p = (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy)/RENDERSIZE.y;
    vec2 m = iMouse.xy / RENDERSIZE.xy;

    vec3 camPos = vec3( 0., 0., 2.);
    vec3 camTar = vec3( 0. , 0. , 0. );
    float camRoll = 0.;
    
    // camera movement
    doCamera(camPos, camTar, camRoll, TIME, m);
    
    // camera matrix
    mat3 camMat = calcLookAtMatrix( camPos, camTar, camRoll );  // 0.0 is the camera roll
    
    // create view ray
    vec3 rd = normalize( camMat * vec3(p.xy,2.0) ); // 2.0 is the lens length
    
    Hit hit = calcIntersection( camPos , rd  );

    vec3 color = render( hit , camPos , rd );    
    color = linearToScreen(color);
    gl_FragColor = vec4(color,1.0);
}