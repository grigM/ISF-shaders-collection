/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#47552.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec2 brickTile(vec2 uv, float zoom)
{
	uv *= zoom;
	uv.x += step(1., mod(uv.y,2.0)) * -TIME;	
	
	return fract(uv);
		
}

vec2 brickTile2(vec2 uv, float zoom)
{
	uv *= zoom;
	
	float t = mod(uv.y,2.0) < 1. ? -TIME : TIME;
	
	uv.x += step(0., mod(uv.y,2.0)) * t;
	
	return fract(uv);
		
}

vec2 brickTile3(vec2 uv, float zoom)
{
	uv *= zoom;	
	
	float tx = mod(uv.y,2.0) < 1. ? -TIME : TIME;
	float ty = mod(uv.x,2.0) < 1. ? -TIME : TIME;
	
	float b = step(1.,mod(TIME,2.)); 
	
	if(b < 1.)
	{			
		uv.x += step(0., mod(uv.y,2.0)) * tx;	
		
	}
	else
	{	
		uv.y += step(0., mod(uv.x,2.0)) * ty;		
	}
	
	
	return fract(uv);
		
}
	

void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );
	
	position.x *= RENDERSIZE.x / RENDERSIZE.y;
	
	//position = brickTile(position,5.);
	//position = brickTile2(position,10.);
	position = brickTile3(position,10.);
	
	float d = distance(position,vec2(.5));
	
	float radius = 0.2;
	
	float s = smoothstep(radius,d + radius, d);
	
	
	vec3 color = vec3(s * 5.);
	color = mix(color,1.- color,sin(TIME) + 1.);	
	
	gl_FragColor = vec4( color , 1.0 );

}