/*
{
  "CATEGORIES" : [
    "Automatically Converted"
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36591.1"
}
*/


#ifdef GL_ES
precision highp float;
#endif


#define EPSILON 0.001

float distance_function(vec3 position) {
	vec3 f = abs(fract(position) - 0.5);
	vec3 g = vec3(
		max(f.y, f.z),
		max(f.z, f.x),
		max(f.x, f.y)
	);
	
	
	return min(g.x, min(g.y, g.z)) - 0.01;
}

void main( void ) {
	vec3 position = vec3(0, 0, 5);
	vec3 direction = normalize(vec3((gl_FragCoord.xy - RENDERSIZE / 2.0) / min(RENDERSIZE.x, RENDERSIZE.y), -1.0));
	
//	float thx = (mouse.x - 0.5) * 8.0;
//	float thy = (mouse.y - 0.5) * 4.0;
	float thx = TIME;
	float thy = TIME;
	
	mat3 ry = mat3(
		1,0,0,
		0,cos(thy),sin(thy),
		0,-sin(thy),cos(thy)
	);
	mat3 rx = mat3(
		cos(thx),0,sin(thx),
		0,1,0,
		-sin(thx),0,cos(thx)
	);
	
	direction = rx * ry * direction;
		
	vec3 color = vec3(0,0,0);
	
	for (int i = 0; i < 64; i++) {
		float closest = distance_function(position);
		position = position + direction * closest;
		if (closest < EPSILON) {
			color = vec3(1, 1, 0) * 1.0 / length(position + vec3(0, 0, -5));
			break;
		}
	}
	
	gl_FragColor = vec4(color, 1.0 );
}