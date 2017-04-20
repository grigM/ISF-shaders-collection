/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39620.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

uniform sampler2D tex;


vec3 rotatey(in vec3 p, float ang) { return vec3(p.x*cos(ang)-p.z*sin(ang),p.y,p.x*sin(ang)+p.z*cos(ang)); }
vec3 rotatex(in vec3 p, float ang) { return vec3(p.x, p.y*cos(ang)-p.z*sin(ang),p.y*sin(ang)+p.z*cos(ang)); }
vec3 rotatez(in vec3 p, float ang) { return vec3(p.x*cos(ang)-p.y*sin(ang),p.x*sin(ang)+p.y*cos(ang), p.z); }

float scene(vec3 p)
{
	p = rotatey(p,TIME*0.25); 
	p = rotatex(p,TIME*0.3); 
	p = rotatez(p,TIME*0.2); 
	float d1 =  length(p) - 0.5 + sin(-4.0*TIME+60.0*p.x)*0.01+sin(1.5*TIME+50.0*p.y)*0.02; 
	return d1;
}

vec3 get_normal(vec3 p)
{
	vec3 eps = vec3(0.001,0,0); 
	float nx = scene(p + eps.xyy) - scene(p - eps.xyy); 
	float ny = scene(p + eps.yxy) - scene(p - eps.yxy); 
	float nz = scene(p + eps.yyx) - scene(p - eps.yyx); 
	return normalize(vec3(nx,ny,nz)); 
}
void main()
{
	vec2 p = 2.0 * (gl_FragCoord.xy / RENDERSIZE) - 1.0; 
	p.x *= RENDERSIZE.x/RENDERSIZE.y; 
	vec3 color = vec3(0); 


	color = vec3(1.0-length(p*0.5))*0.4; 
	
	vec3 ro = vec3(0,0,1.0); 
	vec3 rd = normalize(vec3(p.x,p.y,-1.0));  
	vec3 pos = ro; 
	float dist = 0.0; 
	for (int i = 0; i < 64; i++) {
		float d = scene(pos);
		pos += rd*d;
		dist += d;	
	}
	if (dist < 100.0) {
		vec3 lightpos = vec3(100.0,0.0,0.0); 
		vec3 n = get_normal(pos);
		//vec3 l = normalize(vec3(1,0,0.0)); 
		vec3 l = normalize(lightpos-pos); 
		float diff = 0.0*clamp(dot(n, l), 0.0, 1.0); 
		float fres = clamp(dot(n, -rd), 0.0, 1.0); 
		float amb = 0.1; 
		float spec0 = 0.5*pow(clamp(dot(reflect(n,l), normalize(vec3(-1.0,0,1.0))),0.0,1.0), 50.0); 
		float spec1 = 0.5*pow(clamp(dot(reflect(n,l), normalize(vec3(1.0,0,1.0))),0.0,1.0), 50.0); 
		float spec2 = 3.0*pow(clamp(dot(reflect(n,l), normalize(vec3(0.0,0.5,1.0))),0.0,1.0), 10.0); 
		color = diff*vec3(1.0)/dist;
		color +=0.0*amb*vec3(1.0,1.0,1.0)*clamp(p.y,0.0,1.0)*1.0;
		color = mix(vec3(1,1,1)*0.2,vec3(1,1,1)*0.0,fres);
		//color += spec0*vec3(1,1,1);
		//color += smoothstep(0.0,1.0,spec1)*vec3(1,1,1);
		//color += 1.0*pow(spec1,2.0)*vec3(1,1,1);
		color += smoothstep(0.0,0.5,spec2)*vec3(1,1,1)*pos.y*2.0; 
	}

	gl_FragColor = vec4(color, 1.0); 
}