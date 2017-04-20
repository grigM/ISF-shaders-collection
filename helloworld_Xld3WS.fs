/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "test",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xld3WS by andrewww1.  test",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


    
void main(){
  vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
  vec2 ms = iMouse.xy / RENDERSIZE.xy;
  float aspect=RENDERSIZE.x/RENDERSIZE.y;
      
    
  vec3 c=vec3(uv,0.5+0.5*sin(TIME));
    
  if(length((uv-ms)*vec2(aspect,1.0)) < 0.1) {
    // c=vec3(1.0);
    //c=mix(c,1.0-texture2D(iChannel0, uv).xyz,0.5);
  }
    
  gl_FragColor = vec4(c,1.0);
}