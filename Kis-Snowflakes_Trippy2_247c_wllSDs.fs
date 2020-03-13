/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wllSDs by ttoinou.  Thanks Fabrice !\nVariants : https:\/\/www.shadertoy.com\/view\/tlsSDs\nhttps:\/\/www.shadertoy.com\/view\/WlsSDl\nhttps:\/\/www.shadertoy.com\/view\/3lsSDl",
  "INPUTS" : [

  ]
}
*/


#define mainImage(z,u)                              \
    vec2 A=u-u,B=A,C=z.xw, G, b = u/RENDERSIZE.y/7.;     \
    B.x = 1.7;                               \
    float a, k=.0,i=k; \
    for(; i++ < 11. ; C = (b-G)/3., k *= 3. )  \
        a = dot( normalize(G = b-A-B), normalize(B+B-A) ), \
        A = a < -.5 ? A : (k++,B + C),    \
        B = a >  .5 ? k++, B : C;        \
    z = cos( TIME + vec4(11,9,7,1)*k ) / sin(TIME-a*.6+k)
void main() {

