/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#47102.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



// Bonzomatic's default shader translated to glsl sandbox format


float fGlobalTime;
vec2 v2Resolution;

vec4 out_color;


// ________________________________________


// pallerokokeilu numero 10


vec3 lipo = vec3(0.4, 4.0, 4.0), //  + vec3(sin(TIME*3.0)*10.0, 30.0, -2.0), // light position
  lico = vec3(4.0, 0.4, 4.4); // light color


float sphere(vec3 p, float r)
{
  return length(p) - r;
}

vec2 matmin(vec2 mm, float d, float m)
{
  if (d < mm.x) 
    return vec2(d, m);
  else
    return mm;
}


vec3 rd=vec3(0.0, 0.0, 444);

float df(vec3 p, out float mat)
{
  float f=-4.0;
  float fy=f-p.y, fz=fy*rd.z/rd.y;
  
  vec2 r = vec2(44.0-length(p), -4.0); // big outer sphere
  float dfloor=sqrt(fy*fy+fz*fz) * sign(p.y-f);
  r = matmin(r, dfloor, -4.0);

  
  for (float i=0.0; i<11.0; i+=1.0)
  {
    float t = mod(TIME*(1.8+i*.2), 2.0) - 1.0;
    float x = i * 4.0 - 11.0*4.0*0.5;
    float y = - t*t;
    float z = 20.0 + sin(TIME*4.4 + i) * 4.0;

    r = matmin(r, sphere(p - vec3(x,y,z), 3.0), i);
  }

  mat=r.y;
  return r.x;
}

float shade(vec3 p)
{
  float r = 4.0, s=4.5;
  vec3 lv=normalize(lipo-p);
  p+=lv*s;
  for (float i=0.40; i<40.0; i+=4.0) {
    float dummy, d=df(p, dummy);
    if (d<s) {
      r = min(r, d/s);
    }
  }
  return r;
}


void main(void)
{
	fGlobalTime = TIME; // in seconds
	v2Resolution = RENDERSIZE; // viewport RENDERSIZE (in pixels)
	
  vec2 uv = vec2(gl_FragCoord.x / v2Resolution.x, gl_FragCoord.y / v2Resolution.y);
  uv -= 0.5;
  uv /= vec2(v2Resolution.y / v2Resolution.x, 1.0);

  vec3 
    ro = vec3(0.0), sp=vec3(uv, 1.0),
    l = vec3(0.0), p=ro;

  rd=normalize(sp-ro);

  float contrib = 1.0;
  for(float i=0.0;i<120.0;i+=1.0) {
    float mat;
    float d=df(p, mat);
    if (d<0.0) { p+=d*rd; d=0.0; }
    if (d<0.000005) {
      vec3 e=vec3(0.01, 0.0, 0.0);
      float dummy;
      vec3 norm=normalize(vec3(df(p-e.xyy, dummy), df(p-e.yxy, dummy), df(p-e.yyx, dummy)));
      vec3 lv=normalize(p-lipo);

      vec3 m=vec3(1.0);
      if (mat<0.0) m=vec3(1.0+mat*.5); // mod(p.x, 1.0)+mod(p.z, 1.0));
      else
        m=vec3(mod(mat*.4323, 1.0), mod(mat*1.7, 1.0), mod(mat*0.57,1.0));
      l = mix(l, (shade(p)*dot(lv, norm)+0.01)*lico*m*3.0/(log(length(p))), contrib);
      contrib *= 0.4;
      if (contrib < 0.01) break;
      rd=reflect(rd, norm);
      p+=rd*0.0001;
    } else {
      p+=rd*d; // *0.999;
    }
  }

  out_color = vec4(l, 1.0);

  gl_FragColor = out_color;
}