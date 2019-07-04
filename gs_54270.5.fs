/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54270.5"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float rand(vec2 n) { 
	return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}

float bayer2(vec2 a){
	return fract(dot(a,a));
    a = floor(a);
    return fract( dot(a, vec2(.5, a.y * .75)) );
}

#define bayer4(a)   (bayer2( .5*(a))*.25+bayer2(a))
#define bayer8(a)   (bayer4( .5*(a))*.25+bayer2(a))
#define bayer16(a)  (bayer8( .5*(a))*.25+bayer2(a))
#define bayer32(a)  (bayer16(.5*(a))*.25+bayer2(a))
#define bayer64(a)  (bayer32(.5*(a))*.25+bayer2(a))
#define bayer128(a) (bayer64(.5*(a))*.25+bayer2(a))

float triangularRand(vec2 n){
	return bayer16(n) + fract(bayer16(n - 0.2) -0.6) - 0.5;
}

void main( void ) {

	vec2 p = vv_FragNormCoord;
	vec2 m = p;
	
	vec2 position = p;
	position = abs(position);
	float dp = dot(position,position);
	position *= cos(dp);
	
	vec3 color = vec3(0.0);
	float dither = triangularRand(gl_FragCoord.xy);
	
	const float steps = 2.0;
	//color += floor(position.x * steps + dither) / steps;
	
	color = fract(color+dp);

	color = fract(color+dot(m,m)+TIME);

	gl_FragColor = vec4(color, 1.0 );

}