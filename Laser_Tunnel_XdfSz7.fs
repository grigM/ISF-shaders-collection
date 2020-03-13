/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel1",
      "PATH" : "0c7bf5fe9462d5bffbd11126e82908e39be3ce56220d900f633d58fb432e56f5.png"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XdfSz7 by Pitzik4.  Just a quick little shader I made when I was bored. I think it looks pretty good.",
  "INPUTS" : [

  ]
}
*/


#define PI 3.14159265359
#define REC_PI .3183098862
float lenSq(vec2 c) {
	return c.x*c.x + c.y*c.y;
}
vec2 get_polar(vec2 cart) {
	vec2 pol = vec2(atan(cart.y, cart.x), log(lenSq(cart)));
	pol.x = pol.x * REC_PI * .5 + .5;
	return pol;
}
float roundTo(float x, float prec) {
	return (floor(x*prec)+.5)/prec;
}
float get_beam(vec2 pol, float prec) {
	return IMG_NORM_PIXEL(iChannel1,mod(vec2(roundTo(pol.x, prec), roundTo((pol.y+pol.x*.1)*.01-TIME*.1,prec)*.5),1.0)).r;
}
void main() {

	vec2 uv = (gl_FragCoord.xy / RENDERSIZE.xy) * 2. - 1.;
	vec2 pol = get_polar(uv);
	float prec = IMG_SIZE(iChannel1).x;
	float beam = get_beam(pol, prec);
	beam = clamp(beam * 1024. - 920., 0., 1.5);
	beam *= sin((pol.x * prec - .25) * PI * 2.) * .5 + .5;
	gl_FragColor = vec4(beam , beam , beam, 1.0);
}
