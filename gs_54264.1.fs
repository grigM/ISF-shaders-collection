/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54264.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


// hash no sine
float hash(vec2 p) {
	p = fract(p)-vec2(0.5,0.5);
	p += dot(p, p / sin(TIME)*10.0);
	return abs(fract((p.x/p.y)/abs(p.x+cos(TIME)+sin(-TIME))));
}

void main() {
	vec2 uv = ( gl_FragCoord.xy - RENDERSIZE) / RENDERSIZE.y;
	uv.x += TIME*0.2;
	
	vec3 col = vec3(hash(uv));
	gl_FragColor = vec4(col.x,col.y,col.x, 1.);
}