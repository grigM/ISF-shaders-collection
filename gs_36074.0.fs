/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36074.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

// inertia rotation --joltz0r

#define TIME 0.2*TIME

float check(vec2 p, float size) {
	return mod(floor(p.x * size) + floor(p.y * size),2.0);
}

void main( void ) {

	vec2 p = ((gl_FragCoord.xy / RENDERSIZE) - 0.5) * 2.0;
	p.x *= RENDERSIZE.x/RENDERSIZE.y;	

	//inertia towards the edges
	float t = sin(TIME*10. - 4.0*distance(p, vec2(0.0)))*0.5;
	p *= mat2(cos(t), -sin(t),
		  sin(t),  cos(t)
	);

	gl_FragColor = vec4(check(p, 3.0) * (1.0/length(p))*0.5);
}