/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
  	{
		"NAME": "rotate",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": -1,
		"MAX": 1
	},
	
	{
		"NAME": "zoom",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 2
	},
	
  	{
		"NAME": "pos1X",
		"TYPE": "float",
		"DEFAULT": -0.5,
		"MIN": -2,
		"MAX": 2.0
	},
	
	{
		"NAME": "pos2X",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": -2,
		"MAX": 2.0
	},
	
	{
		"NAME": "pos3X",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": -2,
		"MAX": 2.0
	},
	
	{
		"NAME": "pos1Y",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": -2,
		"MAX": 2.0
	},
	{
		"NAME": "pos2Y",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": -2,
		"MAX": 2.0
	},
	{
		"NAME": "pos3Y",
		"TYPE": "float",
		"DEFAULT": -0.5,
		"MIN": -2,
		"MAX": 2.0
	},
	
	
	{
		"NAME": "size1",
		"TYPE": "float",
		"DEFAULT": 0.1,
		"MIN": -0.05,
		"MAX": 2.0
	},
	{
		"NAME": "size2",
		"TYPE": "float",
		"DEFAULT": 0.05,
		"MIN": -0.05,
		"MAX": 2.0
	},
	{
		"NAME": "size3",
		"TYPE": "float",
		"DEFAULT": 0.1,
		"MIN": -0.05,
		"MAX": 2.0
	},
	{
		"NAME": "size4",
		"TYPE": "float",
		"DEFAULT": 0.02,
		"MIN": -0.05,
		"MAX": 2.0
	},
	{
		"NAME": "size5",
		"TYPE": "float",
		"DEFAULT": 0.1,
		"MIN": -0.05,
		"MAX": 2.0
	},
	{
		"NAME": "size6",
		"TYPE": "float",
		"DEFAULT": 0.1,
		"MIN": -0.05,
		"MAX": 2.0
	},
	
	{
		"NAME": "size7",
		"TYPE": "float",
		"DEFAULT": 0.1,
		"MIN": -0.05,
		"MAX": 2.0
	},
	{
		"NAME": "size8",
		"TYPE": "float",
		"DEFAULT": 0.1,
		"MIN": -0.05,
		"MAX": 2.0
	},
	
	{
		"NAME": "size9",
		"TYPE": "float",
		"DEFAULT": 0.04,
		"MIN": -0.1,
		"MAX": 2.0
	},
	{
      "NAME" : "color_1",
      "TYPE" : "color",
      "DEFAULT" : [
        1.0,
        1.0,
        1.0,
        1
      ],
      "LABEL" : ""
    },
    {
      "NAME" : "color_2",
      "TYPE" : "color",
      "DEFAULT" : [
        0.0,
        0.0,
        0.0,
        0.0
      ],
      "LABEL" : ""
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39866.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif



#define PI 3.14159265359




float smoothedge(float v, float f) {
    return smoothstep(0.0, f / RENDERSIZE.x, v);
}

float circle(vec2 p, float radius) {
  return length(p) - radius;
}

float rect(vec2 p, vec2 size) {
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.0) + length(max(d,0.0));
}

float roundRect(vec2 p, vec2 size, float radius) {
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.0) + length(max(d,0.0))- radius;
}

float ring(vec2 p, float radius, float width) {
  return abs(length(p) - radius * 0.500) - width;
}

float hexagon(vec2 p, float radius) {
    vec2 q = abs(p);
    return max(abs(q.y), q.x * 0.866025 + q.y * 0.5) - radius;
}

float triangle(vec2 p, float size) {
    vec2 q = abs(p);
    return max(q.x * 0.866025 + p.y * 0.5, -p.y * 0.5) - size * 0.5;
}

float ellipse(vec2 p, vec2 r, float s) {
    return (length(p / r) - s);
}

float capsule(vec2 p, vec2 a, vec2 b, float r) {
    vec2 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}

//http://thndl.com/square-shaped-shaders.html
float polygon(vec2 p, int vertices, float size) {
    float a = atan(p.x, p.y) + 0.2;
    float b = 6.28319 / float(vertices);
    return cos(floor(0.5 + a / b) * b - a) * length(p) - size;
}

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

void main() {
	vec2 st = 2.0*vec2(gl_FragCoord.xy-0.5*RENDERSIZE.xy)/RENDERSIZE.y;
	
    //rotate canvas
 	st = rotate2d(rotate*PI ) * st;
 	st *= zoom;
 	
    float  d = ellipse(st - vec2(pos1X, pos1Y), vec2(0.9, 1.2), size1);
    d = min(d, capsule(st - vec2(pos2X, pos1Y), vec2(-0.05, -0.05), vec2(0.05, 0.05), size2));
    d = min(d, polygon(st - vec2(pos3X, pos1Y), 5, size3));

    d = min(d, ring(st - vec2(pos1X, pos2Y), 0.18, size4));
    d = min(d, hexagon(st - vec2(pos2X, pos2Y), size5));
    d = min(d, triangle(st - vec2(pos3X, pos2Y), size6));
    
    d = min(d, circle(st - vec2(pos1X, pos3Y), size7));
    d = min(d, rect(st - vec2(pos2X, pos3Y), vec2(size8, size8)));
    d = min(d, roundRect(st - vec2(pos3X, pos3Y), vec2(0.08, 0.06), size9));
    
    vec3 color = mix(vec3(color_1), vec3(color_2), smoothedge(d, 10.0));
    gl_FragColor = vec4(color, 1.0);
}