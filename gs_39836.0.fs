/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39836.0"
}
*/


// http://glslsandbox.com/e#25749.0

#ifdef GL_ES
//precision highp float;
precision mediump float;
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
#define phi   1.6180339887498948482045868343656 //golden ratio
#define hfpi  1.5707963267948966192313216916398 //half pi, 1/pi
#define cupi  1.4645918875615232630201425272638 //cube root of pi
#define prpi  1.4396194958475906883364908049738 //pi root of pi
#define lnpi  1.1447298858494001741434273513531 //logn(pi);
#define trpi  1.0471975511965977461542144610932 //one third of pi, pi/3
#define thpi  0.99627207622074994426469058001254//tanh(pi)
#define lgpi  0.4971498726941338543512682882909 //log(pi)      
#define rcpi  0.31830988618379067153776752674503// reciprocal of pi  , 1/pi 
#define rcpipi  0.0274256931232981061195562708591 // reciprocal of pipi  , 1/pipi

float tt = ((TIME*e));
float t = (rcpi*(pi+tt/pisq))+pepi;
float k = (lgpi*(pi+tt/chpi))+chpi;

vec3 qAxis = normalize(vec3(sin(t*(prpi)), cos(k*(cupi)), cos(k*(hfpi)) ));
vec3 wAxis = normalize(vec3(cos(k*(-trpi)/pi), sin(t*(rcpi)/pi), sin(k*(lgpi)/pi) ));
vec3 sAxis = normalize(vec3(cos(t*(trpi)), sin(t*(-rcpi)), sin(k*(lgpi)) ));
float axe = pow(qAxis.x+qAxis.y+qAxis.z+wAxis.x+wAxis.y+wAxis.z+sAxis.x+sAxis.y+sAxis.z,2.0);

vec3 camPos = (vec3(0.0, 0.0, -1.0));
vec3 camUp  = (vec3(0.0,1.0,0.0));
float focus = pi*2.0;
vec3 camTarget = vec3(0.1)*sAxis;

vec3 rotate(vec3 vec, vec3 axis, float ang)
{
    return vec * cos(ang) + cross(axis, vec) * sin(ang) + axis * dot(axis, vec) * (1.0 - cos(ang));
}



vec3 pin(vec3 v)
{
    vec3 q = vec3(0.0);
   
    q.x = sin(v.x)*0.5+0.5;
    q.y = sin(v.y+0.66666667*pi)*0.5+0.5;
    q.z = sin(v.z+1.33333333*pi)*0.5+0.5;
   
    return normalize(q);
}

vec3 spin(vec3 v)
{
    for(int i = 1; i <4; i++)
    {
        v=pin((v.yzx*twpi)*(float(i)));
    }
    return v.zxy;
}


float len( vec3 v )
{
	v=mod(v,vec3(0.5))-0.25;
	return (max(0.0,length(v)-0.2)); 
}

vec3 nrm(vec3 p) {
	vec2 q = vec2(0.0, 0.1);
	return normalize(vec3( len(p + q.yxx) - len(p - q.yxx),
		     len(p + q.xyx) - len(p - q.xyx),
		     len(p + q.xxy) - len(p - q.xxy) ))*(0.5*sin(TIME)*(p.x+p.y)+0.5);
}

void main( void )
{
    vec2 pos = (vv_FragNormCoord)*twpi;//(sin(t*pi)+2.0);
    float ang = (sin(t*lnpi)*pi)+(distance(sAxis,wAxis)+distance(qAxis,sAxis)+distance(wAxis,qAxis));
    camPos = e*(camPos * cos(ang) + cross(qAxis, camPos) * sin(ang) + qAxis * dot(qAxis, camPos) * (1.0 - cos(ang)));
   
    vec3 camDir = normalize(camTarget-camPos);
    camUp = rotate(camUp, camDir, sin(t*prpi)*pi);
    vec3 camSide = cross(camDir, camUp);
    vec3 sideNorm=normalize(cross(camUp, camDir));
    vec3 upNorm=cross(camDir, sideNorm);
    vec3 worldFacing=(camPos + camDir);
    vec3 rayDir = -normalize((worldFacing+sideNorm*pos.x + upNorm*pos.y - camDir*((focus))));
   
    float show=0.0;
   
    float t = .1;
    float temp = 0.;
    for(int i = 0 ; i < 40; i++) {
	temp = len((camPos + rayDir * (t)));
	    if(temp < 0.001) {show = 1.0;break;}
	    if(t>pisq){break;} 
        t += temp;
    }
    vec3 clr = rotate(mod((camPos + rayDir * (t)),vec3(0.5))-0.25,rotate(sAxis,wAxis,ang*pisq),ang*pi+t);
    clr = (show*(spin(clr))/(t))*(dot(rayDir,nrm(mod((camPos + rayDir * (t)),vec3(0.5))-0.25)));
    gl_FragColor = vec4(clr, 1.0);
}