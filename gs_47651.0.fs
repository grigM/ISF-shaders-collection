/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#47651.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

//author: tigrou dot ind at gmail dot com
//13.06.2018 : optimization


float tri(vec2 pos)
{
	float a = -(pos.x * sqrt(3.0) -0.2) - pos.y;
	float b = pos.x * sqrt(3.0) - pos.y + 0.2;
	float c = pos.y + 0.1;
	
	float result = min(min(a, b), c);	
	return step(0.0, result);
}

float tri1(vec2 pos)
{
	return tri(pos)*2.0-1.0;
}

float tri2(vec2 pos)
{ 
	return tri(pos*4.0+vec2(sqrt(3.0)/5.0,-0.2)) + 
               tri(pos*4.0-vec2(sqrt(3.0)/5.0, 0.2)) + 
	       tri(pos*4.0-vec2(0.0,-0.4));
}

void main( void ) {

	vec2 pos = (2.0*gl_FragCoord.xy - RENDERSIZE.xy)/min(RENDERSIZE.x, RENDERSIZE.y);
	
	mat2 rot = mat2(cos(TIME),-sin(TIME), 
			sin(TIME), cos(TIME));
        pos *= rot;
	
	float zoom = 1.0/(mod(TIME, 1.0)+1.0)*3.0-1.0;
	pos *= zoom;
        
	vec2 invpos = vec2(pos.x, -pos.y);
	
	float result = tri1(pos*0.0625)
		      *tri1(invpos*0.125)
		      *tri1(pos*0.25)
		      *tri1(invpos*0.5)
		      *tri1(pos)
		      *tri1(invpos*2.0)
		      *tri1(pos*4.0)
		      *tri1(invpos*8.0)
		      *tri1(pos*16.0)
		      *tri1(invpos*32.0)
		      *tri1(pos*64.0);
	
	float result2 = tri2(pos*0.125)
		       +tri2(pos*0.5)
		       +tri2(pos*2.0)
		       +tri2(pos*8.0)
		       +tri2(pos*32.0);
	
        result = max(result, 0.0);	
	
	gl_FragColor = vec4( mix(vec3(0.2, 0.0, 0.2), vec3(0.0,0.0,0.0), result) + 
			     mix(vec3(0.0, 0.0, 0.0), vec3(0.2,0.2,0.0), result2), 1.0 );
}