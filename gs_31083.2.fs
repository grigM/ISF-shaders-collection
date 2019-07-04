/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "ofset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 0.5
		},
		{
			"NAME": "amp",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "line_thick",
			"TYPE": "float",
			"DEFAULT": 0.01,
			"MIN": 0.01,
			"MAX": 0.09
		},
		{
			"NAME": "zoom_x",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.05,
			"MAX": 1.0
		},
		{
			"NAME": "zoom_y",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.05,
			"MAX": 1.0
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#31083.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



void main( void ) {
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	
	// distortion
	uv += (uv - 0.5) * (1.0 + 0.25 * sin(((TIME*speed)-ofset))) * pow(1.0 - length(uv - 0.5), 1.0 + amp * sin(((TIME*speed)-ofset) * 10.0));
	
	// grid
	float color = step(mod(uv.x, zoom_x), line_thick) + step(mod(uv.y, zoom_y), line_thick);
	
	gl_FragColor = vec4(color);

}