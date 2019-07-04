/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#53711.1"
}
*/


//subdivided icosahedron with IDs	
//tdhooper -  2016-02-21
//https://www.shadertoy.com/view/4syGDG < source 

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

#define PHI (sqrt(5.)*0.5 + 0.5)
#define PI 3.14159265
#define t TIME


// HG_SDF

float fPlane(vec3 p, vec3 n, float distanceFromOrigin) 
{
	return dot(p, n) + distanceFromOrigin;
}

// Reflect space at a plane
float pReflect(inout vec3 p, vec3 planeNormal, float offset) 
{
	float t = dot(p, planeNormal)+offset;
	if (t < 0.) {
		p = p - (2.*t)*planeNormal;
	}
	return sign(t);
}

void pR(inout vec2 p, float a) 
{
	p = cos(a)*p + sin(a)*vec2(p.y, -p.x);
}

float sgn(float x) {
	return (x<0.)?-1.:1.;
}


// Mirror at an axis-aligned plane which is at a specified distance <dist> from the origin.
float pMirror (inout float p, float dist) 
{
	float s = sgn(p);
	p 	= abs(p) - dist;
	return s;
}

// Like pMirror but returns the side index, instead of +/-1
float pMirrorSide (inout float p, float dist) 
{
	return pMirror(p, dist) / 2. + .5;
}

