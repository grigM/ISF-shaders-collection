/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#42766.3"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

// Two Spirals 2017-11-30 by @hintz


void main(void)
{	
	vec2 p = (gl_FragCoord.xy-0.5*RENDERSIZE)/RENDERSIZE.y;
	p = vec2(cos(p.x+0.1*TIME),p.y); 
	float y0 = p.y + 0.2*sin(4.0*p.x+TIME-1.0+0.5*cos(TIME));
	float y1 = p.y + 0.2*cos(4.0*p.x+TIME+1.0+0.5*sin(TIME));
	y0 *= y0;
	y1 *= y1;
	y0 = sqrt(1.0 - y0 * 100.0);
	y1 = sqrt(1.0 - y1 * 100.0);
	float y2 = cos(4.0*p.x+TIME-1.0+0.5*cos(TIME));
	float y3 = -sin(4.0*p.x+TIME+1.0+0.5*sin(TIME));
	float y = max(y0+y2,y1+y3);
	float c = y;
	vec3 color = c*normalize(vec3(c,p.x+c,c+p.y));
	gl_FragColor = vec4(color.zxy,1.0);
}