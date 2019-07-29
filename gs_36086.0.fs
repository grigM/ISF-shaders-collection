/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36086.0"
}
*/


// MG - Now in 3D!
#ifdef GL_ES
precision mediump float;
#endif

#define M_PI		(3.1415926535897932384626433832795)
#define MAXDISTANCE	(64.0)
#define MAXITERATIONS	(64)
#define PRECISION	(0.002)


float rand(vec2 co){
	return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

float sphere(vec3 p, float r) {
	return length(p) - r;
}

float plane(vec3 p, float z) {
	return p.z - z;
}

float udBox(vec3 p, vec3 b, out int object) {
	float tme = TIME*2.0 + 1024.0;
	
	p.y += tme / 2.0;
	float rowOffset = rand(vec2(0.0, floor(p.y/2.0)));
	p.x += 2.0 * rowOffset * tme + tme;
	
	object = int(6.0 * mod(p.x / (32.0*6.0), 1.0)) + 2;
	
	vec2 c = vec2(32.0, 2.0);
	p.xy = mod(p.xy, c)-0.5*c;
		
	return length(max(abs(p)-b,0.0));
}

float getMap(vec3 p, out int object) {
	float distance = MAXDISTANCE;
	float tempDist;
	int tempObject;
	
	distance = plane(p, 0.0);
	object = 1;
	
	tempDist = udBox(p, vec3(12.0, 0.5, 0.5), tempObject);
	if (tempDist <= distance) {
		distance = tempDist;
		object = tempObject;
	}
	
	return distance;
}

vec2 castRay(vec3 origin, vec3 direction, out int object) {
	float distance = 0.0;
	float delta = 0.0;
	object = 0;
	
	for (int i = 0; i < MAXITERATIONS; i++) {
		vec3 p = origin + direction * distance;
		
		delta = getMap(p, object);
		
		distance += delta;
		if (delta < PRECISION) {
			return vec2(distance, float(i));
		}
		if (distance > MAXDISTANCE) {
			object = 0;
			return vec2(distance, float(i));
		}
	}
	
	object = 0;
	return vec2(distance, MAXITERATIONS);
}

vec3 drawScene(vec3 origin, vec3 direction) {
	vec3 color = vec3(0.0, 0.0, 0.0);
	int object = 0;
	
	vec2 ray = castRay(origin, direction, object);
	
	if (object != 0) {
		if (object == 1) {
			color = vec3(1.0, 1.0, 1.0);
		}
		else if (object == 2) {
			color = vec3(1.0, 0.3, 0.3);
		}
		else if (object == 3) {
			color = vec3(0.3, 1.0, 0.3);
		}
		else if (object == 4) {
			color = vec3(0.3, 0.3, 1.0);
		}
		else if (object == 5) {
			color = vec3(1.0, 1.0, 0.3);
		}
		else if (object == 6) {
			color = vec3(0.3, 1.0, 1.0);
		}
		else if (object == 7) {
			color = vec3(1.0, 0.3, 1.0);
		}
	}
	
	return mix(color, vec3(-0.0, -0.0, -0.0), ray.y/float(MAXITERATIONS));
}

void main(void) {
	vec2 p = (gl_FragCoord.xy / RENDERSIZE) * 2.0 - 1.0;
	float aspect = RENDERSIZE.x / RENDERSIZE.y;
	p.y /= aspect;
	p *= 10.0;
	
	vec3 color = vec3(0.0, 0.0, 0.0);
	vec3 origin = vec3(0.0, 0.0, 12.0);
	vec3 direction = normalize(vec3(p.x, p.y, -1.0));
	direction.xy = mat2(cos(M_PI/4.0), sin(M_PI/4.0), -sin(M_PI/4.0), cos(M_PI/4.0)) * direction.xy;
	
	color = drawScene(origin, direction);
	
	gl_FragColor = vec4(color, 1.0);
}