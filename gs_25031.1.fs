/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#25031.1"
}
*/


//precision mediump float;


float circle(vec2 p, vec2 center) {
	float d = distance(p, center);
	return 1.0 - d;
}

void main( void ) {
	vec2 p = gl_FragCoord.xy / RENDERSIZE;
	p = 2.0 * p - 1.0;
	p.x *= RENDERSIZE.x / RENDERSIZE.y;
	
	float mask1 = circle(p, vec2(-0.25 + sin(TIME), 0));
	float mask2 = circle(p, vec2(0.25 - sin(TIME), 0));
	
	vec3 bg = vec3(0.0, 0.0, 0.0);
	vec3 color = mix(bg, vec3(1.0, 0.0, 0.0), mask1);
	color = mix(color, vec3(0.0, 0.0, 1.0), mask2);

	gl_FragColor = vec4(color, 1.0);

}