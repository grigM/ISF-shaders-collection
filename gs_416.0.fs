/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "redSpeed",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 3
	},
	{
		"NAME": "greenSpeed",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 3
	},
	{
		"NAME": "blueSpeed",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 3
	},
	{
		"NAME": "line_width",
		"TYPE": "float",
		"DEFAULT": 2,
		"MIN": 0.1,
		"MAX": 10
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#416.0"
}
*/


// By @paulofalcao
//
// Blobs

#ifdef GL_ES
precision highp float;
#endif


float makePoint(float x,float y,float fx,float fy,float sx,float sy,float t){
   float xx=x+tan(t*fx)*sx;
   float yy=y+sin(t*fy)*sy;
   return 1.0/sqrt(length(xx+yy));
}

void main( void ) {

   vec2 p=(gl_FragCoord.xy/RENDERSIZE.x)*2.0-vec2(1.0,RENDERSIZE.y/RENDERSIZE.x);

   p=p*2.0;
   
   float x=p.x;
   float y=p.y;

   float a=
       makePoint(x,y,3.3,2.9,0.3,0.3,TIME*redSpeed);
   a=a+makePoint(x,y,1.9,2.0,0.4,0.4,TIME*redSpeed);
   a=a+makePoint(x,y,0.8,0.7,0.4,0.5,TIME*redSpeed);
   a=a+makePoint(x,y,2.3,0.1,0.6,0.3,TIME*redSpeed);
   a=a+makePoint(x,y,0.8,1.7,0.5,0.4,TIME*redSpeed);
   a=a+makePoint(x,y,0.3,1.0,0.4,0.4,TIME*redSpeed);
   a=a+makePoint(x,y,1.4,1.7,0.4,0.5,TIME*redSpeed);
   a=a+makePoint(x,y,1.3,2.1,0.6,0.3,TIME*redSpeed);
   a=a+makePoint(x,y,1.8,1.7,0.5,0.4,TIME*redSpeed);   
   
   float b=
       makePoint(x,y,1.2,1.9,0.3,0.3,TIME*greenSpeed);
   b=b+makePoint(x,y,0.7,2.7,0.4,0.4,TIME*greenSpeed);
   b=b+makePoint(x,y,1.4,0.6,0.4,0.5,TIME*greenSpeed);
   b=b+makePoint(x,y,2.6,0.9,0.6,0.3,TIME*greenSpeed);
   b=b+makePoint(x,y,0.7,1.4,0.5,0.4,TIME*greenSpeed);
   b=b+makePoint(x,y,0.7,1.7,0.4,0.4,TIME*greenSpeed);
   b=b+makePoint(x,y,0.8,0.5,0.4,0.5,TIME*greenSpeed);
   b=b+makePoint(x,y,1.4,0.7,0.6,0.3,TIME*greenSpeed);
   b=b+makePoint(x,y,0.7,1.3,0.5,0.4,TIME*greenSpeed);

   float c=
       makePoint(x,y,3.7,0.3,0.3,0.3,TIME*blueSpeed);
   c=c+makePoint(x,y,1.9,1.3,0.4,0.4,TIME*blueSpeed);
   c=c+makePoint(x,y,0.8,0.9,0.4,0.5,TIME*blueSpeed);
   c=c+makePoint(x,y,1.2,1.7,0.6,0.3,TIME*blueSpeed);
   c=c+makePoint(x,y,0.3,0.6,0.5,0.4,TIME*blueSpeed);
   c=c+makePoint(x,y,0.3,0.3,0.4,0.4,TIME*blueSpeed);
   c=c+makePoint(x,y,1.4,0.8,0.4,0.5,TIME*blueSpeed);
   c=c+makePoint(x,y,0.2,0.6,0.6,0.3,TIME*blueSpeed);
   c=c+makePoint(x,y,1.3,0.5,0.5,0.4,TIME*blueSpeed);
   
   vec3 d=vec3(a,b,c)/32.0;
   
   gl_FragColor = vec4(d.x,d.y,d.z,1.0);
}