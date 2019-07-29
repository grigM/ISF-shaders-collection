/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wlXSDB by Hirai_worthless.  ???",
  "INPUTS" : [

  ]
}
*/


void main() {



    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    // Time varying pixel color
    float x = floor(uv.x * 10.0) * 0.1;
    float y = floor((uv.x * (17.3 / 2.0) + uv.y * 5.0)) * 0.1;
    float z = floor((-uv.x * (17.3 / 2.0) + uv.y * 5.0)) * 0.1;
    vec3 col = 0.5 + 0.5*cos(TIME * 2.0 + 2.0 * vec3(x,y,z)+vec3(0,2,4));
    // Output to screen
    gl_FragColor = vec4(col,1.0);
}
