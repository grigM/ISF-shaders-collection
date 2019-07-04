/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#42172.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main(){
	vec2 pos = vv_FragNormCoord;

	const float pi = 3.14159;
	const float n = 16.0;
	
	float t = 0.0;
	
	float color = 0.0;
	for (float i = 0.0; i < n; i++){
		color += 0.001/abs(sin( 4. * pi * pos.x) * sin( 4. * pi * pos.y) * 0.3 * sin(1.0*pi*(t + TIME)));
	}
	
	gl_FragColor = vec4(vec3(1.5, 0.5, 0.15) * color, color);
	
}