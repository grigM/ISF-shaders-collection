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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57366.0"
}
*/


#ifdef GL_ES
precision lowp
float;
#endif

#extension GL_OES_standard_derivatives : enable

uniform vec2 surfaceSize;

void main( void ) {
	
	vec2 uv = abs(mix(vv_FragNormCoord,gl_FragCoord.xy/RENDERSIZE,(cos(TIME)*0.5+0.5)));
	uv -= mouse;

	float t = (abs(fract((surfaceSize.x*surfaceSize.y * mouse.x + mouse.y)+(dot(vv_FragNormCoord,vv_FragNormCoord)))));
	
	gl_FragColor = vec4( (vec3( t * (uv * (t*2.0-1.0) + uv), t )), 1.0 );

}