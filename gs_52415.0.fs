/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
   	{
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    },
	{
      "NAME" : "grid_size",
      "TYPE" : "float",
      "DEFAULT" : 10.0,
      "MAX" : 30,
      "MIN" : 2
    },
    {
      "NAME" : "speed",
      "TYPE" : "float",
      "DEFAULT" : 1.0,
      "MAX" : 5,
      "MIN" : 0
    },
    {
      "NAME" : "point_pose_rndm",
      "TYPE" : "float",
      "DEFAULT" : 1.0,
      "MAX" : 2,
      "MIN" : 0
    },
    {
      "NAME" : "rad",
      "TYPE" : "float",
      "DEFAULT" : 1.0,
      "MAX" : 2,
      "MIN" : 0
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#52415.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


//const float SPEED = 2.0;
//const float RADIUS = 0.25;
//const float BRIGHTNESS = 2.0;

vec2 random2(vec2 st)
{
    st = vec2( dot(st,vec2(127.1,311.7)),
              dot(st,vec2(269.5,183.3)) );
    return -1.0 + 2.0*fract(sin(st)*43758.5453123);
}

void main()
{
    vec2 st = gl_FragCoord.xy / min(RENDERSIZE.x, RENDERSIZE.y)-mouse;
    float color;

    st *= grid_size; //division

    vec2 ipos = floor(st);
    vec2 fpos = fract(st);

    vec2 point = random2(ipos)*point_pose_rndm;

    float direction = mod(ipos.x, 2.0) * 2.0 - 1.0;

    float t = TIME*speed;
    point += vec2(cos(t), sin(t)) * direction;

    vec2 diff = point - fpos;
    float distance = length(diff)/rad;

    color = length(distance);
    gl_FragColor = vec4(vec3(color),1.0);
}