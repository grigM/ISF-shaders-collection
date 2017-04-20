/*{
  "CREDIT": "by mojovideotech",
  "CATEGORIES": [
    "Automatically Converted"
  ],
  "DESCRIPTION": "Automatically converted from http://glslsandbox.com/e#21481.4",
  "INPUTS": []
}*/

// UnicornPoo by mojovideotech
// http://glslsandbox.com/e#21481.4


#ifdef GL_ES
//precision highp float;
precision mediump float;
#endif 

   


#define 	pipi  	36.462159607207912 	// pi pied, pi^pi
#define 	pepi 	23.140692632779269 	// powe(pi);
#define 	chpi 	11.591953275521521  	// cosh(pi)
#define 	pisq  	9.869604401089359	// pi squared, pi^2
#define 	twpi  	6.283185307179586  	// two pi, 2*pi
#define 	pi   	3.141592653589793 	// pi
#define 	sqpi 	1.772453850905516	// square root of pi
#define 	hfpi  	1.570796326794897 	// half pi, 1/pi
#define 	cupi  	1.464591887561523        	// cube root of pi
#define 	prpi 	1.439619495847591 	// pi root of pi
#define 	lnpi  	1.144729885849400 	// logn(pi);
#define 	trpi  	1.047197551196598 	// one third of pi, pi/3
#define 	thpi  	0.996272076220750	// tanh(pi)
#define 	lgpi  	0.497149872694134 	// log(pi)      
#define 	rcpi  	0.318309886183791	// reciprocal of pi  , 1/pi 



float tt = TIME;
float t = (rcpi*(pi+tt/pisq))+pepi;
float k = (rcpi*(pi+tt/chpi))+chpi;
vec3 qAxis = normalize(vec3(sin(t*(prpi)), cos(k*(cupi)), cos(k*(hfpi)) ));
vec3 wAxis = normalize(vec3(cos(k*(-trpi)/pi), sin(t*(rcpi)/pi), sin(k*(lgpi)/pi) ));
vec3 sAxis = normalize(vec3(cos(t*(trpi)), sin(t*(-rcpi)), sin(k*(lgpi)) ));
vec3 camPos = (vec3(0.0, 0.0, 1.0))/(pi+twpi+sin(t)*pi);
vec3 camUp  = (vec3(0.0,1.0,0.0));
float focus = pi+sin(t)*sqpi;
vec3 camTarget = normalize(vec3(cos(t*(-thpi)), sin(t*(trpi)), (sin(t*(lnpi))+cos(t*(lnpi)))*1.5 ));

vec3 rotate(vec3 vec, vec3 axis, float ang)
{
	return vec * cos(ang) + cross(axis, vec) * sin(ang) + axis * dot(axis, vec) * (1.0 - cos(ang));
}

vec3 swr(vec3 p){
	vec3 col = vec3((sin(p))*0.5+0.5);
	for(int i=1; i<2; i++)	{
		float ii = float(i);
		col.xyz=(sin((col.zxy+col.yzx)*ii)*0.5+0.5)+(sin((col.zxy*col.yzx)*ii)*0.5+0.5);
	//	col *= (col+(mix(cos(p*ii+col*3.14)*0.5+0.5,1./(1.+col),sin(p.z)*0.49+0.5)))/hfpi;
	}
	return (normalize(col)*normalize(col.zxy)*normalize(col.yzx))*col;
}

vec3 axr(vec3 p){
	vec3 col = vec3((sin(p)));
	vec3 lol = col;
	for(int i=1; i<2; i++)	{
		float ii = float(i);
		lol.xyz = rotate(col.xyz, qAxis, ii*pi*distance(sAxis,wAxis));
		lol.yzx = rotate(col.yzx, wAxis, ii*pi*distance(qAxis,sAxis));
		lol.zyx = rotate(col.zxy, sAxis, ii*pi*distance(wAxis,qAxis));
		col += lol/pi;
	}
	
	return ((col)*0.5+0.5);
}
void main( void )
{
	vec2 pos = isf_FragNormCoord*pi;
	float ang = (sin(t*lnpi)*pi)+(distance(sAxis,wAxis)+distance(qAxis,sAxis)+distance(wAxis,qAxis));
	camPos = (camPos * cos(ang) + cross(qAxis, camPos) * sin(ang) + wAxis * dot(sAxis, camPos) * (1.0 - cos(ang)))*pi;
	
	vec3 camDir = normalize(camTarget-camPos);
	camUp = rotate(camUp, camDir, sin(t*prpi)*pi);
    	vec3 camSide = cross(camDir, camUp);
	vec3 sideNorm=normalize(cross(camUp, camDir));
	vec3 upNorm=cross(camDir, sideNorm);
	vec3 worldFacing=(camPos + camDir);
    	vec3 rayDir = normalize((worldFacing+sideNorm*pos.x + upNorm*pos.y - camDir*((focus))))/pi;
	vec3 tv=rayDir;
	vec3 rdt=rayDir;
	vec3 clr = (axr(rayDir*pipi));
	float ii = 1.0;
	for(int i=1;i<20;i++) 
	{
		ii += pow(float(i),pi/(float(i*i)));
		rayDir = rotate(rayDir, qAxis, (rdt.z*ii));
		rayDir = rotate(rayDir, wAxis, (rdt.x*ii));
		rayDir = rotate(rayDir, sAxis, (rdt.y*ii));
		tv += rayDir*ii;
		clr += axr(tv)/ii;
		if((clr.x*clr.y*clr.z)>0.9)
		{
			rayDir = ((rayDir*(swr(clr)))/ii);
		}
	}
	clr =  ((axr(tv))/(distance(camPos,tv)));
    clr=sqrt(clr);
	
	gl_FragColor = vec4((clr),1.0);
}