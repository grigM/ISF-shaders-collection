/*{
	"CREDIT": "by isak.burstrom",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"INKA", "Stylize", "Generator"
	],
	"INPUTS": [ 
	 	{
			"NAME": "inputImage",
			"TYPE": "image"
		},
	 	{
			"NAME": "pos",
			"TYPE": "float",
			"DEFAULT": 0.5
		},{
			"NAME": "shape1",
			"TYPE": "float",
			"DEFAULT": 0.5
		}, {
			"NAME": "shape2",
			"TYPE": "float",
			"DEFAULT": 0.5
		}, {
			"NAME": "dist",
			"TYPE": "float",
			"DEFAULT": 0.5
		}, {
			"NAME": "size",
			"TYPE": "float",
			"DEFAULT": 0.5
		}, {
			"NAME": "width",
			"TYPE": "float",
			"DEFAULT": 0.8
		},{
			"NAME": "smooth",
			"TYPE": "float",
			"DEFAULT": 1.0
		}, {
			"NAME": "fade",
			"TYPE": "float",
			"DEFAULT": 0.9
		},  {
			"NAME": "color",
			"TYPE": "color",
			"DEFAULT":  [
				1.0,
				0.0,
				0.0,
				1.0
			]
		}
	]
}*/

#define SIZE 4.0
#define TWOPI 6.283185308
#define PI 3.141592654
// simply broke out and customized the shape generator from:
// https://www.shadertoy.com/view/Ms3SzB
// and: http://glslsandbox.com/e#35580.1

void main() {
	float A, b, v;
	vec4 O;
	float h = RENDERSIZE.y;
	vec2 U = 2. * (gl_FragCoord.xy - 0.5 * RENDERSIZE.xy) / RENDERSIZE.y;

	//U = 2. * fract(U) -1.;  // fractions
	
	float a = atan(U.y, U.x); // polar coordinates
	float r = length(U);
	
	
	for(int i=0; i<4; i++) {
		A = ceil(SIZE * 2. * shape1) / ceil(SIZE *  shape2) * a + pos * TWOPI;
		b = smoothstep(1.1 - width, ((1.1 - 120. / h) - width) * smooth, 8. * abs(r - (dist * .33) * sin(A) - size + (dist * .33)));
		v = max(v, ( 1. + fade * cos(A) ) / 1.8 * b);				 //1// cos(A) = depth-shading
		a += TWOPI;         										// next turn
	}
	
	O = v * color;
	gl_FragColor = vec4(vec3(O), 1.0); 
}

