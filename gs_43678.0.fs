/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43678.0"
}
*/


// by rotwang, some functions for Krysler

#ifdef GL_ES
precision mediump float;
#endif


const float PI = 3.1415926535;
const float TWOPI = PI*2.0;




float Krysler_190(vec2 p, float blades)
{
	float angle = (atan(p.y, p.x)+PI)/TWOPI;
	float r = length(p); 

	float f =  sin(fract(angle*blades));
	
	float shade = f*(1.0-r); 

	return shade;
}


vec3 Krysler_190_clr(vec2 p, float blades)
{
	float r = length(p);
	
	float shade = Krysler_190(p, blades);
	vec3 clr = vec3(shade*0.2, shade*0.6, shade*1.0);
	return clr;
}






void main( void ) {
	float speed = TIME *0.5;
float aspect = RENDERSIZE.x / RENDERSIZE.y;
vec2 unipos = ( gl_FragCoord.xy / RENDERSIZE );
vec2 pos = vec2( (unipos.x*2.0-1.0)*aspect, unipos.y*2.0-1.0);

	vec3 clr = Krysler_190_clr(pos, 4.0)*fract(speed);
	gl_FragColor = vec4( clr, 1.0 );
}