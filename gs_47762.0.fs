/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "SPEED",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -8.0,
			"MAX": 8.0
			
		},
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#47762.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

//author: tigrou dot ind at gmail dot com
//30.06.2018 : try another method


float DEG2RAG = 0.0174532925199;


float tri2(vec2 pos)
{
	float a = dot(pos, vec2(sin( 60.0*DEG2RAG), cos( 60.0*DEG2RAG)));
	float b = dot(pos, vec2(sin(-60.0*DEG2RAG), cos(-60.0*DEG2RAG)));	
	float c = -pos.y;
	float d = 0.2*cos(60.0*DEG2RAG);
			
	float u1 = a - d*0.25;
	float v1 = b - d*0.25;
	float w1 = c - d*0.30;
	
	float u2 = c + d*1.25;
	float v2 = b + d*1.25;
	float w2 = a + d*1.25;
	
	float t1 = min(u2, min(u1, v1));
	float t2 = min(v2, min(w1, u1));
	float t3 = min(w2, min(v1, w1));
	
	float res = max(max(t1, t2), t3);
	return 0.002/abs(res);
}


float tri(vec2 pos)
{
	float a = dot(pos, vec2(sin( 60.0*DEG2RAG), cos( 60.0*DEG2RAG)));
	float b = dot(pos, vec2(sin(-60.0*DEG2RAG), cos(-60.0*DEG2RAG)));	
	float c = -pos.y;
				
	float result = 0.002/abs(min(min(0.05-a,0.05-b), 0.05-c));
	return result;
}

void main( void ) {

	vec2 pos = (2.0*gl_FragCoord.xy - RENDERSIZE.xy)/max(RENDERSIZE.x, RENDERSIZE.y) / 16.0;
	
	//rotate & zoom
	mat2 rot = mat2(cos(TIME*SPEED),-sin(TIME*SPEED), 
			sin(TIME*SPEED), cos(TIME*SPEED));
        pos *= rot;
	
	float zoom = 1.0/(mod((TIME*SPEED), 1.0)+1.0)*3.0-1.0;
	pos *= zoom;
	
	//effect
	vec3 result = vec3(0.0);
	for(int i = 0 ; i < 6 ; i++)
	{
		result += vec3(tri(pos), tri(-pos*0.5), tri2(pos));
		pos /= 0.25;
	}
	gl_FragColor = vec4( mix(vec3(0.0, 0.0, 0.0), vec3(0.5, 0.0,1.0), result.x)
			    +mix(vec3(0.0, 0.0, 0.0), vec3(0.5,0.25,0.0), result.y)
			    +mix(vec3(0.0, 0.0, 0.0), vec3(0.0,0.5,0.1), result.z), 1.0 );
}