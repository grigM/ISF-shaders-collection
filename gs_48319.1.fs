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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#48319.1"
}
*/



// CircleIntersection    http://glslsandbox.com/e#48319.0

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float circle(vec2 p, float r) {
	return smoothstep(0.0, 0.0025, abs(length(p) - r)); 
}

float sdSegment(vec2 p, vec2 a, vec2 b) 
{
	vec2 pa = p - a;
	vec2 ba = b - a;
	float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
	return smoothstep(0.0, 0.005, length( pa - ba*h ));
}

// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
bool CircleIntersection(in vec2 p1,in float r1,in vec2 p2,in float r2
          ,out vec2 S1, out vec2 S2)
{
    vec2 m = p2 - p1; 
    float d = length(m);       // d = |A->B|
    m = m / d;                 // norm vector A->B  
    r1 *= r1;
    float x = (r1 - r2*r2 + d*d) / (2.0*d);
    float y = r1 - x*x;        // r1^2 - x^2
    bool ci = (y >= 0.);       // circle intersection ?
    if (ci)                    // intersection ?
    {   y = sqrt(y);
        vec2 n = vec2(-m.y, m.x);  // normal vector 
        S1 = p1 +x*m +y*n;
        S2 = p1 +x*m -y*n;
    }
    else S1 = S2 = vec2(0.);
  return ci;	
}

/* 
// https://www.shadertoy.com/view/ldlGR7 by iq
vec2 solve( vec2 p, float l1, float l2 )
{
	vec2 q = p*( 0.5 + 0.5*(l1*l1-l2*l2)/dot(p,p) );

	float s = l1*l1/dot(q,q) - 1.0;

	if( s<0.0 ) return vec2(-100.0);
	
    return q + q.yx*vec2(-1.0,1.0)*sqrt( s );
}
*/
void main( void ) 
{
	vec2 p = ( gl_FragCoord.xy - 0.5 * RENDERSIZE.xy ) / RENDERSIZE.y;
	vec2 m = (mouse.xy - 0.5 ) * RENDERSIZE.xy / RENDERSIZE.y;
	vec2 S1,S2;

	float c = circle(p, 0.3);
	c = min(c, circle(p, 0.01));
	c = min(c, circle(p - m, 0.2));
	c = min(c, circle(p - m, 0.01));

	bool ci = CircleIntersection(vec2(0.0), 0.3, m, 0.2, S1,S2);
        if (ci)
	{ c = min(c, sdSegment(p, S1, vec2(0.0)));
	  c = min(c, sdSegment(p, m, S1));
	  c = min(c, circle(p-S1, 0.01));
	
	  c = min(c, sdSegment(p, S2, vec2(0.0)));
	  c = min(c, sdSegment(p, m, S2));
	  c = min(c, circle(p-S2, 0.01));
	}
	gl_FragColor = vec4( vec3( c ), 1.0 );
}