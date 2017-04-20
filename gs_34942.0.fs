/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34942.0"
}
*/


#extension GL_OES_standard_derivatives : enable

#ifdef GL_ES
precision mediump float;
#endif


//NRLABS 2016



void main( void )
{	
	vec2 uv =  ((gl_FragCoord.xy - RENDERSIZE*0.5) * 1.0) / (RENDERSIZE.xy * 1.0);
	uv.x += sin(TIME*0.5)*0.2;uv.y += cos(TIME*0.5)*0.2;
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
 	
	float color = cos(uv.y*uv.y+uv.x*uv.x - (TIME*0.1))* 100.0;
	
	color = mod(color, 2.0)- cos(color);
	
	color -= cos(color);
	
	gl_FragColor = vec4( vec3( color, color,color), color );
}