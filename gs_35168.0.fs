/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
	{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 0.21,
			"MIN": 0.0,
			"MAX": 2.0
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35168.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main(void){
	vec2 v = (gl_FragCoord.xy - RENDERSIZE/2.0) / min(RENDERSIZE.y,RENDERSIZE.x) * 20.0;
	vec2 vv = v; vec2 vvv = v;
	float tm = 03.03+TIME*speed;
	vec2 mspt1 = (vec2(
			sin(tm)+cos(tm*0.2)+sin(tm*0.5)+cos(tm*-0.4)+sin(tm*1.3),
			cos(tm)+sin(tm*0.1)+cos(tm*0.8)+sin(tm*-1.1)+cos(tm*.5)
			)+1.0)*0.1; //5x harmonics, scale back to [0,1]
	
	vec2 mspt2 = (vec2(
			sin(tm)+cos(tm*0.2)+sin(tm*0.5)+cos(tm*-0.4)+sin(tm*3.),
			cos(tm)+sin(tm*0.1)+cos(tm*0.8)+sin(tm*-1.1)+cos(tm*.1)
			)+1.0)*0.12; //5x harmonics, scale back to [
	
	vec2 mspt = (mspt1*mspt2.x);
	
	float R = 0.0;
	float RR = 0.0;
	float RRR = 0.0;
	float a = (.6-mspt.x)*6.2;
	float C = cos(a);
	float S = sin(a);
	vec2 xa = vec2(C, S*S);
	vec2 ya = vec2(-S, C);
	vec2 shift = vec2( 0, 1.62);
	float Z = 1.0 + mspt.y*7.0;
	//float ZZ = 1.0 + (mspt.y)*6.2;
	//float ZZZ = 1.0 + (mspt.y)*6.9;
	
	for ( int i = 0; i < 40; i++ ){
		float r = dot(v,v);
		if ( r > 1.0 )
		{
			r = (1.0)/r ;
			v.x = v.x * r;
			v.y = v.y * r;
		}
		R *= .99;
		R += r;
		if(i < 39){
			RR *= .99;
			RR += r;
			if(i < 38){
				RRR *= .99;
				RRR += r;
			}
		}
		
		v = vec2( dot(v, xa), dot(v, ya)) * Z + shift;
	}
	float c = ((mod(R,2.0)>1.0)?1.0-fract(R):fract(R));
	float cc = ((mod(RR,2.0)>1.0)?1.0-fract(RR):fract(RR));
	float ccc = ((mod(RRR,2.0)>1.0)?1.0-fract(RRR):fract(RRR));
	gl_FragColor = vec4(ccc, cc, c, 1.0); 
}