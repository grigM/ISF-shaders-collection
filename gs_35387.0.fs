/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35387.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float hash(vec2 p) {
	return fract(sin(dot(p, vec2(15.38, 35.76))) * 43728.23);
}

void main( void ) {

	vec2 p = ( gl_FragCoord.xy / RENDERSIZE.xy );
	float span = 1.0;
	float gt = mod(TIME, span * 4.0) / (span * 4.0);
	float m = 1.0;
	float n = 1.0;
	
	float t = mod(TIME, span) / span;
	p = 2.0 * p - 1.0;
	p.x *= RENDERSIZE.x / RENDERSIZE.y;
	vec2 k = p;
	
	p *= 100.0;
	vec2 q = p;
	float o = hash(floor(q / (5.0 * sqrt(2.0))));
	float r = floor(o * 64.0) * 3.141592 / 2.0;
	
	p = mod(p, sqrt(2.0) * 5.0) - sqrt(2.0) * 2.5;
	p *= 0.6;
	
	float co = cos(3.141592 / 4.0);
	float si = sin(3.141592 / 4.0);
	
	if(gt < 0.25) { m = 1.0; n = 1.0; }
	else if(gt < 0.5) { m = 1.0; n = -1.0; }
	else if(gt < 0.75) { m = -1.0; n = 1.0; }
	else { m = -1.0; n = -1.0; }
	
	p = mat2(co, -n * si, n * si, co) * p;
	p = mat2(cos(r), -sin(r), sin(r), cos(r)) * p;
	p.x += m * -(sin(t * 3.14 * 0.5) * 2.0 - 1.0);
	p.y += m * -1.0;
	float c = 0.0;
	//c = max(abs(p.x), abs(p.y)) - 0.5;
	//c = length(p) - 0.5;
	c = sin(max(abs(p.x), abs(p.y)) * 3.14 * (0.1 + 4.9 * o) - (o > 0.5 ? -1.0 : 1.0) * TIME * (0.1 + 4.9 * o));
	c = 0.2 / (clamp(c, 0.0, 1.0) + 0.1);
	//c = smoothstep(0.0, 0.2, c);
	
	//c *= (0.5 + 0.5 * o);
	c = 2.5 - c;
	c *= (0.35 + 0.2 * k.x);
	vec3 col =  vec3(c * (1.0 - o), c * 0.5, c + sin(o * 3.14 * 10.0));
	gl_FragColor = vec4(vec3(c), 1.0 );

}