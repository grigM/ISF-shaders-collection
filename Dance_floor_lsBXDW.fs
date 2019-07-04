/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/lsBXDW by Nrx.  Simple test...",
  "INPUTS" : [
    {
      "NAME" : "iChannel0",
      "TYPE" : "audio"
    }
  ]
}
*/


vec3 hsv2rgb (in vec3 hsv) {
	hsv.yz = clamp (hsv.yz, 0.0, 1.0);
	return hsv.z * (1.0 + 0.5 * hsv.y * (cos (2.0 * 3.14159 * (hsv.x + vec3 (0.0, 2.0 / 3.0, 1.0 / 3.0))) - 1.0));
}

float rand (in vec2 seed) {
	return fract (sin (dot (seed, vec2 (12.9898, 78.233))) * 137.5453);
}

void main() {

	vec2 frag = (2.0 * gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.y;
	frag *= 1.0 - 0.2 * cos (frag.yx) * sin (3.14159 * 0.5 * IMG_NORM_PIXEL(iChannel0,mod(vec2 (0.0),1.0)).x);
	frag *= 5.0;
	float random = rand (floor (frag));
	vec2 black = smoothstep (1.0, 0.8, cos (frag * 3.14159 * 2.0));
	vec3 color = hsv2rgb (vec3 (random, 1.0, 1.0));
	color *= black.x * black.y * smoothstep (1.0, 0.0, length (fract (frag) - 0.5));
	color *= 0.5 + 0.5 * cos (random + random * TIME + TIME + 3.14159 * 0.5 * IMG_NORM_PIXEL(iChannel0,mod(vec2 (0.7),1.0)).x);
	gl_FragColor = vec4 (color, 1.0);
}
