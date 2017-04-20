/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34973.5"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define PI 3.14159265359
#define TAU 6.28318530718
#define EPS 0.000001

#define SIDES floor(5.0 - 4.9*cos(0.2*TIME))

vec2 rotate(vec2 vec, float angle)
{
	return mat2(cos(angle), sin(angle), -sin(angle), cos(angle)) * vec;	
}

void main( void ) {

	vec2 p = ( gl_FragCoord.xy / RENDERSIZE.xy ) * 2.0 - 1.0;
	p.x /= RENDERSIZE.y / RENDERSIZE.x;
	
	float angle = floor((atan(p.y,p.x) + PI - EPS) / TAU * SIDES) / SIDES * TAU;
	float color = 0.0;
	p = rotate(p, (PI / SIDES * (SIDES-1.0)) - angle);
	color = sin(p.x*40.0-TIME*6.);
	gl_FragColor = vec4( vec3( color, color * 0.5, sin( color + TIME / 8.0 ) * .9 ), 77.0 );

}