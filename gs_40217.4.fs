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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40217.4"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


bool square(vec2 v, float l){
	return max(abs(v.x), abs(v.y)) < l/2.;
}

bool plus(vec2 v){
	v *= 3.;
	return square(v, 1.)
	    || square(v + vec2(0, 1), 1.)
	    || square(v + vec2(0,-1), 1.)
	    || square(v + vec2(-1,0), 1.)
	    || square(v + vec2( 1,0), 1.)
	;
}

float tmod(float t){
	const float m = 3.;
	float mt = mod(t, m);
	if(mt >= 1./m) return 0.;
	return (3.14159/2.)*(mt*m);
}

vec4 white(vec2 v, float t){
	bool b = false;
	
	t *= 0.75;
	v += vec2(2.5, 1.5);
	
	vec2 _v = v;
	float _t = t;
	
	#define _rot2(rot2_angle) mat2(sin(rot2_angle), cos(rot2_angle), cos(rot2_angle), -sin(rot2_angle))
	#define _stamp b = b || plus(v*_rot2(tmod(t)));
	#define _shift_right v.x -= 1.;v.y += 1./3.;t += 2.;
	#define _reset v = _v; t = _t;
	#define _shift_up v.x -= 1./3.;v.y -= 1.;t += 2./3.;
	
	_stamp;
	_shift_right; _stamp;
	_shift_right; _stamp;
	_shift_right; _stamp;
	_shift_right; _stamp;
	_reset;
	_shift_up; _stamp;
	_shift_right; _stamp;
	_shift_right; _stamp;
	_shift_right; _stamp;
	_shift_right; _stamp;
	_reset;
	_shift_up; _shift_up; _stamp;
	_shift_right; _stamp;
	_shift_right; _stamp;
	_shift_right; _stamp;
	_shift_right; _stamp;
	_reset;
	_shift_up; _shift_up; _shift_up; _stamp;
	_shift_right; _stamp;
	_shift_right; _stamp;
	_shift_right; _stamp;
	_shift_right; _stamp;
	
	return vec4(float(b));
}

vec4 black(vec2 v, float t){
	v.x -= 2./3.;
	v.y -= 1./3.;
	return 1.-white(v, t);
}

void main( void ) {
	vec2 p = vv_FragNormCoord*4.;
	
	gl_FragColor = white(p, TIME);
	if(gl_FragCoord.x < RENDERSIZE.x * mouse.x) gl_FragColor = black(p, TIME);
}