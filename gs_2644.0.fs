/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#2644.0",
  "INPUTS" : [
    {
      "NAME" : "color_invert",
      "TYPE" : "bool",
      "DEFAULT" : 1
    },
    {
      "NAME" : "iter",
      "TYPE" : "float",
      "MAX" : 46.0,
      "DEFAULT" : 23.0,
      "MIN" : 0.0
    },
    {
      "NAME" : "item_dist",
      "TYPE" : "float",
      "MAX" : 3.0,
      "DEFAULT" : 0.8,
      "MIN" : 0.15
    },
    {
      "NAME" : "speed",
      "TYPE" : "float",
      "MAX" : 3.0,
      "DEFAULT" : 1.0,
      "MIN" : 0.1
    },
    {
      "NAME" : "time_incr",
      "TYPE" : "float",
      "MAX" : 3.0,
      "DEFAULT" : 1.0,
      "MIN" : 0.1
    },
    {
      "NAME" : "grad_param",
      "TYPE" : "float",
      "MAX" : 0.3,
      "DEFAULT" : 0.01,
      "MIN" : -0.3
    },
    {
      "NAME" : "glow_contrast",
      "TYPE" : "float",
      "MAX" : 2.0,
      "DEFAULT" : 1.0,
      "MIN" : 1.0
    },
  ],
  "ISFVSN" : "2"
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
		vec2 foo = vec2(sin((TIME*speed)*2.5), cos((TIME*speed)*1.5));
		d *= length(foo * item_dist - x);
		TIME += (10.0*time_incr);
	}
	return vec4(d);
}

vec2 grad(vec2 p) {
	vec2 h = vec2(grad_param, 0.);
	return vec2(f(p+h.xy).w - f(p-h.xy).w, f(p+h.yx).w - f(p-h.yx).w)/(2.0*h.x);
}

vec3 color(vec2 x) {
	vec4 v = f(x);
	vec2 g = grad(x);
	float de = abs(v.w) / length(g);
	float blah = 0.05;
	return vec3(glow_contrast-(blah*0.3, blah, de));
}

void main( void ) {

	vec2 position = gl_FragCoord.xy / RENDERSIZE.xy * 2. - 1.;
	//position /= 17.0;

	vec3 v = color(position);
	
	vec3 inv_color = vec3(1.0-v.r, 1.0-v.g, 1.0-v.b);
	if(color_invert){
		gl_FragColor = vec4( inv_color, 1.0 );
	}else{
		gl_FragColor = vec4( v, 1.0 );
	}
}