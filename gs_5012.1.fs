/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#5012.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void rX(inout vec3 p, float a) { float c,s;vec3 q=p;	c = cos(a); s = sin(a);	p.y = c * q.y - s * q.z;	p.z = s * q.y + c * q.z; }
void rY(inout vec3 p, float a) {	float c,s;vec3 q=p;	c = cos(a); s = sin(a);	p.x = c * q.x + s * q.z;	p.z = -s * q.x + c * q.z;}
void rZ(inout vec3 p, float a) {	float c,s;vec3 q=p;	c = cos(a); s = sin(a);	p.x = c * q.x - s * q.y;	p.y = s * q.x + c * q.y; }



float star(vec3 p, float s) { return length(p)-s; }
float starfield(vec3 p, float s, vec3 c) { vec3 q = mod(p,c)-.5*c; return star(q,s); }
vec2 map(vec3 p)
{
   float a = starfield(p, .1+.05*abs(sin(TIME)), vec3(1.,1.,1.));
   return vec2(a,1.);
}

vec2 intersect(in vec3 ro, in vec3 rd)
{
   for (float t = .0; t < 100.; t+= .2)
   {
       vec2 h = map(ro+rd*t);
       if (h.x < .0001) return vec2(t,h.y);
   }
   
   return vec2(0);
}

void main( void ) {
   
   vec2 p = -1.0 + 2.0 *  ( gl_FragCoord.xy / RENDERSIZE.xy );
	  p.x *= RENDERSIZE.x / RENDERSIZE.y;

   vec3 ro = vec3(0,0,1.0-TIME);
   vec3 rd = normalize(vec3(p, -1.));
   rX(rd,mouse.y-.5);
   rY(rd,-mouse.x+.5);
   vec3 color = vec3(0);
   
   vec2 t = intersect(ro,rd);
    
   if (t.y > .0) {
      color = vec3(t.x,1.-mod(t.x,1.),abs(sin(TIME/5.))+.5);
   }
   
   gl_FragColor = vec4(color,1.0);
}