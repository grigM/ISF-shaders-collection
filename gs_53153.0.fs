/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#53153.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



void main() {
	vec2 uv = (2. * gl_FragCoord.xy - RENDERSIZE) / RENDERSIZE.y;
	vec3 col = vec3(0.);

	
	uv = abs(uv);
    	float d = dot(uv, vec2(1.3, 0.86));
    	d = max(d,.40/uv.x);
	
	d=sin(d)-.020/d-cos(d);
	
	d = abs(d - 0.8)*atan(d)*sin(d)*d*abs(sin(TIME));
	col += smoothstep(.015, .0, d);	
	gl_FragColor = vec4(col, 1.);
}