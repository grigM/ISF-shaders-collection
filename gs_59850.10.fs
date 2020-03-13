/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59850.10"
}
*/


// Your Mum's husk (deep computed)
#ifdef GL_ES
precision highp float;
#endif


#define PI 3.141592

vec3 quantum(vec2 uv, float offset, vec3 color)
{
	float speed = TIME*0.8;
	float pulse = sin(uv.x + speed + offset) + 2.0;
	pulse *= 0.25;
	float d = clamp(1.0 - distance(pulse, uv.y)*0.8, 0.0, 1.0);
	d = pow(d, 15.0);
	return color * d;
}
vec3 pal(float t)
{
	return vec3(sin(t/2.0)+cos(t/5.76+14.5)*0.5+0.5,sin(t/2.0)+cos(t/4.76+14.5)*0.5+0.4,sin(t/2.0)+cos(t/3.76+14.5)*0.5+0.3);
}


void main(void)
{
	vec2 uv = (gl_FragCoord.xy - RENDERSIZE * .5) / RENDERSIZE.yy;
	float d = 1.0-abs(uv.x);
	uv.x = mod(uv.x+fract(TIME*0.2)*1.1,1.1)-0.55;
	uv*=1.8;
	uv.y = dot(uv,uv);
	const int nnn = 8;
	float vv = TIME;
	float vva = 20.;
	float pp = TIME*0.5;
	float ppa = PI/float(nnn);
	
	vec3 col = vec3(0.0);
	for (int i=0;i<nnn;i++)
	{	
		col += quantum(uv.xy, pp, pal((24.0*uv.x)+vv))*0.7;
		pp += ppa+uv.y;
		vv += vva;
	}
	col = (col*col)*d;
	gl_FragColor=vec4(col.xyz,1.0);
}