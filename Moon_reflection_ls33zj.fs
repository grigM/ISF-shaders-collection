/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "reflection",
    "moon",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ls33zj by narobins.  testing ",
  "INPUTS" : [

  ]
}
*/


const float xw = 0.1; // x warp
const float yw = 0.15; // y warp
const float xf = 3.0; // x frequency
const float yf = 18.0; // y frequency
const float xs = 0.5; // x speed
const float ys = -0.4; // y speed


const vec4 m = vec4(0.93, 0.9, 0.8, 1.0); // moon
const vec4 w = vec4(0.03, 0.01, 0.3, 1.0); // water


void main()
{   
    vec2 uv = gl_FragCoord.xy;
    uv.x = 2.0 * (uv.x - RENDERSIZE.x / 2.0);
    uv /= RENDERSIZE.yy;;
    uv.y = mod(uv.y * 2.0, 1.0) - 0.5;
    uv *= 2.0;
    if (gl_FragCoord.y / RENDERSIZE.y < 0.5) {
   		uv += vec2(xw * sin(xf * (uv.x - xs * TIME)), 
               yw * cos(yf * (uv.y - ys * TIME)));
    }
    gl_FragColor = mix(m, w, 
                    vec4(smoothstep(0.1, 0.15, dot(uv, uv))));
}