/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tlBXD3 by foran.  the clouds",
  "INPUTS" : [

  ]
}
*/


const int octaves=12;
const float seed=43758.5453123;
const float seed2=73156.8473192;
const vec3 color1=vec3(.101961,.619608,.666667);//color
const vec3 color2=vec3(.666667,.666667,.498039);//color
const vec3 color3=vec3(0,0,.164706);//color
const vec3 color4=vec3(.666667,1.,1.);//color

float rand(vec2 co){
	// implementation found at: lumina.sourceforge.net/Tutorials/Noise.html
	return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

float noise2f( in vec2 p )
{
	vec2 ip = vec2(floor(p));
	vec2 u = fract(p);
	// http://www.iquilezles.org/www/articles/morenoise/morenoise.htm
	u = u*u*(3.0-2.0*u);
	//u = u*u*u*((6.0*u-15.0)*u+10.0);
	
	float res = mix(
		mix(rand(ip),  rand(ip+vec2(1.0,0.0)),u.x),
		mix(rand(ip+vec2(0.0,1.0)),   rand(ip+vec2(1.0,1.0)),u.x),u.y);
	return res*res;
	//return 2.0* (res-0.5);
}


float fbm(vec2 c) {
	float f = 0.0;
	float w = 1.0;
	for (int i = 0; i < 12; i++) {
		f+= w*noise2f(c);
		c*=2.0;
		w*=0.5;
	}
	return f;
}
//--------------
vec2 random2(vec2 st,float seed){
	st=vec2(dot(st,vec2(127.1,311.7)),
	dot(st,vec2(269.5,183.3)));
	return-1.+2.*fract(sin(st)*seed);
}

float noise(vec2 st,float seed){
	vec2 i=floor(st);
	vec2 f=fract(st);
	
	vec2 u=f*f*(3.-2.*f);
	
	return mix(mix(dot(random2(i+vec2(0.,0.),seed),f-vec2(0.,0.)),
	dot(random2(i+vec2(1.,0.),seed),f-vec2(1.,0.)),u.x),
	mix(dot(random2(i+vec2(0.,1.),seed),f-vec2(0.,1.)),
	dot(random2(i+vec2(1.,1.),seed),f-vec2(1.,1.)),u.x),u.y);
}
float fbm1(in vec2 _st){
	float v=0.;
	float a=.95;//                       резкость
	vec2 shift=vec2(100.);
	// Rotate to reduce axial bias
	mat2 rot=mat2(cos(.5),sin(.5),
	-sin(.5),cos(.50));
	for(int i=0;i<octaves;++i){
		v+=a*noise(_st,seed);
		_st=rot*_st*2.+shift;
		a*=.45;
	}
	return v+.5014;//                яркость
}
//-----------
vec2 cMul(vec2 a, vec2 b) {
	return vec2( a.x*b.x -  a.y*b.y,a.x*b.y + a.y * b.x);
}

float pattern(  vec2 p, out vec2 q, out vec2 r )//        туманные облака (не резко)
{
	
	q.x = fbm1( p  +0.00*TIME*.5);
	q.y = fbm1( p + vec2(1.0));
	//r=cMul(q,q);
	r.x = fbm1( p +1.0*q + vec2(1.7,9.2)+0.15*TIME*.5 );
	r.y = fbm1( p+ 1.0*q + vec2(8.3,2.8)+0.126*TIME*.5);
   //	r = cMul(q,q);
	return fbm1(p +1.0*r + 0.0* TIME*.5);
}


vec3 colour(vec2 c,vec2 q,vec2 r) {
	float f = pattern(c,q,r)*1.1;//                 яркость 1.1
	vec3 col = mix(color1,color2,clamp((f*f)*4.0,0.0,1.0));
	col = color2;
	col = mix(col,color3,clamp(length(q),0.0,1.0));
	col = mix(col,color4,clamp(length(r.x),0.0,1.0));
	return (f*f+0.452)*col;
}

void main() {

  vec2 uv=(gl_FragCoord.xy-.5*RENDERSIZE.xy)/RENDERSIZE.y;
  vec2 q=vec2(0.,0.);
  vec2 r=vec2(0.,0.);
  vec3 col=colour(uv,q,r);
  gl_FragColor=vec4(col,1.);
}
