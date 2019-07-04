/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#44706.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float rand(vec2 co){
	return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

vec2 offset(ivec2 tile) {
	float r = rand(vec2(tile * 10));
	if (r < 5.0 / 15.0) return vec2(0.0);
	if (r < 10.0 / 15.0) return vec2(-1.0);
	if (r < 14.0 / 15.0) return vec2(1.0);
	return vec2(-0.2);
}

const vec3 orange = vec3(0.85, 0.35, 0.3);
const vec3 blue = vec3(0.12, 0.2, 0.3);
const vec3 ecru = vec3(0.8, 0.75, 0.68);
const vec3 dark = vec3(0.02, 0.02, 0.06);
const vec3 dark_blue = vec3(0.08, 0.1, 0.15);
const vec3 light = vec3(0.85, 0.85, 0.84);

vec3 rand_color (vec2 tile) {
	float r = rand(tile);
	if (r < 1.0 / 6.0) return orange;
	if (r < 3.0 / 6.0) return ecru;
	if (r < 4.0 / 6.0) return dark_blue;
	if (r < 5.0 / 6.0) return dark;
	return light;
}

const float dimx = 16.0;
const float dimy = 9.0;

void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );
	ivec2 tile = ivec2(int(position.x * dimx), int(position.y * dimy));
	vec2 subpos = vec2(dimx * position.x - float(tile.x), dimy * position.y - float(tile.y));

	vec3 a = rand_color(2.34 * vec2(tile) + 5.2);
	vec3 b = rand_color(vec2(tile) * 2.78 + 7.8);
	
	vec3 k = mix(rand_color(16.5 * vec2(tile)), rand_color(9.5 * vec2(tile)), vec3(0.5 + 0.5 * sin(TIME)));
	vec3 u = mix(rand_color(2.4 * vec2(tile)), rand_color(5.8 * vec2(tile)), vec3(0.5 + 0.5 * cos(TIME)));
	
	float diff = length(subpos + sin(0.8 * TIME) * offset(tile) + 1.5 * cos(0.3 * TIME + 42.0 * vec2(tile)));
	vec3 value = diff > 1.0 ? k : u;
	
	gl_FragColor = vec4( vec3(value), 1.0 );

}