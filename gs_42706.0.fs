/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#42706.0"
}
*/



#ifdef GL_ES
precision mediump float;
#endif


float Sphere(vec3 p, float r) 
{
	return length(p) - r;
}

float Cube(vec3 p, vec3 b) 
{
	return length(max(abs(p) - b, 0.0));
}

float scene(vec3 p, float t, out vec3 color) 
{
	int c = -1;
	float coobs = 1.;
	float coobs_inner1;
	for (int i = 0; i < 3; i++) 
	{
		t += float(i);
		
		mat3 r = mat3(1, 0, 0,  0, cos(t), -sin(t),  0, sin(t), cos(t));
		r *= mat3(cos(t), 0, sin(t),  0, 1, 0,  -sin(t), 0, cos(t));
		r *= mat3 (cos(t), -sin(t), 0,  sin(t), cos(t), 0,  0, 0, 1);
		
		coobs_inner1 = Sphere((p + vec3(-2 + i * 2, 0., 3.)) * r, 1.2);
		coobs_inner1 = max(Cube((p + vec3(-2 + i * 2, 0., 3.)) * r, vec3(1.0)), -coobs_inner1);
		if(coobs > coobs_inner1)
		{
			coobs = coobs_inner1;
			c = i;
		}
	}
	if (c == -1) color = vec3(0.3, 0.5, 0.5);
	if (c == 0) color = vec3(0.0, 0.0, 0.8);
	if (c == 1) color = vec3(0.0, 0.8, 0.0);
	if (c == 2) color = vec3(0.8, 0.0, 0.0);
	return min(coobs, -p.z);	
}

vec3 normal(vec3 p, float t)
{
	vec3 c;
	float d = 0.001;	
	float dx = scene(p + vec3(d, 0.0, 0.0), t, c) - scene(p + vec3(-d, 0.0, 0.0), t, c);
	float dy = scene(p + vec3(0.0, d, 0.0), t, c) - scene(p + vec3(0.0, -d, 0.0), t, c);
	float dz = scene(p + vec3(0.0, 0.0, d), t, c) - scene(p + vec3(0.0, 0.0, -d), t, c);
	return normalize(vec3(dx, dy, dz));
}

void main(void) 
{
	vec3 pos = vec3(0, 0, -8);
	vec3 dir = normalize(vec3((gl_FragCoord.xy - RENDERSIZE.xy * .5) / RENDERSIZE.x, .5));
	vec3 col;
	
	float t = TIME / 1.5;
	float what = 0.;
	for (int i = 0; i < 35; i++) 
	{
		float dist = scene(pos, t, col);
		pos += dist * dir;
		what += dist;
	}
	vec3 nrm = normal(pos, t);
	gl_FragColor = vec4(col / (1.0 + nrm.y), 1.);
}