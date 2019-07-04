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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#46656.5"
}
*/


#ifdef GL_ES
precision highp float;
#endif

#extension GL_OES_standard_derivatives : enable

uniform vec2 surfaceSize;

#define SP (vv_FragNormCoord.xy)
#define M (mouse.xy)
#define I (M * 2.0 - 1.0)
#define F (gl_FragCoord.xy)
#define R (RENDERSIZE.xy)
#define Rmin min(R.x,R.y)
#define W ((F - R/2.0) / Rmin)

void main( void ) {

	vec2 p = SP*9.;
	vec2 _p = p;
	p.x -= 4.*sign(p.x)*float(abs(p.x) > 2.);
	p.y -= sign(p.y)*2.;
	
	vec3 v =  vec3(p,length(p)-.5);
	
	
	if(_p.x > 2.){
		if(_p.y > 0.){
			v = v.rgb;
		}else{
			v = v.rbg;
		}
	}else if(_p.x > -2.){
		if(_p.y > 0.){
			v = v.gbr;
		}else{
			v = v.grb;
		}
	}else{
		if(_p.y > 0.){
			v = v.brg;
		}else{
			v = v.bgr;
		}
	}
	
	v = normalize(v) * 0.5 + 0.5;
	gl_FragColor = vec4( v, 1.0 );

}