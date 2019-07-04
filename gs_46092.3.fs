/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#46092.3"
}
*/


// http://glslsandbox.com/e#46092.3
//
// Referred to https://thebookofshaders.com/edit.php?log=160414040957

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float random(in vec2 uv){
	return fract(sin(dot(uv, vec2(12.9898,78.233))) * 43758.5453);
}

float smoothedge(float v) {
	return smoothstep(0.0, 1.0 / RENDERSIZE.x, v);
}

float circle(vec2 p, float radius) {
	return length(p) - radius;
}

float rect(vec2 p, vec2 size) {  
	vec2 d = abs(p) - size;
	return min(max(d.x, d.y), 0.0) + length(max(d,0.0));
}

float hexagon(vec2 p, float radius) {
	vec2 q = abs(p);
	return max(abs(q.y), q.x * 0.866025 + q.y * 0.5) - radius;
}

float triangle(vec2 p, float size) {
	vec2 q = abs(p);
	return max(q.x * 0.866025 + p.y * 0.5, -p.y * 0.5) - size * 0.5;
}

float polygon(vec2 p, int vertices, float size) {
	float a = atan(p.x, p.y) + 0.2;
	float b = 6.28319 / float(vertices);
	return cos(floor(0.5 + a / b) * b - a) * length(p) - size;
}

float getShape(vec2 st, int i) {
	if (i == 0) {
		return circle(st, 0.4);
	} else if (i == 1) {
		return rect(st, vec2(0.4, 0.32));
	} else if (i == 2) {
		return triangle(st, 0.4);
	} else if (i == 3) {
		return polygon(st, 5, 0.4);
	} else {
		return hexagon(st, 0.5);
	}
}


void main() {
	vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE.xy) / min(RENDERSIZE.x, RENDERSIZE.y);
	
	vec2 scaledUv = uv * 2.0;
	vec2 repeatedUv = fract(scaledUv);
	repeatedUv -= 0.5;
	
	float speed = 4.0 + floor(scaledUv.y);
	float shapeKindNum = 5.0;
	
	float t0 = mod(TIME * speed, shapeKindNum);
	float t1 = mod(TIME * speed + 1.0, shapeKindNum);
	
	int i0 = int(t0);
	int i1 = int(t1);
	
	float f = fract(t0);
	
	float beforeShape = getShape(repeatedUv, i0);
	float afterShape = getShape(repeatedUv, i1);
	
	float shape = smoothedge(mix(beforeShape, afterShape, f));
	vec3 color = vec3(shape, 0.0, random(floor(scaledUv) * floor(TIME * 10.0)));
	
	gl_FragColor = vec4(color, 1.0);
}