/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#53630.0"
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


vec3 cv( float cc )
{
	vec3 rgb = clamp( abs(mod(cc*6.0+vec3(0.0,4.0,2.0),6.0)-3.0)-1.0, 0.0, 1.0 );
	rgb = rgb*rgb*(3.0-2.0*rgb);
	return rgb;
}
float hash(vec2 p)
{
	return fract(4346.45 * sin(dot(p, vec2(45.45, 757.5))));
}

void main()
{
	vec2 uv = (2. * gl_FragCoord.xy - RENDERSIZE) / RENDERSIZE.y;
	
	uv.x *= 1.0+sin(TIME*0.5+(uv.y*0.8))*0.5;
	uv.y *= 1.0+sin(TIME*0.25+(uv.x*0.5))*0.5;
	
	vec3 col = vec3(0.5);
	
	uv *= 12. + sin(TIME*0.5)*2.0;
	
	uv *= rotate(TIME*0.2);
	uv -= TIME*1.5;
	
	vec2 i = floor(uv);
	vec2 f = fract(uv) - .5;
	
	
	f *= rotate(floor(hash(i) * 18.) * 3.14 / 2.);
	
	
	float d = dot(f, vec2(1.0));
	col += smoothstep(.015, .0, d);
	
	//col.b *= hash(i);
	//col.g *= hash(i*2.0);
	//col.r *= hash(i*4.0);
	col *= cv(hash(i));
	
	gl_FragColor = vec4(col, 1.);
}