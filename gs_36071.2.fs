/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36071.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float pro(float v)
{
	float p= 0.1;

	for(int i = 1; i<=8; i++)//8
	{
		p += cos(((mod(TIME,1000.)+v*3.+cos(p*1.58))/float(i))*3.14);
		
	}
	return  (1./(atan(p)+2.))-0.5;
}

float fit(float dist) {
	dist /= length(vec2(dFdx(dist), dFdy(dist)));
    return smoothstep(0.3, .7, sqrt(abs(dist)*0.3));
}

void main( void ) {

	vec2 pos = ((gl_FragCoord.xy / RENDERSIZE) - 0.5) * 2.0;
	pos.x *= RENDERSIZE.x/RENDERSIZE.y;
	
	vec3 col = vec3(1);
	#define draw col = min(col+i/300., vec3(24.*abs(pro(pos.x)-pos.y + (.25*cos((pos.x-.5)*4.)))));
	for(float i = 0.; i <= 1.; i += 1./320.){
		draw;
		pos.x += 0.0001*(1.+i)*TIME;
		pos.y += 0.004;
	}
	gl_FragColor = vec4( col, 1.0 );

}