/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "AMOUNT_1",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 50.0
			
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54364.0"
}
*/


#ifdef GL_ES
precision highp float;
#endif

#extension GL_OES_standard_derivatives: enable


float hash(float x) {
    return fract(sin(x * 133.3) * 13.13);
}

void main(void) {
    vec2 RENDERSIZE = vec2 (500.0);
    vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
    vec3 c = vec3(0.6, 0.7, 0.8);

    float a = 0.5;
float t = (-1.+3.*cos(TIME*0.25+cos(TIME*0.0789)*0.1+uv.x*0.01)) / 8. * 3.14;
	
    float si = sin(a +t);
    float co = cos(a + t);
	float TIME = TIME + 1.*si*sin(co*3.14);

    uv *= mat2(co, -si, si, co);
//    uv *= length(uv + vec2(0.,4.9)) * .3 + 1.;

    float v = 1.0 - sin(hash(floor(uv.x * 100.0)) * 2.0);
    float b = clamp(abs(sin(20. * TIME * .75 * v + uv.y * (5.0 / (2.0 + v)))) - .95, 0., 1.) * 20.;
    c *= v * b;
    gl_FragColor = vec4(c, 1.0);

}