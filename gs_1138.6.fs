/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#1138.6"
}
*/


#ifdef GL_ES
precision mediump float;
#endif
#define pi 3.141592653589793238462643383279


// How fast it animates
float tscale = 1.5;

float wave(vec2 position, float freq, float height, float speed) {
	float result = sin(position.x*freq - TIME*tscale*speed);
	result = result * 2.0 - 1.0;
	result *= height;
	return result;
}

vec3 combo(vec2 position, float center, float size) {
	
	float offset = pi * (center - 0.5);
	float lum   = abs(tan(position.y * pi + offset)) - pi/2.0;
	lum *= size;
	
        float red   = wave(position, 5.0, 0.9*size, 1.008);
	float green = wave(position, 3.5, 0.5*size, 2.23);
	float blue  = wave(position, 1.5, 1.2*size, 1.42);
	
	return vec3( lum + red, lum + green, lum + blue );
}

void main( void ) {
	// normalize position
	vec2 position = gl_FragCoord.xy / RENDERSIZE.xy;
	
	vec3 result = vec3(0.0, 0.0, 0.0);
	result += combo(position, 0.1, 0.05);
	result += combo(position, 0.5, 0.25);
	result += combo(position, 0.9, 0.05);

	gl_FragColor = vec4(result, 1.0);

}