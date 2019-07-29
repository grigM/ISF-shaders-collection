/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#56174.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

float sphere (vec3 p,float s){
	return length(p)-s;
}
float rand(vec2 co){
	return fract(sin(dot(co,vec2(12.345,67.89012)))+45678.912);
}
float smoothmin(float a,float b,float k){
	float p = exp(-k*a)+exp(-k*b);
	return -log(p)/k;
}
float dist(vec3 p){
	vec3 pm =p;
	float k =0.43;
	vec2 id = floor(pm.xz/k);
	pm.xz = mod(pm.xz,k)-0.5*k;
	float ran1 = rand(id);
	float ran2 =rand(id.yx);
	float ran3 =rand(id.yx+1.3);
	float ran4 =rand(id.yx+5.7);
	pm.y -=0.2*ran1*abs(sin((ran3+0.8)*TIME*0.3)+ran4);
	pm.xz += 0.1*vec2(ran4,ran3);
	float d1 = sphere(pm,0.1-0.1*ran2);
	float d2 = p.y;
	d1 =smoothmin(d1,d2,17.);
	return d1;
}

vec3 draw (vec3 p,float t){
	vec3 col =vec3(0.);
	col = 0.5*vec3(exp(-0.2*t));
	return col;
}

void main( void ) {

	vec2 p = ( gl_FragCoord.xy / RENDERSIZE.xy )*2.-1.;
	p.y *= RENDERSIZE.y/RENDERSIZE.x;
	vec3 tm = -TIME*vec3(1.,0.,1.)+vec3(0.23,0.,0);
	vec3 ro = vec3(0.3,0.02,0.3)+tm;
	vec3 ta = vec3(0.,0.02,0.)+tm;
	vec3 cdir =normalize(ta-ro);
	vec3 up = vec3(0.,1.,0.);
	vec3 side = cross(cdir,up);
	up = cross(side,cdir);
	float fov = 5.0;
	vec3 rd = normalize(side*p.x+up*p.y+fov*cdir);
	float d;
	float t =1.;
	for(int i=0;i<99;i++){
		d = dist(ro+rd*t);
		t +=d;
	}
	vec3 col = draw(ro+rd*t,t);
	gl_FragColor = vec4(col*vec3(7.7,4.,1.), 7.3 );

}