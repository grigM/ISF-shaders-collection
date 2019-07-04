/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "f735bee5b64ef98879dc618b016ecf7939a5756040c2cde21ccb15e69a6e1cfb.png"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MdSGW1 by kig.  Tinkering with a lightning effect. A particle seeks out a random path from cloud to ground, then the ground fills the path with electric fury...",
  "INPUTS" : [

  ]
}
*/


void main() {



	vec2 o = vec2(RENDERSIZE.x/2.0, RENDERSIZE.y*0.9);
	vec2 d = vec2(0.0, -1.0);
	vec2 uv = gl_FragCoord.xy;
	vec3 col = vec3(0.0);
	for (int i=0; i<100; i++) {
		vec4 tex = IMG_NORM_PIXEL(iChannel0,mod(vec2(float(i)/256.0, TIME),1.0),-100.0);
		vec2 tgt = vec2(RENDERSIZE.x/2.0, RENDERSIZE.y*0.1)-o;
		vec2 seek = normalize(tgt)*(16.0/(length(tgt)+1.0));
		d = normalize(seek+vec2(1.5, -1.0)*(vec2(-0.5, 0.0)+tex.gb));
		float len = min(length(tgt), 9.0 * (tex.r+0.1));
		float dist = abs(dot(o-uv, d.yx*vec2(1.0,-1.0)));
		o += d*len;
		if (dist < 1.5 && length(o-uv) < len*0.75) {
			col = vec3(1.0);
			break;
		}
	}
	gl_FragColor = vec4(col, 1.0);
}
