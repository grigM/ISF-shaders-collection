/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#37459.3",
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
  "PASSES" : [
    {
      "TARGET" : "backbuffer",
      "PERSISTENT" : true
    }
  ],
  "ISFVSN" : "2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec4 px(int dx, int dy) {
	// Fetch pixel RGBA at relative location
	vec2 pos = vec2(gl_FragCoord.x - float(dx), gl_FragCoord.y - float(dy));
	if (pos.x < 0.0) {pos.x = RENDERSIZE.x-1.0;;}
	if (pos.y < 0.0) {pos.y = RENDERSIZE.y-1.0;}
	if (pos.x >= RENDERSIZE.x) {pos.x = 0.0;}
	if (pos.y >= RENDERSIZE.y) {pos.y = 0.0;}
	return IMG_NORM_PIXEL(backbuffer,mod(pos / RENDERSIZE,1.0));
}

float rand(int seed, vec2 pos) {
	// Random float based on TIME, location and seed
	return fract(sin(TIME*23.254 + float(seed)*438.5345 - pos.x*37.2342 + pos.y*73.25423)*3756.234);
}

float DoLife(float life, float neighborlife, float food, float poison) {
	bool living = life > 0.5;
	if ((neighborlife <= 0.0) && (food > .50)) {living = true;}
	if (neighborlife >= 1.5) {living = false;}
	if (poison * .35 >= food) {living = false;}
	return living ? 1.0 : 0.0;
}

void main( void ) {
	vec4 here = px(0,0);
	gl_FragColor.a = 1.0;
	vec4 buffer = IMG_NORM_PIXEL(backbuffer,mod(gl_FragCoord.xy / RENDERSIZE,1.0)); 
	if (here.a > 0.0) {
	
		if ((mod(TIME, 0.3) > 1.0/60.0) && (mouse.y < 0.1)) {
			// Mouse in bottom tenth of screen for slow motion
			gl_FragColor.rgb = here.rgb;
			return;
		}
		
		vec4 sum = here;
		sum += px(-1, 0);
		sum += px( 0,-1);
		sum += px( 0, 1);
		sum += px( 1, 0);
	
		here.r = DoLife(here.r, sum.r, sum.g, sum.b);
		here.g = DoLife(here.g, sum.g, sum.b, sum.r);
		here.b = DoLife(here.b, sum.b, sum.r, sum.g);
		gl_FragColor.rgb = mix(here.rgb, buffer.rgb, .49);
	} else {
		vec2 pos = gl_FragCoord.xy - RENDERSIZE.xy * 0.5;
		if (length(pos) < 40.0) {
			gl_FragColor.rgb = vec3(rand(464, pos)>0.5?1.0:0.0, rand(153, pos)>0.5?1.0:0.0, rand(83, pos)>0.5?1.0:0.0);
		} else {
			gl_FragColor.rgb = vec3(0.0);
		}
	}
}