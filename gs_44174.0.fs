/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#44174.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

#define MAX_ITER 64
#define EPSILON 0.01
#define FOG_COLOR vec3(0.5, 0.5, 0.5)
#define FOG_START 64.0


vec3 camPos = vec3(0.0, 0.0, 0.0);
vec3 lightPos = vec3(5.0, 10.0, 0.0);

struct raymarch_result_t {
	bool hit;
	vec3 pos;
	vec3 normal;
	vec3 color;
};
	
raymarch_result_t raymarch(vec3 origin, vec3 dir);
mat4 inverse(mat4 m);

float sdPlane(vec3 p, vec4 n)
{
  return dot(p, n.xyz) + n.w;
}

float sdBox( vec3 p, vec3 b )
{
  vec3 d = abs(p) - b;
  return min(max(d.x,max(d.y,d.z)),0.0) + length(max(d,0.0));
}

vec3 opTx( vec3 p, mat4 m )
{
    vec3 q = (inverse(m) * vec4(p, 1.0)).xyz;
    return q;
}

float opSubtract(float d1, float d2) {
	return max(-d2, d1);
}

mat4 translation(vec3 pos) {
	return mat4(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, pos.x, pos.y, pos.z, 1.0);
}

mat4 rotationZ(float ang) {
	mat4 m = mat4(1.0);
	m[0][0] = cos(ang);
	m[1][0] = -sin(ang);
	m[0][1] = sin(ang);
	m[1][1] = cos(ang);
	return m;
}

mat4 rotationY(float ang) {
	mat4 m = mat4(1.0);
	m[0][0] = cos(ang);
	m[2][0] = sin(ang);
	m[0][2] = -sin(ang);
	m[2][2] = cos(ang);
	return m;
}

float calcTotalDistance(vec3 origin) {
	float plane = sdPlane(vec3(0.0, -4.0, 0.0) - origin, vec4(0.0, -1.0, 0.0, 0.0));
	
	float baseBox = opSubtract(sdBox(opTx(origin, translation(vec3(0.0, -2.5, 10))), vec3(6.0, 0.4, 6.0)), sdBox(opTx(origin, translation(vec3(0.0, -2.5, 10))), vec3(3.0, 0.8, 3.0)));
	
	float animT = mod(TIME, 12.0) / 8.0;
	
	vec3 pos = vec3(0.0, (5.0 - animT * 5.0), 0.0);
	float rotation = 0.0 - animT * 0.0;
	
	if(animT > 1.0) {
		pos = vec3(0.0, 0.0, 0.0);
		rotation = 0.0;
		
		if(animT > 1.1) {
			float newAnimT = animT - 2.0;
			float raisedT = sin(min(3.1415 / 16.0 / 2.0, newAnimT) * 8.0);
			pos = vec3(0.0, raisedT * 0.0, 0.0) + raisedT * vec3(sin(newAnimT * 00.0 * raisedT), sin(newAnimT * 0.0 * raisedT), sin(newAnimT * 0.0 * raisedT));
			rotation = cos(raisedT * 0.0) * raisedT;
		}
	}
	
	float box = sdBox(opTx(origin, translation(vec3(0.0, -2.5 + 1.0, 10) + pos) * rotationY(rotation + 0.0)), vec3(3.0, 0.4, 3.0));
	
	return min(plane, min(baseBox, box));
	//return min(min(sphere, plane), box);
}

vec3 calcNormal(vec3 p) {
    return normalize(vec3(
        calcTotalDistance(vec3(p.x + EPSILON, p.y, p.z)) - calcTotalDistance(vec3(p.x - EPSILON, p.y, p.z)),
        calcTotalDistance(vec3(p.x, p.y + EPSILON, p.z)) - calcTotalDistance(vec3(p.x, p.y - EPSILON, p.z)),
        calcTotalDistance(vec3(p.x, p.y, p.z  + EPSILON)) - calcTotalDistance(vec3(p.x, p.y, p.z - EPSILON))
    ));
}

