/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#4658.0"
}
*/


// fun@exile

#ifdef GL_ES
precision highp float;
#endif


#define PI 3.14159

#define MAX_ITER_RENDER 100
#define MAX_ITER_SHADOW 20
#define MAX_DIST 20.0
#define EPS_DIST 0.01
#define EPS_GRAD 0.005

float hash(float n)
{
    return fract(sin(n)*43758.5453);
}

float noise(in vec3 x)
{
    vec3 p = floor(x);
    vec3 f = fract(x);

    f = f*f*(3.0-2.0*f);

    float n = p.x + p.y*57.0 + 113.0*p.z;

    float res = mix(mix(mix(hash(n+  0.0), hash(n+  1.0),f.x),
                        mix(hash(n+ 57.0), hash(n+ 58.0),f.x),f.y),
                    mix(mix(hash(n+113.0), hash(n+114.0),f.x),
                        mix(hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
    return res;
}

float sd_plane(in vec3 p, in vec4 n)
{
	return dot(p, n.xyz) - n.w;
}

float sd_sphere(in vec3 p, in float r)
{
	return length(p) - r;
}

float sd_box(in vec3 p, in vec3 e)
{
	vec3 q = abs(p) - e;
	return min(max(q.x,max(q.y,q.z)), 0.0) + length(max(q, 0.0));
}

float sd_torus(in vec3 p, in vec2 t)
{
	vec2 q = vec2(length(p.xz)-t.x, p.y);
	return length(q) - t.y;
}

float ud_box(in vec3 p, in vec3 e)
{
	return length(max(abs(p)-e, 0.0));
}

float ud_box_round(in vec3 p, in vec3 e, in float r)
{
	return ud_box(p, e) - r;
}

float op_union(in float d0, in float d1)
{
	return min(d0, d1);
}

float op_subtract(in float d0, in float d1)
{
	return max(-d1, d0);
}

float op_intersect(in float d0, in float d1)
{
	return max(d0, d1);
}

float op_blend(in float d0, in float d1, in float a)
{
	return mix(d0, d1, a);
}

vec3 translate(in vec3 p, in vec3 t)
{
	return p-t;
}

vec3 repeat(vec3 p, in vec3 s)
{
	p.x = mod(p.x + 0.5*s.x, s.x) - 0.5*s.x;
	p.y = mod(p.y + 0.5*s.y, s.y) - 0.5*s.y;
	p.z = mod(p.z + 0.5*s.z, s.z) - 0.5*s.z;
	return p;
}

vec3 rotateX(in vec3 p, in float a)
{
	float s = sin(a);
	float c = cos(a);
	return vec3(p.x, mat2(c, -s, s, c)*p.yz);
}

vec3 rotateY(in vec3 p, in float a)
{
	float s = sin(a);
	float c = cos(a);
	vec2 q = mat2(c, s, -s, c)*p.xz;
	return vec3(q.x, p.y, q.y);
}

vec3 rotateZ(in vec3 p, in float a)
{
	float s = sin(a);
	float c = cos(a);
	return vec3(mat2(c, -s, s, c)*p.xy, p.z);
}

float scene(in vec3 p)
{
	p = rotateZ(p, -0.005*sin(0.5*TIME)*p.z*p.z);

	vec3 q = p;
	
	float k0 = noise(p);
	
	q.x += 0.5*cos(TIME+p.y);
	q.y += 0.5*sin(TIME+p.x);
	
	p.x = q.x;
	p.y = q.y;
	
	q.y += k0;

	float k1 = noise(q+vec3(0.0,0.0,8.0*k0+2.0*TIME));
	
	q.x -= 0.1*k1;
	q.z += 0.2*k1;
	
	float t = clamp(pow(2.0*pow(sin(0.2*TIME), 3.0), 2.0), 0.0, 1.0);
	
	q = mix(q, p, t);
	q = repeat(translate(q, vec3(1.0, 1.0, -7.5)), vec3(2.0, 2.0, 4.0));
	
	float d0 = op_intersect(sd_box(q, vec3(0.5, 0.5, 0.5)), sd_sphere(q, 0.7));
	float d1 = sd_box(rotateX(rotateY(q, 2.0*TIME), TIME), vec3(0.5, 0.5, 0.5));
	
	return op_blend(d0, d1, t);
}

vec3 gradient(in vec3 p)
{
	const vec3 dx = vec3(EPS_GRAD, 0.0, 0.0);
	const vec3 dy = vec3(0.0, EPS_GRAD, 0.0);
	const vec3 dz = vec3(0.0, 0.0, EPS_GRAD);
	
	return vec3(
		scene(p+dx) - scene(p-dx),
		scene(p+dy) - scene(p-dy),
		scene(p+dz) - scene(p-dz)
	);
}

float shadow(in vec3 p, in vec3 v, in float s, in float k)
{
	float a = 1.0;
	float t = 10.0*EPS_DIST;
	float d = MAX_DIST;
	
	for (int i = 0; i < MAX_ITER_SHADOW; i++)
	{
		if (t >= s || d <= EPS_DIST)
		{
			break;
		}

		d = scene(p + v*t);
		a = min(a, k*(d/t));
		t += d;
	}
	
	if (d <= EPS_DIST) 
	{
		a = 0.0;
	}

	return 1.0-a;
}

void render(in vec3 p, in vec3 v, in vec3 pL, out vec4 c)
{
	float t = 0.0;
	float d = MAX_DIST;
	float dL;
	
	vec3 q;
	
	vec3 L;
	vec3 N;
	vec3 V = -v;
	vec3 R;
	
	vec3 Ia;
	vec3 Id;
	vec3 Is;
	float kS = 20.0;
	
	for (int i = 0; i < MAX_ITER_RENDER; i++)
	{
		if (t >= MAX_DIST || d <= EPS_DIST)
		{
			break;
		}

		q = p + v*t;
		d = scene(q);
		t += d;
	}
	
	if (d <= EPS_DIST)
	{
		dL = distance(q, pL);
		
		L = (pL-q) / dL;
		N = normalize(gradient(q));
		R = reflect(-L, N);
				
		Ia = vec3(0.1, 0.1, 0.1);
		Id = vec3(0.5, 0.5, 0.5) * max(0.0, dot(N, L));
		Is = vec3(1.0, 0.7, 0.5) * pow(max(0.0, dot(R, V)), kS);
				
		c.rgb = Ia + Id + Is;//(1.0 - shadow(q, L, dL, 8.0)) * (Id + Is);
		c.rgb *= vec3(1.0 - t/MAX_DIST);
		c.a = 1.0;
	}
}

void main()
{
	float ey = tan(0.25 * PI);
	float ex = ey * (RENDERSIZE.x / RENDERSIZE.y);
	
	float py = ey * (gl_FragCoord.y / RENDERSIZE.y - 0.5);
	float px = ex * (gl_FragCoord.x / RENDERSIZE.x - 0.5);
	
	vec3 p = vec3(px, py, -1.0);
	vec3 v = normalize(p);
	
	vec3 pL = vec3(mouse.x*30.0-15.0, mouse.y*30.0-15.0, 0.0);
	
	render(p, v, pL, gl_FragColor);	
}