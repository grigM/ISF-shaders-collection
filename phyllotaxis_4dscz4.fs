/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "chromaticaberration",
    "blackandwhite",
    "squares",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4dscz4 by mahalis.  just a lil square dance",
  "INPUTS" : [

  ]
}
*/


vec2 r(vec2 p, float a) {
    float c = cos(a);
    float s = sin(a);
    return vec2(c * p.x - s * p.y, s * p.x + c * p.y);
}

float sdSquare(vec2 p, float s) {
	vec2 d = abs(p) - s;
	return min(max(d.x, d.y), 0.0) + length(max(d, 0.0));
}

#define TWO_PI 6.2832

vec2 opRepeatRadial(vec2 p, int sectorCount) {
    float fSectors = float(sectorCount);
    float segmentAngle = (floor((atan(p.y, p.x) / TWO_PI - 0.5) * fSectors) + 0.5) * TWO_PI / fSectors;
    return -r(p, -segmentAngle);
}

float evaluate(vec2 uv, float time, float timeOffset) {
	float timeOffsetMultiplier = (1. - .8*length(uv)/0.5) * 0.913;
	uv *= (1. + .02 * sin(1.13 * time + (timeOffset * timeOffsetMultiplier)));
	const float ringWidth = 0.07;
	float ringIndex = floor(length(uv) / ringWidth - 0.5) + 0.5;
	float centerX = (ringIndex + 0.5) * ringWidth;
	float centerness = 1. - ringWidth * ringIndex;
	float ringRotation = time * 0.6 * pow(centerness, 4.);
	vec2 repeatedUV = opRepeatRadial(r(uv, ringRotation), (int(ringIndex) + 1) * 4);
	float squareSize = 0.013 + 0.01 * sin(length(uv) * 11.1 + time * 0.6);
	float d = sdSquare(r(repeatedUV - vec2(centerX, 0.), sin(time * centerness * 2.3 + uv.y * 3.1 - uv.x * 2.3)), squareSize);
	float value = smoothstep(0., 0.001, d);
	value = max(max(value, float(ringIndex > 6.)), float(ringIndex < 1.));
	return 1. - value;
}

vec4 aberrate(vec2 uv, float time) {
	float aberrationAmount = 0.2 * pow(max(0., 1. - length(uv) * 2.), 1.3) + 0.1 * sin(time * 0.73 + length(uv) * 1.1);
	return (vec4(1.0) - evaluate(uv, time, 0.) * vec4(1,0,0,0) - evaluate(uv, time + aberrationAmount, 1.) * vec4(0,1,0,0) - evaluate(uv, time + 2. * aberrationAmount, 2.) * vec4(0,0,1,0));
}

void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	float aspect = RENDERSIZE.y / RENDERSIZE.x;
	
	uv -= 0.5;
	uv.y *= aspect;
	//uv *= 1.1;
	gl_FragColor = aberrate(uv, TIME) * aberrate(uv*(1. + .12 * sin(TIME * 0.331)), 4.11 - TIME * 0.96);
}
