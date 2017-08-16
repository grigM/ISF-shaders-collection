/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40297.0"
}
*/


// FlatSpiral

// Daily 2017-05-02 original by Koltes
	
#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


mat2 rotz(float a)
{
  float c=cos(a),s=sin(a);
  return mat2(c,s,-s,c);
}

float map(vec3 p)
{
  p.xy*=rotz(TIME*4.0);
  p.yz*=rotz(1.567);
  p.xz+=rotz(p.y*2.)*vec2(3.,0.);
  return length(p.xz)-1.5;
}

void main()
{
  vec3 ro=vec3(0),rd=vec3(0),mp=vec3(0);
  float d=0.;
  vec2 uv=(gl_FragCoord.xy-0.5*RENDERSIZE.xy)/RENDERSIZE.y;
  ro=vec3(uv,-25.);
  rd=0.1*normalize(vec3(uv,1.));
  mp=ro;
  for(int i=0;i<99;++i)
  {
    d=map(mp);
    if (d<.01) break;
    mp+=d*rd;
  }
  gl_FragColor = vec4(length(mp)*.05);
}