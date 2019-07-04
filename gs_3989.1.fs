/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#3989.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


const float pi=3.1415926535;

vec2 clog(vec2 v)
{
	return vec2(0.5*log(v.x*v.x+v.y*v.y),atan(-v.y,v.x));
}

vec2 cdiv(vec2 a,vec2 b)
{
	return vec2(a.x*b.x+a.y*b.y,a.y*b.x-a.x*b.y)/(b.x*b.x+b.y*b.y);
}

vec4 checkerboard(vec2 pos)
{
	float a=(fract(pos.x)-0.5)*(fract(pos.y)-0.5);
	if(a>0.0) return vec4(0.7,0.72,0.7,1.0);
	else return vec4(0.3,0.33,0.3,1.0);
}

vec4 grid(vec2 pos,float thickness)
{
	vec2 a=abs(fract(pos)-0.5);
	if(a.x>thickness && a.y>thickness) return vec4(0.7,0.72,0.7,1.0);
	else return vec4(0.3,0.33,0.3,1.0);
}

vec4 wave(vec2 pos)
{
	float a=fract(pos.x+0.5*sin(pos.y*1.0*pi));
	if(a>0.5) return vec4(0.7,0.72,0.7,1.0);
	else return vec4(0.3,0.33,0.3,1.0);
}

vec4 rainbow(vec2 pos)
{
	return vec4(sin(pos.x*2.0*pi)+1.0,sin(pos.x*2.0*pi+pi*1.0/3.0)+1.0,sin(pos.x*2.0*pi+pi*2.0/3.0)+1.0,1.0)*(sin(pos.y*8.0*pi)+1.0)*0.7;
}

void main()
{
	vec2 position=2.0*((2.0*gl_FragCoord.xy-RENDERSIZE)/max(RENDERSIZE.x,RENDERSIZE.y));

	const float p1=1.0;
	const float p2=1.0;
	float u_corner=2.0*pi*p2;
	float v_corner=log(256.0)*p1;
	float diag=sqrt(u_corner*u_corner+v_corner*v_corner);
	float sin_a=v_corner/diag;
	float cos_a=u_corner/diag;
	float scale=diag/2.0/pi;

	float offset=0.7;

	vec2 p=clog(position+vec2(offset,0))
		-clog(position+vec2(-offset,0));

	vec2 rotated=vec2(p.x*cos_a-p.y*sin_a,
		          p.x*sin_a+p.y*cos_a);
	vec2 scaled=rotated*scale/vec2(log(256.0),2.0*pi);
//	vec2 scaled=p/vec2(log(256.0),2.0*pi);
//	vec2 translated=scaled-vec2(0.35+TIME*0.1,0.0);
	vec2 translated=scaled-vec2(TIME*0.1);

	
//	gl_FragColor=checkerboard(translated*2.0);
//	gl_FragColor=wave(translated*8.0+0.3);
	gl_FragColor=rainbow(translated);
}