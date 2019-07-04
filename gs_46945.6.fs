/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#46945.6"
}
*/


// http://glslsandbox.com/e#46945.6

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float map(float value, float beforeMin, float beforeMax, float afterMin, float afterMax) {
	return afterMin + (afterMax - afterMin) * ((value - beforeMin) / (beforeMax - beforeMin));
}

float random(in vec2 uv){
	return fract(sin(dot(uv, vec2(12.9898,78.233))) * 43758.5453);
}

mat2 rotate2d(float angle){
	return mat2(cos(angle), -sin(angle),  sin(angle), cos(angle));
}

float circle(vec2 uv, float size) {
	return step(size, length(uv));
}

float rect(vec2 uv, float width, float height) {
	vec2 rect = vec2(step(-width, uv.x), step(-height, uv.y)) * vec2((1.0 - step(width, uv.x)), 1.0 - step(height, uv.y));
	return min(rect.x, rect.y);
}

float obliqueLine(vec2 uv){
	return step(0.6, fract((uv.x + uv.y + TIME * 0.8) * 4.0));
}

float pluralRing(vec2 uv, float interval) {
	return sin(length(uv) * interval);
}

void main( void ) {
	vec2 uv = (gl_FragCoord.xy * 2.0 -  RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
	
	float num = 30.0;
	vec2 scaledUv = uv *num;
	vec2 repeatedUv = fract(scaledUv);
	repeatedUv -= 0.5;

	float randomOffset = abs(sin(TIME * 5.0 * random(floor(scaledUv))));
	float ballRadius = 0.4 * randomOffset;
	
	float point = circle(repeatedUv, ballRadius);
	vec2 circleCenterInUv = (floor(scaledUv) + 0.5) / num;  // normalized uv.
	
	vec3 color = vec3(1.0);
	float offsetX = 0.7;
	
	// Word "I"
	float word_I = rect(vec2(circleCenterInUv.x + 0.4 + offsetX, circleCenterInUv.y), 0.1, 0.5);
	if (word_I > 0.0) {
		color.r -= rect(vec2(uv.x + 0.4 + offsetX, uv.y), 0.08, 0.48);
		color.gb -= obliqueLine(uv * 2.0) * 0.8;
		color.rg -= (1.0 - point);
	}
	
	// Word "/"
	/*
	float word_slash = rect(vec2(circleCenterInUv.x + 0.4 + offsetX, circleCenterInUv.y) * rotate2d(-1.2), 0.6, 0.07);
	if (word_slash > 0.0) {
		color.rg -= (1.0 - point);
	}
	*/
	
	// Word "O"
	float word_O = 1.0 - circle(vec2(circleCenterInUv.x - 0.33 + offsetX, circleCenterInUv.y), 0.5);
	if (word_O > 0.0) {
		float innerCircle1 = 1.0 - circle(vec2(uv.x - 0.33 + offsetX, uv.y), map(cos(TIME * 8.0), -1.0, 1.0, 0.2, 0.3));
		color.b -= innerCircle1 * 0.5;
		float innerCircle2 = 1.0 - circle(vec2(uv.x - 0.33 + offsetX, uv.y), map(sin(TIME * 8.0), -1.0, 1.0, 0.4, 0.45));
		color.g -= innerCircle2;
		color.b -= obliqueLine(uv * 3.0) * 0.8;
		color.rg *= point;
	}
	
	// Word "1"
	float word_1 = rect(vec2(circleCenterInUv.x - 1.1 + offsetX, circleCenterInUv.y), 0.1, 0.5);
	if (word_1 > 0.0) {
		color.rb -= rect(vec2(uv.x -1.1 + offsetX, uv.y), 0.08, map(sin(TIME * 5.0), -1.0, 1.0, 0.42, 0.48));
		color.g -= (1.0 - point);
	}
	
	// Word "8"
	float word_8_top = 1.0 - circle(vec2(circleCenterInUv.x - 1.6 + offsetX, circleCenterInUv.y - 0.25), 0.3);
	if (word_8_top > 0.0) {
		float innerCircle = 1.0 - circle(vec2(uv.x - 1.6 + offsetX, uv.y - 0.25), 0.15);
		color.g -= innerCircle * 0.8;
		color.rb -= obliqueLine(uv * 6.0 * rotate2d(-1.2)) * 0.6;
		color.rg *= point;
	}
	float word_8_under = 1.0 - circle(vec2(circleCenterInUv.x - 1.6 + offsetX, circleCenterInUv.y + 0.25), 0.3);
	if (word_8_under > 0.0) {
		vec2 v = vec2(uv.x - 1.6 + offsetX, uv.y + 0.25);
		float innerCircle = 1.0 - circle(v, 0.24);
		color.r -= step(0.5, pluralRing(v * 8.0, abs(cos(TIME * 3.0))));
		color.gb -= innerCircle * 0.8;
		color.rg *= point;
	}

	gl_FragColor = vec4(color, 1.0);
}