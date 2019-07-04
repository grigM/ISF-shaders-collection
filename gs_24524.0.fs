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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#24524.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif
// Amiga decruncher v0.0000001


const float PI = 3.14159265358979323846264;

void main( void ) 
{
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	
	
	uv.x *= RENDERSIZE.x/ RENDERSIZE.y;

	
	
	float a = atan(uv.x, uv.y ) * 64./PI;
	float r = length(uv);

	
	vec3 finalColor1 = vec3( tan(TIME), 1.0,64.*sin(TIME) );
	
	vec3 finalColor2 = vec3(sin(1.*uv.y - 2.*TIME)*(floor((uv.x/(cos(TIME)/10.0)))+30.));
	
	vec3 FinalColor= mix( finalColor1, sin(finalColor2*TIME*PI), sin(TIME*mouse.y/10.)) ;
	
	gl_FragColor = vec4(cos(10.*(floor(FinalColor/0.4)*0.4)), 1.);
}