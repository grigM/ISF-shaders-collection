/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#16744.0"
}
*/


//check board by uggway

// yay faux perspective -jz
#ifdef GL_ES
precision highp float;
#endif

//#define CM 

vec3 check(vec2 p, float y, float s)
{
	float c = clamp(floor(mod(p.x/s+floor(p.y/s),2.0))*s,0.1,0.9)*2.0;
	c *= c;
	return vec3(c);
}

void main( void ) {

	vec2 p = -1.0 + 2.0 * ( gl_FragCoord.xy/ RENDERSIZE.xy  );
	p.x *=  RENDERSIZE.x/RENDERSIZE.y;

	vec3 col = vec3(1.0);
	
	float y = p.y + (p.y + (sin((sin(TIME*3.2+p.y)+p.x*-cos(TIME)-TIME+p.x))*0.5)) + cos(p.x*3.)*.05*(sin(p.x)*1.);
	vec2 uv;
	uv.x = p.x/y;
	uv.y = 1.0/abs(y)+TIME/3.0;
	col = check(uv, y, 0.50)*length(y);
	float t = pow(abs(y),0.0);

	gl_FragColor = vec4( col*t, 1.0 );

}