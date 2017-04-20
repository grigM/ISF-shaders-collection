/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
	{
			"NAME": "qposX",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 3.0
	},
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35376.3"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float hash(vec2 p) {
	return fract(sin(dot(p, vec2(15.38, 35.76))) * 43758.23);
}

void main( void ) {

	vec2 p = ( gl_FragCoord.xy / RENDERSIZE.xy );
	float span = 1.0;
	float gt = mod(TIME, span * 4.0) / (span * 4.0);
	float m = 1.0;
	float n = 1.0;
	/*
	if(gt < 0.25) { m = 1.0; n = 1.0; }
	else if(gt < 0.5) { m = 1.0; n = -1.0; }
	else if(gt < 0.75) { m = -1.0; n = 1.0; }
	else { m = -1.0; n = -1.0; }
	float t = mod(TIME, span) / span;
	*/
	p = 2.0 * p - 1.0;
	p.x *= RENDERSIZE.x / RENDERSIZE.y;
	vec2 q = p;
	
	p *= 20.0;
	p = mod(p, 5.0) - 2.5;
	
	float co = cos(3.141592 / 4.0);
	float si = sin(3.141592 / 4.0);
	p = mat2(co, -n * si, n * si, co) * p;
	p.x += m * -(sin(0.0 * 3.14 * 0.5) * 2.0 - 1.0);
	p.y += m * -1.0;
	float c = max(abs(p.x), abs(p.y)) - 0.5;
	c = smoothstep(0.0, 0.01, c);
	
	q -= qposX;
	q = mat2(co, -si, si, co) * q;
	//c *= hash(floor((q - 0.075) * 5.0));
	c *= hash(floor(q * 3.0));

	gl_FragColor = vec4( c, c, c, 1.0 );

}