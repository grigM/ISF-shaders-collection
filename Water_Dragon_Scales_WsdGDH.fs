/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WsdGDH by mathmasterzach.  A Simple Effect: Water Dragon Scales\nA quick sketch of an idea I had",
  "INPUTS" : [

  ]
}
*/


//Water Dragon Scales
//classic rand function
#define R(a) fract(sin(dot(a,vec2(12.9898,78.233)))*43758.5453)
const float S=sqrt(2.)/2.;
void main() {

    vec2 R=RENDERSIZE.xy,
         p=(gl_FragCoord.xy+gl_FragCoord.xy-R)/R.y,
         q=6.*p*mat2(S,S,-S,S),
         r=fract(q)+vec2(R(gl_FragCoord.xy)-.5)/25.,
    	 d=floor(q)/2.;
    float h=R(d);
    vec3 c=normalize(vec3(0.,h,1.));
    float f=.2*float(r.x>r.y)+.8;
    float wv=.5*sin(3.*TIME+d.x+d.y);
    float sd=min(.5*h+3.7+wv-2.*max(r.x,r.y)-1.*(r.x+r.y),1.);
    gl_FragColor=vec4(sd*f*(c-.2*wv),1.);
}
