/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "ZOOM",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -0.5,
			"MAX": 3.0
			
		},
		{
			"NAME": "TETRAHERDON_p2",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.5,
			"MAX": 1.5
			
		},
		{
			"NAME": "iter",
			"TYPE": "float",
			"DEFAULT": 45,
			"MIN": 0,
			"MAX": 45
			
		},
		
		{
			"NAME": "deform",
			"TYPE": "float",
			"DEFAULT": 0.35,
			"MIN": 0,
			"MAX": 0.35
			
		},
		
		
		{
			"NAME": "ROT_SPEED",
			"TYPE": "float",
			"DEFAULT": 1,
			"MIN": 0,
			"MAX": 5
			
		},
		{
			"NAME": "ROT_OFSET",
			"TYPE": "float",
			"DEFAULT": 0,
			"MIN": 0,
			"MAX": 3
			
		},
		{
			"NAME": "COLOR_SPEED",
			"TYPE": "float",
			"DEFAULT": 1,
			"MIN": 0,
			"MAX": 10
			
		},
		
		{
			"NAME": "COLOR_OFFSET",
			"TYPE": "float",
			"DEFAULT": 0,
			"MIN": 0,
			"MAX": 6
			
		},
		
		{
			"NAME": "GLOW",
			"TYPE": "float",
			"DEFAULT": 0.03,
			"MIN": 0,
			"MAX": 0.1
			
		},

		
	{
		"NAME": "COS_DEFORM",
		"TYPE": "bool",
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#53580.2"
}
*/


// @machine_shaman
#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


//
// tetrahedron
//

#define rot(a) mat2(cos(a + vec4(0, 33, 11, 0)))

float plane(vec3 p, vec3 o, vec3 n) {
	return dot(p - o, n);
}

float tetrahedron(vec3 p, float s) {

	float k = .57735026919;
	
	float a = plane(p, vec3(s, s, s), vec3(-k, k, k));
	float b = plane(p, vec3(s, -s, -s), vec3(k, -k, k));
	float c = plane(p, vec3(-s, s, -s), vec3(k, k, -k));
	float d = plane(p, vec3(-s, -s, s), vec3(-k, -k, -k));
	
	// return max(a, b);
	return max(max(a, b), max(c, d));

}

float map(vec3 p) {
	p.xy *= rot((TIME*ROT_SPEED)+ROT_OFSET);
	p.xz *= rot((TIME*ROT_SPEED)+ROT_OFSET);
	return tetrahedron(p, TETRAHERDON_p2);
	
}

vec3 normal(vec3 p) {
	float eps = .001;
	return normalize(vec3(
		map(vec3(p.x + eps, p.y, p.z)) - map(vec3(p.x - eps, p.y, p.z)),
		map(vec3(p.x, p.y + eps, p.z)) - map(vec3(p.x, p.y - eps, p.z)),
		map(vec3(p.x, p.y, p.z + eps)) - map(vec3(p.x, p.y, p.z - eps))
	));
}

void main() {
	
	
	vec2 uv = (2. * gl_FragCoord.xy - RENDERSIZE) / RENDERSIZE.y;
	
	vec3 col = vec3(0.);
	vec3 ro = vec3(0, 0, -2);
	vec3 rd = vec3(uv, ZOOM);
	
	float t = 0.;
	for (int i = 0; i < int(iter); i++) {
		vec3 p = ro + rd * t;
		float d = map(p);
		t += deform * d;
		col += GLOW / t;
	}
	
	vec3 p = ro + rd * t;
	vec3 N = normal(p);
	vec3 L = vec3(0, 2, -6);
	
	vec3 ld = normalize(L - p);
	float diff = max(dot(ld, N), 0.);
	
	
	col += diff / (1. + t * t * .025);
	col = .5 + .5 * cos((TIME*COLOR_SPEED)+COLOR_OFFSET + col * 4. + vec3(23, 21, 0));
	
	gl_FragColor = vec4(col, 1.);
}