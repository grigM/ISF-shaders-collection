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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#13430.3"
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
	return step(0.0, (max(d.x,d.y)-0.2));
}

vec2 offset_line(float x) {
	return normalize(vec2(1.0)) * x;
}

vec2 offset_spiral(float x) {
	float a = x * 2.0 * M_PI * 0.3819444 * 521.0;
	return vec2(cos(a),sin(a))*pow(x,1.0/1.618);
}

#define offset offset_line
//#define offset offset_spiral

float linsmoothtri(float x) {
    return smoothstep(0.0,1.0,abs(mod(x,2.0) - 1.0));
}

mat4 dither_mtx = mat4(
     vec4( 1.0, 33.0,  9.0, 41.0),
     vec4(49.0, 17.0, 57.0, 25.0),
     vec4(13.0, 45.0,  5.0, 37.0),
     vec4(61.0, 29.0, 53.0, 21.0)
);

float dither(vec2 uv)
{
    vec2 puv = mod(uv, 4.0);
    int x = int(puv.x);
    int y = int(puv.y);
    
    vec4 row;
    if (x == 0) row = dither_mtx[0];
    else if (x == 1) row = dither_mtx[1];
    else if (x == 2) row = dither_mtx[2];
    else row = dither_mtx[3];
	    
    float col;
    if (y == 0) col = row[0];
    else if (y == 1) col = row[1];
    else if (y == 2) col = row[2];
    else col = row[3];

    // add noise to lsb
    float n = rand(vec3(uv,mod(TIME,1.0)));
	    
    return (n+col)/(64.0);
}

void main(void) {
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy);
	vec2 c = uv;
	float aspect = RENDERSIZE.x / RENDERSIZE.y;
	uv -= 0.5;
	uv.x *= aspect;
	uv += 0.5;
	
	float color0 = 0.0;
	float f = 1.0 / float(NUM_SAMPLES);
	float o = 0.0;
	float d = 0.0;//mod(TIME*60.0,4.0);
	if ((c.x-0.05) > mouse.x)
		o = f * dither(gl_FragCoord.xy + vec2(0,d));
	else if ((c.x+0.05) > mouse.x)
		o = f * rand(vec3(uv, mod(TIME,1.0)));
	float s = mix(0.0, INTENSITY, linsmoothtri(uv.y+TIME)); 
	for (int i = 0; i < NUM_SAMPLES; ++i) {
		float j = float(i) * f;
		color0 += fetch(uv + offset(o+j)*s);
	}
	gl_FragColor = vec4(vec3(pow(color0 * f,2.2)), 1.0);
}