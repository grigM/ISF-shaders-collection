/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#2100.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

// moire plasma by psonice


void main( void ) {

	float t = TIME+1000.;
	vec2 p = (gl_FragCoord.xy-RENDERSIZE.xy*.5)*(sin(t*0.001)*0.005+0.012);
	//t*=sin(t*0.01)*45.+55.;
	float a = 0.;
	for(float i=0.; i<5.; i++){
		p *= vec2(sin(i)*sin(p.y+t)*cos(p.x+t)*.005+.6666, cos(i)*sin(p.x+t)*cos(p.y+t)*0.005+0.6666);
		a +=  sin(p.x + t) * sin(p.y + t) > sin(p.x*t+0.)*sin(p.y*t+0.)+0.4 ? 1. : 0.;
	}
	float o = a*.5;

	gl_FragColor = vec4( vec3(o), 1.0 );

}