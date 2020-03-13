/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59336.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define TWO_PI 6.2831833
#define PI 3.14159265

float circ(vec2 p)
{
    float r = length(p);
    r = log(sqrt(r));
	 
	
    return  abs(mod(r*4., TWO_PI) - PI) * 3. + .2;
}

void main( void ) {

	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	
	float t = TWO_PI * TIME / 5.0 * 0.05; 
	
	vec2 p = uv - 0.5;
	
	p.x *=  RENDERSIZE.x / RENDERSIZE.y;
	p *= 4.0; 
	
	float loopWiggle = 0.1; 
	
	float warp = loopWiggle / 5.; 
	
	p.x += warp * cos(0.5*t  + 10. * p.y);
	p.y += warp * sin(0.5*t + 10. * p.x); 
	
	p /= exp(mod(t * 10., PI));
	
	float v = circ(p);
	
	float disparity = 0.4; 
	
	disparity =  disparity * pow(abs(0.1 - v),.9);
	
	vec3 loopColor = vec3(0.2, 0.1, 0.4);
		
	vec3 col = loopColor / disparity;
	
	col =pow( abs(col), vec3(.99));
	
	float alpha = smoothstep(0.6, 0.9, min(min(col.r, col.g), col.b));
	
	gl_FragColor = vec4(col, alpha);
}