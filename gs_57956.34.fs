/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57956.34"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float minimum_distance(vec2 v, vec2 w, vec2 p)
{
 	float l = distance(v, w);
	if (l == 0.0) 
	{
		return distance(p, v);
	}
 
	float t = clamp(dot(p - v, w - v) / (l * l), 0.0, 1.0);
	vec2 projection = v + t * (w - v); 
 
	return distance(p, projection);
}

float modify0(float x)
{
	return 1.0 - smoothstep(0.0, 1.5,pow(x + 0.9, 20.0));
}

float modify1(float x)
{
	return 1.0 - smoothstep(0.0, 1.5,pow(1.0 - abs(fract(TIME + x) - 0.5), 2.0));
}

#define modify modify0
			 
// dancing lines by @hintz
void main(void) 
{
	vec2 position = 2.0 * (gl_FragCoord.xy - 0.5 * RENDERSIZE.xy) / RENDERSIZE.yy;

	vec2 p1 = vec2(sin(TIME), sin(TIME * 1.5));
	vec2 p2 = vec2(sin(TIME*1.2), sin(-TIME * 1.34));
	vec2 p3 = vec2(-sin(TIME*1.01), sin(TIME * 1.51));
	vec2 p4 = vec2(-sin(TIME*1.21), sin(-TIME * 1.341));
	
	float c1 = modify(minimum_distance(p1, p2, position));
	float c2 = modify(minimum_distance(p3, p4, position));
	float c3 = modify(minimum_distance(p2, p3, position));
	float c4 = modify(minimum_distance(p4, p1, position));
	
	
	c3 -= c2 * c3;
	c2 -= c4 * c2;
	c4 -= c1 * c4;
	c1 -= c3 * c1;
	
	c2 -= c1 * c2;
	c3 -= c4 * c3;
	
	
	gl_FragColor = vec4(c1 + c4, c2 + c4, c3, 1.0);
}