/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#6423.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


// a raymarching experiment by kabuto
//fork by tigrou ind (2013.01.22)
//optimized

#define MAXITER 90
#define MAXITER_SQR MAXITER*MAXITER

vec3 field(vec3 p) {
	p = abs(fract(p)-.5);
	p *= p;
	return sqrt(p+p.yzx*p.zzy)-.015;
}

void main( void ) {
	vec3 dir = normalize(vec3((gl_FragCoord.xy-RENDERSIZE*.5)/RENDERSIZE.x,1.));
	float a = TIME * 0.4;
	vec3 pos = vec3(TIME*0.6 + sin(TIME)*0.2,sin(TIME)*0.25,-3.0);

	vec3 color = vec3(0);
	for (int i = 0; i < MAXITER; i++) {
		vec3 f2 = field(pos);
		vec3 rep = vec3(1.0);
		float f = min(min(min(f2.x,f2.y),f2.z), length(mod(pos-vec3(0.5,0.5,0.2),rep)-0.5*rep)-0.15);
		pos += dir*f;
		color += float(MAXITER-i)/(f2+1e-5);
	}
	vec3 color3 = vec3(-1./(1.+color*(.5/float(MAXITER_SQR))));
	color3 *= color3;
	gl_FragColor = vec4(vec3(color3.r+color3.g+color3.b),1.);
}