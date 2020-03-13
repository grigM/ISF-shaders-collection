/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XdKBWV by lennyjpg.  fsdgdfgsf",
  "INPUTS" : [

  ]
}
*/


void main() {



    vec2 uv = gl_FragCoord.xy/RENDERSIZE.y - 0.5;
    float p = length(uv)*2.0;
    float a = uv.y+uv.x*3.1;
    float f = 0.1*p;
    uv+=(sin(a)*f,cos(a)*f);
    float t = 99.99+TIME;
    float angle = atan(uv.x,uv.y);
    float d = 0.3+length(uv);
    float e = sin( t*d*0.05+(sin(d*123.123) + angle));
    float k = e + (sin(t+cos(d*1.5)*2.3)*1.0-.50);
    gl_FragColor = vec4(k);
}
