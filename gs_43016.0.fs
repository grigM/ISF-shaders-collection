/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43016.0"
}
*/



const float PI = 3.1415926535;


void main(void)
{
    vec2 coord = gl_FragCoord.xy - RENDERSIZE * 0.5;
	coord *= 7.99;

    float phi = atan(coord.y, coord.x + 1e-6);
    phi = phi / PI * 0.5 + 0.5;
    float seg = floor(phi * 6.0);

    float theta = (seg + 0.5) / 6.0 * PI * 2.0;
    vec2 dir1 = vec2(cos(theta), sin(theta));
    vec2 dir2 = vec2(-dir1.y, dir1.x);

    float l = dot(dir1, coord);
    float w = sin(seg * 31.415926535) * 18.0 + 20.0;
    float prog = l / w + TIME * 2.0;
    float idx = floor(prog);

    float phase = TIME * 0.8;
    float th1 = fract(273.84937 * sin(idx * 5.67458 + floor(phase    )));
    float th2 = fract(90.84937 * sin(idx * 4.67458 + floor(phase + 1.0)));
    float thresh = mix(th1, th2, smoothstep(0.75, 1.0, fract(phase)));

    float l2 = dot(dir2, coord);
    float slide = fract(idx * 32.74853) * 200.0 * TIME;
    float w2 = fract(idx * 39.721784) * 990.0;
    float prog2 = (l2 + slide) / w2;

    float c = clamp((fract(prog) - thresh) * w * 0.3, 0.0, 1.0);
    c *= clamp((fract(prog2) - 1.0 + thresh) * w2 * 01.3, 0.0, 1.0);

    gl_FragColor = vec4(c, c, c, 1);
}