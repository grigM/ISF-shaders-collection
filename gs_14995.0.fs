/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#14995.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


//MrOMGWTF

vec3 flare(vec2 spos, vec2 fpos, vec3 clr)
{
	vec3 color;
	float d = distance(spos, fpos);
	vec2 dd;
	dd.x = spos.x - fpos.x;
	dd.y = spos.y - fpos.y;
	dd = abs(dd);
	
	color = clr * max(0.0, 0.025 / dd.y) * max(0.0, 1.1 -  dd.x);
	color += clr * max(0.0, 0.05 / d);
	color += clr * max(0.0, 0.1 / distance(spos, -fpos)) * 0.15 ;
	color += clr * max(0.0, 0.13 - distance(spos, -fpos * 1.5)) * 1.5 ;
	color += clr * max(0.0, 0.07 - distance(spos, -fpos * 0.4)) * 2.0 ;
	
	
	return color;
}

float noise(vec2 pos)
{
	return fract(1111. * sin(111. * dot(pos, vec2(2222., 22.))));	
}

void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy * 2.0 ) - 1.0;
	position.x *= RENDERSIZE.x / RENDERSIZE.y;
	float omega = TIME*2.-(sin(TIME)/1.5);
	float divisor = 1.-.5*cos(omega);
	vec3 color = flare(position, vec2(sin(omega)/2./divisor, cos(omega)/2./divisor) * 0.5 , vec3(0.5, 0.8, 1.5));
	
	

	gl_FragColor = vec4( color * (0.95 + noise(position*0.001 + 0.00001) * 0.05), 1.0 );

}