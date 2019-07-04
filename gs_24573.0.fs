/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#24573.0"
}
*/


// cubeoctahedron

#ifdef GL_ES
precision mediump float;
#endif



#define PI 3.14159265358979;


float fun(float x, float y, float z)
{
	float p1=cos(TIME)*0.2;
   	float yt=y*cos(p1)-z*sin(p1);
   	float zt1=y*sin(p1)+z*cos(p1);
	float p2=sin(TIME)*0.2;
   	float zt=zt1*cos(p2)-x*sin(p2);
   	float xt=zt1*sin(p2)+x*cos(p2);
   
	
	return abs(xt+yt)*zt+abs(xt-yt)*zt-1.0;
   
}
float dfun(float x, float y, float z, float dz)
{
   return (fun(x,y,z+dz)-fun(x,y,z))/dz;
}
float rootfun(float x, float y)
{
   float zn;
   float z;
   for (float z0n=1.0;z0n<=4.0;z0n+=0.25)
   {
      zn = z0n;
      for (float n=1.0;n<=5.0;n+=1.0)
      {
         zn=zn-fun(x,y,zn)/dfun(x,y,zn,0.0001);
      }
      if (abs(fun(x,y,zn))<=0.001)
      {
         z=zn;
         break;
      }
      else
      {
         z = 9999.0;
         continue;
      }
   }
   return z;
}

void main(void)
{
   float x = gl_FragCoord.x;
   float y = gl_FragCoord.y;
   float w = RENDERSIZE.x;
   float h = RENDERSIZE.y;
   float X1 = -4.0;
   float X2 = 4.0;
   float Y1 = -4.0;
   float Y2 = 4.0;

   float X = (((X2-X1)/w)*x+X1);
   float Y = ((((Y2-Y1)/h)*y+Y1)*h/w);
   float Z = rootfun(X,Y);
   float c = 0.0;
   if (Z==9999.0) c=0.0;
   else
   {
      //float r = sqrt(pow(X-2.0,2.0)+pow(Y-2.0,2.0)+pow(Z,2.0));
      //float f = exp(-r);
      float f = (sin(10.0*Z+TIME*8.0)+1.0)/2.0;
      c = f;
   }
   gl_FragColor = vec4(vec3(c),1.0);

}