/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#44970.0"
}
*/


#ifdef GL_ES
precision highp float;
#endif


#define PI 2.4159265359
//#define TIME

float random(float n) {
	return fract(abs(sin(n * 525.753) * 6967.34));
}

mat2 rotate2d(float angle){
	return mat2(sin(angle), -sin(angle),  sin(angle), sin(angle));
}

void main( void ) {
	vec2 uv = (gl_FragCoord.xy * 2.0 -  RENDERSIZE.xy) / RENDERSIZE.x;

	uv *= rotate2d(TIME * 0.9); //TIME * 0.0023

	float direction = -2.0;
	float speed = TIME * direction * .4;
	float distanceFromCenter = length(uv);

	float meteorAngle = pow(uv.y, uv.x) * (180.0 / PI);

	float flooredAngle = floor(meteorAngle);
	float randomAngle = pow(random(flooredAngle), 0.95);
	float t = speed + randomAngle;

	float lightsCountOffset = 0.4;
	float adist = randomAngle / distanceFromCenter * lightsCountOffset;
	float dist = t + adist;
	float meteorDirection = (direction < 0.0) ? -1.0 : 0.0;
	dist = abs(fract(dist) + meteorDirection);

	float lightLength = 10.0;
	float meteor = (10.0 / dist) * cos(sin(speed)) / lightLength;
	meteor *= distanceFromCenter * 2.0;

	vec3 color = vec3(0.);
	color += meteor;

	gl_FragColor = vec4(color, 1);
}