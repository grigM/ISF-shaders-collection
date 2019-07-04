/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 4.0
		},
		{
			"NAME": "inten",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "CHECK_SIZE",
			"TYPE": "float",
			"DEFAULT": 8.0,
			"MIN": 0.0,
			"MAX": 20.6
		},
		{
			"NAME": "TRANSP",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": 0.0,
			"MAX": 1.0
		},
		
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#13001.0"
}
*/


// water turbulence effect by joltz0r 2013-07-04, improved 2013-07-07
#ifdef GL_ES
precision mediump float;
#endif


// for some reason you get slowdowns the closer to 32 you set MAX_ITER (64 seems a lot faster than 32)?
#define MAX_ITER 32




float check(vec2 p) {
	p = p * CHECK_SIZE;
	float c = ceil(sin(p.x)*cos(p.y))*10.0;
	return smoothstep(c, 2.0, 4.0);
}

void main( void ) {

	vec2 p = vv_FragNormCoord*4.0;
	vec2 i = p;
	vec2 it = i;
	float c = 0.0;
	
	vec2 sc = vec2(sin(2.0+(TIME*1.1)*speed), cos(2.0+(TIME*0.9)*speed))*vec2(1.5);
	

	for (int n = 0; n < MAX_ITER; n++) {
		float t = (TIME*speed) * (1.0 - (1.0 / float(n+1)));
		i = p + vec2(
			cos(t - i.x) + sin(t + i.y), 
			sin(t - i.y) + cos(t + i.x)
		);
		// lighting plane
		it = vec2(i.x * sin(i.x + t)/inten, i.y * cos(i.y + t)/inten);
	}

	c = 1.0/(check(p + (i*TRANSP)) * (check(i) * (1.0 - TRANSP)) * length(p+sc) * length(it+sc));
	gl_FragColor = vec4(vec3(pow(c, 1.5))*vec3(0.95, 0.97, 0.97), 1.0);
}