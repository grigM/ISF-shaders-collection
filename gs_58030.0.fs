/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#58030.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

#define PI 3.14159265358979323
#define FADE(a, b, mu) (((a) * (mu)) + ((b)*(1.0-(mu))))

vec3 hsv2rgb(vec3 c)
{
   vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
   vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
   return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

float npoly(float n, float theta) {
	float u = mod(PI * TIME * 0.05 + theta / (2. * PI), 1.) * (n); 
	//float u = theta / (PI * 2.) + 0.1;
	float a = PI / n;
	float b = a * (PI * floor(u) + 1.);
	float c = 2. * u - 2. * floor(u) - 1.;
	float x = cos(a) * cos(b) - c * sin(a) * sin(b);
	float y = cos(a) * sin(b) + c * sin(a) * cos(b);
	return length(vec2(x, y));
}

void main(void) {
	const int num = 3;
	vec4 layers[5];
	layers[0] = vec4( 6.0, 2.0, 0.49, 0.4);
	layers[1] = vec4( 6.0, 4.0, 0.59, 0.7 );
	layers[2] = vec4( 6.0, 7.0, 0.69, 0.9 );
	layers[3] = vec4( 6.0, 10.0, 0.79, 0.9 );
	layers[4] = vec4( 6.0, 14.0, 0.89, 0.9 );
	
	vec3 color = hsv2rgb(vec3(0.5 + 0.5 * mouse.x, 0.95, 0.5 + 0.5 * mouse.y));	

	vec2 center = (gl_FragCoord.xy - RENDERSIZE * 0.5) / min(RENDERSIZE.x, RENDERSIZE.y);

	vec3 mixed = vec3(1., 1., 1.);
	for (int i = 0; i < num; ++i) {
		vec2 scroll = center + vec2(mouse.x * 0.5 * float(i+1) + TIME * 0.2 * float(i), TIME * 0.1 * layers[i].w);
	
		vec2 p = mod((1. + scroll) * layers[i].y, 2.) - 1.;
		vec2 q = floor((scroll) * layers[i].y) /2. / layers[i].y;
	
		float r = length(p);
		float angle = atan(p.x, p.y);
		vec4 col = vec4(0., 0., 0., 0.);
	
		if (r < npoly(layers[i].x, angle) * 0.70) col = vec4(hsv2rgb(vec3(q.x + q.y, 1., 1.)), layers[i].z);
		//else if (r < npoly(layers[i].x, angle) * 0.95) col = vec4(color, 0.95); 
		
		mixed = mix(mixed, col.rgb, col.a);
	}
	
	float r = clamp(length(center) * RENDERSIZE.y / RENDERSIZE.x, 0., 1.);
	//mixed = pow(mixed, pow(1.-r, 1.8));
	gl_FragColor = vec4(1.-mixed, 1.);
}