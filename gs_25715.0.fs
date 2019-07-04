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
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#25715.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif



#define PI 3.14159265359

#define SCALE 8.

#define CORNER 16.

float rand(vec2 n) { 
	return fract(cos(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}
float noise(vec2 n) {
	const vec2 d = vec2(0.0, 1.0);
	vec2 b = floor(n), f = smoothstep(vec2(0.0), vec2(1.0), fract(n));
	return mix(mix(rand(b), rand(b + d.yx), f.x), mix(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}
float fbm(vec2 n) {
	float total = 0.0, amplitude = 1.0;
	for (int i = 0; i < 4; i++) {
		total += noise(n) * amplitude;
		n += n;
		amplitude *= 0.5;
	}
	return total * 0.5;
}

vec3 hsv2rgb(vec3 c) {
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main( void ) {

	vec2 position = vv_FragNormCoord * SCALE;
	vec2 realPos = ( gl_FragCoord.xy / RENDERSIZE.xy) - 0.5;
	
	vec2 mousePos = (mouse) - 0.5;
	vec3 light = vec3((mousePos - realPos), 0.5);

	float shiftrand = fbm(position*5.)*4.-2.;
	vec2 ptan = vec2(tan(position.x * PI), tan(position.y * PI)) - shiftrand;
	vec3 normal = normalize(vec3(ptan*.5, CORNER));
	
	float bright = dot(normal, normalize(light));
	bright = pow(bright, 5.);
	//bright *= step(length(position), 1.);
	
	vec3 color = hsv2rgb(vec3((floor(position.x + 0.5))/SCALE, 1., 1.)) * bright;
	
	//vec3 heif = normalize(light + vec3(0., 0., 1.));
	vec3 heif = normalize(light);
	
	vec3 spec = vec3(pow(dot(heif, normal), 96.));
	
	color += spec;

	//gl_FragColor = vec4( vec3( color, color * 0.5, sin( color + TIME / 3.0 ) * 0.75 ), 1.0 );
	gl_FragColor = vec4(color, 1.);

}