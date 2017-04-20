/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "procedural",
    "mondrian",
    "short",
    "quadtree",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MlsXDf by FabriceNeyret2.  translate and move with mouse.\n#define tunes the probabilty of subdividing.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


#define P_SUBDIV .2+.2*sin(TIME)
//#define P_SUBDIV .2

float rnd(vec3 v) { return fract(4e4*sin(dot(v,vec3(13.46,41.74,-73.36))+17.34)); }
    
void main() {

	vec4 fragCoordPos = gl_FragCoord;
	

    vec2 u, R=RENDERSIZE.xy, m=iMouse.xy;
    if (m.x+m.y<1e-2*R.x) m = R*(.5+.5*sin(.1*TIME+vec2(0,1.6)));
    fragCoordPos.x -= 8.*(m.x-R.x/2.);
    fragCoordPos.xy /= (1.-m.y/R.y)*4.;
    
	float z = R.y;
    for (int i=0; i<128; i++) {
        u = floor(fragCoordPos.xy/z)+.5;
        if (rnd(vec3(z*u, z)) < P_SUBDIV) break;
        z /= 2.;
    }
    fragCoordPos.xy = z/2.-abs(fragCoordPos.xy-z*u);
    gl_FragColor = min(fragCoordPos.x,fragCoordPos.y)<1. ? vec4(0) :
    			// vec4(1); // vec4(z/R.y);
				.6+.4*cos(6.28*rnd(vec3(z*u+1.,z))+vec4(0,2.1,-2.1,0));
}
