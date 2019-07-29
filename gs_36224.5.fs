/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36224.5"
}
*/


// polygonColorSample  yuki_b

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


struct Figure
{
	float radius;
	int parts;
	vec3 color;
	vec2 position;
	float rotate;
	float blur;
	bool light;
};

struct Polygon4
{
	vec3 color;
	vec2 p0;
	vec2 p1;
	vec2 p2;
	vec2 p3;
	float blur;
	bool light;
};

#define pi 3.141592
#define maxpart 1000
	
#define hp  pi / 2.0
#define rmatHp  mat2(cos(hp), -sin(hp), sin(hp), cos(hp))
	
vec3 getCircleColor(Figure circle, vec2 pos) {
	float dist = length(pos - circle.position);
	vec3 color = vec3(0.0, 0.0, 0.0);
	float r = smoothstep(circle.radius, circle.radius+circle.blur,dist); 
	if (circle.light) {
		if (dist < circle.radius)
			color = circle.color;
		else
			color = circle.color * circle.blur   / (dist + circle.blur - circle.radius);
		return color;
	} else {
		return circle.color * (1.0 - r);
	}
}

vec3 getRegularPolygonColor(Figure fig, vec2 pos){
	float a = 2.0 * pi * (1.0 / float(fig.parts));
	float size = fig.radius * cos(a /2.0);
	float cs = cos(a);
	mat2 rmat = mat2(cs, -sin(a), sin(a), cs);
	float r = 0.0;
	vec2 lp = vec2(sin(-a + pi + fig.rotate), cos(-a + pi + fig.rotate));
	vec2 addv = lp;
	for(int i=0; i<maxpart; i++){
		addv = rmat * addv;
		float t = 0.0;
		if (fig.light) {
			float dist = dot(pos - fig.position, normalize(addv));
			if (dist < fig.radius)
				t = 0.0;
			else t = (1.0 - fig.blur   / (dist + fig.blur - fig.radius));
		} else {
			t = smoothstep(size, size+fig.blur, dot(pos - fig.position, normalize(addv)));
		}
		r = max(r, t);
		if (i==fig.parts)
			break;
	}
	return (1.0 - r) * fig.color;	
}

vec3 getColor(Figure fig, vec2 pos) {
	if (fig.parts==0)
		return getCircleColor(fig, pos);
	else
		return getRegularPolygonColor(fig, pos);
}

float dir(vec2 a, vec2 b){
	return a.x * b.y - b.x * a.y; //cross product
}

float getR(float r,vec2 p0, vec2 p1,  float blur, bool light, vec2 o, vec2 pos) {
	vec2 d = p1 - p0;
	vec2 addv =  rmatHp * d;
	float l = abs(dir(p1-o, p0-o)) / length(d);
	float dist = dot(pos - o, normalize(addv));
	float t =0.;
	if (light) {
		if (dist < l)
			t = 0.0;
		else t = (1.0 - blur   / (dist + blur - l));
	} else
		t = smoothstep(l, l+blur, dist);
	return max(r, t);
}

vec3 getPolygonColor(Polygon4 pol, vec2 pos) {
	vec2 p = (pol.p0+pol.p1+pol.p2+pol.p3)/4.0;
	float r = 0.;
	r = getR(r, pol.p0, pol.p1, pol.blur, pol.light, p, pos);
	r = getR(r, pol.p1, pol.p2, pol.blur, pol.light, p, pos);
	r = getR(r, pol.p2, pol.p3, pol.blur, pol.light, p, pos);
	r = getR(r, pol.p3, pol.p0, pol.blur, pol.light, p, pos);
	return (1.0 - r) * pol.color;	
}


Figure f1 = Figure(0.08, 0, vec3(0.0, 1.0, 1.0), vec2(0.2, 0.7), 0., 0.01, true);
Figure f2 = Figure(0.08, 0, vec3(0.0, 1.0, 1.0), vec2(0.2, 0.35),0., 0.03, false);
Figure f4 = Figure(0.08, 5, vec3(1.0, 1.0, 0.0), vec2(0.5, 0.7),0., 0.01, true);
Figure f3 = Figure(0.08, 5, vec3(1.0, 1.0, 0.0), vec2(0.5, 0.35),0., 0.03, false);

Polygon4 pol1 = Polygon4(vec3(1.0, 0.0, 1.0), vec2(0.95, 0.25),  vec2(0.85, 0.4), vec2(0.7, 0.45), vec2(0.75, 0.3), 0.01, false);
Polygon4 pol2 = Polygon4(vec3(1.0, 0.0, 1.0), vec2(0.95, 0.25+0.35),  vec2(0.85, 0.4+0.35), vec2(0.7, 0.45+0.35), vec2(0.75, 0.3+0.35), 0.01, true);


void main( void ) {
	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );
	position.y = 0.5 + ((position.y - 0.5) * (RENDERSIZE.y/RENDERSIZE.x));

	vec3 col = getColor(f1, position);
	col += getColor(f2, position);
	col += getColor(f3, position);
	col += getColor(f4, position);
	col += getPolygonColor(pol1, position);
	col += getPolygonColor(pol2, position);

	gl_FragColor = vec4(col, 1.0 );

}