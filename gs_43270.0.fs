/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43270.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


// MODS BY NRLABS



float nrand (vec2 co)
{	
	float a = fract(cos(co.x * 8.3e-3 + co.y) * 4.7e5);
   	float b = fract(sin(co.x * 0.3e-3 + co.y) * 1.0e5);
	return a * .5 + b * .5;
}

float genstars (float starsize, float density, float intensity, vec2 seed)
{
	float rnd = nrand(floor(seed * starsize));
	float stars = pow(rnd,density) * intensity;
	return stars;
}
vec3 White = vec3(1,1,1);
void main (void)
{
	vec2 offset = vec2(TIME * 8.25,0);
	
	vec2 p = 2.0 * (gl_FragCoord.xy / RENDERSIZE) - 1.0;
	p.x *= RENDERSIZE.x / RENDERSIZE.y;
	
	p *= 350.0;
		
	float intensity = genstars(0.025, 16.0, 2.5, p + offset * 40.);
	intensity += genstars(0.05, 16.0, 1.5, p + offset * 20.);
	intensity += genstars(0.10, 16.0, 0.5, p + offset * 10.);
	
	gl_FragColor = vec4(intensity * White, 1);
}