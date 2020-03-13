/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57562.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif



#define speed 0.5
#define freq 1.8
#define amp 0.1
#define phase 2.9

//Let's make some friends
// https://github.com/Allakazan

// I love you guys <3


void main( void ) {

	vec2 p = ( gl_FragCoord.xy / RENDERSIZE.xy ) - 0.3;
	
	float sx = (amp)*1.9 * sin( 4.0 * (freq) * (p.x-phase) - 6.0 * (speed)*TIME)*sqrt((p.x+0.3)*TIME*0.6);
	
	float dy = 43./ ( 60. * abs(4.9*p.y - sx - 1.));
	
	gl_FragColor = vec4( (p.x + 0.05) * dy, 0.2 * dy, dy, 2.0 );
	

}