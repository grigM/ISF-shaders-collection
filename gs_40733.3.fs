/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40733.3"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main( void ) {

	float size = 0.05;
	float speed = 0.2;
	vec3 color1 = vec3(1., 0., 0.);
	vec3 color2 = vec3(1.);
	
	
	float dist = fract(TIME * speed);
	vec2 normalizedCoord = (gl_FragCoord.xy / RENDERSIZE.yy) - vec2(0.5);
	float coordMag = length(normalizedCoord);
	float level = step(distance(dist, coordMag),size);
	vec3 value = mix(color1, color2, level);
	gl_FragColor = vec4(value, 1.);
}