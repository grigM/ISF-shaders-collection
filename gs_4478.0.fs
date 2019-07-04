/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#4478.0",
  "INPUTS" : [

  ],
  "PERSISTENT_BUFFERS" : [
    "bb"
  ],
  "PASSES" : [
    {
      "TARGET" : "bb",
      "PERSISTENT" : true
    }
  ]
}
*/


#ifdef GL_ES
precision mediump float;
#endif


//dam u str8 babygurl
//~MrOMGWTF

float noise(vec2 p) {
	p=(p);
	return abs(fract(sin(p.x*45.11+p.y*97.23)*878.73+733.17)*2.0-1.0);
}
float wave(vec2 uv, float speed)
{
	return 1.0 - max(0.0,abs((sin(uv.x - TIME * speed) + 1.0) * 0.5 * 1.0 + noise(vec2(sin(uv.y), cos(uv.y))) * 10.0 - uv.y));	
}

void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );
	float rotate = sin(TIME) * 0.00;
	mat2 rot = mat2(
		vec2(cos(rotate), -sin(rotate)),
		vec2(sin(rotate), cos(rotate))
		);
	position = rot * position;
	
	vec3 color = vec3(0.0);
	
	color += vec3(1.0) * max(0.0,(wave((vec2(position.x, position.y - 0.1) * 1.5) * 10.0, 10.0) - 0.9) * 10.0);
	color += vec3(20.8) * max(0.0,(wave((vec2(position.x, position.y - 0.2) * 2.0) * 10.0, 10.0) - 0.9) * 10.0);
	color += vec3(20.6) * max(0.0,(wave((vec2(position.x, position.y - 0.3) * 2.5) * 10.0, 10.0) - 0.9) * 10.0);
	color += vec3(20.5) * max(0.0,(wave((vec2(position.x, position.y - 0.4) * 3.0) * 10.0, 10.0) - 0.9) * 10.0);
	color += vec3(10.3) * max(0.0,(wave((vec2(position.x, position.y - 0.5) * 3.5) * 10.0, 10.0) - 0.9) * 10.0);
	color += vec3(10.2) * max(0.0,(wave((vec2(position.x, position.y - 0.55) * 4.0) * 10.0, 10.0) - 0.9) * 10.0);
	color += vec3(10.1) * max(0.0,(wave((vec2(position.x, position.y - 0.6) * 5.5) * 10.0, 10.0) - 0.9) * 10.0);
	
	color += IMG_NORM_PIXEL(bb,mod(position,1.0)).xyz * 0.85;
	
	color = pow(color, vec3(sin(position.x + 1.0), 1.1, 0.9));
	
	gl_FragColor = vec4( color, 1.0 );

}