/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#3177.1"
}
*/


//by @mrdoob, @IndialanJones
//mod ToBSn
//mod nnorm
#ifdef GL_ES
precision highp float;
#endif


void main( void ) {
	vec2 position = gl_FragCoord.xy / RENDERSIZE.xy;
	float t = 8.;
	float color = 0.0;
	color = sin( position.x * cos( t / 166.0 ) * 20.0 ) + cos( position.y * cos( t / 165.0 ) * 10.0*sin(TIME) );
	color *= sin( position.y * sin( t / 10.0 ) * 40.0 ) + cos( position.x * sin( t / 225.0 ) * 100.0*sin(TIME) );
	color *= sin( position.x * sin( t / 5.0 ) * 10.0 ) + sin( position.y * sin( t / 35.0 ) * 80.0*sin(TIME) );
	color -= sin( t / 100.0 );
	color /= 0.001*abs(sin(TIME));
	float c1 = smoothstep(0.0, color, -15.5);
	float c2 = smoothstep(color, 0.0, -500.5);
	float c3 = c1 + c2;
	gl_FragColor = vec4( vec3( c3, c3, c3 ), 1.0 );

}