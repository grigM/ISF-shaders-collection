/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#31171.0",
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
    },
    {
      "NAME" : "max_iter",
      "TYPE" : "float",
      "MAX" : 6,
      "DEFAULT" : 4,
      "MIN" : 1
    },
    {
      "NAME" : "raymarch_iter",
      "TYPE" : "float",
      "MAX" : 48,
      "DEFAULT" : 32,
      "MIN" : 16
    },
    {
      "NAME" : "raymarch_exp",
      "TYPE" : "float",
      "MAX" : 20,
      "DEFAULT" : 2,
      "MIN" : 1
    },
    {
      "NAME" : "raymarch_multypl",
      "TYPE" : "float",
      "MAX" : 0.09,
      "DEFAULT" : 0.02,
      "MIN" : 0.001
    }
  ],
  "ISFVSN" : "2"
}
*/


// PlayingMarble.glsl
// original code from https://www.shadertoy.com/view/MtX3Ws
// simplified edit: Robert 25.11.2015
// see also https://www.shadertoy.com/view/Mlj3zWprecision mediump float;
// modified color calculation by I.G.P.

#ifdef GL_ES
precision mediump float;
#endif


vec3 roty(vec3 p,float a)
{ return mat3(cos(a),0,-sin(a), 0,1,0, sin(a),0,cos(a)) * p; }

float map(in vec3 p) 
{
	vec3 c=p; float res=0.;
	for (int i=0; i < int(max_iter); i++) 
	{
		p= abs(p)/dot(p,p) -.7;
		p.yz= vec2(p.y*p.y-p.z*p.z,2.*p.y*p.z);
		res += exp(-20. * abs(dot(p,c)));
	}
	return res*0.5;
}

vec3 raymarch(vec3 ro, vec3 rd)
{
	float t=5.0;
	vec3 col=vec3(0); float c=0.;
	for( int i=0; i < int(raymarch_iter); i++ )
	{
		t+= exp(c*-raymarch_exp) *raymarch_multypl;
		c= map(t *rd +ro);               
		col= vec3(c*c, c, c*c*c) *0.16 + col *0.96; //green
		col= vec3(c*c*c, c*c, c) *0.16 + col *0.96; //blue
		col= vec3(c, c*c*c, c*c) *0.16 + col *0.96; //red

	}
	return col;
}

void main()
{
    vec2 p= (gl_FragCoord.xy - RENDERSIZE*0.5) / RENDERSIZE.y;
    vec3 ro= roty(vec3(3.), TIME*0.1 + mouse.x);
    vec3 uu= normalize(cross(ro, vec3(0.0, 1.0, 0.0)));
    vec3 vv= normalize(cross(uu, ro));
    vec3 rd= normalize(p.x*uu + p.y*vv - ro*0.5 );
    gl_FragColor.rgb= log(raymarch(ro,rd) +1.0) *0.5;
    gl_FragColor.a= 1.0;
}