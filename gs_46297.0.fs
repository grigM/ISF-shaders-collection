/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#46297.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main( void ) 
{
vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.y );
vec3 color = vec3(fract(sin(dot(floor(floor(uv.xy*floor(fract(TIME*0.1)*12.0))+TIME*3.0),vec2(5.364,6.357)))*357.536));
gl_FragColor = vec4(color,1.0);
}