/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#48670.0"
}
*/


// water turbulence effect by joltz0r 2013-07-04
//Ziad, 2018

#ifdef GL_ES
precision mediump float;
#endif


mat2 m =mat2(0.8,0.6, -0.6, 0.8);

float rand(vec2 n) { 
	return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}

float noise(vec2 n) {
	const vec2 d = vec2(0.0, 1.0);
  	vec2 b = floor(n), f = smoothstep(vec2(0.0), vec2(1.0), fract(n));
	return mix(mix(rand(b), rand(b + d.yx), f.x), mix(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}

float fbm(vec2 p){
	float f=.0;
	f+= .5000*noise(p); p*= m*2.02;
	f+= .2500*noise(p); p*= m*2.03;
	f+= .1250*noise(p); p*= m*2.01;
	f+= .0625*noise(p); p*= m*2.04;
	
	f/= 0.9375;
	
	return f;
}

float udRoundBox( vec3 p, vec3 b, float r )
{
  return length(max(abs(p)-b,0.0))-r;
}


#define MAX_ITER 32
void main( void ) {

	vec2 p = vv_FragNormCoord*4.0;
	vec2 i = p;
	float c = 0.0;
	float inten = 1.0;
    vec3 ro = vec3(.0, .0, -9.);
	vec3 rd = normalize(vec3((gl_FragCoord.xy * 2. - RENDERSIZE.xy) / min(RENDERSIZE.x, RENDERSIZE.y), 1.));


	for (int n = 0; n < MAX_ITER; n++) {
		float t = TIME * (1.0 - (1.0 / float(n+1)));
		i = p + vec2(
			cos(t - i.x) + sin(t + i.y), 
			sin(t - i.y) + cos(t + i.x)
		);
		c += 1.0/length(vec2(
			p.x / (sin(i.x+t)/inten),
			p.y / (cos(i.y+t)/inten)
			)
		);
		//c+=fbm(i*5.);
		ro += min(udRoundBox(rd,vec3(0.7,0.7,0.5),0.3), 0.5) * rd;
		c+=fbm(ro.zy*3.);
		c-=fract(c);
	}
	c /= float(MAX_ITER);
	float v = -ro.z*.2;
	
	c*=v;
	
	gl_FragColor = vec4(vec3(pow(c,1.5))*vec3(0.95, 0.97, 1.8), 1.0);
}