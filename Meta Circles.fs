/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "generator"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tl3Rn by luutifa.  Looks sort of nice.",
  "INPUTS" : [ 
	{
	"NAME": "pos",
	"TYPE": "float"
	},{
	"NAME": "mod1",
	"TYPE": "float"
	},{
	"NAME": "mod2",
	"TYPE": "float"
	},
	{
	  "NAME": "color",
	  "TYPE": "color",
	  "DEFAULT": [
	    0.0,
	    1.0,
	    0.9,
	    1
	  ]
	}
  ]
}
*/


#define PI 3.141592654
#define TWO_PI 6.283185308

float roundLookingBlob(vec2 fragCoord, vec2 tPos, float r) {
    vec2 pos = fragCoord.xy/RENDERSIZE.yy - vec2(0.5);
    pos.x -= ((RENDERSIZE.x-RENDERSIZE.y)/RENDERSIZE.y)/2.0;
    return pow(max(1.0-length(pos-tPos), 0.0) , r);
}

void main() {
	float position = pos * TWO_PI;

	float v = roundLookingBlob(gl_FragCoord.xy,vec2(sin(position)*0.8 * mod2, cos(position)*0.4 * mod2), 7.0);
    v += roundLookingBlob(gl_FragCoord.xy,vec2(sin(position)*0.2 * mod1, cos(position)*0.3 * mod2), 6.0);
    v += roundLookingBlob(gl_FragCoord.xy,vec2(cos(position-0.8)*0.9 * mod1, sin(position-1.1) * 0.4 * mod2), 5.0);
    v += roundLookingBlob(gl_FragCoord.xy,vec2(cos(position)*0.2 * mod1, sin(position-0.9)*0.3 * mod1), 8.0);
    v = clamp((v-0.5)*1000.0, 0.0, 1.0);
	gl_FragColor = vec4(v, v, v, 1.0) * color;
}
