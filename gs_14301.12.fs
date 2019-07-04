/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#14301.12"
}
*/




#ifdef GL_ES
//precision highp float;
precision mediump float;
//precision lowp float;
#endif


const float ptpi = 1385.4557313670110891409199368797; //powten(pi)
const float pipi = 36.462159607207911770990826022692; //pi pied, pi^pi
const float picu = 31.006276680299820175476315067101; //pi cubed, pi^3
const float pepi = 23.140692632779269005729086367949; //powe(pi);
const float chpi = 11.59195327552152062775175205256 ; //cosh(pi)
const float shpi = 11.548739357257748377977334315388; //sinh(pi)
const float pisq = 9.8696044010893586188344909998762; //pi squared, pi^2
const float twpi = 6.283185307179586476925286766559 ; //two pi, 2*pi 
const float pi   = 3.1415926535897932384626433832795; //pi
const float sqpi = 1.7724538509055160272981674833411; //square root of pi 
const float hfpi = 1.5707963267948966192313216916398; //half pi, 1/pi
const float cupi = 1.4645918875615232630201425272638; //cube root of pi
const float prpi = 1.4396194958475906883364908049738; //pi root of pi
const float lnpi = 1.1447298858494001741434273513531; //logn(pi); 
const float trpi = 1.0471975511965977461542144610932; //one third of pi, pi/3
const float thpi = 0.99627207622074994426469058001254;//tanh(pi)
const float lgpi = 0.4971498726941338543512682882909; //log(pi)       
const float rcpi = 0.31830988618379067153776752674503;// reciprocal of pi  , 1/pi
 
const int   complexity  = 5; //How deep to smoot 
const int   twerk       = 16; //how much to twerk 
 
vec3 smoot(vec3 v) 
{
	//some fixex
	float tc = sin(v.z*pi);
	vec3 p=vec3(cos(v.x)*sin(tc+v.z*sqpi),sin(v.y*cupi)*cos(tc+v.z),cos(lgpi*v.x+tc)*sin(lgpi*v.y+tc));
	vec3 q=normalize(vec3(v.x+cos(tc*hfpi+cupi)*pi,v.y+sin(tc*hfpi+cupi)*pi,v.x+v.y));
	vec3 r=normalize(p);
	p = normalize((p+q+r));
	vec3 swapp=p;
	
  	for(int i=1;i<complexity+1;i++)
  	{
		swapp = p + vec3( cos(sqrt(abs(p.x*q.z))*pi)+sin(p.y)*(cos(float(i)*lgpi)), 
				 sin(sqrt(abs(p.y*q.z))*pi)+cos(p.x)*(sin(float(i)*lgpi)), 
				 ((sin(p.z+p.z)+cos((q.y*q.x)+tc)+sin(p.z+float(i)/pi))) );
		p = swapp;
		
		q += cos(sqrt(abs(p))*pi)+0.5;
		r += (normalize(q)/pi)+((q*float(i)/(pisq))*pi)+0.5;
  	}
	q = normalize((q+r));
	float d = cos((q.x+q.y+q.z)*twpi)*0.4+0.6;
	p=(((cos(p))))*pi;
	vec3 col=(vec3((sin(p.x)*0.5+0.5)*d,((sin(p.y)*cos(p.z))*0.5+0.5)*d,(cos(p.z+p.y)*0.5+0.5)*d));
	vec3 hol = normalize(col);
	return (col+hol)/cupi;
	
}
 vec3 rotate(vec3 vect, vec3 axis, float angle)
{
    axis = normalize(axis);
    float s = sin(angle);
    float c = cos(angle);
    float oc = 1.0 - c;
    
    mat4 rm = mat4(oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,  0.0,
                oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,  0.0,
                oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c,           0.0,
                0.0,                                0.0,                                0.0,                                1.0);
	return (vec4(vect, 1.0)*rm).xyz;
}


void main( void )
{
	float t = ((TIME+pipi)/pipi)+pipi;
	vec2 pos = vv_FragNormCoord;
    	vec3 camPos = (vec3(cos(t*hfpi-lgpi)*pi, sin(t*hfpi-rcpi)*pi, cos(t*hfpi-cupi)*pi));
    	vec3 camTarget = normalize(vec3(cos(t*sqpi-lgpi), cos(t*sqpi-rcpi), sin(t*sqpi-cupi)))*pi;
	
	t*=lgpi;
    	vec3 camDir = normalize(camTarget-camPos);
    	vec3 camUp  = normalize(vec3(cos((t+rcpi)*(sqpi-cupi)), cos(t*(sqpi-rcpi)), sin(t*(cupi-rcpi))));
    	vec3 camSide = cross(camDir, camUp);
	
	t*=rcpi;
    	float focus = hfpi+(cos((sin((t+lgpi)/pi)*twpi*sin((t-lgpi)/(lgpi)))*sin((t-rcpi)/(pipi)))+sin((cos((t+hfpi)/pi)*pi*cos((t+prpi)/(lgpi)))*cos(t/(pipi))));
    	vec3 rayDir = normalize(camSide*pos.x + camUp*pos.y + camDir/(sqrt(focus)));
    	vec3 porxy = camPos;
	float diest = 0.0;
	float fjior = 0.0;
	vec3 culnd = vec3(0.0,0.0,0.0);
	
	
	for(int i=1; i<twerk; i++){
		float nern = twpi/log(float(twerk-i));
		rayDir += rotate(rayDir, rayDir+normalize(camPos+camTarget+camUp)+normalize(camPos-camTarget+camUp+porxy)+normalize(sin(camPos)+camTarget-camUp-porxy), (length(rayDir)+sin(pos.x+pos.y)+cos(porxy.x+porxy.y+porxy.z))*twpi)/(nern*pi);
		porxy += rayDir;
		if( length(porxy-camPos)>diest) 
		{
			diest = (length(porxy-camPos));}
			fjior+=1.0/nern;
			culnd += smoot(porxy/(pi*(sin(t)*pisq+chpi)))/nern; 
		
		
	}
	
	culnd /= fjior;
	float shuld = 1.0/sqrt(diest/(shpi));
	shuld *= shuld*shuld*shuld*shuld;
	vec3 carlz = culnd*shuld;
	//vec3 carlz = vec3(1.0,1.0,1.0)*shuld;
	
	gl_FragColor = vec4(carlz,1.0);
}