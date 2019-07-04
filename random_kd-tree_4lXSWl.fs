/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4lXSWl by FabriceNeyret2.  translate and move with mouse.\n#define tunes the probabilty of subdividing.",
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

float rnd(vec4 v) { return fract(4e4*sin(dot(v,vec4(13.46,41.74,-73.36,1.172))+17.34)); }
    
void main() {

	vec2 px;

	//px.xy = gl_FragCoord.xy;
    vec2 u, R=RENDERSIZE.xy, m=iMouse.xy;
    if (m.x+m.y<1e-2*R.x) m = R*(.5+.5*sin(.1*TIME+vec2(0,1.6)));
    
    px.x = gl_FragCoord.x - 8.*(m.x-R.x/2.);
    px.xy = gl_FragCoord.xy/(1.-m.y/R.y)*4.;
    
	vec2 z = R;
    for (int i=0; i<16; i++) {
        u = floor(px.xy/z)+.5;
        if (rnd(vec4(z*u, z)) < P_SUBDIV) break;
        if (rnd(vec4(z*u+.1, z))<.5) z.x /= 3.; else z.y /= 3.;
    }
    px.xy = z/2.-abs(px.xy-z*u);
    gl_FragColor = min(px.x,px.y)<2. ? vec4(0) 
    			: rnd(vec4(z*u+.2,z))<.8 ? vec4(1)
				: cos(6.28*rnd(vec4(z*u+1.,z))+vec4(0,2.1,-2.1,0));
}
