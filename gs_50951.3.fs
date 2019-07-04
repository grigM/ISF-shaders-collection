/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#50951.3"
}
*/




 
#ifdef GL_ES
precision highp float;
#endif

#define	TAU 6.28318

 
vec3 hsv2rgb_smooth( in vec3 c )
{
	vec3 rgb = clamp( abs(mod(c.x*6.0+vec3(0.0,4.0,2.0),6.0)-3.0)-1.0, 0.0, 1.0 );
	rgb = rgb*rgb*(3.0-2.0*rgb); // cubic smoothing	
	return c.z * mix( vec3(1.0), rgb, c.y);
}
void main( void ) 
{
float ss = 16.0*sin(TIME*0.9);
	vec2 gg = gl_FragCoord.xy;
	gg = ceil(gg / ss) * ss;	
	vec2 aspect = RENDERSIZE.xy / RENDERSIZE.y;
	vec2 uv = ( gg / RENDERSIZE.y ) - aspect / 2.0;
	float t1 = fract(TIME*0.3)*TAU;
	float t2 = fract(TIME*0.1)*TAU;
	uv *= 38.0;
	// REPEAT
	uv.x += TIME*10.0;
	uv.x = mod(uv.x,32.0)-16.0;
	uv.x *= sin(t1+uv.y*0.125)+2.1;
	float d = length(uv)*0.06;
	d = d*d;
	float a = sin(t2);
	vec3 col = hsv2rgb_smooth(vec3(fract(TIME*0.4)+(uv.y+uv.x*a)*((d+0.5)*0.05),1.2-d,0.9-d));
	gl_FragColor = vec4(col, 1.0);
}
 