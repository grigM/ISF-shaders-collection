/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40024.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float sdSphere(vec3 p, float r) {
	return length(p) - r;
}

float sdTorus(vec3 p, vec2 t) {
	vec2 q = vec2(length(p.xy) - t.x, p.z);
	return length(q) - t.y;}

float opS(float d1, float d2) {return max(-d1, d2);}//hg_sdf

//distance to torus.hollow() + blob()_displacement
//tosus shape makes the sinusoidial displacement more apparent
float df(vec3 p){
	float l=sdTorus(p,vec2(2.,.4));
	l*=2.;
		//l=length(p);
	float wallT=.1;//wall thickness in addition to rarius
	float 
	hollow=(min(l,-l+wallT));//NEGATIVE distance to hollow ed object
      //hollowSphere=-max(-(l-1.),l-1.5);//NEGATIVE distance to hollow sphere
      //hollowSphere=-opS(  l-1. ,l-1.5);//NEGATIVE distance to hollow sphere
	float d=sin(p.x)*sin(p.y +TIME)*sin(p.z);//blob() displacement. see hg_sdf
	//d=cos(length(p.yz))-p.x;
	float f=(sin(TIME)*.5+.5)*4.;
	return f*d-hollow;
}//from http://glslsandbox.com/e#38280.0
//lipschits roughly equal to 2., but 6. is also happening, 3. is a good compromise.




vec3 intersect(vec3 from, vec3 rayDir) {
	float totalDist = 0.0;
	vec3 p;
	for(int i = 0; i < 300; i++) {
		p = from + totalDist*rayDir;
		float d = df(p)*.3;
		totalDist += d;
		if(d < 0.001) {
			break;
		}
	}
	return p;
}

vec3 calcNormal(vec3 p) {
	float d = 0.001;
	return normalize(vec3(
		df(p + vec3(d, 0, 0)) - df(p + vec3(-d, 0, 0)),
		df(p + vec3(0, d, 0)) - df(p + vec3(0, -d, 0)),
		df(p + vec3(0, 0, d)) - df(p + vec3(0, 0, -d))
		));
}

void main( void ) {
	vec2 uv = (2.0*gl_FragCoord.xy - RENDERSIZE)/RENDERSIZE.x;
	
	vec3 camPos = vec3(0, 0, -5);
	vec3 camFront = vec3(0, 0, 1.0);
	vec3 camUp = vec3(0, 1.0, 0);
	vec3 camRight = cross(camFront, camUp);
	
	vec3 rayDir = uv.x*camRight + uv.y*camUp + 1.0*camFront;
	
	gl_FragColor = vec4(-calcNormal(intersect(camPos, rayDir)), 1.0);
}