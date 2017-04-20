/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35195.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#define RAY_MARCHING_ITERATINOS 32


float sphere(vec3 p, float r) {
	return length(p) - r;
}

float box(vec3 p, vec3 b)
{
	vec3 d = abs(p) - b - b - b ;
	return min(max(d.x, max(d.y, d.z)), 0.0) + length(max(d, 0.0));
}

float torus(vec3 p, vec2 t)
{
	vec2 q = vec2(length(p.xz) - t.x, p.y);
  	return length(q) - t.y;
}

float union_(float d1, float d2) {
	return min(d1, d2);
}

float difference(float d1, float d2) {
	return max(-d1, d2);
}

float intersect(float d1, float d2) {
	return max(d1, d2);
}

float distance_field(vec3 p) {
	float a = box(p, vec3(0.5));
	float b = torus(p, vec2(0.4, 0.2));
	return mix(a, b, .5+0.25*sin(((TIME*0.1)+length(p.yz))*12.));
}

vec3 normal(vec3 p) {
	float eps = 1e-2;
	float d = distance_field(p);
	vec3 n;
	vec3 ex = vec3(eps, 0.0, 0.0);
	vec3 ey = vec3(0.0, eps, 0.0);
	vec3 ez = vec3(0.0, 0.0, eps);
	n.x = distance_field(p + ex) - d;
	n.y = distance_field(p + ey) - d;
	n.z = distance_field(p + ez) - d;
	return normalize(n);
}

mat3 make_y_rot(float angle) {
	float c = cos(angle);
	float s = sin(angle);
	return mat3(c, 0.0, s, 0.0, 1.0, 0.0, -s, 0.0, c);
}

mat3 make_x_rot(float angle) {
	float c = cos(angle);
	float s = sin(angle);
	return mat3(1.0, 0.0, 0.0, 0.0, c, s, 0.0, -s, c);
}

void main( void ) {
	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy ) - vec2(0.5, 0.5);
	position.x *= RENDERSIZE.x/RENDERSIZE.y;
	
	vec3 origin = vec3(0.0, 0.0, -2.0);
	vec3 ray = normalize(vec3(position, 1.0));
	
	mat3 trans = make_y_rot(TIME*0.5)*make_x_rot(TIME*0.2);
	
	float t = 0.0;
	vec3 p;
	for (int i = 0; i < RAY_MARCHING_ITERATINOS; ++i) {
		p = origin + t*ray;
		p = trans*p;
		float distance = distance_field(p);
		if (distance < 1e-3) break;
		t += distance;
	}
	
	float c = -dot(normal(p), trans*vec3(0.0, -0.5, 1.5))/t;
	gl_FragColor = c*vec4(1.0, 1.0, 1.0, 1.0);
}