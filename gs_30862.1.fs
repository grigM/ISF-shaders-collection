/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "line_cross_w",
			"TYPE": "float",
			"DEFAULT":1.0,
			"MIN": 0.0,
			"MAX": 250.0
		},
		{
			"NAME": "line_rect_w",
			"TYPE": "float",
			"DEFAULT":1.0,
			"MIN": 0.0,
			"MAX": 150.0
		}
  ],
  "DESCRIPTION" : "Automa150ically converted from http:\/\/glslsandbox.com\/e#30862.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	float b = clamp((step(uv.x,uv.y)-step(uv.x,uv.y-(line_cross_w/RENDERSIZE.y)))+(step(1.-uv.x,uv.y)-step(1.-uv.x,uv.y-(line_cross_w/RENDERSIZE.y)))+float(uv.y<line_rect_w/RENDERSIZE.y||uv.y>1.-(line_rect_w/RENDERSIZE.y)||uv.x<line_rect_w/RENDERSIZE.x||uv.x>1.-(line_rect_w/RENDERSIZE.x)),0.,line_cross_w);
	gl_FragColor = vec4(vec3(b),1.);
}