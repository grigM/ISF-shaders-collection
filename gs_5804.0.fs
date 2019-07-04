/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#5804.0"
}
*/


// @author germangb

#ifdef GL_ES
precision mediump float;
#endif


float seed = 33.0;
float random () {
	seed = mod((13821.0 * seed), 32768.0);
	return mod(seed, 2.0)+1.0;
}

void main( void ) {

	vec2 pixel_pos = gl_FragCoord.xy;
	vec3 color1 = vec3(0.7, 0.25, 2.25*sin(TIME));
	vec3 color2 = vec3(0.25, 0.7, 0.5*cos(TIME));
	vec3 color3 = vec3(0.5*sin(TIME), 0.25, 0.7);
	
	vec3 final_color = vec3(0.05*cos(TIME), 0.025*cos(TIME*0.5), 0.05*sin(TIME));
	for (int i = 0; i < 9; ++i) {
		vec2 center = RENDERSIZE/2.0 + vec2(sin(0.1 * float(random()) *TIME * 3.0 + pow(2.0, float(i*2)))*100.0, cos(0.05 * (float(i)-2.0) * TIME) * 100.0);
		float dist = length(pixel_pos-center);
		float intensity = pow((8.0 + 8.0 * mod(float(i), 2.5))/dist, 2.0);
		
		if (mod(float(i), 3.0) == 0.0)
			final_color += color1 * intensity;
		else if (mod(float(i), 3.0) == 1.0)
			final_color += color2 * intensity;
		else
			final_color += color3 * intensity;
	}


	gl_FragColor = vec4(final_color * abs(sin(TIME * 0.25) + 1.5)*mod(gl_FragCoord.y, 2.0)*mod(gl_FragCoord.x, 2.0), 1);
}