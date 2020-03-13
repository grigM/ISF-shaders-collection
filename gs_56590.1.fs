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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#56590.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif



void main() {
    vec2 p = gl_FragCoord.xy / RENDERSIZE.x * 0.7;
	vec2 rw = (gl_FragCoord.xy / (RENDERSIZE.x + RENDERSIZE.y));
    vec3 col;
    for(float j = 0.0; j <3.0; j++){
        for(float i = 1.0; i < 10.0; i++){
            p.x += 0.1 / (i + j) * sin(i * 10.0 * p.y + mouse.x*.1*TIME + cos((TIME-rw.x / (12. * i)) * i + j));
            p.y += 0.1 / (i + j)* cos(i * 10.0 * p.x +  mouse.y*.1*TIME + sin((TIME-rw.y / (12. * i)) * i + j));
        }
        col[int(j)] = abs(p.x + p.y);
    }
    gl_FragColor = vec4(col-rw.x, 1.);
}