// Repeat around the origin by a fixed angle.
// For easier use, num of repetitions is use to specify the angle.
float pModPolar(inout vec2 p, float repetitions) 
{
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



// Repeat only a few times: from indices <start> to <stop> (similar to above, but more flexible)
float pModInterval1(inout float p, float size, float start, float stop) {
	float halfsize = size*0.5;
	float c = floor((p + halfsize)/size);
	p = mod(p+halfsize, size) - halfsize;
	if (c > stop) { //yes, this might not be the best thing numerically.
		p += size*(c - stop);
		c = stop;
	}
	if (c <start) {
		p += size*(c - start);
		c = start;
	}
	return c;
}


// Knighty https://www.shadertoy.com/view/XlX3zB

int Type = 5;

vec3 nc,pab,pbc,pca;
void init() 
{
	//setup folding planes and vertex
	float cospin	= cos(PI/float(Type));
	float scospin 	= sqrt(0.75-cospin * cospin);

	//3rd folding plane. The two others are xz and yz planes
	nc		= vec3(-0.5,-cospin, scospin);
	
	pab		= vec3(0.,0.,1.);
	pbc		= vec3(scospin,0.,0.5);
	pca		= vec3(0.,scospin,cospin);
}


float indexSgn(float s) 
{
	return s / 2. + 0.5;
}

bool boolSgn(float s) 
{
	return bool(s / 2. + 0.5);
}

// Barycentric to Cartesian 
vec3 bToC(vec3 A, vec3 B, vec3 C, vec3 barycentric) {
	return barycentric.x * A + barycentric.y * B + barycentric.z * C;
}


float pModIcosahedron(inout vec3 p, int subdivisions) {

	float x = indexSgn(sgn(p.x));
	float y = indexSgn(sgn(p.y));
	float z = indexSgn(sgn(p.z));
    p = abs(p);
	pReflect(p, nc, 0.);

	float xai = sgn(p.x);
	float yai = sgn(p.y);
    p.xy = abs(p.xy);
	float sideBB = pReflect(p, nc, 0.);

	float ybi = sgn(p.y);
	float xbi = sgn(p.x);
    p.xy = abs(p.xy);
	pReflect(p, nc, 0.);
    
    float idx = 0.;

    float faceGroupAi = indexSgn(ybi * yai * -1.);
    float faceGroupBi = indexSgn(yai);
    float faceGroupCi = clamp((xai - ybi -1.), 0., 1.);
    float faceGroupDi = clamp(1. - faceGroupAi - faceGroupBi - faceGroupCi, 0., 1.);

    idx += faceGroupAi * (x + (2. * y) + (4. * z));
    idx += faceGroupBi * (8. + y + (2. * z));
    idx += faceGroupCi * (12. + x + (2. * z));
    idx += faceGroupDi * (16. + x + (2. * y));
    
    if (subdivisions > 0) 
    {
        
        idx *= 4.;

        vec3 A = pbc;
       	vec3 C = reflect(A, normalize(cross(pab, pca)));
        vec3 B = reflect(C, normalize(cross(pbc, pca)));
       
        vec3 n;

        // Fold in corner A 
        
        vec3 p1 = bToC(A, B, C, vec3(.5, .0, .5));
        vec3 p2 = bToC(A, B, C, vec3(.5, .5, .0));
        n = normalize(cross(p1, p2));

        float side = pReflect(p, n, 0.);
        
        float faceAi = indexSgn(side * -1.) * indexSgn(xbi * -1.);
        float faceBi = indexSgn(xai) * indexSgn(side * -1.) * indexSgn(xbi);
        float faceCi = indexSgn(xai * -1.) * indexSgn(side * -1.) * indexSgn(xbi);
        
        idx += 1. * faceAi;
        idx += 2. * faceBi;
        idx += 3. * faceCi;
        
        if (subdivisions > 1) {

       		idx *= 4.;
            
            // Get corners of triangle created by fold

            A = reflect(A, n);
            B = p1;
            C = p2;
            
            // Fold in corner A

            p1 = bToC(A, B, C, vec3(.5, .0, .5));
            p2 = bToC(A, B, C, vec3(.5, .5, .0));
            n = normalize(cross(p1, p2));
			pReflect(p, n, 0.);
            

            // Fold in corner B
            
            p2 = bToC(A, B, C, vec3(.0, .5, .5));
            p1 = bToC(A, B, C, vec3(.5, .5, .0));
            n = normalize(cross(p1, p2));
            float sideB = pReflect(p, n, 0.);
            
            float faceAi = indexSgn(sideB * -1.) * indexSgn(sideBB);
            float faceBi = indexSgn(sideB * -1.) * indexSgn(sideBB * -1.) * indexSgn(xai);
            float faceCi = indexSgn(sideB * -1.) * indexSgn(sideBB * -1.) * indexSgn(xai * -1.);

            idx += 1. * faceAi;
            idx += 2. * faceBi;
            idx += 3. * faceCi;
        }
        
    }
    
    return idx;
}


float sdBox(vec3 p, vec3 b) {
    vec3 d = abs(p) - b;
    return min(max(d.x, max(d.y, d.z)), 0.0) + length(max(d, 0.0));
}

vec3 lerp(vec3 a, vec3 b, float s) {
	return a + (b - a) * s;
}


float faceCount(float c, vec3 p) {
    // Align face with the xy plane
    vec3 rn = normalize(lerp(pca, vec3(0,0,1), 0.5));
    p = reflect(p, rn);

    vec3 boxSize = vec3(.9, .66, 2);
	float box1 = sdBox(p, boxSize);

    float d = box1;

    vec3 center = normalize(vec3(0, 0, 1));
    vec3 edge = normalize(vec3(0, -1, PHI+1.));
    vec3 corner = edge;
    pR(corner.xy, -PI / 3.);
    corner.xy *= edge.y/corner.y;
    corner = normalize(corner);
    
    vec3 countP = p;
    
    countP.z += boxSize.z + 0.012;
	pModInterval1(countP.z, 0.012, -c, 0.);
    
    float counter = length(countP) - 0.017;
  
    d = min(
        min(
        	d,
        	counter
        ),
        min(
            length(p + edge * 2.) - 0.1,
            length(p + corner * 2.) - 0.1
        )
    );
    
    return d;
}
float face(vec3 p) 
{
    // Align face with the xy plane
	vec3 rn = normalize(lerp(pca, vec3(0,0,1), 0.5));
    	p = reflect(p, rn);
	return min(fPlane(p, vec3(0,0,-1), -1.4), length(p + vec3(0,0,1.4)) - 0.02);
}


vec2 icoTestModel(vec3 p, int subdivisions) {
    float c = pModIcosahedron(p, subdivisions);

    float d;
    //d = faceCount(c, p);
    d = face(p);
    
    return vec2(d, c);
}

// checks to see which intersection is closer
// and makes the y of the vec2 be the proper id
vec2 opU( vec2 d1, vec2 d2 ){
    
	return (d1.x<d2.x) ? d1 : d2;
}




vec2 model(vec3 p) 
{
    vec3 pp;
    pp = p + vec3(4,0,0);
	vec2 A = icoTestModel(pp, 0);
    A.y /= 20.;

    pp = p + vec3(0,0,0);
	vec2 B = icoTestModel(pp, 1);
    B.y /= 80.;

    pp = p + vec3(-4,0,0);
	vec2 C = icoTestModel(pp, 2);
    C.y /= 320.;

    return B;//opU(opU(A, B), C);
}


vec2 clipModel(vec3 p) 
{
	return vec2(1000000., 0.);
	vec2 x = vec2( sdBox(p, vec3(.001,2,2)), 0. );
	vec2 y = vec2( sdBox(p, vec3(2,.001,2)), .3 );
	vec2 z = vec2( sdBox(p, vec3(2,2,.001)), .6 );
	return opU(opU(x, y), z);
}

// The MINIMIZED version of https://www.shadertoy.com/view/Xl2XWt
const float MAX_TRACE_DISTANCE 		= 20.0;           // max trace distance
const float INTERSECTION_PRECISION 	= 0.001;        // precision of the intersection
const int NUM_OF_TRACE_STEPS 		= 100;





//--------------------------------
// Materials 
//--------------------------------

vec3 doBackground(vec3 rayVec) 
{
	return vec3(.13);
}

vec3 hsl2rgb( in vec3 c )
{
	vec3 rgb = clamp( abs(mod(c.x*6.0+vec3(0.0,4.0,2.0),6.0)-3.0)-1.0, 0.0, 1.0 );
	
	return c.z + c.y * (rgb-0.5)*(1.0-abs(2.0*c.z-1.0));
}



//--------------------------------
// Modelling 
//--------------------------------
vec2 map( vec3 p )
{ 		
	vec2 res = model(p); 
	
	vec2 clip = clipModel(p);
	
	res = opU(clip, res);
	
	return res;
}


vec2 calcIntersection( in vec3 ro, in vec3 rd )
{
	float h =  INTERSECTION_PRECISION*2.0;
	float t = 0.0;
	float res = -1.0;
	float id = -1.;
	
	for( int i=0; i< NUM_OF_TRACE_STEPS ; i++ )
	{
	
		if( h < INTERSECTION_PRECISION || t > MAX_TRACE_DISTANCE ) break;
		vec2 m = map( ro+rd*t );
		h = m.x;
		t += h;
		id = m.y;
	
	}
	
	if( t < MAX_TRACE_DISTANCE ) res = t;
	if( t > MAX_TRACE_DISTANCE ) id =-1.0;

   	return vec2( res , id );    
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

void doCamera(out vec3 camPos, out vec3 camTar, in float TIME, in vec2 mouse) 
{
	vec2 m 	= mouse;
	float y = m.y += 0.00001;
	float x = m.x += 0.00001;
	camPos 	= vec3(0,8,0);
	pR(camPos.zy, y * 3.);
	pR(camPos.zx, x * PI * 3.);
	
	camTar = vec3(0);
}


// Calculates the normal by taking a very small distance,
// remapping the function, and getting normal for that
vec3 calcNormal( in vec3 pos ){
    
	vec3 eps = vec3( 0.001, 0.0, 0.0 );
	vec3 nor = vec3(
	    map(pos+eps.xyy).x - map(pos-eps.xyy).x,
	    map(pos+eps.yxy).x - map(pos-eps.yxy).x,
	    map(pos+eps.yyx).x - map(pos-eps.yyx).x );
	return normalize(nor);
}


vec3 render( vec2 res , vec3 ro , vec3 rd )
{
	vec3 color = doBackground(rd);
	
	if( res.y > -.5 )
	{	
		vec3 pos 	= ro + rd * res.x;
		vec3 norm 	= calcNormal( pos );
		vec3 ref 	= reflect(rd, norm);
		vec3 l 		= vec3(0., 32., -24.);
		vec3 ld 	= normalize(l+ref);
		float incident 	= dot(norm, ld);
		float bounce	= max(1.-cos(dot(ld, ref)), .0625)*.5;
		float spec	= max(sin(dot(ld, ref)), .0625)*.75;
		color 		= hsl2rgb(vec3(res.y, 1., .5));
		color		= (color + color * incident) * .5;
		color 		+= bounce + spec;
		color		= pow(color*.8, vec3(1.26));
	}
	
	return color;
}



void main(void)
{
	init();
	
	vec2 p 		= (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy)/RENDERSIZE.y;
	vec2 m 		= mouse.xy;
	
	vec3 ro 	= vec3( 0., 0., 0.);
	vec3 ta 	= vec3( 0. , 0. , 0. );
	
	// camera movement
	doCamera(ro, ta, TIME, m);
	
	// camera matrix
	mat3 camMat 	= calcLookAtMatrix( ro, ta, 0.0 );  // 0.0 is the camera roll
	
	// create view ray
	float focal 	= 3.;
	vec3 rd 	= normalize( camMat * vec3(p.xy, focal) ); // 2.0 is the lens length
	vec2 res 	= calcIntersection( ro , rd  );
	vec3 color 	= render( res , ro , rd );
	
	gl_FragColor 	= vec4(color,1.0);
}