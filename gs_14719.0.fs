/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#14719.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

//Gasy
//kosmync64
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
 
const int   complexity  = 1; 

vec3 color(vec3 space){

	vec3 s=(0.5*sin(space+cos(length(space)))+0.5); 
	space = s;
	
	for(int i=1;i<complexity+1;i++)
	{
		float ii = sqrt(float(i)/pi);
		float ee = pi/(float(i));
		
		space.x+=ee*(cos(s.z*(ii*pi))*cos(s.y*(ii*hfpi))+(sin(s.x*(ee*sqpi)))+(sin(s.z*(ee*pi))));
		space.y+=ee*(sin(s.z*(ii*pi))*cos(s.x*(ii*hfpi))+(cos(s.y*(ee*sqpi)))+(cos(s.z*(ee*pi))));
		space.z+=ee*(cos((s.y+s.x+s.z)*(ii*pi))+sin(s.x*(ii*hfpi))+(cos(s.z*(ee*sqpi)))+(sin(s.y*(ee*hfpi))));
		s += space/pi;
	}
	vec3 sol = normalize(s);
	float lol = (sol.x+sol.y+sol.z)/3.;
	vec3 col = 0.5+sin(s)*0.5;
	vec3 hol = 0.5+cos(s)*0.5;;
	col*=(col*col*hol*length(hol*col));
	//return lol*(hol+col+abs(sin(space/hfpi)))/2.;
	return ((hol+col))*lol;
	//return sol;
}
vec3 getGas(vec2 p){
	float ret = ((cos(p.y*90.0)+1.0)*0.25)+
	       ((sin(p.x*90.0)+1.0)*0.25);
	
	return vec3(
		ret + clamp(sin( p.x * 0.6 ), 0.0, 0.15 ),
		ret - clamp(cos( p.y * 0.8 ), 0.0, 0.13 ),
		ret - clamp(tan( (p.y + p.x) * 0.5 ), -0.5, 0.5 )
		);
}

void main( void ) {

	vec2 position = ( vv_FragNormCoord);
	
	vec2 p = vec2(position.x+position.y,position.x-position.y);
	float dist;
	for(int i=1;i<50;i++){
		vec2 newp=p;
		newp.x+=(0.11/float(i))*sin(float(i)*p.y*4.0+TIME*0.3)*2.2+1.2;
		newp.y+=(0.1/float(i))*sin(float(i)*p.x*3.0+TIME*0.3)*1.2-1.2;
		dist += distance(p,newp)*float(i);
		p=newp;
		
	}

	vec3 clr = (getGas(p/sqrt(dist)));
	clr *= clr*color(0.6*clr);
	
	gl_FragColor = vec4( clr, 1.0 );
}