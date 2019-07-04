/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#48579.1"
}
*/


//precision mediump float;

void main(){
  vec3 color = vec3(0.0, 0.9, 0.3);

  vec2 p = gl_FragCoord.xy / RENDERSIZE.x;

  float a = 0.5;
  float b = (sin(p.y * 5.0 + TIME * 5.55) + sin(p.y * 9.0 + TIME * 12.0) + sin(p.y * 5.0 + TIME * 27.0))/6.0;
  float c = cos(p.y * 5.0 + TIME * 17.5) / 2.3;
  float d = sin(p.y * 5.0 + TIME * 7.0) / 2.44;

  float f = 0.01 / (p.y + b);
  float g = 0.01 / abs(p.x + b);
  float h = 0.01 / abs(p.x + c);
  float i = 0.01 / abs(p.x + d);

  vec3 destColor = color * f ;
  gl_FragColor = vec4(destColor, 1.0);
}