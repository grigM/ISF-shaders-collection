/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35226.0",
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
    }
  ],
  "PERSISTENT_BUFFERS" : [
    "backbuffer"
  ],
  "PASSES" : [
    {
      "TARGET" : "backbuffer"
    }
  ]
}
*/


#ifdef GL_ES
precision highp float;
#endif


void main( void ) {
	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );
	vec2 pixel = 1./RENDERSIZE;
	vec4 me = IMG_NORM_PIXEL(backbuffer,mod(position,1.0));

	vec2 rnd = vec2(mod(fract(sin(dot(position + TIME * 0.001, vec2(14.9898,78.233))) * 43758.5453), 1.0),
	                mod(fract(sin(dot(position + TIME * 0.001, vec2(24.9898,44.233))) * 27458.5453), 1.0));
	vec2 nudge = vec2(12.0 + 10.0 * cos(TIME * 0.03775),
	                  12.0 + 10.0 * cos(TIME * 0.02246));
	vec2 rate = -0.005 + 0.02 * (0.5 + 0.5 * cos(nudge * (position.yx - 0.5) + 0.5 + TIME * vec2(0.137, 0.262)));

	float mradius = 0.007;//0.07 * (-0.03 + length(zoomcenter - mouse));
	if (length(position-mouse) < mradius) {
		me.r = 0.5+0.5*sin(TIME * 1.234542);
		me.g = 0.0;
		me.b = 0.0;
	} else {
		rate *= 6.0 * abs(vec2(0.5, 0.5) - mouse);
		rate += 0.5 * rate.yx;
		vec2 mult = 1.0 - rate;
		vec2 jitter = vec2(1.1 / RENDERSIZE.x,
		                   1.1 / RENDERSIZE.y);
		vec2 offset = (rate * mouse) - (jitter * 0.5);
		vec4 source = IMG_NORM_PIXEL(backbuffer,mod(position * mult + offset + jitter * rnd,1.0));
		
		me = me * 0.05 + source * 0.95;
	}
	gl_FragColor = me;
}