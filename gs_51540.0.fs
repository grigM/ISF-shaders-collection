/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#51540.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif



const float PI = 3.14159;
const float RADIUS = 50.;

const float CELL_SIZE = 4.;
const float CELL_THICK = 0.5;
const float CELL_RADIUS = .5*CELL_SIZE - CELL_THICK;

float
squared_distance(const in vec2 a, const in vec2 b)
{
	vec2 d = a - b;
	return dot(d, d);
}

float
add_blob(const in vec2 center)
{
	return 1.5e5/(1. + squared_distance(gl_FragCoord.xy, center));
}

void main(void)
{
	vec2 origin  = .5*RENDERSIZE;
	float v =
		add_blob(origin + vec2(150.*sin(2.1 + 1.6*TIME), 205.*cos(5.7 + 1.8*TIME))) +
		add_blob(origin + vec2(100.*cos(.5 + 2.1*TIME), 100.*sin(7.5 + 5.2*TIME))) +
		add_blob(origin + vec2(60.*sin(.3 + 1.6*TIME), 200.*cos(3.3 + 1.6*TIME)));	
	float w = v - RADIUS;
	
	float r = pow(1. + pow(w, 2.), -.2)*CELL_RADIUS;

	vec2 offs = vec2(step(CELL_SIZE, mod(gl_FragCoord.y, 2.*CELL_SIZE))*.5*CELL_SIZE, 0);
	vec2 cell_center = offs + CELL_SIZE*floor((gl_FragCoord.xy - offs)/CELL_SIZE) + .5*vec2(CELL_SIZE, CELL_SIZE);
	float d = distance(gl_FragCoord.xy, cell_center);
	float c = smoothstep(r + CELL_THICK, r, d);
	
	gl_FragColor = vec4(c, c, c, 1);
}