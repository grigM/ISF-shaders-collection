/*
{
  "CATEGORIES": [
    "Automatically Converted"
  ],
  "INPUTS": [
    
  ]
}
*/


// By @paulofalcao
//
// Blobs

#ifdef GL_ES
precision highp float;
#endif


float makePoint(float x,float y,float fx,float fy,float sx,float sy,float t){
   float xx=x*cos(t*fx);
   float yy=y*sin(t*fy);
   return 1./ (sqrt(length(xx+yy) + length(xx*yy)));
}

void main( void ) {

   vec2 p=(gl_FragCoord.xy/RENDERSIZE.x)*2.0-vec2(1.0,RENDERSIZE.y/RENDERSIZE.x);
   
   float x=p.x;
   float y=p.y;

   float a=
       makePoint(x,y,3.3,2.9,0.3,0.3,TIME);
   a=a+makePoint(x,y,1.9,2.0,0.4,0.4,TIME);
   a=a+makePoint(x,y,0.8,0.7,0.4,0.5,TIME);
   a=a+makePoint(x,y,2.3,0.1,0.6,0.3,TIME);
   a=a+makePoint(x,y,0.8,1.7,0.5,0.4,TIME);
   a=a+makePoint(x,y,0.3,1.0,0.4,0.4,TIME);
   a=a+makePoint(x,y,1.4,1.7,0.4,0.5,TIME);
   a=a+makePoint(x,y,1.3,2.1,0.6,0.3,TIME);
   a=a+makePoint(x,y,1.8,1.7,0.5,0.4,TIME);   
   
   float b=
       makePoint(x,y,1.2,1.9,0.3,0.3,TIME);
   b=b+makePoint(x,y,0.7,2.7,0.4,0.4,TIME);
   b=b+makePoint(x,y,1.4,0.6,0.4,0.5,TIME);
   b=b+makePoint(x,y,2.6,0.9,0.6,0.3,TIME);
   b=b+makePoint(x,y,0.7,1.4,0.5,0.4,TIME);
   b=b+makePoint(x,y,0.7,1.7,0.4,0.4,TIME);
   b=b+makePoint(x,y,0.8,0.5,0.4,0.5,TIME);
   b=b+makePoint(x,y,1.4,0.7,0.6,0.3,TIME);
   b=b+makePoint(x,y,0.7,1.3,0.5,0.4,TIME);

   float c=
       makePoint(x,y,3.7,0.3,0.3,0.3,TIME);
   c=c+makePoint(x,y,1.9,1.3,0.4,0.4,TIME);
   c=c+makePoint(x,y,0.8,0.9,0.4,0.5,TIME);
   c=c+makePoint(x,y,1.2,1.7,0.6,0.3,TIME);
   c=c+makePoint(x,y,0.3,0.6,0.5,0.4,TIME);
   c=c+makePoint(x,y,0.3,0.3,0.4,0.4,TIME);
   c=c+makePoint(x,y,1.4,0.8,0.4,0.5,TIME);
   c=c+makePoint(x,y,0.2,0.6,0.6,0.3,TIME);
   c=c+makePoint(x,y,1.3,0.5,0.5,0.4,TIME);
   
   vec3 d=vec3(a,b,c)*0.01;
   
   gl_FragColor = vec4(d.x,d.y,d.z,1.0);
}