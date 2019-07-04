/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WdfXRB by stubob.  yeah",
  "INPUTS" : [

  ]
}
*/


float rnd (vec2 st) {
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233)))*
        43758.5453123);
}

void main() {



    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    uv.x *= 160.;
    uv.y *= 120.;
    
    vec2 ipos = ceil(uv);  // get the integer coords
    vec3 col = vec3(rnd(ipos + TIME));
    col +=  abs(vec3(tan(gl_FragCoord.y - TIME * 8.)) * 0.1);
    
    // Output to screen
    gl_FragColor = vec4(col,1.0);
}
