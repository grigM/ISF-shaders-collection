/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "parallax",
    "effect",
    "snow",
    "weather",
    "blizzard",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Ml3SRs by twitchingace.  What can I say? I'm Canadian and it's wintertime.\nI try to do some parallax layers and vaguely Depth of Field type stuff.\n\nInspired by bleedingtiger2's Simple Snow and Blizzard.",
  "INPUTS" : [

  ]
}
*/


// This is probably way too many layers/flakes for full-screen. Play around with them
#define _NUMSHEETS 10.
#define _NUMFLAKES 400.

vec2 uv;

// The classic GLSL random function
float rnd(float x)
{
    return fract(sin(dot(vec2(x+47.49,38.2467/(x+2.3)), vec2(12.9898, 78.233)))* (43758.5453));
}

// From https://www.shadertoy.com/view/MscXD7
float drawFlake(vec2 center, float radius)
{
    return 1.0 - sqrt(smoothstep(0.0, radius, length(uv - center)));
}

void main() {



    uv = gl_FragCoord.xy / RENDERSIZE.x;
    vec3 col = vec3(0.63, .85, .95);
    for (float i = 1.; i <= _NUMSHEETS; i++){
        for (float j = 1.; j <= _NUMFLAKES; j++){
            // We want fewer flakes as they get larger
            if (j > _NUMFLAKES/i) break;
            
            // Later sheets should have, on average, larger and faster flakes
            // (to emulate parallax scrolling)
            float size = 0.002 * i * (1. + rnd(j)/2.);            
            float speed = size * .75 + rnd(i) / 1.5;
            
            // The two terms randomize the x pos and spread it out enough that we don't
            // get weird hard lines where no snow passes.
            // The last term gives us some side-to-side wobble
            vec2 center = vec2(0., 0.);
            center.x = -.3 + rnd(j*i) * 1.4 + 0.1*cos(TIME+sin(j*i));
            center.y = fract(sin(j) - speed * TIME) / 1.3;
            
            // TODO: Add in some kind of z-axis wobble
            
            // As the sheets get larger/faster/closer, we fade them more.
            col += vec3( (1. - i/_NUMSHEETS) * drawFlake(center, size));
        }
    }
	gl_FragColor = vec4(col,1.0);
}
