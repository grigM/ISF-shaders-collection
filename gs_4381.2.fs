/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#4381.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

// Ordered dither without matrices



float getPattern(float x,float y,float b)
{
		
	float col = 0.0;
	float d;
	d = (abs(.5+mod(floor(x),2.)*2.-mod(floor(y),2.)*4.)-.5)*1.;
	d += (abs(.5+mod(floor(x*.5),2.)*2.-mod(floor(y*.5),2.)*4.)-.5)*.25;
	d += (abs(.5+mod(floor(x*.25),2.)*2.-mod(floor(y*.25),2.)*4.)-.5)*.0625;
	d += (abs(.5+mod(floor(x*.125),2.)*2.-mod(floor(y*.125),2.)*4.)-.5)*.015625;
	return step(b,d);
}

void main( void ) {

	vec2 p = ( gl_FragCoord.xy );

	float color = 0.0;
	
	p -= RENDERSIZE*.5;
	
	float t = TIME*.3;
	p = vec2(p.x*cos(t)-p.y*sin(t),p.y*cos(t)+p.x*sin(t))*exp(sin(TIME*.3)*2.);
	
	p *= .005;
	
	p = abs(fract(p+.5)-.5);
	
	color = max(p.x,p.y)*8.;

	color = getPattern(gl_FragCoord.x,gl_FragCoord.y,color);
	

	gl_FragColor = vec4( vec3( color), 1.0 );

}