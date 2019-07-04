/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#50913.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec2 transform(vec2 uv) {
	uv = .5 * (uv - uv.yx * vec2(1., -1.));
	uv += sign(uv) * min(abs(uv.x), abs(uv.y));
	return uv;
}

void main( void ) {
	const float width = 4.;
	
	vec2 position = (gl_FragCoord.xy * 2. - RENDERSIZE) / RENDERSIZE.y * width * .5;
	vec3 col = vec3(.5);

	float TIME = TIME * 1.8;
	
	float tt = mod(floor(TIME / 2.), 2.);
	position.x += (tt - .5) * width;
	
	float t = fract(TIME) * fract(TIME) * (3. - 2. * fract(TIME));
	
	position = mix(position, transform(position), t);
	if (mod(TIME, 2.) >= 1.) position = transform(position);
	
	if (mod(floor(position.x) + floor(position.y) + tt, 2.) < .5) {
		col *= 0.;
	}
	
	gl_FragColor = vec4(col, 1.);

}