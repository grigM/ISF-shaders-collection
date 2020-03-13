/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tdd3W8 by foran.  throughColor",
  "INPUTS" : [

  ]
}
*/


#define PI 3.14159

vec2 modA(vec2 p, float c)
{
  float a = atan(p.y,p.x);
  a = mod(a , c);

  return vec2(cos(a),sin(a)) * length(p);
}

float smin(float a, float b, float k)
{
  float h = clamp(.5 + .5 * (b -a)/k,0.,1.);
  return mix(b,a,h) - k * h *(1. - h);
}

mat2 rot(float a)
{
  float sa = sin(a);  float ca  = cos(a);
  return mat2(ca,-sa,sa,ca);
}

#define time (TIME)

float map(vec3 p, out float id)
{
  id = 0.;
  float dist  = 10000.;
  vec3 cp = p;
  p.yz = modA(p.yz, 6.);//  непрерывность спирали
  p.yz *= rot(p.x * .75 + time*2.);// форма спирали
  p.y += 3.;//  амплитуда
  float co = length(p.yz) - (1. + sin(p.x) * .58);//  спираль

  p = cp;
  float ti = time;
  for(float i  = 1.; i < 5.; ++i)//  фон с фигуркой
  {
    p.y += (sin(ti + p.x ) + cos(ti + p.z)) * 1.13;//  фигурка
    dist = smin(length(p) - .85, dist, 1.25);//  слияние амплитуды фигурки
    ti += i * .2;
    p *= 1.1;// FOP фигурки  близко-далеко
    p.xz *= rot(.25 + i);//  вращение по xz
    p.yz *= rot(i * 2.);//  вращение по yz
    p.x += .6;
  }

  if(co < .01) id = 1.;//  спираль
  dist = min(dist , co);
  return dist;
}

float Ray(inout vec3 cp, vec3 rd, out float id)
{
  float st = 0.;
  float cd = 0.;
  for(; st < 1.; st += 1. / 40.)  //128.)  // глубина
  {
    cd = map(cp, id);
    if(cd < .01) break;// epsilon=0.01
    cp += rd * cd * .45;
  }

  return st;
}

vec3 normal(vec3 p)
{
  float id;
  float m = map(p, id);
  vec2 e = vec2(.01,.0);
  return normalize(vec3(
  m - map(p + e.xyy, id),
  m - map(p + e.yxy, id),
  m - map(p + e.yyx, id)
));
}

vec3 LookAt(vec3 eye, vec3 sub,vec2 uv)
{
  vec3 fo = normalize(sub - eye);
  vec3 ri = cross(fo, vec3(0.,1.,0.));
  vec3 up = cross(ri,fo);
  return normalize(fo + ri * uv.x + up * uv.y);
}

void main() {



  vec2 uv=vec2(gl_FragCoord.x/RENDERSIZE.x,gl_FragCoord.y/RENDERSIZE.y);
  uv-=.5;
  uv/=vec2(RENDERSIZE.y/RENDERSIZE.x,1);
  vec3 eye = vec3(0.,0.,-12.);//  FOV
  vec3 sub = vec3(0.);
  vec3 cp = eye;
  float st = 0.;
  float id = 0.;
  vec3 rd = LookAt(eye, sub, uv);
  st = Ray(cp, rd, id);
  vec3 ld = normalize(sub - eye);
  vec3 norm = normal(cp);//  cp из Ray
  float li = dot(ld, norm);
  li = 1. - li;
  li *= 1.5;// яркость
  li = pow(li, 2.);
  norm.xy *= rot(time + cp.z);
  norm.xz *= rot(time + cp.y);
  norm.yz *= rot(time + cp.x);
  vec4 out_color=vec4(1.);
  out_color = vec4(norm, 0.) * li;
  out_color *= (1. - st) * (5. );
  gl_FragColor=vec4(out_color);
}
