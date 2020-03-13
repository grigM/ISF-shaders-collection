/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59889.0"
}
*/


// bendy dicks
#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable




float wiggle(vec2 pos, float yoff, float ampl, float freq, float shape) {
	float thick = 0.1;
	float s = (sin(pos.x*shape+TIME*freq + yoff*17.0)-sin(TIME*freq + yoff*17.0)) * ampl;
	float d = pos.y - s - yoff;
	float color = abs(d) < thick*(1.0-pos.x) ? (thick-abs(d))/thick: 0.0;
	return color;	
}
vec3 pal(float t)
{
	return vec3(sin(t/2.0)+cos(t/5.76+14.5)*0.5+0.5,sin(t/2.0)+cos(t/4.76+14.5)*0.5+0.4,sin(t/2.0)+cos(t/3.76+14.5)*0.5+0.3);
}
	     
void main( void ) {
	float mn = min(RENDERSIZE.x,RENDERSIZE.y);
	vec2 pos = ( gl_FragCoord.xy / mn ) - vec2(0.0, 0.5);
	float color = wiggle(pos,  0.25, 0.05, 4.0, 8.5);
	color = max(color, wiggle(pos,  0.0, 0.05, 5.0, 9.7));
	color = max(color, wiggle(pos, -0.25, 0.05, 6.0, 10.2));
	gl_FragColor = vec4( pal(-TIME*12.0+pos.y*12.0+abs(pos.x)*16.0)*color, 1.0 );

}