/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#55246.0"
}
*/


//Edit @ToBSn

#ifdef GL_ES
precision mediump float;
#endif
 

float getValue(vec2 p, float x2, float y2, float cmin, float cmax)
{
  float x=p.x;
  float y=p.y;
  float theta=dot(y-y2, x2);
  vec2 d = vec2(-cos(theta * x2), cos(theta - cmin));
  d *= sin(TIME * 0.1) * (cmax-cmin) + cmin;
  d += vec2(y, x) + length(p) * distance(y,x);
  d *= vec2(x, y) * cos(p * TIME * 0.1) * dot(x,y);
  return length(d-p);
}

void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy ) * 2.0 - vec2(1.0,RENDERSIZE.y/RENDERSIZE.x);
  	position = position * 5.0 ;

  	float a = 0.0;
  	a += getValue(position, sin(TIME)*.1, cos(TIME)*.2, 5.3, 0.8);
    	a *= getValue(position, sin(TIME)*.1, cos(TIME)*.2, 5.3, 0.8);
  	a += getValue(position, sin(TIME)*.1, cos(TIME)*.2, 5.3, 0.8);
  	a /= getValue(position, sin(TIME)*.1, cos(TIME)*.2, 5.3, 0.8);
  	a = 1./a;
  
  	float b = 0.0;
  	b += getValue(position, cos(TIME)*.1, sin(TIME)*.2, 5.3, 0.8);
    	b *= getValue(position, cos(TIME)*.1, sin(TIME)*.2, 5.3, 0.8);
  	b += getValue(position, cos(TIME)*.1, sin(TIME)*.2, 5.3, 0.8);
  	b /= getValue(position, cos(TIME)*.1, sin(TIME)*.2, 5.3, 0.8);
  	b = 1./b;
  
  	float c = 0.0;
  	c += getValue(position, sin(TIME)*.1, cos(TIME)*.2, 5.3, 0.8);
    	c *= getValue(position, sin(TIME)*.1, cos(TIME)*.2, 5.3, 0.8);
  	c += getValue(position, sin(TIME)*.1, cos(TIME)*.2, 5.3, 0.8);
  	c /= getValue(position, sin(TIME)*.1, cos(TIME)*.2, 5.3, 0.8);
  	c = 1./c;
  
	gl_FragColor = vec4( a, b, c, 1.0 );
}