/*{
	"CREDIT": "by mojovideotech",
	"DESCRIPTION": "mod of interactiveshaderformat/sketches/2358",
	"CATEGORIES": [
		"particles",
		"noise"
	],
	"INPUTS": [
	]
}*/

////////////////////////////////////////////////////////////
// ParticularVortex  by mojovideotech
// mod of : interactiveshaderformat.com/\sketches/\2358
//
// based on :
// glslsandbox/\e#44948.0
//
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0
////////////////////////////////////////////////////////////

#define 	pi	3.141592653589793 	// pi
#define 	T   (TIME + 100000.0) * 0.1

float cash(float c) { return mix(mod(c, cos(c)), sin(pi+pi)*c, cos(c*pi)); }

float hash(float r) { return fract(cos(sin(r * 1113.27) * 43758.5453123)); }

float bash(float n) { return fract((sin(n)/log2(T))-(atan(n,-T))/55.753); }

float noise( vec2 w ) {
	vec3 x = vec3(w.x,length(w),w.y);
    vec3 p = floor(x), f = fract(x);
    f = f*f*(3.0-2.0*f);
    float n = p.x + p.y*157.0 + 113.0*p.z;
    return mix(mix(mix( bash(n+0.0), bash(n+1.0),f.x),
                   mix( bash(n+157.0), bash(n+158.0),f.x),f.y),
               mix(mix( bash(n+113.0), bash(n+114.0),f.x),
                   mix( bash(n+270.0), bash(n+271.0),f.x),f.y),f.z);
}

mat2 rotate2d(float angle){ return mat2(cos(angle), -sin(angle),  sin(angle), cos(angle)); }

void main() {
	vec2 uv = (gl_FragCoord.xy * 2.0 -  RENDERSIZE.xy) / RENDERSIZE.x;
	vec2 p = uv * rotate2d(T * 0.213); 
	float direction = 120.0/(T/p.x,T/p.y,sqrt(T));
	float speed = T * direction ;
	float distanceFromCenter = cash(direction*dot(uv,p) - length(p));
	p.yx *= rotate2d(-T * 0.239);
	float meteorAngle = atan(p.y, p.x) * (359.0 + cos(atan(speed,pi))+pi);
	float flooredAngle = floor(meteorAngle);
	float randomAngle = pow(hash(flooredAngle),mix(cos(direction*pi),-sin(speed*pi),distanceFromCenter));
	float t = speed + randomAngle;
	float lightsCountOffset = bash(direction);
	float adist = randomAngle / distanceFromCenter * lightsCountOffset;
	float dist = t + adist;
	float meteorDirection = (direction < 0.5) ? -1.0 : 0.0;
	dist = abs(fract(dist) + meteorDirection);
	float lightLength = 33.0/noise(uv.xy);
	float meteor = (noise(p.xy)  / dist)  * cos(sin(speed)) / lightLength;
	meteor -= dot(vec2(distanceFromCenter,meteorDirection),vec2(randomAngle,meteorAngle));
	vec3 color = vec3(0.0);
	color += meteor;

	gl_FragColor = vec4(color, 1.0);
}