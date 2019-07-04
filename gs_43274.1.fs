/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43274.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


mat2 rot(float a) {
	float c = cos(a), s = sin(a);
	return mat2(c, s, -s, c);
}

float map(vec3 p) {
	p.xy *= rot(TIME);
	p.yz *= rot(TIME);
	p = mod(p + 2.5, 5.) - 2.5;
	return length(p) - 1.;
}

void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );

	position -= 0.5;
	position.x *= RENDERSIZE.x / RENDERSIZE.y;
	
	// float d = length(position);
	// Create a circle
	// float c = step(0.2, d);
	// Create a smoothed circle
	// float c = smoothstep(0.2, 0.21, d);
	// Create a smoothed circle that move, yeah
	// float c = smoothstep(0.2, 0.21, d - 0.2 + sin(TIME * 1.) * 0.05);

	// Ray marching
	// ro = ray origin
	vec3 ro = vec3(0, 0, -3.5),
	     rd = normalize(vec3(position, 1.)),
	     mp = ro;
	
	float ff;
	for(float f = 0.; f < 30.; ++f) {
		ff = f;
		float d = map(mp);
		if(abs(d) < .01)
			break;
		
		mp += rd * d;
	}
	
	float c = 1. - ff / 30.;
	
	// Give the color of the fragment
	gl_FragColor = vec4(c, c, c, 1.0 );

}