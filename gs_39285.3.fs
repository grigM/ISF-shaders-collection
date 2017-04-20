/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39285.3"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



float beam(vec2 uv, vec2 p1, vec2 p2,float T) {
  float a=abs(distance(p1, uv));
  float b=abs(distance(p2, uv));
  float c=abs(distance(p1, p2));
  if (a>=c||b>=c)return 0.0;
  float p=(a+b+c)*0.5;
  float h=2.0/c*sqrt(p*(p-a)*(p-b)*(p-c));
  return mix(1.0,0.0,smoothstep(0.5*T,1.5*T,h));
}



float drawSmoothCircle( vec2 uv, vec2 center,float radius){
	return 1.0 - smoothstep( radius-0.005, radius+0.005, length(uv-center));
}

vec3 Circles(vec2 uv, vec2 center,float radius,vec3 bgcolor){
	float tmp;
	vec3 col1 = vec3(1.0,0.0,0.0);
	vec3 col2 = vec3(1.0,1.0,1.0);
	vec3 pixl=bgcolor;
	vec2 anim;
	vec2 nc = center;
	float nr = radius;
	float rot;
	
	for(float i=1.0;i<=5.0;i+=1.0){
		rot=(mod(i,2.0)==0.0)?1.0:-1.0;
		anim=vec2(sin(rot*TIME),cos(rot*TIME))*0.25;
		tmp+=drawSmoothCircle(uv, nc, nr);
		pixl=(rot==-1.0)?mix(pixl, col1*cos(rot*TIME), drawSmoothCircle(uv, nc, nr)):mix(pixl, col2*sin(rot*TIME), drawSmoothCircle(uv, nc, nr));
		nr=radius/(i*1.5);
		nc+=(anim/(i*1.5));
	}
	
	return pixl;
}


void main( void ) {
	vec2 uv = 2.0*vec2(gl_FragCoord.xy-0.5*RENDERSIZE.xy)/RENDERSIZE.y;
	vec3 bcol = vec3(1.0);
	vec3 pixel = bcol;
	pixel*=Circles(uv,vec2(0.0,0.0),0.5,bcol);
	pixel+=beam(uv,vec2(-1.10,-1.10),vec2(-sin(TIME),-cos(TIME)),0.4);
	gl_FragColor = vec4(pixel, 1.0);
}