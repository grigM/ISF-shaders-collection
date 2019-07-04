/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54956.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


const vec3 b = vec3(0.5);
const float eps = 0.0001;

mat4 rotationMatrix(vec3 axis, float angle)
{
    axis = normalize(axis);
    float s = sin(angle);
    float c = cos(angle);
    float oc = 1.0 - c;
    
    return mat4(oc * axis.x * axis.x + c         , oc * axis.x * axis.y - axis.z * s, oc * axis.z * axis.x + axis.y * s, 0.0,
                oc * axis.x * axis.y + axis.z * s, oc * axis.y * axis.y + c         , oc * axis.y * axis.z - axis.x * s, 0.0,
                oc * axis.z * axis.x - axis.y * s, oc * axis.y * axis.z + axis.x * s, oc * axis.z * axis.z + c         , 0.0,
                0.0                              , 0.0                              , 0.0                              , 1.0);
}

float box(vec3 p)
{
	vec3 d = abs(p) - b;
	return length(max(d, 0.0)) + min(max(d.x, max(d.y, d.z)), 0.0);
}

float boxf(vec3 p)
{
	vec3 d = abs(p) - vec3(4.0, 1.0, 4.0);
	return length(max(d, 0.0)) + min(max(d.x, max(d.y, d.z)), 0.0);
}

vec3 rot(vec3 p, vec3 ax, float an)
{
	return (vec4(p, 0.0) * rotationMatrix(ax, an)).xyz;
}

float dist(vec3 p)
{
	float P = boxf(p + vec3(0.0, 2.0, 0.0));
	
	float A = box(rot(p + vec3(0.0, -0.5, 0.0), vec3(-1.0, 1.0, 0.0), TIME));
	float B = box(rot(p + vec3(2.5, -2.0, 0.0), vec3(1.0, 0.0, 0.0), -TIME));
	float C = box(rot(p + vec3(0.0, -2.0, 2.5), vec3(0.0, 1.0, 0.0), TIME));
	
	return min(A, min(B, min(C, P)));
}

vec3 normal(vec3 p)
{
	return normalize(vec3(
		dist(p) - dist(vec3(p.x - eps, p.y, p.z)),
		dist(p) - dist(vec3(p.x, p.y - eps, p.z)),
		dist(p) - dist(vec3(p.x, p.y, p.z - eps))
	));
}

float shadow(vec3 ro, vec3 rd)
{
	vec3 cur = ro + eps;
	
	for (int i = 0; i < 64; ++i)
	{
		float d = dist(cur);
		
		if (abs(d) < eps)
			return 0.5;
		
		cur += rd * d;
	}
	
	return 1.0;
}

void main()
{
	vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
	
	vec3 col = vec3(0.0);
	vec3 ray = normalize(vec4(uv, -1.0, 0.0) * rotationMatrix(vec3(1.0, 0.0, 0.0), -0.1) * rotationMatrix(vec3(0.0, 1.0, 0.0), 0.5)).xyz;
	vec3 cam = vec3(1.0, 0.5, 2.0);
	vec3 lightDir = normalize(vec3(0.3, 0.8, 1.0));
	
	vec3 cur = cam;
	
	for (int i = 0; i < 64; ++i)
	{
		float d = dist(cur);
		
		if (abs(d) < eps)
		{
			vec3 norm = normal(cur);
			float diff = clamp(dot(norm, lightDir), 0.0, 1.0);
			float sh   = shadow(cur, norm);
			col = vec3(diff * sh);
			break;
		}
		
		cur += ray * d;
	}
	
	gl_FragColor = vec4(col, 1.0);
}