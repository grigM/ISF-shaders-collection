/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#16757.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


#define OCTAVES 5

// Squish and strech the tunnel
#define STRETCH 10.0
#define SQUISH 1.0

// Everthing should be a tunnel :)
// From http://glsl.heroku.com/e#11554.0
// Everything should be retro! :p (slight edit by @eiyeron : quantized colors + dithering)
// Grom http://glsl.heroku.com/e#16749.1

float rand(vec2 n) { 
	return fract(sin(dot(n, vec2(13, 5))) * 43758.5453);
}

float noise(vec2 n) {
	const vec2 d = vec2(0.0, 1.0);
	vec2 b = floor(n), f = smoothstep(vec2(0.0), vec2(1.0), fract(n));
	return mix(mix(rand(b), rand(b + d.yx), f.x), mix(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}

float fbm(vec2 n) {
	float total = 0.0, amplitude = 1.0;
	for (int i = 0; i < OCTAVES; i++) {
		total += noise(n) * amplitude;
		n += n;
		amplitude *= 0.5;
	}
	return total;
}

vec3 tex(vec2 pos) {
	const vec3 c1 = vec3(.1,0,0);
	const vec3 c2 = vec3(.7,0,0);
	const vec3 c3 = vec3(.2,0,0);
	const vec3 c4 = vec3(1,.9,0);
	const vec3 c5 = vec3(.1);
	const vec3 c6 = vec3(.9);
	vec2 p = pos;
	float q = fbm(p - TIME * -0.1);
	vec2 r = vec2(fbm(p + q + TIME - p.x - p.y), fbm(p + q + TIME));
	vec3 c = mix(c1, c2, fbm(p + r)) + mix(c3, c4, r.x) - mix(c5, c6, r.y);
	return c;
}


void main(void) {
    // some modifications for the coordinate transformation, seemless wrapped texture -- novalis
    const float PI = 3.14159265358979;
    vec2 p = (gl_FragCoord.xy / RENDERSIZE - vec2(.5)) * vec2(RENDERSIZE.x/RENDERSIZE.y, 1.);

    float r = length(p);
    float a = atan(p.y, p.x);

    vec2 uv = vec2(sin(a), cos(a))/pow(r, 1.+.5*sin(TIME));
	
    vec3 texCol = tex(uv);
    vec3 col = floor(texCol + (floor(texCol*2.2)*mod(gl_FragCoord.x +gl_FragCoord.y,2.))); //Glorious dithering! :D
    gl_FragColor = vec4(col*length(p),1);
}
