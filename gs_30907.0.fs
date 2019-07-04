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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#30907.0"
}
*/


//precision mediump float;
void main(){vec3 p = vec3((gl_FragCoord.xy)/(RENDERSIZE.y),mouse.x);
  for (int i = 0; i < 100; i++){
    p.xzy = vec3(1.3,0.999,0.9)*(abs((abs(p)/dot(p,p)-vec3(1.0,1.0,mouse.y*0.5))));}
  gl_FragColor.rgb = p;gl_FragColor.a = 5.0;}