/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60040.0"
}
*/



#ifdef GL_ES
precision mediump float;
#endif


vec2 position;
vec3 color;

float barsize = .2;

const float b=0.2;

float hf( float h )
{
	return clamp((abs(fract(h*b) * 6. -3.) -1.0), 0.0, 1.0);
}
vec3 mixcol(float value, float r, float g, float b)
{
	return vec3(value * r, value * g, value * b);
}
void bar(float pos, float r, float g, float b)
{
	 if ((position.y <= pos + barsize) && (position.y >= pos - barsize))
		color = mixcol(1.0 - abs(pos - position.y) / barsize, r, g, b);
}
void main( void )
{
	position = ( gl_FragCoord.xy / RENDERSIZE.xy );
	position = position * vec2(4.0) - vec2(2.0,2.0); 	
	float t = TIME;
	position.y =position.x*cos(t)+position.y*sin(t);
	color = vec3(0., 0., 0.);
	for(float i=-10.2;i<10.2;i+=0.4)
	{
		bar(i+0.4,  hf((position.y)+t), hf((position.y+.33)+t), hf((position.y+.66)+t));
	}
	gl_FragColor = vec4(color+(color-0.8), 1.0);
}