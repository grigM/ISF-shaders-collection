/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40720.0"
}
*/


//precision highp float;

#extension GL_OES_standard_derivatives : enable


float rand(vec2 co)
{
	return fract(sin(dot(co.xy,vec2(12.9898,78.233))) * 43758.5453);
}

void main( void ) {
	vec2 p =  (2.0 * gl_FragCoord.xy  - RENDERSIZE)  / min(RENDERSIZE.x, RENDERSIZE.y);
	
	p *= 4.0;
	
	float t = mod(floor(p.x) + floor(p.y), 2.0);
	float u = mod(floor(p.y), 2.0);


	vec2 o1;
	float o2;
	
	if(u == 0.0) {
		o1 =  vec2(fract(TIME));
	} else {
		o1 = vec2(0.0);
	}
	
	if(t == 0.0) {
		o2 = 0.0;
	} else {
		o2 = floor(p.x) * 0.25;
	}
	
	p = fract(p);

	float v = min(rand(p + o1) + o2, 1.0);
	
	float a = (cos(TIME) + 1.0) / ( 10.0 * distance(p, vec2(0.5, 0.5)) );
	
	vec3 color;
	if(t == 0.0) {
		color = vec3(a);
	} else {
		color = vec3(step(v, a));
	}

	gl_FragColor = vec4(color, 1.0 );

}