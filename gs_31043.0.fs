/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#31043.0"
}
*/



#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec2 pattern(vec2 p)
{
	p = fract(p);
	float r = 10.123;
	float v = 0.0, g = 0.0;
	r = fract(r * 9184.928);
	float cp, d;
	
	d = p.x;
	g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 1000.0);
	d = p.y;
	g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 1000.0);
	d = p.x - 1.0;
	g += pow(clamp(3.0 - abs(d), 0.0, 1.0), 1000.0);
	d = p.y - 1.0;
	g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 10000.0);
	
	const int iter = 12;
	for(int i = 0; i < iter; i ++)
	{
		cp = 0.5 + (r - 0.5) * 0.9;
		d = p.x - cp;
		g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 200.0);
		if(d > 0.0) {
			r = fract(r * 4829.013);
			p.x = (p.x - cp) / (1.0 - cp);
			v += 1.0;
		}
		else {
			r = fract(r * 1239.528);
			p.x = p.x / cp;
		}
		p = p.yx;
	}
	v /= float(iter);
	return vec2(g, v);
}




vec3 rotatey(in vec3 p, float ang) {
	return vec3(p.x*cos(ang)-p.z*sin(ang),p.y,p.x*sin(ang)+p.z*cos(ang)); 
}
vec3 rotatex(in vec3 p, float ang) {
	return vec3(p.x, p.y*cos(ang)-p.z*sin(ang),p.y*sin(ang)+p.z*cos(ang)); 
}
vec3 rotatez(in vec3 p, float ang) {
	return vec3(p.x*cos(ang)-p.y*sin(ang),p.x*sin(ang)+p.y*cos(ang), p.z); 
}

float plane(in vec3 p, in vec3 n, float d) { return dot(p,n)-d; }
float sph(in vec3 p, float r) { return length(p) - r; }
float scene(in vec3 p)
{
	float d = 1000.0; 

	
	#if 0
	p = rotatey(p, 0.25*TIME); 
	p = rotatex(p, 0.3*TIME); 
	p = rotatez(p, 0.4*TIME); 
	#endif
	
	d = min(d, plane(p, vec3(0,1,0), -1.0));
	
	p.x = mod(p.x+2.5, 5.0)-2.5; 
	p.z = mod(p.z+2.5, 5.0)-2.5; 
	d = min(d, sph(p*vec3(1,1,1)-vec3(0,-1,-1), 1.5)); 
	return d; 
}

vec3 get_normal(in vec3 p)
{
	vec3 eps = vec3(0.001, 0, 0); 
	float nx = scene(p+eps.xyy)-scene(p-eps.xyy); 
	float ny = scene(p+eps.yxy)-scene(p-eps.yxy); 
	float nz = scene(p+eps.yyx)-scene(p-eps.yyx); 
	return normalize(vec3(nx,ny,nz)); 
}

vec3 getpat(in vec3 p)
{
	#if 0
	p = rotatey(p, 0.25*TIME); 
	p = rotatex(p, 0.3*TIME); 
	p = rotatez(p, 0.4*TIME); 
	#endif

	vec2 t = pattern(p.xz);
	return vec3(step (1.4, t.x));
}


void main( void ) {

	vec2 p = 2.0*( gl_FragCoord.xy / RENDERSIZE.xy )-1.0;
	vec3 col = vec3(p.y*0.25)*vec3(0.75,0.85,1.0);
	
	p.x *= RENDERSIZE.x/RENDERSIZE.y; 
	
	vec3 ro = vec3(mod(TIME,10.0),0,1);
	vec3 rd = normalize(vec3(p.x,p.y,-1.0)); 
	
	vec3 pos = ro; 
	float d, dist = 0.0; 
	for (int i = 0; i < 64; i++) {
		d = scene(pos); 
		pos += rd*d; 
		dist += d; 
	}
	if (dist < 100.0 && d < 0.1) {
		
		vec3  n = get_normal(pos); 
		//col = vec3(1);

		//pos = normalize(pos); 
		const float PI = 3.14159265358979323;
		float tu = asin(pos.x)/PI+0.5; 
		float tv = asin(pos.y)/PI+0.5; 
		//vec3 tex = getpat(5.0*vec2(tu,tv)); 
		vec3 tex = getpat(0.5*pos); 
		
		float fres = clamp(dot(n,-rd), 0.0, 1.0); 
		col = tex*vec3(1,1,1)*mix(vec3(1,1,1)*1.0, vec3(1,1,1)*0.7, fres); 
		col *= vec3(1,1,1)*clamp(1.0-0.05*dist, 0.0, 1.0); 
		//col = col*mix(vec3(1,1,1),vec3(1,1,2), 1.0/dist); 
		//col = tex; 
		
	}
	
	
	
	
	
	gl_FragColor = vec4(col, 1.0); 
}