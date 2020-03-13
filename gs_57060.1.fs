/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57060.1"
}
*/



// https://www.shadertoy.com/view/tljSWV

#ifdef GL_ES
precision mediump float;
#endif


mat2 rotate(float a)
{
	float c = cos(a);
	float s = sin(a);
	return mat2(c, s, -s, c);
}
void main()
{
	vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
	
	uv *= 1.25+sin(uv.y+TIME*0.9)*0.5;
	
	uv.x *= abs(uv.x);
	uv.y *= abs(uv.y);
	vec2 f = vec2(0.3);
	vec3 c = vec3(1.0,1.0,1.0);
	float light = 0.1;
	
	for (float x = 1.1; x < 10.0; x += 1.0)
	{
		uv *= rotate(x*200.0+sin(TIME*0.1));
		
		f = vec2(cos(cos(TIME*0.6+x + uv.x * x) - uv.y * dot(vec2(x + uv.y), vec2(sin(x), cos(x)))));
		light += (0.04 / distance(uv, f)) - (0.01 * distance(vec2((cos(TIME*0.3 + uv.y))), vec2(uv)));
		
		c.y += sin(x+TIME+abs(uv.y))*0.3;
		if (c.y<0.8)
			c.y = 0.8;
		light-=x*0.001 + c.y*0.001;
		
	}
	
	c *= light;
	c.x += (sin(TIME*2.4)*0.1);
	
	gl_FragColor = vec4(c, 1.0);
}