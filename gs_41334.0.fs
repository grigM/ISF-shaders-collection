/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#41334.0"
}
*/



//via http://glsl.herokuapp.com/e#4841.11

vec2 hash(vec2 v) 
{
	vec2 n;
	n.x = fract(cos(v.y - v.x * 841.0508) * (v.y + v.x) * 3456.7821);
	n.y = fract(sin(v.x + v.y * 804.2048) * (v.x - v.y) * 5349.2627);
	return n;
}


float partition_noise(vec2 p) 
{
	vec2 id;
	
	id = (floor(p)-.5);
	id *= id;
	
	p *= floor(hash(id) * 2.)+1.;
	id = floor(p);
	
	p.yx *= floor(hash(id) * 3.)-4.;
	id -= floor(p);

	p *= floor(hash(id) * floor(22.-22.*vec2(cos(TIME),sin(TIME))))+1.;
	id = floor(p);

	p -= id;

	vec2 u = abs(p - .5) * 2.;

	return max(u.x, u.y);
}


void main() 
{
	vec2 uv			= gl_FragCoord.xy/RENDERSIZE;
	vec2 aspect		= RENDERSIZE/min(RENDERSIZE.x, RENDERSIZE.y);

	
	vec4 result		= vec4(0., 0., 0., 1.);
	
	result 			+= partition_noise((vv_FragNormCoord.xy+0.5) * 5.);
	
	
	gl_FragColor = result;
}//sphinx