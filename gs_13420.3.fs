/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#13420.3"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#define M_PI 3.141592653589793


#define NUM_SAMPLES 16
#define INTENSITY 0.3

float rand(vec3 co){
    return fract(sin(dot(co.xyz,vec3(12.9898,78.233,91.1743))) * 43758.5453);
}

float fetch(vec2 uv) {
	vec2 d = abs(uv - vec2(0.5));
	return step(0.0, (max(d.x,d.y)-0.1));
}

vec2 offset_line(float x) {
	return normalize(vec2(1.0)) * abs(x) - 0.3;
}

vec2 offset_spiral(float x) {
	float a = x * 2.0 * M_PI * 0.3819444 * 521.0;
	return vec2(cos(a),sin(a))*pow(x,1.0/1.618);
}

vec2 offset_box(float x) {
	float v = fract(x*1031.0);	
	return vec2(x,v)-0.5;
}


//#define offset offset_line
//#define offset offset_spiral
#define offset offset_box

float linsmoothtri(float x) {
    return smoothstep(0.0,1.0,abs(mod(x,2.0) - 1.0));
}

void main(void) {
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy);
	vec2 c = uv;
	float aspect = RENDERSIZE.x / RENDERSIZE.y;
	uv -= 0.5;
	uv.x *= aspect;
	uv += 0.5;
	
	float color0 = 0.0;
	float color1 = 0.0;
	float f = 1.0 / float(NUM_SAMPLES);
	float o = 0.0;
	o = f * (rand(vec3(uv,mod(TIME,5.5))));
	float s = mix(0.0, INTENSITY, linsmoothtri(uv.y+TIME)); 
	for (int i = 0; i < NUM_SAMPLES; ++i) {
		float j = float(i) * f;
		color0 += fetch(uv + offset(j)*s);
		color1 += fetch(uv + offset(j+o)*s);		
	}
	float color;
	if (c.x > mouse.x) {
		color = color1;
	} else {
		color = color0;
	}
	gl_FragColor = vec4(vec3(color * f), 1.0);
}