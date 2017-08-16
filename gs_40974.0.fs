/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40974.0"
}
*/


#ifdef GL_ES
precision lowp float;
#endif

#extension GL_OES_standard_derivatives : enable


#define ZOOM 6.

#define POWER 2.0
#define POSTERIZE_SIZE 10.
#define POSTERIZE_NUM  2.

vec2 rotate(vec2 p, float r)
{
	return 	p * mat2(
		cos(r), -sin(r),
		sin(r),  cos(r)
		 );
}

float graphFunction0(vec2 p)
{	
	//return pow(pow(p.x, POWER) + pow(p.y, POWER), 1. / POWER);
	
	return cos(p.x + p.y - (TIME/4.) + cos(p.x*p.y - 8.*sin(TIME/3.)));
}

float graphFunction1(vec2 p)
{
	//return 1. + cos(p.x * p.y + TIME) - sin(p.x - p.y - TIME);
	
	return cos(p.x) - tan(p.y) + sin(p.x - TIME/7.)*0.2;
}

vec3 getColorAt(vec2 p)
{	
	p = rotate(p, TIME/10.);
	
	float value0 = graphFunction0(p);
	float value1 = graphFunction1(p);
	
	float dist = abs(value0 - value1);
	float value = 1. - floor(dist * POSTERIZE_SIZE) / POSTERIZE_NUM;
	
	return vec3(value * asin(value + value1 + p.x), value * sqrt(value1 + (sin(TIME + p.y)+1.)/2.), value / tan(acos(value + sin(p.x - TIME*3.)) - value0 / value1 + TIME));
}

void main( void )
{
	vec2 uv = (gl_FragCoord.xy / RENDERSIZE) - 0.5;
	uv.x *= RENDERSIZE.x / RENDERSIZE.y;
	
	gl_FragColor = vec4(getColorAt(uv * ZOOM), 1);
}