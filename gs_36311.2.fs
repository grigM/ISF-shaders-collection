/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
	{
			"NAME": "SPEED",
			"TYPE": "float",
			"DEFAULT": 20.0,
			"MIN": 0.0,
			"MAX": 100.0
	},
	{
			"NAME": "scale",
			"TYPE": "float",
			"DEFAULT": 0.05,
			"MIN": 0.0,
			"MAX": 0.06
	},
	{
			"NAME": "rotationAmp",
			"TYPE": "float",
			"DEFAULT": 0.03,
			"MIN": 0.0,
			"MAX": 2.0
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36311.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

#define PI 3.1415926535


mat2 rotate(float t)
{
	return mat2(cos(t), sin(t), -sin(t), cos(t));
}

void main( void ) {

	vec2 pos = ( gl_FragCoord.xy - RENDERSIZE.xy / 2.0 ) / RENDERSIZE.y;

	float color = 0.0;
	vec2 id = floor(pos / 0.2);
	
	float speed;
	
		speed = sin(TIME + id.y * SPEED) * 1.0;
		
		pos.x += speed;
		id = floor(pos / 0.2);
		pos = mod(pos, 0.2) - 0.1;
		
		if (mod(id.x, 2.0) < 1.0)
		{
			pos = rotate(TIME * -1.0 * speed * rotationAmp  + id.x) * pos;
		}
		else
		{
			pos = rotate(TIME * -1.0 * speed * rotationAmp * 1.0 + id.x) * pos;
		}
		
	
	if (dot(pos, rotate(0.0) * vec2(0, -1)) < scale
	   && dot(pos, rotate(PI / 3.0 * 2.0) * vec2(0, -1)) < scale
	   && dot(pos, rotate(PI / 3.0 * 4.0) * vec2(0, -1)) < scale)
	{
		color = 1.0 - smoothstep(0.0001, 5.0, 0.0001);
	}

	
	gl_FragColor = vec4(color);

}