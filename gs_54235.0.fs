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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54235.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


//sampler2d

// https://github.com/hughsk/glsl-hsv2rgb/blob/master/index.glsl
vec3 hsv2rgb(vec3 c) {
  vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
  vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
  return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main( void ) {
float ss = 32.0*sin(TIME*0.3);
	vec2 gg = gl_FragCoord.xy;
	gg = ceil(gg / ss) * ss;	

	//vec2 position = ( gg / RENDERSIZE.xy ) + mouse / 4.0;

	/*float color = 0.0;
	color += sin( position.x * cos( TIME / 15.0 ) * 80.0 ) + cos( position.y * cos( TIME / 15.0 ) * 10.0 );
	color += sin( position.y * sin( TIME / 10.0 ) * 40.0 ) + cos( position.x * sin( TIME / 25.0 ) * 40.0 );
	color += sin( position.x * sin( TIME / 5.0 ) * 10.0 ) + sin( position.y * sin( TIME / 35.0 ) * 80.0 );
	color *= sin( TIME / 10.0 ) * 0.5;*/
	//float color = length(position);
	float bins = 10.0;
	vec2 pos = (gg / RENDERSIZE.xy);
	float bin = floor(pos.x * bins + 0.3*sin(3.0*TIME)*sin(TIME+pos.y*10.0));
	gl_FragColor = vec4( hsv2rgb(vec3(bin/bins, 0.5, 0.8)), 1.0 );

}