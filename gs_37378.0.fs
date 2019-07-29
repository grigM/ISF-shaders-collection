/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#37378.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif
#extension GL_OES_standard_derivatives : enable

#define dist(x,y) (sqrt(x*x + y* y))

vec3 hsv2rgb(vec3 c) {//https://github.com/hughsk/glsl-hsv2rgb
  vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
  vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
  return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}



void main( void ) {
	vec2 p = ( gl_FragCoord.xy );
	/*p = p * 2.0;
	p.x -= RENDERSIZE.x;
	p.y -= RENDERSIZE.y;
	//p.x *= RENDERSIZE.x / RENDERSIZE.y;
	*/
	
	float color = 
		(sin(p.x/125.0-TIME)+1.0)
	      + (sin(p.y/125.0-TIME)+1.0)
	      + (sin(p.x+p.y)+1.0)
		;
	
	color = mod(color,4.0)+TIME;
	
	color = color/0.55;
	
	vec3 hsv = hsv2rgb(vec3(color,1,1));
	gl_FragColor = vec4( hsv , 1.0 );	
}