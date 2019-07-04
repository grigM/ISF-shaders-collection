/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel1",
      "PATH" : "85a6d68622b36995ccb98a89bbb119edf167c914660e4450d313de049320005c.png"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XdSGRR by knighty.  This shader scrambles a picture using the Hilbert curve. If you wait looong enougth it will return to it's initial unscrambled state.\nBy uncommenting the first line, it also shows how the hilbert coding can be used to compress image files...",
  "INPUTS" : [
	{
			"NAME": "inputImage",
			"TYPE": "image"
	},
	{
            "NAME": "iter",
            "TYPE": "float",
           "DEFAULT": 9.0,
            "MIN": 0.0,
            "MAX": 15.0
      }
  ]
}
*/


//#define SHOWPACKING
//const int MaxIter=int(iter);//try other values .
//For more fun use the 'checker' texture (2nd row 5th column)

float scl=1.;
float scl2=1.1;
void init(){
	scl=pow(0.5,float(int(iter)));
	scl2=scl*scl;
}

//Coposition of two "rotations"
vec2 fG(vec2 t0, vec2 t1){
	return vec2(dot(t0,t1), dot(t0, t1.yx));
}

//Action of rotation on "elementary" coordinates
vec2 fA(vec2 t, vec2 p){
	return fG(t,p-vec2(0.5))+vec2(0.5);
}

//Given "elementary" coordinates of position, returns the corresponding "rotation".
vec2 fCg(vec2 p){
	return vec2(p.y, (1.-2.*p.x)*(1.-p.y));
}

//Given "elementary" coordinates of position (c=2*p.x+p.y), returns the "elementary" linear coordinates
float fL(float c){
	return max(0.,0.5*((-3.*c+13.)*c-8.));
}

//Given a point inside unit square, return the linear coordinate
float C2L(vec2 p){
	vec2 t=vec2(1.,0.);//initial rotation is the identity
	float l=0.;//initial linear coordinate
	for(int i=0; i<int(iter);i++){
		p*=2.; vec2 p0=floor(p); p-=p0;//extract leading bits from p. Those are the "elementary" (cartesian) coordinates.
		p0=fA(t,p0);//Rotate p0 by the current rotation
		t=fG(t,fCg(p0));//update the current rotation
		float c= p0.x*2.+p0.y;
		l=l*4.+fL(c);//update l
	}
	return l*scl2;//scale the result in order to keep between 0. and 1.
}

//Given the linear coordinate of a point (in [0,1[), return the coordinates in unit square
//it's the reverse of C2L
vec2 L2C(float l){
	vec2 t=vec2(1.,0.);
	vec2 p=vec2(0.,0.);
	for(int i=0; i<int(iter);i++){
		l*=4.; float c=floor(l); l-=c;
		c=0.5* fL(c);
		vec2 p0=vec2(floor(c),2.*(c-floor(c)));
		t=fG(t,fCg(p0));
		p0=fA(t,p0);
		p=p*2.+p0;
	}
	return p*scl;
}

float dist2box(vec2 p, float a){
	p=abs(p)-vec2(a);
	return max(p.x,p.y);
}

float d2line(vec2 p, vec2 a, vec2 b){//distance to line (a,b)
	vec2 v=b-a;
	p-=a;
	p=p-v*clamp(dot(p,v)/(dot(v,v)),0.,1.);//Fortunately it still work well when a==b => division by 0
	return min(0.5*scl,length(p));
}

void main() {



	vec2 uv = 0.5-(gl_FragCoord.xy-0.5*RENDERSIZE.xy) / RENDERSIZE.y;
	init();
	gl_FragColor = vec4(1.0);
	float ds=dist2box(uv-0.5,.5-0.5*scl);
	if(ds>0.5*scl) return;
#ifndef SHOWPACKING
	//scramble the texture
	float l=C2L(uv);
	float t=mod(1./4.*scl*TIME,1.)*1./scl2;
	l=mod(l+t*scl2,1.);
	vec2 ps=L2C(l)+vec2(.5*scl);
	gl_FragColor = IMG_NORM_PIXEL(inputImage,mod(ps,1.0),-1.);
#else
	//shows the texture along the Hilbert curve
	uv=floor(uv/scl)*scl;
	float l=uv.x*scl+uv.y;
	vec2 ps=L2C(l)+vec2(.5*scl);
	gl_FragColor = IMG_NORM_PIXEL(iChannel0,mod(ps,1.0),-1.);
#endif
}
