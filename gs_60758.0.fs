/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
        
    {
      "NAME" : "x_val",
      "TYPE" : "float",
      "MAX" : 1.0,
      "DEFAULT" : 0.5,
      "MIN" : 0.3
    }

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60758.0"
}
*/


// -----------------------------------------------------
// V4 by nabr
// https://www.shadertoy.com/view/3ldGDl
// License Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0)
// https://creativecommons.org/licenses/by-nc/4.0/
// -----------------------------------------------------

//precision highp float;

void main()
{
	vec2 unipos = (gl_FragCoord.xy / RENDERSIZE);
    vec2 U = unipos*2.0-1.0;
    U.x *= RENDERSIZE.x / RENDERSIZE.y;
    
    
    //vec2 U = (1. - 0.4 * mouse.x) * vv_FragNormCoord;
    vec3 ht = smoothstep(0., 2., .25 - dot(U* x_val, U* x_val)) * vec3(U* x_val, -1),
         n = 100. * normalize(ht - vec3(0, -.15 * fract(.0005 * TIME), .65)), 
         p = n;
    for (float i = 0.; i <= 25.; i++)
    {
        p = 20. * n + vec3(cos(.325 * TIME - i - p.x) + cos(.325 * TIME + i - p.y), sin(i - p.y) + cos(i + p.x), 1);
        p.xy = cos(i) * p.xy + sin(i) * vec2(p.y, -p.x);
    }
    float tx = 5. * sqrt(dot(vec3(0, 6, 4), -p));
    gl_FragColor = vec4(pow(sin(vec3(0, 1, 1.57) - tx) * .35 + .5, vec3(1.5)), 1);
}