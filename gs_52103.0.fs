/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#52103.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


vec3 hsv2rgb (in vec3 hsv) {
	hsv.yz = clamp (hsv.yz, 0.0, 1.0);
	return hsv.z * (1.0 + 0.5 * hsv.y * (cos (1.0 * 3.14159 * (hsv.x + vec3 (0.0, 2.0 / 3.0, 1.0 / 3.0))) - 1.0));
}

float rand (in vec2 seed) {
	return fract (sin (dot (seed, vec2 (12.9898, 18.233))) * 137.5453);
}

void main () {
	vec2 frag = (2.0 * gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.y;
	//frag *= 1.0 - 0.2 * cos (frag.yx) * sin (3.14159 * 0.1);// * texture (iChannel0, vec2 (0.0)).x);
	frag *= 10.0;
	float random = rand (floor (frag));
	vec2 black = smoothstep (2.0, 0.9, tan (frag * 3.14159 * 1.0));
	vec3 color = vec3(0.75); //hsv2rgb (vec3 (random, 1.0, 1.0));
	color *= black.x * black.y * smoothstep (2.0, 0.9,length(fract(frag) - 0.5));
	color *= 0.9 + 0.9 * cos (random + random * (TIME * 9.0) + TIME + 3.14159 * 1.0); // * texture (iChannel0, vec2 (0.7)).x);
	gl_FragColor = vec4 (color * vec3(1.0,1.0,1.0), 1.0);
}