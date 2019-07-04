/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#2711.0"
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
	for(int i = 0; i < 25; ++i) {
		vec2 foo = vec2(sin(TIME*2.5), cos(TIME*1.5));
		d *= length(foo * 0.7 - x);
		TIME += 10.0;
	}
	return vec4(d);
}

vec2 grad(vec2 p) {
	vec2 h = vec2(.00001, 0.00001);
	return vec2(f(p+h.xy).w - f(p-h.xy).w, f(p+h.yx).w - f(p-h.yx).w)/(2.0*h.x);
}

vec3 color(vec2 x) {
	vec4 v = f(x);
	vec2 g = grad(x);
	float de = abs(v.w) / length(g);
	return vec3(de*0.2, de, de);
}

void main( void ) {

	vec2 position = gl_FragCoord.xy / RENDERSIZE.xy * 2. - 1.;
	//position /= 17.0;

	vec3 v = color(position);
	gl_FragColor = vec4( v, 1.0 );

}