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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#5688.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


// a raymarching experiment by kabuto


const int MAXITER = 30;

vec3 field(vec3 p) {
	p *= .1;
	float f = .1;
	for (int i = 0; i < 3; i++) {
		p = p.yzx*mat3(.8,.6,0,-.6,.8,0,0,0,1);
		p += vec3(.123,.456,.789)*float(i);
		p = abs(fract(p)-.5);
		p *= 3.0;
		f *= 3.0;
	}
	p *= p;
	return sqrt(p+p.yzx)/f-.05;
}

void main( void ) {
	vec3 dir = normalize(vec3((gl_FragCoord.xy-RENDERSIZE*.5)/RENDERSIZE.x,1.));
	vec3 pos = vec3(mouse+.5,TIME);
	vec3 color = vec3(0);
	for (int i = 0; i < MAXITER; i++) {
		vec3 f2 = field(pos);
		float f = min(min(f2.x,f2.y),f2.z);
		
		pos += dir*f;
		color += float(MAXITER-i)/(f2+.001);
	}
	vec3 color3 = vec3(1.-1./(1.+color*(.09/float(MAXITER*MAXITER))));
	color3 *= color3;
	gl_FragColor = vec4(color3,1.);
}