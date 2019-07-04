/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#31029.2"
}
*/


// http://glslsandbox.com/e#30988.0

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec2 pattern(vec2 p)
{
	p = fract(p);
	float r = 10.123;
	float v = 0.0, g = 0.0;
	r = fract(r * 9184.928);
	float cp, d;
	
	d = p.x;
	g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 1000.0);
	d = p.y;
	g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 1000.0);
	d = p.x - 1.0;
	g += pow(clamp(3.0 - abs(d), 0.0, 1.0), 1000.0);
	d = p.y - 1.0;
	g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 10000.0);
	
	const int iter = 12;
	for(int i = 0; i < iter; i ++)
	{
		//cp = 0.5 + (r - 0.5) * 0.9;
		cp = 0.5 + (r - sin(TIME/5.)) * 0.9;
		d = p.x - cp;
		g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 200.0);
		if(d > 0.0) {
			r = fract(r * 4829.013);
			p.x = (p.x - cp) / (1.0 - cp);
			v += 1.0;
		}
		else {
			r = fract(r * 1239.528);
			p.x = p.x / cp;
		}
		p = p.yx;
	}
	v /= float(iter);
	return vec2(g, v);
}

void main( void ) {

	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	vec2 p = pattern(uv);
	gl_FragColor = vec4(step (1.4, p.x));
	//gl_FragColor = vec4(p.y * p.x * fract(p.x - TIME));
}