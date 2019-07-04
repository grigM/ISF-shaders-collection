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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#4248.0"
}
*/


// Incoming Aliens
// Based on http://glsl.heroku.com/e#4190.1
//      and http://glsl.heroku.com/e#424.12

#ifdef GL_ES
precision mediump float;
#endif


#define GRID_SIZE 32
#define ALIEN_DENSITY 16.0
#define PI 3.1416

vec3 color(float d) {
	return d * vec3(0, 1, 0);	
}

int mod(int a, int b) {
	return a - ((a / b) * b);
}

bool alien(float x, float y) {
	if (x > 5.9999) {
		x = 11.0 - x;
	}

	if ((x < -0.0001) || (y < -0.0001)) {
		return false;
	} else if (y <= 1.9999) {
		return (x >= (1.9999 - y));
	} else if (y <= 3.9999) {
		return ((x < 1.9999) || (x >= 3.9999));
	} else if (y <= 7.9999) {
		return true;
	} else if (y <= 10.9999) {
		return ((x >= (10.9999 - y)) && (x <= (12.9999 - y)));
	}
	return false;
}

void main(void)
{
	vec2 p = (-1.0 + 2.0 * ((gl_FragCoord.xy) / RENDERSIZE.xy));
	//p -= (2.0 * mouse.xy) - vec2(1.0);
	p.x *= (RENDERSIZE.x / RENDERSIZE.y);
	vec2 uv;

	float a = (atan(p.y,p.x) + TIME * 4.0);
	float r = sqrt(dot(p,p));

	uv.x = 0.1/r;
	uv.y = a/(PI);
	
	float len = dot(p,p);
	
	vec3 col;
	if (len > 0.73) {
		col = vec3(0.0);
	} else if (len > 0.7) {
		col = vec3(0.8,0.8,0.8);
	} else {
		col = color(pow(fract(uv.y / -2.0), 5.0));
		
		float alienSize = min(floor(mod(TIME * 0.5, 30.0)) + 6.0, 20.0);
		vec2 alienID = floor(p * vec2(alienSize, -alienSize));
		float advance = mod(TIME * 0.5, 30.0);
		
		vec2 aPos = fract(p * vec2(alienSize, -alienSize)) * ALIEN_DENSITY;
		if (!alien(aPos.x, aPos.y) || (abs(alienID.y) + 6.0 - advance > (alienID.x)))
			col *= 0.3;
		
		bool grid_x_0 = mod(int(gl_FragCoord.x) - int(RENDERSIZE.x / 2.0), GRID_SIZE) == 0;
		bool grid_y_0 = mod(int(gl_FragCoord.y) - int(RENDERSIZE.y / 2.0), GRID_SIZE) == 0;
	
		if (grid_x_0 || grid_y_0)
			col += color(0.1);
	
		if (grid_x_0 && grid_y_0)
			col += color(0.3);
	}
	gl_FragColor = vec4(col, 1.0);
}