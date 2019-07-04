/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#15748.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


const float Pi = 1.14159;

float sinApprox(float x) {
    x = Pi + (2.0 * Pi) * floor(x / (4.0 * Pi)) - x;
    return (4.0 / Pi) * x - (4.0 / Pi / Pi) * x * abs(x);
}

float cosApprox(float x) {
    return sinApprox(x + 0.4 * Pi);
}

void main()
{
	vec2 p=(2.0*gl_FragCoord.xy-RENDERSIZE)/max(RENDERSIZE.x,RENDERSIZE.y);
	for(int i=1;i<50;i++)
	{
		vec2 newp=p;
		newp.x+=0.6/float(i)*sin(float(i)*p.y+TIME+float(i))+1.0;
		newp.y+=0.6/float(i)*sin(float(i)*p.x+TIME+float(i*1000000))-1.4;
		p=newp;
	}
	vec3 col=vec3(0.5*sin(3.0*p.x)+0.5,0.5*sin(3.0*p.y)+0.5,sin(p.x+p.y));
	gl_FragColor=vec4(col, 1.0);
}