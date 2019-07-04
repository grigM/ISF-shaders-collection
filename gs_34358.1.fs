/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
	{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 10.0
		},
		{
			"NAME": "ofset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 4.0
		},
		{
			"NAME": "line_thick",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 5.0
		},
		{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 0.4,
			"MIN": 0.05,
			"MAX": 0.45
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34358.1"
}
*/



vec2 pos;
vec3 outputColor = vec3(0);
vec4 currentColor = vec4(1);

void render(float pixel, vec4 col){
    if (pixel < 1.){ 
        outputColor = mix(outputColor, col.xyz, col.w * (1. - max(pixel, 0.)));
    }
}

void render(float pixel){
    render(pixel, currentColor);
}

void drawLine(vec2 p1, vec2 p2, float lineWidth, int lineCap){
    vec2 delta = p2 - p1;
	float len = length(delta);
	float dist = abs(delta.y * pos.x - delta.x * pos.y + p2.x * p1.y - p2.y * p1.x) / len;
	
	vec2 center = (p1 + p2) / 2.;
	vec2 perp2 = vec2(center.y - p1.y, p1.x - center.x) + center;
	
	float cDist = abs((perp2.y - center.y) * pos.x - (perp2.x - center.x) * pos.y + perp2.x * center.y - perp2.y * center.x) / len * 4.;
	
	if (cDist > len){
        if (lineCap == 1){
            dist = min(length (p1 - pos), length (p2 - pos)) - lineWidth;
        }else{
            dist = max(dist - lineWidth, cDist - len);
        }
	}else{
		dist -= lineWidth;
	}
	
	render(dist);
}

#define PI acos(-1.)

mat4 mat;

vec2 t (vec3 pos){
	vec4 t = mat * vec4(pos, 1.);
	vec2 clip = t.xy / t.w;
	return (clip / 2. + .5) * RENDERSIZE;
}

void drawLine(vec3 p1, vec3 p2){
	drawLine(t(p1), t(p2), line_thick, 1);
}

void perspective(float fov){
	float t = tan(fov / 2.);
	float aspect = RENDERSIZE.x / RENDERSIZE.y;

	float far = 1000.;
	float near = 0.5-zoom;

	float fn = far - near;

	mat = mat4(
		1. / (aspect * t), 0, 0, 0,
		0, 1. / t, 0, 0,
		0, 0, (-near - far) / fn, (2. * near * far) / fn,
		0, 0, -1, 0
	);
}

void translate (vec3 pos){
	mat *= mat4(
		1, 0, 0, 0,
		0, 1, 0, 0,
		0, 0, 1, 0, 
		pos, 1
	);
}

void scale (vec3 scale){
	mat *= mat4(
		scale.x, 0, 0, 0,
		0, scale.y, 0, 0,
		0, 0, scale.z, 0,
		0, 0, 0, 1
	);
}

void rotate (float angle, vec3 axis){
	axis = normalize(axis);

	float s = sin(angle);
	float c = cos(angle);
	float c1 = 1. - c;

	mat *= mat4(
		c + axis.x * axis.x * c1, axis.x * axis.y * c1 - axis.z * s, axis.x * axis.z * c1 + axis.y * s, 0,
		axis.y * axis.x * c1 + axis.z * s, c + axis.y * axis.y * c1, axis.y * axis.z * c1 - axis.x * s, 0,
		axis.z * axis.x * c1 - axis.y * s, axis.z * axis.y * c1 + axis.x * s, c + axis.z * axis.z * c1, 0,
		0, 0, 0, 1
	);
}

void draw (){
	perspective(PI / 2.);
	translate(vec3(0, 0, -10));
	rotate((TIME*speed)-ofset, vec3(0.5233, 0.156723, 0.36235));

	drawLine(vec3(-1, -1, -1), vec3(-1, 1, -1));
	drawLine(vec3(-1, -1, -1), vec3(1, -1, -1));
	drawLine(vec3(1, 1, -1), vec3(-1, 1, -1));
	drawLine(vec3(1, 1, -1), vec3(1, -1, -1));
	drawLine(vec3(-1, -1, 1), vec3(-1, 1, 1));
	drawLine(vec3(-1, -1, 1), vec3(1, -1, 1));
	drawLine(vec3(1, 1, 1), vec3(-1, 1, 1));
	drawLine(vec3(1, 1, 1), vec3(1, -1, 1));
	drawLine(vec3(1, 1, -1), vec3(1, 1, 1));
	drawLine(vec3(-1, -1, -1), vec3(-1, -1, 1));
	drawLine(vec3(1, -1, -1), vec3(1, -1, 1));
	drawLine(vec3(-1, 1, -1), vec3(-1, 1, 1));
}

void main (){
	pos = vec2(gl_FragCoord.x, gl_FragCoord.y);

	draw();

	gl_FragColor = vec4(outputColor, 1.);
}