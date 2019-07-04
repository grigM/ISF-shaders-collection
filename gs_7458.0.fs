/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#7458.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif
 

uniform float depth;

#define PI 3.1415927
#define PI2 (PI*2.0)

// http://glsl.heroku.com/e#7109.0 simplified by logos7@o2.pl

// named some vars _knaut

void main(void)
{
	float depth = 100.0;
	float spinTime = 0.1;
	float spinTimeMultiply = .315;
	float stripeWidth = 0.003;
	
	float rC = 4.0;
	float gC = 0.5;
	float bC = 0.5;
	
	vec2 position = depth * ((2.0 * gl_FragCoord.xy - RENDERSIZE) / RENDERSIZE.xx);

	float r = 4.0*length(position);
	float a = atan(position.y, position.x);
	float d = r - a + PI2;
	float n = PI2 * float(int(d / PI2));
	float k = a + n ;
	

	for (int x = 0; x < 360; x++) {
		spinTime=+ (spinTimeMultiply * TIME);
	}
	
	float rand = sin(1.0004 * floor((0.001 + stripeWidth) * k * k + (spinTime)));

	gl_FragColor.rgba = vec4(fract((0.1*TIME)*rand*vec3(rC, gC, bC)), 1.0);
}