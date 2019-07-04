/*{
  "CREDIT": "by mojovideotech",
  "CATEGORIES": [
    "space"
  ],
  "INPUTS": [
    {
      "NAME": "mouse",
      "TYPE": "point2D",
      "MAX": [
        0,
        1
      ],
      "MIN": [
        -1,
        0
      ]
    },
    {
      "NAME": "mult",
      "TYPE": "float",
      "DEFAULT": 5,
      "MIN": 2,
      "MAX": 12
    },
    {
      "NAME": "width",
      "TYPE": "float",
      "DEFAULT": 22,
      "MIN": 20,
      "MAX": 60
    },
    {
      "NAME": "phase1",
      "TYPE": "float",
      "DEFAULT": 0.8,
      "MIN": 0.01,
      "MAX": 0.99
    },
    {
      "NAME": "phase2",
      "TYPE": "float",
      "DEFAULT": 0.05,
      "MIN": -0.5,
      "MAX": 0.5
    },
    {
      "NAME": "density",
      "TYPE": "float",
      "DEFAULT": 0.15,
      "MIN": 0.05,
      "MAX": 0.5
    },
    {
      "NAME": "morph",
      "TYPE": "float",
      "DEFAULT": 5,
      "MIN": 2,
      "MAX": 10
    },
    {
      "NAME": "hue",
      "TYPE": "float",
      "DEFAULT": 4,
      "MIN": 0.5,
      "MAX": 10
    },
    {
      "NAME": "tint",
      "TYPE": "float",
      "DEFAULT": 9,
      "MIN": 0.5,
      "MAX": 10
    },
    {
      "NAME": "rate",
      "TYPE": "float",
      "DEFAULT": -0.9,
      "MIN": -1,
      "MAX": 1
    }
  ],
  "DESCRIPTION": "from http://glslsandbox.com/e#31191.1"
}*/

///////////////////////////////////////////
// SpaceGhost by mojovideotech
//
// mod of glslsandbox.com/e#31191.1
//
// based on :
// SpaceGlowing.glsl     2016-03-02
// original code from shadertoy.com/\view/MtX3Ws
// simplified edit: Robert 25.11.2015
// see also shadertoy.com/\view/\Mlj3zW
//
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0
///////////////////////////////////////////

#ifdef GL_ES
precision mediump float;
#endif

vec3 roty(vec3 p,float a)
{ return mat3(cos(phase1*a),0,-sin(phase2*a), 0,1,1, sin(phase2*a),0,cos(phase1*a)) * p; }

float map(in vec3 p) 
{
	vec3 c=p; float res=0.;
	for (int i=0; i < 4; i++) 
	{
		p= abs(p)/dot(p,p) -.7;
		p.yz= vec2(p.y*p.y-p.z*p.z,2.*p.y*p.z);
		res += exp(-(80.-width) * abs(dot(p,c)));
	}
	return res*0.5;
}

vec3 raymarch(vec3 ro, vec3 rd)
{
	float t=morph;
	vec3 col=vec3(0); float c=0.;
	float n = floor(mult);
	for( int i=0; i < 12; i++ )
	{
		t+= exp(c*-2.0) *density;
		c= map(t *rd +ro);               
		col= vec3(8.*c*c, hue*hue*c*c, c*c*c) *0.16 + col *0.96; //green
		col= vec3(8.*c*c, c*c, tint*tint*c*c) *0.16 + col *0.96; //blue
		col= vec3(8.*c*c/hue, c*c, c*c*c/tint) *0.16 + col *0.96; //red
        n -= 1.0;
        if (n<1.0) { break; 
        }
	}
	return col;
}

void main()
{
    vec2 p= (gl_FragCoord.xy - RENDERSIZE*0.5) / RENDERSIZE.y;
    vec3 ro= roty(vec3(3.), TIME*rate + mouse.x);
    vec3 uu= normalize(cross(ro, vec3(1.0, .0, 0.0)));
    vec3 vv= normalize(cross(uu, ro));
    vec3 rd= normalize(p.x*uu + p.y*vv - ro*0.5 );
    gl_FragColor.rgb= log(raymarch(ro,rd) +1.0) *0.5;
    gl_FragColor.a= 1.0;
}