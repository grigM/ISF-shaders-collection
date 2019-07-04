/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#48123.2"
}
*/


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

void main() {
	vec2 p = fbm(vv_FragNormCoord) * 3.000 - vec2(15.0, 8.4);
	vec2 i = p;
	float c = 1.1;
	float inten = .05;

	float t = TIME*-.2;
	i = p + fbm((vec2(cos(t - i.x) + sin(t + i.y), sin(t - i.y) + cos(t + i.x)))*20.);
	c += 1.0/length(vec2(p.x / (2.*sin(i.x+t)/inten),p.y / (cos(i.y+t)/inten)));

	c = 1.5-sqrt(pow(c,3.0));
	c = c*c*c*c;
	c += c+smoothstep(0.2,0.9,fbm(vec2(c,c)));
	gl_FragColor = vec4(c,c,c, 1.0);
}