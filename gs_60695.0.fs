/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60695.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#define PI2 6.28318530718

#extension GL_OES_standard_derivatives : enable


float getPhase(float t, float spd, float phase){
	return min(1.0,max(0.0,cos(TIME*spd+phase*PI2)*1.2));
}

float getSpike(vec2 p, float spikes, float t){
	float a = atan(p.y,p.x);
	return cos(a*spikes+t)*0.5+0.5;
}

void main( void ) {

	vec2 position = (( gl_FragCoord.xy / RENDERSIZE.xy ) -vec2(0.5,0.5))*vec2(RENDERSIZE.x/RENDERSIZE.y,1.0)*2.0;
	vec2 center = vec2(0.0,0.0);
	
	vec2 p = position;
	float t=TIME;
	
	float a = atan(p.y,p.x);
	float spikes = 12.0;
	float d = length(center-position);
	
	float spikeMod = 0.0;
	spikeMod += getSpike(p, 5.0, t*0.7)*(cos(t*0.4)*0.5+0.7)*0.1;
	spikeMod += getSpike(p, 3.0, t*0.3)*(cos(t*0.7)*0.5+0.7)*0.1;
	spikeMod += getSpike(p, 2.0, t*0.4)*(cos(t*1.4)*0.5+0.7)*0.1;
	spikeMod += getSpike(p, 12.0, t*5.1)*0.01;
	spikeMod += getSpike(p, 25.0, -t*3.2)*0.02;
	spikeMod += getSpike(p, 37.0, -t*2.3)*0.03;
	float modulator = 1.0;
	modulator += getSpike(p, 5.0, -t*0.2);
	modulator += getSpike(p, 7.0, t*0.3);
	modulator += getSpike(p, 13.0, -t*0.7);
	float light = pow((0.2+spikeMod)/d,modulator);
	
	float spd = 1.0;
	
	vec3 clr = vec3(getPhase(TIME, spd,0.0), getPhase(TIME, spd,1.0/3.0),getPhase(TIME, spd,2.0/3.0));
	clr = vec3(0.6)+clr*0.4;

	vec3 color = vec3(light*clr.r, light*clr.g, light*clr.b);
	
	if(position.x>1.0)color=vec3(0.0);
	if(position.x<-1.0)color=vec3(0.0);
	if(position.y>1.0)color=vec3(0.0);
	if(position.y<-1.0)color=vec3(0.0);

	gl_FragColor = vec4( color, 1.0 );

}