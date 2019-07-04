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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#44648.0"
}
*/


//precision mediump float;


//see: http://glslsandbox.com/e#31291.9

vec3 barycentric(vec2 uv);
float bound(float angle);
float fold(in float x);
vec3 fold(in vec3 p);
float origami(in vec3 position);
float noise(in vec2 uv);
float fbm(in vec2 uv);
float sphere(vec2 position, float radius);
float cube(vec2 position, vec2 scale);
float segment(vec2 position, vec2 a, vec2 b);
vec2 project(vec2 position, vec2 a, vec2 b);


vec2 hash(vec2 v) 
{
	vec2 n;
	n.x=fract(cos(v.y-v.x*841.0508)*(v.y+v.x)*3456.7821);
	n.y=fract(sin(v.x+v.y*804.2048)*(v.x-v.y)*5349.2627);
	return n;
}

//via http://glsl.herokuapp.com/e#4841.11
float partition_noise(vec2 p) 
{
	vec2 id;
	
	id = floor(floor(p)-.5);
	
	p *= floor(hash(id) * 2.)+1.;
	id = floor(p);
	
	p.yx *= floor(hash(id) * 3.)-4.;
	id -= floor(p);

	p *= floor(hash(id) * 2.)+1.;
	id = floor(p);

	p -= id;

	vec2 u = abs(p - .5) * 10.;

	return max(u.x, u.y);
}

mat2 rmat(float t)
{
	float c = cos(t);
	float s = sin(t);
	return mat2(c,s,-s, c);
	
}

float map(vec2 position)
{
	position.y		-= .52;
	position 		*= .65;
	
	float perlin		= fbm(position);
	
	
	
	vec2 bp0			= position;
	bp0.x			= mod(abs(bp0.x-1.)-.65, 1.5)-.3;
	bp0.xy			*= rmat(floor(bp0.x*9.)-.3);

	bp0			+= vec2(-.05, .1);
	float b0			= cube(bp0, vec2(.00225, .3));
	
	
	vec2 bp1			= position;
	bp1.x			= mod(abs(bp1.x+.15)-.5, .5);
	bp1			+= vec2(-.05, .45+perlin*.1);
	bp1.xy			*= rmat(.55);
	bp1.x			= abs(bp1.x-.3);
	float b1			= cube(bp1, vec2(.0025, .2));
	
	float bridge		= max(step(b0, 0.), step(b1,0.));
	
	
	float s0			= partition_noise(perlin * 8. - position*12.);
	float s1			= partition_noise(floor(perlin * .5) - position*24.);
	
	float structures		= (s0 - s1);
	
	
	position			+= fbm(position);
	position 		*= .25;
	vec3 uvw			= barycentric(position);
	float terrain		= origami(uvw);
	
	float zones 		= floor(max(terrain-.85, 0.)*64.);
	structures		*= float(zones > 1. && zones < 45.);
	terrain			= max(1.15-inversesqrt(abs(terrain)), 0.);
	terrain 			+= bridge*.125*float(terrain<.125);

	structures		*= float(zones > 23. && zones < 62.);
	
	terrain 			+= structures*.08;
	return terrain;
}

void main() 
{
	vec2 uv			= gl_FragCoord.xy/RENDERSIZE;
	vec2 aspect		= RENDERSIZE/min(RENDERSIZE.x, RENDERSIZE.y);
	vec2 position		= (uv - .5) * aspect;
	vec2 mouse		= (mouse - .5) * aspect;
	
	
	vec4 result		= vec4(0., 0., 0., 1.);
	result			+= map(position*max(1.-mouse.y*8., 1.));
	
	gl_FragColor = result;
}//sphinx


vec3 barycentric(vec2 uv)
{	
	uv.y		/= 1.73205080757;
	vec3 uvw		= vec3(uv.y - uv.x, uv.y + uv.x, -(uv.y + uv.y));
	uvw		*= .86602540358;
	return (uvw);
}


float bound(float angle)
{
	return max(angle, .00392156);
}


float fold(in float x)
{
	return bound(abs(fract(TIME*0.002+x)-.5));
}


vec3 fold(in vec3 p)
{
	return vec3(fold(p.x), fold(p.y), fold(p.z));
}


float origami(in vec3 position)
{
	float amplitude = 0.4;	
    	float frequency	= 4.;
	float result	= 1.;
	for(int i = 0; i < 4; i++)
	{
        	position 	+= fold(position + fold(position).yzx).zxy;
        	result		+= length(cross(position, position.zyx)) * amplitude;
		position 	*= frequency;

		amplitude 	*= 0.5;
	}
	
	return result;
}


float noise(in vec2 uv)
{
    	const float k 	= 257.;
    	vec4 l  		= vec4(floor(uv),fract(uv));
    	float u 		= l.x + l.y * k;
    	vec4 v  		= vec4(u, u+1.,u+k, u+k+1.);
    	v       		= fract(fract(1.23456789*v)*v/.987654321);
    	l.zw    		= l.zw*l.zw*(3.-2.*l.zw);
    	l.x     		= mix(v.x, v.y, l.z);
    	l.y     		= mix(v.z, v.w, l.z);
    	return mix(l.x, l.y, l.w);
}
 

float fbm(vec2 uv)
{
	float a = .5;
	float f = 2.;
	float p = 1.;
	float n = 0.;
	for(int i = 0; i < 8; i++)
	{
		n += noise(uv*f+p)*a;
        	a *= .5;
        	f *= 2.;
   	}
    	return n;
}


float sphere(vec2 position, float radius)
{
	return length(position)-radius;
}


float cube(vec2 position, vec2 scale)
{
	vec2 vertex 	= abs(position) - scale;
	vec2 edge 	= max(vertex, 0.);
	float interior	= max(vertex.x, vertex.y);
	return min(interior, 0.) + length(edge);
}


float segment(vec2 position, vec2 a, vec2 b)
{
	return distance(position, project(position, a, b));
}


vec2 project(vec2 position, vec2 a, vec2 b)
{
	vec2 q	 	= b - a;	
	float u 	= dot(position - a, q)/dot(q, q);
	u 		= clamp(u, 0., 1.);
	return mix(a, b, u);
}