raymarch_result_t raymarch_shadow(vec3 origin, vec3 dir) {
	raymarch_result_t result;
	
	vec3 pos = origin;
	float totalDist = 0.0;
	for(int i = 0; i < MAX_ITER; i++) {
		float dist = calcTotalDistance(pos);
		
		if(dist < EPSILON) {
			result.hit = true;
			result.pos = pos;
			
			return result;
		}
		
		pos += dir * dist;
		totalDist += dist;
		
		if(totalDist > FOG_START) {
			return result;
		}
	}
	
	return result;
}

raymarch_result_t raymarch(vec3 origin, vec3 dir) {
	raymarch_result_t result;
	
	vec3 pos = origin;
	float totalDist = 0.0;
	for(int i = 0; i < MAX_ITER; i++) {
		float dist = calcTotalDistance(pos);
		
		if(dist < EPSILON) {
			result.hit = true;
			result.pos = pos;
			result.normal = calcNormal(pos);
			result.color = vec3(1.0, 1.0, 1.0) * (0.3 + max(0.0, dot(result.normal, normalize(lightPos - pos))) * 0.7);
			
			raymarch_result_t r = raymarch_shadow(lightPos, normalize(pos - lightPos));
			if(r.hit) {
				if(length(r.pos - pos) > EPSILON * 10.0) {
					result.color *= 0.1;
				}
			}
			
			return result;
		}
		
		pos += dir * dist;
		totalDist += dist;
		
		if(totalDist > FOG_START) {
			return result;
		}
	}
	
	return result;
}

vec3 render(vec2 position) {
	raymarch_result_t result = raymarch(camPos, vec3((position.x - 0.5) * 2.0, (position.y - 0.5) * 2.0, 1.0));
	
	if(!result.hit)
		return FOG_COLOR;
	
	float dist = length(result.pos - camPos);
	float dupa = min(1.0, dist / FOG_START);
	
	return result.color * (1.0 - dupa) + FOG_COLOR * dupa;
}

void main( void ) {
	vec2 position = (gl_FragCoord.xy / RENDERSIZE.yy) - vec2(RENDERSIZE.y / RENDERSIZE.x, 0.0);

	gl_FragColor = vec4(render(position), 1.0);
}

mat4 inverse(mat4 m) {
  float
      a00 = m[0][0], a01 = m[0][1], a02 = m[0][2], a03 = m[0][3],
      a10 = m[1][0], a11 = m[1][1], a12 = m[1][2], a13 = m[1][3],
      a20 = m[2][0], a21 = m[2][1], a22 = m[2][2], a23 = m[2][3],
      a30 = m[3][0], a31 = m[3][1], a32 = m[3][2], a33 = m[3][3],

      b00 = a00 * a11 - a01 * a10,
      b01 = a00 * a12 - a02 * a10,
      b02 = a00 * a13 - a03 * a10,
      b03 = a01 * a12 - a02 * a11,
      b04 = a01 * a13 - a03 * a11,
      b05 = a02 * a13 - a03 * a12,
      b06 = a20 * a31 - a21 * a30,
      b07 = a20 * a32 - a22 * a30,
      b08 = a20 * a33 - a23 * a30,
      b09 = a21 * a32 - a22 * a31,
      b10 = a21 * a33 - a23 * a31,
      b11 = a22 * a33 - a23 * a32,

      det = b00 * b11 - b01 * b10 + b02 * b09 + b03 * b08 - b04 * b07 + b05 * b06;

  return mat4(
      a11 * b11 - a12 * b10 + a13 * b09,
      a02 * b10 - a01 * b11 - a03 * b09,
      a31 * b05 - a32 * b04 + a33 * b03,
      a22 * b04 - a21 * b05 - a23 * b03,
      a12 * b08 - a10 * b11 - a13 * b07,
      a00 * b11 - a02 * b08 + a03 * b07,
      a32 * b02 - a30 * b05 - a33 * b01,
      a20 * b05 - a22 * b02 + a23 * b01,
      a10 * b10 - a11 * b08 + a13 * b06,
      a01 * b08 - a00 * b10 - a03 * b06,
      a30 * b04 - a31 * b02 + a33 * b00,
      a21 * b02 - a20 * b04 - a23 * b00,
      a11 * b07 - a10 * b09 - a12 * b06,
      a00 * b09 - a01 * b07 + a02 * b06,
      a31 * b01 - a30 * b03 - a32 * b00,
      a20 * b03 - a21 * b01 + a22 * b00) / det;
}