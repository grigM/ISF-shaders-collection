/*{
	"CREDIT": "by mojovideotech",
  "CATEGORIES": [
    "Generator"
  ],
  "DESCRIPTION": "",
  "INPUTS": [
    {
      "MAX": [
        1.5,
        1.5
      ],
      "MIN": [
        -0.5,
        -0.5
      ],
      "NAME": "mouse",
      "DEFAULT":[0.5,0.5],
      "TYPE": "point2D"
    },
                    {
            "NAME": "density",
            "TYPE": "float",
           "DEFAULT": 0.05,
            "MIN": 0.0005,
            "MAX": 0.1
          },
          {
            "NAME": "shape",
            "TYPE": "float",
           "DEFAULT": 0.125,
            "MIN": 0.0001,
            "MAX": 0.9
          },
           {
            "NAME": "speed",
            "TYPE": "float",
           "DEFAULT": 0.03,
            "MIN": -0.1,
            "MAX": 0.1
          },
           {
            "NAME": "Tau",
            "TYPE": "float",
           "DEFAULT": 11.0,
            "MIN": 2.0,
            "MAX": 100.0
          }
  ]
}
*/

// ParticleZoom by mojovideotech
// http://glslsandbox.com/e#24972.0


#ifdef GL_ES
precision mediump float;
#endif


float ran(vec2 seed) {
    return fract(sin(seed.x + seed.y * 1e3) * 1e5);
}

float Cell(vec2 coord) {
    vec2 cell = fract(coord) * vec2(.5, 2.) - vec2(.1, .5);
    return (1. - length(cell * 2. - 1.)) * step(ran(floor(coord)), density) * 5.;
}

void main(void) {

    vec2 p = gl_FragCoord.xy / RENDERSIZE - mouse;
	p.x*=RENDERSIZE.x/RENDERSIZE.y; 

    float a = fract(atan(p.x, p.y) / Tau);
    float d = length(p);

    vec2 coord = vec2(pow(d, shape), a) * 256.;
    vec2 delta = vec2(-TIME * speed * 256., 1.0);

    float c = 0.;
    for (int i = 0; i < 8; i++) {
        coord += delta;
        c = max(c, Cell(coord));
    }

    gl_FragColor = vec4(c * d);
}