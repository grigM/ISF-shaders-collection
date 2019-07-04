/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#16411.0"
}
*/


// forked from nimitz's "Neon parallax" https://www.shadertoy.com/view/XssXz4

#ifdef GL_ES
precision mediump float;
#endif


float pulse(float cn, float wi, float x)
{
	return 1.-smoothstep(0., wi, abs(x-cn));
}

float hash11(float n)
{
    return fract(sin(n)*43758.5453);
}

vec2 hash22(vec2 p)
{
    p = vec2( dot(p,vec2(127.1, 311.7)), dot(p,vec2(269.5, 183.3)));
	return fract(sin(p)*43758.5453);
}

vec2 field(in vec2 p)
{
	vec2 n = floor(p);
	vec2 f = fract(p);
	vec2 m = vec2(1.);
	vec2 o = hash22(n)*0.17;
	vec2 r = f+o-0.5;
	float d = abs(r.x) + abs(r.y);
	if(d<m.x)
    {
		m.x = d;
		m.y = hash11(dot(n,vec2(1., 2.)));
	}
	return vec2(m.x,m.y);
}

void main(void)
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy-0.5;
	uv.x *= RENDERSIZE.x/RENDERSIZE.y*0.9;
	uv *= 4.;
	
	vec2 p = uv*.01;
	p *= 1./(p-1.);
	
	//global movement
	uv.y += TIME*1.2;
	uv.x += sin(TIME*0.3)*0.8;
	vec2 buv = uv;
	
	float rz = 0.;
	vec3 col = vec3(0.);
	for(float i=1.; i<=26.; i++)
	{
		vec2 rn = field(uv);
		uv -= p*(i-25.)*0.2;
		rn.x = pulse(0.35,.02, rn.x+rn.y*.15);
		col += rn.x*vec3(sin(rn.y*10.), cos(rn.y)*0.2,sin(rn.y)*0.5);
	}
	
	//animated grid
	buv*= mat2(0.707,-0.707,0.707,0.707);
	float rz2 = .4*(sin(buv*10.+1.).x*40.-39.5)*(sin(uv.x*10.)*0.5+0.5);
	vec3 col2 = vec3(0.2,0.4,2.)*rz2*(sin(2.+TIME*2.1+(uv.y*2.+uv.x*10.))*0.5+0.5);
	float rz3 = .3*(sin(buv*10.+4.).y*40.-39.5)*(sin(uv.x*10.)*0.5+0.5);
	vec3 col3 = vec3(1.9,0.4,2.)*rz3*(sin(TIME*4.-(uv.y*10.+uv.x*2.))*0.5+0.5);
	
	col = max(max(col,col2),col3);
	
	gl_FragColor = vec4(col,1.0);
}