/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36165.3"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float rand(vec3 co)
{
	return fract(sin(dot(co.xyz, vec3(12.9898, 78.233, 56.787))) * 43758.5453);
}

float noise(vec3 pos)
{
	vec3 ip = floor(pos);
	vec3 fp = smoothstep(0.0, 1.0, fract(pos));
	vec4 a = vec4(
		rand(ip + vec3(0, 0, 0)),
		rand(ip + vec3(1, 0, 0)),
		rand(ip + vec3(0, 1, 0)),
		rand(ip + vec3(1, 1, 0)));
	vec4 b = vec4(
		rand(ip + vec3(0, 0, 1)),
		rand(ip + vec3(1, 0, 1)),
		rand(ip + vec3(0, 1, 1)),
		rand(ip + vec3(1, 1, 1)));
 
	a = mix(a, b, fp.z);
	a.xy = mix(a.xy, a.zw, fp.y);
	return mix(a.x, a.y, fp.x);
}


float perlin(vec3 pos)
{
	return (noise(pos) * 7.0 +
		noise(pos * 2.0 ) * 4.0 +
		noise(pos * 4.0) * 2.0 +
		noise(pos * 8.0) ) / 16.0;
}

float box(vec2 pos, vec2 rect)
{
	return length(max(vec2(0.0), abs(pos) - rect)) - 0.001;	
}

float hChar(vec2 pos)
{
	return min(min(
		box(pos - vec2(-0.06, 0.0), vec2(0.02, 0.1)),
		box(pos - vec2(0.06, 0.0), vec2(0.02, 0.1))),
		box(pos - vec2(0.0, -0.00), vec2(0.08, 0.02)));
}

float sChar(vec2 pos)
{
	float weight = 0.02;
	return min(min(min(min(
		box(pos - vec2(-0.06, 0.03), vec2(weight, 0.045)),
		box(pos - vec2(0.0, -0.08), vec2(0.08, weight))),
		box(pos - vec2(0.0, -0.000), vec2(0.08, weight))),
		box(pos - vec2(0.0, 0.08), vec2(0.08, weight))),
		box(pos - vec2(0.06, -0.05), vec2(weight, 0.05)));
}

float tChar(vec2 pos)
{
	return min(box(pos - vec2(0.0, 0.0), vec2(0.02, 0.1)),
		   box(pos - vec2(0.0, 0.08), vec2(0.08, 0.02)));
}

float iChar(vec2 pos)
{
	float weight = 0.02;
	return min(min(
		box(pos - vec2(0.0, 0.0), vec2(0.02, 0.1)),
		   box(pos - vec2(0.0, -0.08), vec2(0.05, 0.02))),
		   box(pos - vec2(0.0, 0.08), vec2(0.05, 0.02)));
}

float dist(vec2 pos) {
	return min(min(min(
		sChar(pos - vec2(-0.3, 0.0)),
		hChar(pos - vec2(-0.1, 0.0))),
		iChar(pos - vec2(0.075, 0.0))),
		tChar(pos - vec2(0.25, 0.0)));
		
}

mat2 rot(float a)
{
	float s = sin(a);
	float c = cos(a);
	return mat2(c, s, 
		    -s, c);
}

void main( void ) {

	vec2 pos = (gl_FragCoord.xy - RENDERSIZE.xy / 2.0) / RENDERSIZE.y;
	pos.y += sin(TIME * 10.0 + pos.x * 10.0) * 0.01;
	pos *=  10.0;
	pos.x /= 1.0 + length(pos) * 0.3;
	//pos = mod(pos + vec2(0.45, 0.15), vec2(0.9, 0.3)) - vec2(0.45, 0.15) ;
	
	vec3 color = vec3(0.0, 0.0, 0.0);
	color += vec3(1.0, 1.0, 1.0) * (0.005 / abs(dist(pos)));
	color += clamp((perlin(vec3(pos.x * 120.0, 0.0, TIME * 0.3)) - 0.5) * 10.0, 0.0, 0.8) * vec3(1.0, 1.0, 1.0);

	gl_FragColor = vec4(color, 1.0);

}