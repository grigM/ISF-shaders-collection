/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "test",
    "draft",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llffzj by grigoriev_misha.  test, draft",
  "INPUTS" : [
  		
  		{
			"NAME": "SPEED",
			"TYPE": "float",
			"DEFAULT": 1,
			"MIN": 0.0,
			"MAX": 3.0
		},
		{
			"NAME": "ofset",
			"TYPE": "float",
			"DEFAULT": 0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "line_width",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
  		{
			"NAME": "iterations",
			"TYPE": "float",
			"DEFAULT": 8,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "minorAngleMod",
			"TYPE": "float",
			"DEFAULT": 4.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "cameraDirZ",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.01,
			"MAX": 2.0
		},
		{
			"NAME": "cameraPosX",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -5.0,
			"MAX": 5.0
		},
		{
			"NAME": "cameraPosZ",
			"TYPE": "float",
			"DEFAULT": -3.8,
			"MIN": -5.0,
			"MAX": 0.0
		},
		{
			"NAME": "TorusXmod",
			"TYPE": "float",
			"DEFAULT": 4.0,
			"MIN": 1.5,
			"MAX": 5.0
		},

		{
			"NAME": "planeU_x_mod",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -2.0,
			"MAX": 2.0
		},
		{
			"NAME": "planeU_y_mod",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "planeU_z_mod",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		
		
		{
			"NAME": "planeV_x_mod",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "planeV_y_mod",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.5,
			"MAX": 5.0
		},
		{
			"NAME": "planeV_z_mod",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.5,
			"MAX": 1.5
		}
  ]
}
*/



//Thank you iquilez for some of the primitive distance functions!


const float PI = 3.14159265358979323846264;


const int MAX_PRIMARY_RAY_STEPS = 64; //decrease this number if it runs slow on your computer

vec2 rotate2d(vec2 v, float a) { 
	return vec2(v.x * cos(a) - v.y * sin(a), v.y * cos(a) + v.x * sin(a)); 
}

float sdTorus( vec3 p, vec2 t ) {
  vec2 q = vec2(length(p.xz)-t.x,p.y);
  return length(q)-t.y;
}

float distanceField(vec3 p) {
	return -sdTorus(p, vec2(TorusXmod, 3.0));
}

vec3 castRay(vec3 pos, vec3 dir, float treshold) {
	for (int i = 0; i < MAX_PRIMARY_RAY_STEPS; i++) {
			float dist = distanceField(pos);
			//if (abs(dist) < treshold) break;
			pos += dist * dir;
	}
	return pos;
}

void main() {



	vec2 screenPos = (gl_FragCoord.xy / RENDERSIZE.xy) * 2.0 - 1.0;
	vec3 cameraPos = vec3(cameraPosX, 0.0, cameraPosZ);
	
	vec3 cameraDir = vec3(0.0, 0.0, cameraDirZ);
	vec3 planeU = vec3(planeU_x_mod, planeU_y_mod, planeU_z_mod);
	vec3 planeV = vec3(planeV_x_mod, (RENDERSIZE.y / RENDERSIZE.x) * planeV_y_mod, planeV_z_mod);
	vec3 rayDir = normalize(cameraDir + screenPos.x * planeU + screenPos.y * planeV);
	
	
	vec3 rayPos = castRay(cameraPos, rayDir, 0.01);
	
	float majorAngle = atan(rayPos.z, rayPos.x);
	float minorAngle = atan(rayPos.y, length(rayPos.xz) - minorAngleMod);
		
	float edge = mod(float(int(iterations)) * (minorAngle + majorAngle + ((TIME*SPEED)-ofset)) / PI, 1.0);
	float color = edge < 0.7 ? smoothstep(edge, edge+0.03, line_width) : 1.0-smoothstep(edge, edge+0.03, 0.96);
	
	gl_FragColor = vec4(color);
}
