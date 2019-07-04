/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "hyperellipse",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtG3Ry by kjfung.  Draw residual of different hyperellipses of radius 1",
  "INPUTS" : [
	{
            "NAME": "speed",
            "TYPE": "float",
           "DEFAULT": 1.0,
            "MIN": 0.000,
            "MAX": 4.0
      },
      {
            "NAME": "ofset",
            "TYPE": "float",
           "DEFAULT": 0.0,
            "MIN": 0.0,
            "MAX": 1.8
      },
  ]
}
*/


#define pow(a, b) pow(abs(a), b)

void main()
{
    vec2 pos = RENDERSIZE.xy;
    pos = 1.5 * (2.0 * gl_FragCoord.xy - pos) / pos.y;

    float n = 8.0 * pow(sin((TIME*speed)-ofset), 2.0);
    vec2 pos_n = pow(pos, vec2(n));
    float res = pos_n.x + pos_n.y - 1.0
                / pow(pos_n.x + pos_n.y, 1.0/n - 1.0)
                / length(pos_n / pos);

    gl_FragColor = vec4(1.0 - abs(res));
}