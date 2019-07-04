/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#6207.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


const int MAXITER = 50;

vec3 field(vec3 p) {
	p = abs(fract(p)-.5);
	p *= p;
	return sqrt(p+p.zyy)-.01;
}

void main( void ) {
	vec3 dir = normalize(vec3((gl_FragCoord.xy-RENDERSIZE*.5)/RENDERSIZE.x,1.));
	float a = TIME * 0.1;
	vec3 pos = vec3(-2.0*cos(TIME),sin(TIME*2.)*0.25,TIME*5.);

	vec3 color = vec3(0);
	for (int i = 0; i < MAXITER; i++) {
		vec3 f2 = field(pos);
		float f = min(min(f2.x,f2.y),f2.z);
		
		pos += dir*f;
		color += float(MAXITER-i)/(f2+.01);
	}
	vec3 color3 = vec3(1.-1./(1.+color*(.09/float(MAXITER*MAXITER))));
	color3 *= color3;
	gl_FragColor = vec4(vec3(color3.r+color3.g+color3.b),1.);
}