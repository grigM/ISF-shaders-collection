/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#14893.19"
}
*/




#ifdef GL_ES
//precision highp float;
precision mediump float;
//precision lowp float;
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
#define sqpi  1.7724538509055160272981674833411 //square root of pi 
#define hfpi  1.5707963267948966192313216916398 //half pi, 1/pi
#define cupi  1.4645918875615232630201425272638 //cube root of pi
#define prpi  1.4396194958475906883364908049738 //pi root of pi
#define lnpi  1.1447298858494001741434273513531 //logn(pi); 
#define trpi  1.0471975511965977461542144610932 //one third of pi, pi/3
#define thpi  0.99627207622074994426469058001254//tanh(pi)
#define lgpi  0.4971498726941338543512682882909 //log(pi)       
#define rcpi  0.31830988618379067153776752674503// reciprocal of pi  , 1/pi  
#define rcpipi  0.0274256931232981061195562708591 // reciprocal of pipi  , 1/pipi 

const int   complexity  = 7; //color level  
const int   depth       = 2; //how deep to go

vec3 rotate(vec3 vect, vec3 axis, float angle)
{
    axis = normalize(axis);
    float s = sin(angle);
    float c = cos(angle);
    float oc = 1.0 - c;
    float azs = axis.z * s;
    float axs = axis.x * s;
    float ays = axis.y * s;
    float ocxy = oc * axis.x * axis.y;
    float oczx = oc * axis.z * axis.x;
    float ocyz = oc * axis.y * axis.z;  
	    
    
    mat4 rm = mat4(oc * axis.x * axis.x + c, ocxy - azs, oczx + ays, 0.0,
                   ocxy + azs, oc * axis.y * axis.y + c, ocyz - axs, 0.0,		   
                   oczx -ays, ocyz + axs, oc * axis.z * axis.z + c,  0.0,
                   0.0, 0.0, 0.0, 1.0);
	
	return (vec4(vect, 1.0)*rm).xyz;
}


vec3 color(vec3 space){
	
	vec3 s = space*pi; 
	space+=(2.+cos(s)+sin(s))/pi;
	s = (rotate(space,-s,length(space)/pi));
	
	for(int i=1;i<complexity+1;i++)
	{
		float ii = float(i);
		float ee = pi/(float(i));
		space +=  (cos(s/ii)+sin(s*ee))*ee;
	        space.x+= (sin(s.z/ii)*sin(s.y/ii)+cos(s.x*ee))*ee;
		space.y+= (sin(s.x/ii)*cos(s.x/ii)+sin(s.y*ee))*ee;
		space.z+= (cos(s.y/ii)*sin(s.z/ii)+cos(s.z*ee))*ee;
		space +=  (rotate(-space,s,ee))/ii;
		
		s += space;
	}
	
	vec3 col = 0.5+sin(s)*0.5;
	vec3 hol = 0.5+cos(s)*0.5;;
	return ((hol+col+(hol*col))/3.);
	//return sol;
}



void main( void )
{
	float t = ((rcpi*TIME+pipi)/pipi)+ptpi;
	vec2 pos = vv_FragNormCoord;
    	vec3 camPos = (vec3(cos(t*(-trpi)), sin(t*(rcpi)), cos(t*(-prpi))));
	vec3 camTarget = normalize(vec3(cos(t*(-lgpi)), cos(t*(rcpi)), sin(t*(-prpi))))*pi;
	vec3 camDir = normalize(camTarget-camPos);
	t*=rcpi;
    	vec3 camUp  = normalize(vec3(cos((t+rcpi)*(lnpi-cupi)), cos(t*(lnpi-rcpi)), sin(t*(lnpi-lnpi))));
    	vec3 camSide = cross(camDir, camUp);
	
	t*=rcpi;
    	float focus =0.75+(cos((sin((t+lgpi)/pi)*twpi*sin((t-lgpi)/(lgpi)))*sin((t-rcpi)/(pipi))))*0.5;
    	vec3 rayDir = normalize(camSide*pos.x + camUp*pos.y + camDir/((focus)));
    	vec3 porxy=camPos+rayDir*(0.20+sin(t)*0.19);
	float diest = 0.0;
	vec3 acCol = color(porxy);
	for(int i=1; i<depth+1; i++)
	{
		porxy += (rayDir/float(i*i*i*i))/pisq;
		diest =  (distance(porxy,camPos));
		acCol = mix(acCol,color(porxy*((sin(t*pi)*twpi+pisq)))/(diest*diest),1./float(i*i));
		
	}
	

	acCol =  normalize(acCol);
	float avLen = (acCol.x+acCol.y+acCol.z)/3.;
	acCol = mix(normalize(1.-avLen/acCol),1.0-acCol/avLen,length(1.0-acCol/avLen));
	vec3 carlz = (acCol*acCol);
	gl_FragColor = vec4(carlz,1.0);
}