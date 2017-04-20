/*{
	"CREDIT": "by mojovideotech",
"CATEGORIES" : [
    "generator",
    "2d",
    "iterations"
  ],
  "DESCRIPTION" : "",
  "INPUTS" : [
	 {
    	"NAME":   "XX",
      	"TYPE":   "point2D",
      	"MAX":    [ 100.0, 100.0 ],
      	"MIN":    [ -100.0, -100.0 ],
      	"DEFAULT":[ 23.0, -13.0 ]
    },
    	 {
    	"NAME":   "YY",
      	"TYPE":   "point2D",
      	"MAX":    [ 100.0, 100.0 ],
      	"MIN":    [ -100.0, -100.0 ],
      	"DEFAULT":[ 9.0, 13.0 ]
    },
    {
		"NAME" :	"rate",
		"TYPE" :	"float",
		"DEFAULT" :	0.5,
		"MIN" :	0.1,
		"MAX" :	2.0
	},
	{
		"NAME" :	"loops",
		"TYPE" :	"float",
		"DEFAULT" :	13.0,
		"MIN" :	2.0,
		"MAX" :	26.0
	},
    {
		"NAME" :	"c1",
		"TYPE" :	"float",
		"DEFAULT" : 1.0,
		"MIN" :  	0.0,
		"MAX" : 	1.0
	},
	{
		"NAME" :	"c2",
		"TYPE" :	"float",
		"DEFAULT" : 0.67,
		"MIN" :  	0.0,
		"MAX" : 	1.0
	},
	{
		"NAME" :	"c3",
		"TYPE" :	"float",
		"DEFAULT" : 0.5,
		"MIN" :  	0.0,
		"MAX" : 	1.0
	},
	{
		"NAME" :	"c4",
		"TYPE" :	"float",
		"DEFAULT" : 0.75,
		"MIN" :  	0.0,
		"MAX" : 	1.0
	},
	{
		"NAME" :	"c5",
		"TYPE" :	"float",
		"DEFAULT" : 0.25,
		"MIN" :  	0.0,
		"MAX" : 	1.0
	},
	{
		"NAME" :	"c6",
		"TYPE" :	"float",
		"DEFAULT" : 0.75,
		"MIN" :  	0.0,
		"MAX" : 	1.0
	},
	{
		"NAME" :	"brightness",
		"TYPE" :	"float",
		"DEFAULT" : 5.0,
		"MIN" :  	2.0,
		"MAX" : 	10.0
	},
	{
		"NAME" :	"saturation",
		"TYPE" :	"float",
		"DEFAULT" : 1.0,
		"MIN" :  	0.05,
		"MAX" : 	1.5
	},
	{
		"NAME" :	"multiplier",
		"TYPE" :	"float",
		"DEFAULT" : 0.15,
		"MIN" :  	0.0125,
		"MAX" : 	0.25
	},
	{
		"NAME" :	"depth",
		"TYPE" :	"float",
		"DEFAULT" : 0.25,
		"MIN" :  	0.01,
		"MAX" : 	0.75
	},
	{
		"NAME" :	"distortion",
		"TYPE" :	"float",
		"DEFAULT" : 5.0,
		"MIN" :  	2.0,
		"MAX" : 	24.0
	}
  ]
}
*/


////////////////////////////////////////////////////////////
// InversionFlow  by mojovideotech
//
// based on :
// shadertoy.com\/view\/XdXGDS  by inigo quilez - IQ 2013
//
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0
////////////////////////////////////////////////////////////


void main() {
    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    float L = floor(loops);	
	float T = TIME*rate + YY.x;
	vec2 z = -1.5 + 4.0*uv;
	vec3 col = vec3(1.0);
	for( int j=0; j<19; j++ )
	{
        float s = float(j)/L;
		float f = multiplier*(0.5 + 2.0*fract(sin(s*pow(abs(YY.y+L),L))*43758.5453123));
		vec2 c = depth*vec2( cos(f*T+XX.x*s),sin(f*T+XX.y*s) );
		z -= c;
		float zr = length( z );
	    float ar = atan( z.y, z.x ) + zr*cos(z.x*pow(f,T*s*s));
	    z  = vec2( cos(ar-dot(z,c)), sin(ar+pow(f,s)) )/zr;
		z += c;
        z += 0.005*-sin(distortion*z.x);
        col -= saturation*exp( -brightness*dot(z,z) )* (0.5+0.5*sin( 4.2*s + vec3(0.9+c3,c6,c5) ));
	}
    col *= 0.75 + 0.25*clamp(length(z-uv)*0.5,0.0,0.1);
	float h = dot(col,vec3(0.0));
	vec3 nor = normalize( vec3( cos(h*pow(h,TIMEDELTA*h*h)), sin(h*log2(T)), 1.0/RENDERSIZE.x ) );
	col -= 0.05*vec3(c1,c2,c4)*dot(nor,vec3(1.0-c2,c3,1.0-c6));;
	col += 0.5*(1.0-0.1*col)*nor.z*nor;
	col *= 1.2;
    col = pow( clamp(col,0.0,1.0), vec3(1.0) );
	col *= 0.9 + 0.2*pow(L*uv.x*uv.y*(1.0-uv.x)*(2.0-uv.y), 0.9 );
	gl_FragColor = vec4(col, 1.0 );
}
