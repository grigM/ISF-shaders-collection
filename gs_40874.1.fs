/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40874.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float lengthN(vec2 v, float n)
{
  vec2 tmp = pow(abs(v), vec2(n));
  return pow(tmp.x+tmp.y, 1.0/n);
}
 
float rings(vec2 p)
{
  float mo = fract(TIME*0.1)-0.5; //16.0 * step( 0.0, floor(mod(TIME*1.0,2.0)));
  vec2 p2 = mod(p*mo*0.5, mo);
	/*
  if(p2.x > 1.0)
  {
    p2.x -= max(sin(p2.y*5.0)*0.3, 0.0);
  }
  if(p2.y > 1.0)
  {
    p2.y -= max(sin(p2.x*10.0)*0.3, 0.0);
  }
*/
  return 1.-(sin(lengthN(p2, 4.0)*216.0)*0.5+0.5);
}
 
vec2 trans(vec2 p)
{
	/*	
  const float height = 0.5;
  float r = height/p.y;
  return vec2(p.y+0.5*sin(TIME)*r, r-0.3*cos(TIME));
*/

  float theta = atan(p.y, p.x);
  float r = length(p);
  const float radius = 1.0;
  return vec2(theta, radius/r+0.14*TIME);

}
 
void main() {
  vec2 pos = (gl_FragCoord.xy*2.0 -RENDERSIZE) / RENDERSIZE.y;
 
  gl_FragColor = vec4(rings(trans(vec2(pos.x-0.7*cos(pos.y*3.*cos(TIME))*sin(TIME),pos.y))));
}