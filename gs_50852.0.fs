/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#50852.0",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "deform",
      "TYPE" : "float",
      "MAX" : 3,
      "DEFAULT" : 1.05,
      "MIN" : 0
    },
    {
      "NAME" : "line_lenth",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 0,
      "MIN" : -2
    },
    {
      "NAME" : "line_width",
      "TYPE" : "float",
      "MAX" : 0.20000000000000001,
      "DEFAULT" : 0.10000000000000001,
      "MIN" : 0
    },
    {
      "NAME" : "line_ofset",
      "TYPE" : "float",
      "MAX" : 5,
      "DEFAULT" : 1.8500000000000001,
      "MIN" : 1
    },
    {
      "NAME" : "rotation_speed",
      "TYPE" : "float",
      "MAX" : 5,
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "NAME" : "mod_repitions",
      "TYPE" : "float",
      "MAX" : 1.59,
      "DEFAULT" : 0.95499999999999996,
      "MIN" : 0
    }
  ]
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/XtyfDc
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy globals
float iTime = 0.0;
vec3  iResolution = vec3(0.0);

// --------[ Original ShaderToy begins here ]---------- //
// variant of https://shadertoy.com/view/4lGfDc

#define S(r)  smoothstep(  9./R.y, 0., abs( U.x -r ) -line_width )
void mainImage(inout vec4 O, vec2 u) {
    vec2 R = iResolution.xy,
         U = u+u - R;
    U =  length(U+U)/R.y    /* .955 = 3/pi  1.05 = pi/3 */
         *cos( ( mod( mod_repitions*atan(U.y,U.x) - iTime ,2.) - .92 ) *deform -vec2(line_lenth,1.57));
    U.x+U.y < line_ofset ? O += mix( .5* S(.5), S(.7), .5+.5*U.y ) : O ;
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    iTime = TIME*rotation_speed;
    iResolution = vec3(RENDERSIZE, 0.0);

    gl_FragColor.rgb = vec3(0.0);
    mainImage(gl_FragColor, gl_FragCoord.xy);
    gl_FragColor.a = 1.0;
}