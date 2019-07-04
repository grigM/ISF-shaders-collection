/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54107.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	vec2 p = 2.0*( gl_FragCoord.xy / RENDERSIZE.xy )-1.0;
	p.x *= RENDERSIZE.x/RENDERSIZE.y;
	vec3 col = vec3(0);
	
	
	vec2 op = p;
	
	for (int w = 0; w < 8; w++) {
		p = op+0.001*vec2(cos(float(w)),sin(float(w)));
	for (int k = 0; k < 6; k++) {
		
		for (int j = 0; j < 2; j++) {
			for (int i = 0; i < 3; i++) {		
				if (abs(length(p.xy-vec2(i-1,float(j)-0.5))-0.3) < 0.003) col += vec3(1)/(1.0+0.5*float(k));
			}
		}
		p *= -1.+0.2*cos(-TIME+float(w)*222.+1000.*float(k));
		//col *= 0.7;
	}
		col *= 0.5;
}
	gl_FragColor = vec4(col, 1.0);
}