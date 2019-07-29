/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3llXR4 by mattz.  {4, 5} Truchet tiling of the hyperbolic plane in the Poincaré disk model. Click left of circle to auto-rotate, inside circle to scroll, right of circle to center. LMB down = B&W coloring",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


// "Hyperbolic Truchet tiles" by mattz
// Licence https://creativecommons.org/licenses/by-nc/3.0/us/
//
// Combining my love of Truchet tiling with my newfound 
// interest in hyperbolic geometry.

#define RED vec3(0.8, 0, 0)
#define BLUE vec3(0, 0, 0.8)
#define BLACK vec3(0)
#define WHITE vec3(1)
#define LIGHTGRAY vec3(.8)
#define LIGHTRED vec3(0.9, 0.5, 0.5)
#define LIGHTBLUE vec3(0.5, 0.5, 0.9)

#define PI 3.141592653589793

const vec4 NO_INTERSECT = vec4(-1e5);

const float TOL = 1e-5;
const float TOL_SQR = TOL*TOL;

//////////////////////////////////////////////////
// geometric utility functions

// are two points on the same diameter of the unit circle?
bool alongDiameter(vec2 p, vec2 q) {
   
    vec4 pq = abs(vec4(p, q));
    float m = max(max(pq.x, pq.y), max(pq.z, pq.w));
    
    float k = abs(p.x*q.y - p.y*q.x);
    
    return k < TOL*m;
    
}

// are two points the same length?
bool sameLength(float pp, float qq) {
    return abs(pp - qq) < TOL*max(pp, qq);
}

// rotate by 90 degrees
vec2 perp(vec2 p) {
    return vec2(-p.y, p.x);
}

// circle centered at center containing point p
vec3 compass2D(vec2 ctr, vec2 p) {
    vec2 diff = p - ctr;
    return vec3(ctr, dot(diff, diff));
}

// Construction 1.2: invert a point through a circle
vec2 invertPC(vec2 p, vec3 c) {
    vec2 po = p - c.xy;    
    return c.xy + po * c.z / dot(po, po);
}

//////////////////////////////////////////////////
// hyperbolic geometry functions

// distance from a point to a line or circle
float geodesicDist(vec3 l, vec2 p) {
	if (l.z > 0.0) {
		return length(p-l.xy) - sqrt(l.z);
	} else {
		return dot(normalize(l.xy), p);
	}
}

// special case of Construction 1.6 for unit circle
vec3 geodesicFromPole(vec2 p) {
    float h2 = dot(p, p);
    float r2 = (h2 - 1.);
    return vec3(p, r2);
}

// Polar of a point p about the unit circle
// 2D line passing thru the midpoint of p and its inverse, perp. to p.
vec3 polarFromPoint(vec2 p) {
    return vec3(p, -0.5*dot(p, p) - 0.5);
}

// invert point about geodesic (either arc or line)
vec2 reflectPG(vec2 p, vec3 c) {
    if (c.z == 0.) {
        return p - (2.*dot(p, c.xy))*c.xy;
    } else {
        return invertPC(p, c);
    }
}

// Construction 2.2: geodesic from polars of points
vec3 geodesicFromPoints(vec2 p, vec2 q) {
    
    if (alongDiameter(p, q)) {
        vec2 n = normalize(perp(p - q));
        return vec3(n, 0);
    }

    vec3 ppolar = polarFromPoint(p);
    vec3 qpolar = polarFromPoint(q);
    vec3 inter = cross(ppolar, qpolar);

    return compass2D(inter.xy/inter.z, p);
    
}


// return a geodesic passing thru p perpendicular to the diameter
// through p - undefined if p == (0, 0)
vec3 geodesicPerpTo(vec2 p) {
    
    float a2 = dot(p, p);
    float x = a2 + 1.;
    
    float h_over_a = x / (2.*a2);
    float h2 = 0.5*x*h_over_a;
    
    vec2 c = p * h_over_a;
    
    return vec3(c, (h2 - 1.));
    
}

// Construction 2.3: hyperbolic compass. 
// construct hyperbolic circle with center p that passes thru q
vec3 hyperbolicCompass(vec2 p, vec2 q) {
    
    float pp = dot(p, p);
    float qq = dot(q, q);
    
    if (pp < TOL_SQR) {
        return vec3(p, (qq));
    } 
    
    if (alongDiameter(p, q)) {
        vec3 pperp = geodesicPerpTo(p);
        vec2 qp = invertPC(q, pperp);
        vec2 qmid = 0.5*(q + qp);
        return compass2D(qmid, q);
    }
    
    // get polars of p and q
    vec3 ppolar = vec3(p, -0.5*pp - 0.5);
    vec3 qpolar = vec3(q, -0.5*qq - 0.5);
    
    // homogeneous coords of pole of geodesic pq
    vec3 pole = cross(ppolar, qpolar);
    
    // this is the direction from point q to the pole
    vec2 dqpole = pole.xy - pole.z*q; 
    
    // 2D line tangent to geodesic pq at q
    vec3 lq = vec3(dqpole, -dot(dqpole, q));
    
    // 2D line containing p and the origin
    vec3 lp = vec3(perp(p), 0);
    
    // homogeneous coords of intersection of these lines
    vec3 rval = cross(lq, lp);
    
    // return circle
    return compass2D(rval.xy/rval.z, q);
    
}


