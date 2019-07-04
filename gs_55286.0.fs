/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#55286.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


const float Pi = 3.14159;

float sinApprox(float x) {
    x = Pi + (2.0 * Pi) * floor(x / (2.0 * Pi)) - x;
    return (4.0 / Pi) * x - (4.0 / Pi / Pi) * x * abs(x);
}

float cosApprox(float x) {
    return sinApprox(x + 0.5 * Pi);
}

void main()
{
	vec2 p=(2.0*gl_FragCoord.xy-RENDERSIZE)/max(RENDERSIZE.x,RENDERSIZE.y);
	for(int i=1;i<50;i++)
	{
		vec2 newp=p;
		newp.x+=0.3/float(i)*sin(float(i)*p.y+TIME+0.3*float(i));
		newp.y+=0.3/float(i)*sin(float(i)*p.x+TIME+0.3*float(i+10));
		p=newp;
	}
	vec3 col=vec3(p.x-floor(p.x),p.y-floor(p.y),p.x*p.y*10.0);
	gl_FragColor=vec4(clamp(sqrt(col - 0.5*col.g + col.r),0.0,1.0)-vec3(0.5,0.5,0.4)*0.1+vec3(0.5,0.5,0.4)*0.1, 1.0);
}