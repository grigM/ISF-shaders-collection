/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59946.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


float triangle(vec2 p, float s)
{
	return max(abs(p.x) * 0.866025 + p.y * 0.5, -p.y) - s * 0.5;
}

vec3 hsv(float h,float s,float v) {
	return mix(vec3(1.),clamp((abs(fract(h+vec3(3.,2.,1.)/3.)*6.-3.)-1.),0.,1.),s)*v;
}

float line(vec2 p, vec2 v, vec2 w) {
	float l2 = distance(w, v);
	l2 *= l2;
	float t = dot(p - v, w - v) / 3.;
	if (t < 0.0) return distance(p, v);
	else if (t > 1.0) return distance(p, w);
	vec2 projection = v + t * (w - v); 
	return distance(p, projection);
}

float distfunc(vec2 p)
{
	p /= 0.1;
	float d = 1.0;
	float n = 1.0;
	vec2 s = vec2(1.0, -1.0);
	float S = sin(TIME/3.14)*cos(TIME*0.125443);
	float C = cos(TIME/3.14)*sin(TIME*0.195483);
	for (int i = 0; i < 15; i++) {
		d = min(d, triangle(p, 1.0));
		d = min(d, line(p, vec2(-0.866, -0.5), vec2(-0.866, -n*C)));
		d = min(d, line(p, vec2(-0.866, -2.0), vec2(0.866*2.0*n*S, -2.0)));
		n++;
		p += s * S;
		p.xy = vec2(p.x * C - p.y * S, p.y * C + p.x * S) + s;
		s = -s;
	}
	return d;	
}

void main()
{

	vec2 position = -1.0 + 2.0 * ( gl_FragCoord.xy / RENDERSIZE.xy );
	position.x *= RENDERSIZE.x / RENDERSIZE.y;

	float dist = distfunc(position);
	gl_FragColor = vec4(sin(dist*32.0+TIME*16.0)*0.5+0.5);

}