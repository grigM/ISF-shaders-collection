/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#30230.0"
}
*/



#ifdef GL_ES
precision mediump float;
#endif





vec3 rotatey(in vec3 p, float ang)
{
	return vec3(p.x*cos(ang)-p.z*sin(ang),p.y,p.x*sin(ang)+p.z*cos(ang)); 
}
vec3 rotatex(in vec3 p, float ang)
{
	return vec3(p.x,p.y*cos(ang)-p.z*sin(ang),p.y*sin(ang)+p.z*cos(ang)); 
}
vec3 rotatez(in vec3 p, float ang)
{
	return vec3(p.x*cos(ang)-p.y*sin(ang),p.x*sin(ang)+p.y*cos(ang),p.z); 
}

vec4 getSpherePos(int i)
{
	if (i == 0) return vec4(+0.0,+0.0,+0.0, 0.5); 
	if (i == 1) return vec4(+1.0,+0.0,+0.0, 0.5); 
	if (i == 2) return vec4(-1.0,+0.0,+0.0, 0.2); 
	if (i == 3) return vec4(+0.0,+1.0,+0.0, 0.2); 
	if (i == 4) return vec4(+0.0,-1.0,+0.0, 0.2);
	
	return vec4(0,0,0,1); 
}
float intersectSphere(vec3 origin, vec3 ray, vec3 sphereCenter, float sphereRadius) { 
	vec3 toSphere = origin - sphereCenter; 
	float a = dot(ray, ray); 
	float b = 2.0 * dot(toSphere, ray); 
	float c = dot(toSphere, toSphere) - sphereRadius*sphereRadius; 
	float discriminant = b*b - 4.0*a*c; 
	if(discriminant > 0.0) { 
		float t = (-b - sqrt(discriminant)) / (2.0 * a); 
		if(t > 0.0) return t; 
	} 
	return 10000.0; 
} 

float intersectScene(in vec3 ro, in vec3 rd)
{
	ro = rotatey(ro,TIME); 
	rd = rotatey(rd,TIME);
	float tmin = 1000.0; 
	float t;
	int o = -1; 
	for (int i = 0; i < 5; i++) {
		vec4 sphere = getSpherePos(i); 
		t = intersectSphere(ro,rd, sphere.xyz, sphere.w); 
		if (t > 0.0 && t < tmin) {
			tmin = t; 
			o = i; 
		}
	}
	return tmin;
}
float scene(in vec3 p)	
{
	p = rotatey(p,TIME); 
	//p.x = mod(p.x+0.5, 1.0) - 0.5; 
	//p.y = mod(p.y+0.5, 1.0) - 0.5; 
	float d = 1000.0; 
	for (int i = 0; i < 5; i++) {
		vec4 sphere = getSpherePos(i); 
		d = min(d, length(p-sphere.xyz) - sphere.w);
	}
	return d; 
}
vec3 get_normal(in vec3 p)
{
	vec3 eps = vec3(0.001, 0, 0); 
	float nx = scene(p + eps.xyy) - scene(p - eps.xyy); 
	float ny = scene(p + eps.yxy) - scene(p - eps.yxy); 
	float nz = scene(p + eps.yyx) - scene(p - eps.yyx); 
	return normalize(vec3(nx,ny,nz)); 
}


float random(vec3 scale, float seed) { 
	return fract(sin(dot(gl_FragCoord.xyz + seed, scale)) * 43758.5453 + seed); 
} 



void main( void ) {

	vec2 p = 2.0*( gl_FragCoord.xy / RENDERSIZE.xy )-1.0;
	p.x *= RENDERSIZE.x/RENDERSIZE.y; 

	
	vec3 campos = vec3(sin(TIME)*10.0,1,0); 
	vec3 camtar = vec3(0,1,1); 
	vec3 camup = vec3(0,1,0);
	
	vec3 camdir = normalize(camtar-campos);
	vec3 cu = normalize(cross(camdir, camup)); 
	vec3 cv = normalize(cross(cu, camdir)); 
	
	vec3 color = 0.2*vec3(1,1,1)*clamp(1.0-0.5*length(p),0.0,1.0); 
	
	vec3 ro = vec3(0,0,2.0);
	vec3 rd = normalize(vec3(p.x,p.y,-1.0)); 

	vec3 pos = ro; 
	float dist = 0.0; 
	float d; 

	dist = intersectScene(ro,rd); 
	pos = ro+dist*rd;
	if (dist > 0.0 && dist < 10.0 && abs(d) < 0.01) {
		vec3 n = get_normal(pos); 
		vec3 l = normalize(vec3(1,1,1)); 
		vec3 r = reflect(rd, n); 
		float shade = 1.0; 
		float fres = clamp(dot(n,-rd),0.0, 1.0); 
		float spec = pow(clamp(dot(r,normalize(vec3(0,1,0))), 0.0, 1.0), 5.0); 
		float spec1 = pow(clamp(dot(r,normalize(vec3(-1,0,0))), 0.0, 1.0), 16.0)*fres; 
		float spec2 = pow(clamp(dot(r,normalize(vec3(1,0,-1.0))), 0.0, 1.0), 23.0); 
		float spec3 = pow(clamp(dot(r,normalize(vec3(5,0,0.5))), 0.0, 1.0), 16.0)*fres; 
		//float diff = clamp(dot(n,l), 0.0, 1.0); 
		color = 0.1*mix(vec3(1,1,1)*0.8,vec3(1,1,1)*0.5,fres); 
		color += 0.05*vec3(1,1,1)*clamp(-n.y,0.0,1.0); 
		color += vec3(1,1,1)*spec; 
		color += 0.2*vec3(1,1,1)*spec1; 
		//color += 0.1*vec3(1,1,1)*pow(spec1,16.0); 
		color += 0.2*vec3(1,1,1)*spec2; 
		color += 0.1*vec3(1,1,1)*spec3; 
		//color += 0.1*vec3(1,1,1)*pow(spec3,1.0); 
		color *= shade; 
	}
		
	      
	
	gl_FragColor = vec4(color, 1.0); 
}