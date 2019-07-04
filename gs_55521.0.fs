/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#55521.0"
}
*/


#ifdef GL_ES
precision highp float;
#endif

#extension GL_OES_standard_derivatives : enable



mat2 rotate(float a)
{
	float c = cos(a);
	float s = sin(a);
	return mat2(c, s, -s, c);
}

float hash(vec2 p)
{
	return fract(4346.45 * sin(dot(p, vec2(45.45, 757.5))));
}

void main()
{
	vec2 uv = (2. * gl_FragCoord.xy - RENDERSIZE) / RENDERSIZE.y;
	vec2 uv2 = uv;
	
	
	vec3 col = vec3(0.0);
	
	uv *= 7. + sin(TIME)*2.0;
	
	uv *= rotate(TIME*0.1);
	uv += TIME*2.0;
	
	vec2 i = floor(uv);
	vec2 f = fract(uv)*sin(uv) - .5*cos(TIME);
	
	
	f *= rotate(floor(hash(i) * 18.) * 3.14 / 2.);
	
	
	float d = dot(f, vec2(1.0));
	col += smoothstep(.015, .0, d);
	
	col.b *= hash(i);
	col.g *= hash(i*2.0);
	col.r = hash(i*4.0);
	
	
	col*=1.0-sin(abs(uv2.x-uv2.y));
	
	gl_FragColor = vec4(col, 1.);
}