/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39785.0"
}
*/


//--- marine snow
// by Catzpaw 2017
#ifdef GL_ES
precision mediump float;
#endif

//#extension GL_OES_standard_derivatives : enable


#define ITER 96
#define EPS 0.01
#define NEAR 1.
#define FAR 34.

float map(vec3 p){vec3 p2=floor((p*4.+1.)*.5);p=mod(p*4.+1.,2.)-1.;
	float v=fract(sin(p2.x*133.3)*19.9+sin(p2.y*177.7)*13.3+sin(p2.z*199.9)*17.7);
	if(v<.99)return .8;return length(p)-4.4+v*4.;}

float trace(vec3 ro,vec3 rd){float t=NEAR,d;
	for(int i=0;i<ITER;i++){d=map(ro+rd*t);if(abs(d)<EPS||t>FAR)break;t+=step(d,1.)*d*.1+d*.3;}
	return min(t,FAR);}

void main(void){
	vec2 uv=(gl_FragCoord.xy-0.5*RENDERSIZE.xy)/RENDERSIZE.y;
	float si=sin(TIME*.1),co=cos(TIME*.1);uv*=mat2(si,-co,co,si);
	float v=1.-trace(vec3(0,TIME*7.,-TIME*10.),vec3(uv,-.8))/FAR;
	gl_FragColor=vec4(vec3(1.1,1.1,1.6)*v,1);
}