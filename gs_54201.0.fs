/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54201.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

vec2 random2(vec2 st){
    st = vec2( dot(st,vec2(127.1,311.7)),
              dot(st,vec2(269.5,183.3)) );
    return -1.0 + 2.0*fract(sin(st)*43758.5453123 );
}

vec2 rand3(vec3 st){
	vec2 a=random2(st.xy);
	vec2 b=random2(st.yz+a);
	return random2(a+b+st.zx);
}
void adj(inout vec3 uuv, inout float t){
	vec3 m=(cos((fract(uuv)-0.5)*3.141*2.)+1.)*0.5;
	float mm=min(min(m.x,m.y),m.z);
	vec3 uuvf=floor(uuv);
	uuv.xy-=mm*rand3(uuvf)*0.2;
	t+=mm*rand3(uuvf).x;
}
void main( void )
{
	float ss = 8.0*sin(TIME);
	vec2 gg = gl_FragCoord.xy;
	gg = ceil(gg / ss) * ss;
	

	vec2 uv =  (gg -.5 * RENDERSIZE.xy) / RENDERSIZE.y ;
	
	
	
	float t = TIME * .4;
	vec3 uuv=vec3(uv.x*2.,uv.y*2.,t);
	uv*=2.0;
	adj(uuv,t);
	uuv+=vec3(9.2,1.2,2.2);
	adj(uuv,t);
	uuv+=vec3(9.2,1.2,2.2);
	adj(uuv,t);
	uuv+=vec3(9.2,1.2,2.2);
	adj(uuv,t);
	uuv+=vec3(9.2,1.2,2.2);
	adj(uuv,t);
	uuv+=vec3(9.2,1.2,2.2);
	adj(uuv,t);
	
	
	uuv=mod(uuv,0.25)-0.125;
	float c = 1. / ((pow(uuv.x, 2.) + (pow((uuv.y), 2.)) -0.01) * 250.);
	
	c -= abs(-cos(t));
	float pi=3.141;
	float tt=TIME/pi+pi*0.5;
	vec3 color=vec3(random2(floor(vec2(tt+7.,tt-1.))),random2(floor(vec2(tt+5.,tt+2.))).x);
	//color+=1.;
	//color*=0.5;
	
	gl_FragColor = vec4(
		vec3(c, c, c )*mix(color,vec3(1.0,1.0,1.0),(1.0-cos(TIME*2.))*0.5),
		1.
	);
}	