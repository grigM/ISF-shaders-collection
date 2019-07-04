/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	
		{
			"NAME": "iter",
			"TYPE": "float",
			"DEFAULT": 20.0,
			"MIN": 0.0,
			"MAX": 100.0
		},
		{
			"NAME": "sinSpeed",
			"TYPE": "float",
			"DEFAULT":2.5,
			"MIN": 0.0,
			"MAX": 10.0
		},
		{
			"NAME": "cosSpeed",
			"TYPE": "float",
			"DEFAULT":1.5,
			"MIN": 0.0,
			"MAX": 10.0
		},
		{
			"NAME": "sinCosTimeincr",
			"TYPE": "float",
			"DEFAULT":1.0,
			"MIN": 0.0,
			"MAX": 5.0
		},
		{
			"NAME": "sizeInv",
			"TYPE": "float",
			"DEFAULT":1.0,
			"MIN": 0.0,
			"MAX": 3.0
		},
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#2654.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


/*float f(vec2 x) {
	float r = length(x), a = atan(x.y, x.x);
	return r-1. + 0.5*sin(6.*a+2.*r*r*sin(TIME));
}*/
/*vec4 f(vec2 x) {
	vec2 p = x * abs(sin(TIME/10.0)) * 50.0;
	float d = sin(length(p)+TIME), a = sin(mod(atan(p.y, p.x) + TIME + sin(d+TIME), 3.1416/3.)*3.), v = a + d, m = sin(length(p)*4.0-a+TIME);
	return vec4(-v*sin(m*sin(-d)+TIME*.1), v*m*sin(tan(sin(-a))*sin(-a*3.)*3.+TIME*.5), mod(v,m), v);
}*/
vec4 f(vec2 x) {
	float d = 1.0;
	float TIME = TIME;
	for(int i = 0; i < int(iter); ++i) {
		vec2 foo = vec2(sin(TIME*sinSpeed), cos(TIME*cosSpeed));
		d *= length(foo * sizeInv - x);
		TIME += sinCosTimeincr;
	}
	return vec4(d);
}

vec2 grad(vec2 p) {
	vec2 h = vec2(.01, 0.);
	return vec2(f(p+h.xy).w - f(p-h.xy).w, f(p+h.yx).w - f(p-h.yx).w)/(2.0*h.x);
}

vec3 color(vec2 x) {
	vec4 v = f(x);
	vec2 g = grad(x);
	float de = abs(v.w) / length(g);
	float blah = 0.05;
	return vec3(1.0-smoothstep(blah*0.3, blah, de));
}

void main( void ) {

	vec2 position = gl_FragCoord.xy / RENDERSIZE.xy * 2. - 1.;
	//position /= 10.0;

	vec3 v = color(position);
	gl_FragColor = vec4( v, 1.0 );

}