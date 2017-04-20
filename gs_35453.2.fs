/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
	{
			"NAME": "SPEED",
			"TYPE": "float",
			"DEFAULT": 40.0,
			"MIN": 0.0,
			"MAX": 150.0
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35453.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

vec2 get_sourcepoint(vec2 fragcoord, vec2 res, int xoffset, int yoffset)
{
	vec2 position = floor( 16.0*fragcoord.xy / res.xy ) + vec2(xoffset, yoffset);
	vec2 offset = vec2(rand(position), rand(2.0*position));
	
	return (position+offset)/16.0*res.xy;
}

void main( void ) {
	
	float d1=100000.0, d2=100000.0;
	vec2 tmp;
	
	vec2 fragcoord = vec2(gl_FragCoord.x,0.0)+vec2(0.0, TIME*SPEED);
	
	for (int i=-1; i<=1; ++i)
	{
		for (int j=-1; j<=1; ++j)
		{
			tmp = get_sourcepoint(fragcoord, RENDERSIZE.xy, i, j);
			if (length(fragcoord - tmp) < d1)
			{
				d2 = d1;
				d1 = length(fragcoord - tmp);
			}
			else if (length(fragcoord - tmp) < d2)
			{
				d2 = length(fragcoord - tmp);
			}
		}
	}
	
	if (abs(d1-d2) < 1.0)
	{
		gl_FragColor = vec4(1.0);
	}
	else
	{
		gl_FragColor = vec4(vec3(0.0), 1.0);
	}
}