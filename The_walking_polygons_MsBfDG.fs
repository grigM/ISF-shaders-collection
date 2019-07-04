/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MsBfDG by rubelson.  Test of rotation of polygons",
  "INPUTS" : [

  ]
}
*/


float regularPolygon(float parts, float radius, float blur, vec2 pos, float rotate, vec2 p)
{
float pi = 3.14159265;
float a = 2.0 * pi * (1.0 / parts);
float size = radius * cos(a /2.0);
float ca = cos(a);
float sa = sin(a);
mat2 rmat = mat2(ca, -sa, sa, ca);
ca = pi + rotate -a;
vec2 lp = vec2(sin(ca), cos(ca));
vec2 addv = lp;
float r = 0.0;

for(float i=0.; i<=100.; i++)
	{
	addv = rmat * addv;
	r = max(r, smoothstep(size, size+blur, dot(p - pos, normalize(addv))));
    if(i>parts) break; //and how differently?    
	}
return 1. - r;
}

void main() {



const float n = 10.;
float a = 0.14;
vec2 p = gl_FragCoord.xy / RENDERSIZE.x; 
float t = fract(TIME/2.);
float pi = 3.14159265;
float p2 = pi*2.;
vec3 color[6];
color[0]=vec3(1.,0.,0.);color[1]=vec3(0.,1.,1.);color[2]=vec3(0.,0.,1.);
color[3]=vec3(1.,0.,1.);color[4]=vec3(0.,1.,0.);color[5]=vec3(1.,1.,0.);
float blur = 0.0;
vec3 col = vec3(0.);
for(float i=n-1.; i>=0.; i--)
  {
  float r = a / (2.*sin(pi/(i+3.)));
  float alf = p2 * (t-.5)/(i+3.);
  float rad = p2 * t/(i+3.);
  vec2 pos = vec2(.5 + a*(.5-t) + r*sin(alf), .01 + r*cos(alf));
  col = mix(col,color[int(mod(i,6.))],regularPolygon(i+3., r, blur, pos, rad, p));
  }
gl_FragColor = vec4(col,1.);
}
