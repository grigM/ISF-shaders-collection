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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#52978.0"
}
*/


// Windows 10 Calendar NO DIGITS HAHA

#ifdef GL_ES
precision highp float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

	vec2 p = gl_FragCoord.xy;
	vec2 m = mouse * RENDERSIZE;

	float noise_delta = 0.02;
	
	float color = 0.09 + fract((sin(12.9898 * p.x + 78.233 * p.y)) * 43758.5453) * noise_delta;
	
	float b_color_delta = 0.20 - color;
	float b_selected_delta = 0.2;
	
	float w = 55.0;
	float h = 45.0;
	float b = 3.0;
	float d = 1.0;
	
	float r = 70.0;
	
	float x2 = mod(p.x, w);
	float y2 = mod(p.y, h);
	
	float dist = distance(p, m);
	float near = max(r - dist, 0.0) / r;
	
	if ( x2 > d && x2 < w - d && y2 > d && y2 < h - d && !( x2 > d + b && x2 < w - d - b && y2 > d + b && y2 < h - d - b ) ) {
		if (floor(m.x / w) == floor(p.x / w) && floor(m.y / h) == floor(p.y / h)) {
			color += b_color_delta + b_selected_delta;
		} else {
			color += b_color_delta * pow(near, 0.5);
		}
	}
	if ( x2 > d + b && x2 < w - d - b && y2 > d + b && y2 < h - d - b ) {
		if (floor(m.x / w) == floor(p.x / w) && floor(m.y / h) == floor(p.y / h)) {
			color += b_color_delta * pow(near, 1.0);
		}
	}
	
	
	gl_FragColor = vec4( vec3( color ), 1.0 );

}