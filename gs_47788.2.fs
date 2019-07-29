/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "size",
			"TYPE": "float",
			"DEFAULT":  0.9375,
			"MIN": 0.0,
			"MAX": 3.0
			
		},
		{
			"NAME": "noise_speed",
			"TYPE": "float",
			"DEFAULT":  1.0,
			"MIN": 0.1,
			"MAX": 5.0
			
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT":  1.0,
			"MIN": 0.1,
			"MAX": 5.0
			
		},
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#47788.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


mat2 m =mat2(0.8,0.6, -0.6, 0.8);

float rand(vec2 n) { 
	return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}

float noise(vec2 n) {
	const vec2 d = vec2(0.0, 1.0);
  	vec2 b = floor(n), f = smoothstep(vec2(0.0), vec2(1.0), fract(n));
	return mix(mix(rand(b), rand(b + d.yx), f.x), mix(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}

float fbm(vec2 p){
	float f=.0;
	f+= .5000*noise(p); p*= m*2.02;
	f+= .2500*noise(p); p*= m*2.03;
	f+= .1250*noise(p); p*= m*2.01;
	f+= .0625*noise(p); p*= m*2.04;
	
	f/= size;
	
	return f;
}


void main( void ) {
	vec2 position = gl_FragCoord.xy - RENDERSIZE / 2.0;
	float ss=.5+0.5*sin(.5*(TIME*noise_speed));
	float color = smoothstep(3.0, 1.0, abs(fbm(position*ss)*length(position) - 50.0 + fract(atan(position.y,atan(position.y, position.x))*(TIME*speed)*0.01 * 4.0 - 3.141 / 2.0) * 7.0));
	gl_FragColor = vec4(vec3(color), 1.0 );
}