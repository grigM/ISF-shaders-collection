/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39758.0"
}
*/


//--- torus
// by Catzpaw 2017
#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define ITER 64
#define EPS 0.01
#define NEAR 0.
#define FAR 10.

float map(vec3 p){return 1.-(length(vec2(length(p.xz)-1.8,p.y))-.3);}

float trace(vec3 ro,vec3 rd){float t=NEAR,d;
	for(int i=0;i<ITER;i++){d=map(ro+rd*t);if(abs(d)<EPS||t>FAR)break;t+=step(d,1.)*d*.2+d*.5;}
	vec2 a=ro.xz+rd.xz*t;if(mod(atan(a.y,a.x)*4.+TIME*7.,1.)<.5)t+=3.;
	return min(t,FAR);}

void main(void){
	vec2 uv=(gl_FragCoord.xy-0.5*RENDERSIZE.xy)/RENDERSIZE.y;
	float si=sin(TIME*.3),co=cos(TIME*.3);uv*=mat2(co,-si,si,co);
	float v=1.-trace(vec3(cos(TIME),sin(TIME*.7)*.5,sin(TIME)),vec3(uv,-.5))/FAR;
	gl_FragColor=vec4(mix(vec3(1.5,1.3,1.1),vec3(1.1,1.5,1.3),sin(TIME*.2)*.5+.5)*v,1);
}