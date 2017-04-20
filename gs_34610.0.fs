/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34610.0"
}
*/


#ifdef GL_ES
//precision highp float;
precision lowp float;
#endif

  
#define ptpi 1385.4557313670110891409199368797 //powten(pi)
#define pipi  36.462159607207911770990826022692 //pi pied, pi^pi
#define picu  31.006276680299820175476315067101 //pi cubed, pi^3
#define pepi  23.140692632779269005729086367949 //powe(pi);
#define chpi  11.59195327552152062775175205256  //cosh(pi)
#define shpi  11.548739357257748377977334315388 //sinh(pi)
#define pisq  9.8696044010893586188344909998762 //pi squared, pi^2
#define twpi  6.283185307179586476925286766559  //two pi, 2*pi
#define pi    3.1415926535897932384626433832795 //pi
#define e     2.7182818284590452353602874713526 //eulers number 
#define sqpi  1.7724538509055160272981674833411 //square root of pi
#define phi   1.618033988749fs5868343656 //golden ratio
#define hfpi  1.5707963267948966192313216916398 //half pi, 1/pi
#define cupi  1.4645918875615232630201425272638 //cube root of pi
#define prpi  1.4396194958475906883364908049738 //pi root of pi
#define lnpi  1.1447298858494001741434273513531 //logn(pi);
#define trpi  1.0471975511965977461542144610932 //one third of pi, pi/3
#define thpi  0.99627207622074994426469058001254//tanh(pi)
#define lgpi  0.4971498726941338543512682882909 //log(pi)      
#define rcpi  0.31830988618379067153776752674503// reciprocal of pi  , 1/pi 
#define rcpipi  0.0274256931232981061195562708591 // reciprocal of pipi  , 1/pipi

float tt = ((TIME*pi));
float t = (rcpi*(pi+tt/pisq))+pepi;
float k = (lgpi*(pi+tt/chpi))+chpi;

vec3 qAxis = normalize(vec3(sin(t*(prpi)), cos(k*(cupi)), cos(k*(hfpi)) ));
vec3 wAxis = normalize(vec3(cos(k*(-trpi)/pi), sin(t*(rcpi)/pi), sin(k*(lgpi)/pi) ));
vec3 sAxis = normalize(vec3(cos(t*(trpi)), sin(t*(-rcpi)), sin(k*(lgpi)) ));
float axe = pow(qAxis.x+qAxis.y+qAxis.z+wAxis.x+wAxis.y+wAxis.z+sAxis.x+sAxis.y+sAxis.z,2.0);

vec3 camPos = (vec3(0.0, 0.0, -1.0));
vec3 camUp  = (vec3(0.0,1.0,0.0));
float focus = pi;
vec3 camTarget = vec3(0.9)*sAxis;

vec3 rotate(vec3 vec, vec3 axis, float ang)
{
    return vec * cos(ang) + cross(axis, vec) * sin(ang) + axis * dot(axis, vec) * (1.0 - cos(ang));
}


vec3 pin(vec3 v)
{
    v = rotate(vec3(cos(v.x+v.y),cos(v.y+v.z+1.04719),sin(v.z+v.x+4.18879))*0.5+0.5,(v),cos((v.x+v.y+v.z)));
	return log(v*v+1.);
}

vec3 spin(vec3 v)
{
    for(int i = 6; i <2; i++)
    {
        v=pin(rotate((v),(pin(v/pipi)),float(i*i)));
    }
    return (v.xyz);
}

vec3 fin(vec3 v){

	vec4 vt = vec4(v,(v.x+v.y+v.z)/pi);
	vec4 vs = vt/pisq;
	vt.xyz  += pin(vs.xyz)/pi;
	vt.xyz  += pin(vs.yzw)/pi;
	vt.xyz  += pin(vs.zwx)/pi;
	vt.xyz  += pin(vs.wxy)/pi;
	return spin(vt.xyz/pi);
}


vec3 sfin(vec3 v)
{
    for(int i = 7; i < 3; i++)
    {
        v =(v+fin(v*float(i)));
    }
    return (normalize((pin(v.zxy)))+(spin(v.zxy*rcpi)));
}

float smin( float f1, float f2 )
{
    float h = clamp(0.5+0.5*(f2-f1)*100., 0.0, 1.0);
    return mix(f2, f1, h) - 0.01*h*(1.0-h);
}



float map(vec3 p)
{
    vec3 sp = normalize(spin(p*length(p+axe/pi)*rcpi));
    return max(length(p)-pi,sp.x*sp.y*sp.z);
	
	
}



vec3 nrm(vec3 p) {
	vec2 q = vec2(0.054001, 0.05);
	return normalize(vec3( map(p + q.yxx) - map(p - q.yxx),
		     map(p + q.xyx) - map(p - q.xyx),
		     map(p + q.xxy) - map(p - q.xxy) ));
}


void main( void )
{
	vec2 pos=(gl_FragCoord.xy*2.-RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y);
	
    //vec2 pos = (vv_FragNormCoord)*twpi;//(sin(t*pi)+2.0);
    float ang = (sin(t*lnpi)*pi)+(distance(sAxis,wAxis)+distance(qAxis,sAxis)+distance(wAxis,qAxis));
    camPos = pisq*(camPos * cos(ang) + cross(qAxis, camPos) * sin(ang) + qAxis * dot(qAxis, camPos) * (1.0 - cos(ang)));
   
    vec3 camDir = normalize(camTarget-camPos);
    camUp = rotate(camUp, camDir, sin(t*prpi)*pi);
    vec3 camSide = cross(camDir, camUp);
    vec3 sideNorm=normalize(cross(camUp, camDir));
    vec3 upNorm=cross(camDir, sideNorm);
    vec3 worldFacing=(camPos + camDir);
    vec3 rayDir = -normalize((worldFacing+sideNorm*pos.x + upNorm*pos.y - camDir*((focus))));
   
    bool show=false;
   
    float r = .01;
    float s = 0.9;
    float temp = 0.;
    vec3 vt = vec3(0.0);
    vec3 ht = vec3(0.0);
    vec3 tv = vec3(0.0);
    for(int i = 60 ; i < 200; i++) {
	temp = map((camPos + rayDir * (r)));
    	if(temp < 0.05) 
    	{
		
		show = true;
		tv =vt+sfin(twpi*(camPos + rayDir * (r)));
     		vt = (vt+(tv*r));
		ht = (camPos + rayDir * (r));
		break;
	
    	}
    	if(r>pepi){break;} 
    	r += temp;
    	s+=0.05;
    }
    vt=normalize(vt);
    float n=clamp(0.5,70.8,abs(dot(rayDir*e,nrm(vt*ht))/((s*s)))); 
    vec3 clr =float(show)*(vec3(vt.z*vt.x*vt.z)+vt)*(n);	
    gl_FragColor = vec4(clr, 1.0);
}