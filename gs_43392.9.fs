/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43392.9"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable
#define PI 3.1415926535

#define SHOW_POINTS
#define SHOW_EDGES
#define SHOW_SQUARE
#define PERIOD 1.8


struct Square {
	vec2 center;
	vec2 vertex;
};

struct Circle {
	vec2 center;
	float radius;
};

struct Edge {
	vec2 p1;
	vec2 p2;
};

float manhattanDistance(vec2 pos) {
	return abs(pos.x) + abs(pos.y);
}

bool withinCircle (Circle c, vec2 point) {
	return length(point - c.center) < c.radius;
}

vec2 rotate(vec2 v, float angle) {
	return v * mat2(cos(angle), -sin(angle), sin(angle), cos(angle));
}

bool withinSquare (Square s, vec2 point) {
	vec2 a = s.vertex - s.center;
	vec2 b = point - s.center;

	float rotateAngle = atan(a.x, a.y);
	vec2 ra = rotate(a, rotateAngle);
	vec2 rb = rotate(b, rotateAngle);

	return manhattanDistance(rb) < length(a);
}

float distance(Edge e, vec2 pos) {
	vec2 a = e.p1;
	vec2 b = e.p2;
	vec2 c = pos - a;
	vec2 n = normalize(b - a);

	float mapCoeff = dot(n, c);

	if (mapCoeff < 0.0) {
		return distance(a, pos);
	}

	if (mapCoeff > distance(a, b)) {
		return distance(b, pos);
	}

	return length(c - n * mapCoeff);
}

vec4 subColorSquare(Square s, vec2 pos, vec4 sqColor) {
	vec2 a = s.vertex - s.center;

	#ifdef SHOW_POINTS
	for (int i = 0; i < 4; i++) {
		vec2 vertex = s.center + rotate(a, float(i) * PI / 2.0);
		Circle c = Circle(vertex, 0.017);
		if (withinCircle(c, pos)) {
			return vec4(0.4, 0.4, 0.4, 0.0);
		}
	}
	#endif

	#ifdef SHOW_EDGES
	for (int i = 0; i < 4; i++) {
		vec2 vertex1 = s.center + rotate(a, float(i) * PI / 2.0);
		vec2 vertex2 = s.center + rotate(a, float(i + 1) * PI / 2.0);
		Edge e = Edge(vertex1, vertex2);
		if (distance(e, pos) < 0.008) {
			return sqColor * 1.5;
		}
	}
	#endif

	#ifdef SHOW_SQUARE
	if (withinSquare(s, pos)) {
		return sqColor;
	}
	#endif

	return vec4(0.0);
}

float abspow(float b, float e) {
	if (b < 0.0) return -pow(-b, e);
	return pow(b, e);
}

float middleSquareY(float TIME) {
	float pulseRatio = 0.3;

	float phase = mod(TIME, PERIOD);
	float pulseWidth = pulseRatio * PERIOD;

	if (phase > pulseWidth) {
		return 1.0;
	}
	
	return abspow(cos(phase * PI / pulseWidth), 0.5);
}

float outerSquareX(float TIME) {
	float moveRatio = 0.3;
	
	float phase = mod(TIME, PERIOD);
	float moveTime = moveRatio * PERIOD;

	if (phase > moveTime) {
		return -1.0;
	}

	return abspow(cos(phase * PI / moveTime), 0.8);
}

void main( void ) {
	vec2 pos = vv_FragNormCoord * 3.0;
	vec4 color = vec4(1.0);

	color -= subColorSquare(Square(vec2(0.0), vec2(outerSquareX(TIME), 1.0)), pos, vec4(.0, .0, .3, 0.));
	color -= subColorSquare(Square(vec2(0.0), vec2(outerSquareX(-TIME + 0.25), 1.0)), pos, vec4(.0, .2, .2, 0.));
	color -= subColorSquare(Square(vec2(0.0), vec2(0.0, middleSquareY(TIME + 0.15))), pos, vec4(.0, .2, .3, 0.));

	gl_FragColor = color;
}