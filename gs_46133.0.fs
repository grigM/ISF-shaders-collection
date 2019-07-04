/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#46133.0"
}
*/


//precision mediump float;

float cdist(vec2 v0, vec2 v1){
  v0 = abs(v0-v1);
  return max(v0.x,v0.y);
}

void main( void ){

  float pitime = mod(TIME,3.14159265);

  //fragment position
  vec2 p = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);

  p *= mat2(cos(0.5/length(p)),sin(+0.5/length(p)),-sin(+0.5/length(p)),cos(0.5/length(p)));

  vec2 q1 = vec2(p.x* abs(5.0/p.y), abs(5.0/p.y)+mod(TIME,1.0));
  float grid1 = 2.0 * cdist(vec2(0.5), mod((q1),1.0));
  vec3 color = vec3 (smoothstep(0.9,1.0,grid1)*3.0 / q1.y);

  vec2 q2 = vec2(p.y* abs(5.0/p.x), abs(5.0/p.x)+mod(TIME,1.0));
  float grid2 = 2.0 * cdist(vec2(0.5), mod((q2),1.0));
  color += vec3 (smoothstep(0.9,1.0,grid2)*6.0 / q2.y);

  gl_FragColor = vec4( vec3(1)-color, 1.0 );
}