// Construction 3.1: Perpendicular bisector
vec3 hyperbolicBisector(vec2 p, vec2 q) {

    float pp = dot(p, p);
    float qq = dot(q, q);
    
    if (pp < TOL_SQR) { 
        
        // p is at origin
        float h2 = 1.0/qq;
        return vec3(q*h2, (h2 - 1.));
       
    } else if (qq < TOL_SQR) { 
        
        // q is at origin                
        float h2 = 1.0/pp;
        return vec3(p*h2, (h2 - 1.));
        
    } else if (sameLength(pp, qq)) {
        
        // p and q are same length, return the diameter
        return vec3(normalize(p - q), 0);
        
    }
    
    // this remarkably small piece of code reflects the following algebra:
    //
    // let d = q - p be the difference between p & q
    // let x be the pole of the bisector
    //
    // since the pole of the bisector is on the line from p to q, we know
    //
    //   x = p + k*d
    //
    // for some unknown k with abs(k) > 1 (because the pole isn't between p & q)
    //
    // now let's try to solve for k.
    //
    // we know that since the pole x is orthogonal to the unit circle, 
    // the radius of the bisector circle is governed by
    //
    //   r^2 = ||x||^2 - 1
    //       = ||p + k*d ||^2 - 1
    //       = p.p + 2k*p.d + k^2*d.d
    //
    // also since p and q are inverted through the bisector circle with radius
    // r we know
    //
    //   r^2 = || x-p || * || x-q || = ||d|| * || k*d - d ||
    //       = k*(k-1)*d.d 
    //
    // now we can set the two equations equal and solve for k

    vec2 d = q - p;
    float k = (1.0 - dot(p,p))/(dot(d,d) + 2.0*dot(p,d));
    
    return geodesicFromPole( p + k*d );
    
}

// hyperbolic translation to move the origin to point m
vec2 hyperTranslate(vec2 uv, vec2 m) {

    float mm = dot(m, m);
    if (mm < TOL_SQR || mm >= 1.) { return uv; }

    vec3 g1 = hyperbolicBisector(vec2(0), m);

    vec2 diff = uv.xy - g1.xy;
    float k = g1.z / dot(diff, diff);
    uv.xy = g1.xy + k*diff; 

    vec2 n = m / sqrt(mm);
    uv.xy -= 2.*dot(uv.xy, n)*n;
    
    return uv;
    
}

// return scalar whose sign indicates side of g that p is on
float sidePG(vec2 p, vec3 g) {
    if (g.z == 0.) {
        return dot(p, g.xy);
    } else {
        p -= g.xy;
        return dot(p, p) - g.z;
    }
}

// return true if p & q both on the same side of l 
bool sameSide(vec2 p, vec2 q, vec3 l) {
    return sidePG(p, l) * sidePG(q, l) >= 0.;    
}



///////////////////////////////////////////////////
// utility functions for drawing:

// mix src into dst by smoothstepping k with threshold d
void ink(inout vec3 colorOut, vec3 src, float inkDist) {
    
    colorOut = mix(src, colorOut, smoothstep(0.0, 1.0, inkDist));
    inkDist = 1e5;
    
}

// draw either line or circle (using geodesicDist above)
float drawLine(vec3 l, vec2 p, float lineWidth) {
    return (abs(geodesicDist(l, p.xy))-lineWidth);
}

float drawPoint(vec2 x, vec2 p, float pointSize) {
    return (length(p - x)-pointSize);
}


// From Dave Hoskins' "Hash without sine"
// https://www.shadertoy.com/view/4djSRW 
float hash12(vec2 p) {
	vec3 p3  = fract(vec3(p.xyx) * .1031);
    p3 += dot(p3, p3.yzx + 19.19);
    return fract((p3.x + p3.y) * p3.z);
}

