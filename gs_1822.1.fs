/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#1822.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


vec2 lerp(vec2 a, vec2 b, float v) {
	return a + (b - a) * v;
}

void main( void ) {

	vec2 pos = ((( gl_FragCoord.xy / ( RENDERSIZE.xy * 2.0 ) ) * 6.0 - 2.0 ) / ( gl_FragCoord.xy * sin(TIME/24.0) ));
	pos = lerp(vec2(sqrt(dot(pos, pos)), atan(pos.y, pos.x)), pos, abs(sin(TIME/24.0)));
	float color = (mod(pos.x, 0.1) < 0.05 ? mod(pos.x, 0.1) : 1.0-mod(pos.x, 0.1)) + (mod(pos.x, 0.1) < 0.05 ? mod(pos.y, 0.1) : 1.0-mod(pos.y, 0.1));
	gl_FragColor = vec4(color, color, color, 1.0 );

}