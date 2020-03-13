/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57096.0"
}
*/


//DS 4 u
#ifdef GL_ES
precision highp float;
#endif


void main()
{
	vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
	vec2 f = vec2(5.0);
	vec3 c = vec3(0.1,0.5,0.1);
	float light = 0.1;
	
	for (float x = 0.0; x < 5.0; x += 1.0)
	{
		
		f = vec2(sin(sin(TIME + uv.x * x) - uv.y * dot(vec2(x + uv.y), vec2(sin(uv.y*x), cos(x)))));
		f*=length(uv);
		light += (0.04 / distance(uv, f)) - (0.01 * distance(vec2((sin(TIME + uv.y))), vec2(uv)));
		c.y = sin(TIME + x*x) * 0.1 + 0.5;
	}
	
	c *= light;
	
	c.x *= 0.6;
	c.y *= 0.6;
	c.z *= 6.1;
	
	
	gl_FragColor = vec4(c, 1.0);
}