void main() {

    
    vec3 colorOut = WHITE;
    
    const float diam = 2.0;
        
    float kAlt = step(0.1, max(iMouse.z, iMouse.w));
    float rmax = max(RENDERSIZE.x, RENDERSIZE.y);
    float rmin = min(RENDERSIZE.x, RENDERSIZE.y);
    float csize = (rmin * 0.98);
            
    float px = diam / csize;
    float invPx = csize / diam;
    
    vec2 uv = (gl_FragCoord.xy - 0.5*RENDERSIZE.xy) * px;
    
    const vec3 unitCircle = vec3(0, 0, 1);
    
    if (dot(uv, uv) < 1.) {
        // setup 4, 5, 2 triangle
        vec2 verts[3];
        vec3 edges[3];
        const float cospi5 = 0.8090169943749475; // cos(pi/5)
        const float sinpi5 = 0.5877852522924731; // sin(pi/5)
        const float sqrt22 = 0.7071067811865476; // sqrt(2)/2
        float f = sqrt(2./(cospi5*cospi5 - sinpi5*sinpi5));
        float k = (sqrt22*cospi5 - sqrt22*sinpi5)*f;
        float bsz = (cospi5 - sqrt22)*f;
        verts[0] = vec2(0);
        verts[1] = vec2(k, 0);
        verts[2] = vec2(0, k);
        
        vec2 mid = bsz*vec2(sqrt22, sqrt22);
        
        edges[0] = geodesicFromPoints(verts[1], verts[2]);
        edges[1] = vec3(1, 0, 0);
        edges[2] = vec3(0, 1, 0);
        vec2 m = (iMouse.xy - 0.5*RENDERSIZE.xy) * px;
        bool scroll = false;
        if (dot(m, m) < 0.95) {
            scroll = true;
        } else if (m.x < 0.) {
            float t = TIME*PI/10.0;
            float r = 0.5*smoothstep(0.0, 4.0, TIME);
            m = r*vec2(cos(t), sin(t));
            scroll = true;
        } else {
            m = vec2(0);
        }
        
        if (scroll) {
            for (int i=0; i<3; ++i) {
                verts[i] = hyperTranslate(verts[i], -m);
            }
            for (int i=0; i<3; ++i) {
                int j = (i+1)%3;
                int k = 3-i-j;
                edges[i] = geodesicFromPoints(verts[j], verts[k]);
            }
            mid = hyperTranslate(mid, -m);
        }
        
        bool done = false;
        for (int iter=0; iter<32; ++iter) {
            if (done) { continue; }
            int i = 0;
            if (!sameSide(uv, verts[0], edges[0])) {
                i = 0;
            } else if (!sameSide(uv, verts[1], edges[1])) {
                i = 1;
            } else if (!sameSide(uv, verts[2], edges[2])) {
                i = 2;
            } else {
                done = true;
                continue;
            }
            int j = (i+1)%3;
            int k = 3-i-j;
            mid = reflectPG(mid, edges[i]);
            verts[i] = reflectPG(verts[i], edges[i]);
            edges[j] = geodesicFromPoints(verts[i], verts[k]);
            edges[k] = geodesicFromPoints(verts[i], verts[j]);
            
        }
        
        float ds = 1.0 - dot(uv, uv);
        vec2 ctr = hyperTranslate(verts[0], m);
        vec2 seed = floor(12.*ctr + 0.5) + 19.;
                
        float r = hash12(seed);
        
        float lw = 0.02*ds;
        float cw = 0.05*ds;
        float pw = 0.08*ds;
        
        float bdist = drawLine(edges[2], uv, lw);
        float rdist = drawLine(edges[1], uv, lw);
        
        float cdist;
        
        vec3 altColor;
                
        if (r > 0.5) {
            
            vec3 c = hyperbolicCompass(verts[1], mid);
            
            float d = geodesicDist(c, uv);
            
            altColor = mix(BLACK, WHITE, smoothstep(-0.5*px, 0.5*px, d));
            
            
            cdist = drawLine(c, uv, cw);
            ink(colorOut, LIGHTBLUE, bdist*invPx);
            ink(colorOut, LIGHTRED, rdist*invPx);
            
            
        } else {
            
                        
            vec3 c = hyperbolicCompass(verts[2], mid);
            
            float d = geodesicDist(c, uv);
            
            altColor = mix(WHITE, BLACK, smoothstep(-0.5*px, 0.5*px, d));
            
            cdist = drawLine(c, uv, cw);
            ink(colorOut, LIGHTRED, rdist*invPx);
            ink(colorOut, LIGHTBLUE, bdist*invPx);
            
        }
        
        ink(colorOut, BLACK, cdist*invPx);
        ink(colorOut, BLUE, drawPoint(verts[1], uv, pw)*invPx);
        ink(colorOut, RED, drawPoint(verts[2], uv, pw)*invPx);
        
        colorOut = mix(colorOut, altColor, kAlt);
    }
    
    ink(colorOut, BLACK, drawLine(unitCircle, uv, 0.5*px)*invPx);
    
    colorOut = sqrt(colorOut);
    
    gl_FragColor = vec4(colorOut, 1);
    
}
