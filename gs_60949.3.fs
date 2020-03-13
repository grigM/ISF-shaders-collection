/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60949.3"
}
*/


//precision highp float;



const vec3 col1 = vec3(0.2, 0.6, 0.7);
const vec3 col2 = vec3(0.2, 0.2, 0.5);

float sdCircle(vec2 pos, float radius) {
	 return length(pos) - radius;
}
  vec2 translate(vec2 pos, vec2 offset) {
 return pos - offset;
 }

float random(float v){
	return fract(sin(v*423.42454));	
}

float opUnion(float a, float b){
	return min(a,b);
}
float opSmoothUnion(float d1, float d2, float k) {
 return -log2(exp2(-k * d1) + exp2(-k * d2)) / k;
} 



void main(void) {
vec2 pos = (2.0 * gl_FragCoord.xy - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
pos *= 8.0;
float d = 1e6;
for (float i = 1.0; i <= 50.0; i++) {
vec2 offset = 5.0 * sin(2.0 * vec2(random(i), random(i * 10.0)) * TIME);
float radius = mix(0.1, 0.3, random(i * 100.0));
float cd = sdCircle(translate(pos, offset), radius);
d = opSmoothUnion(d, cd, 4.0);
}
vec3 col = mix(col1, col2,  smoothstep(-0.05, 0.05, d));
gl_FragColor = vec4(col, 1.0);
}