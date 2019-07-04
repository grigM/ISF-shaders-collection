/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#51736.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


float rand(vec2 co){
    return fract(sin(dot(floor(co.xy) ,vec2(0,1)+tan(TIME*0.0001)))*500.0);
}

void main( void ) {

	vec2 position = (gl_FragCoord.xy - RENDERSIZE/2.0) / vec2(min(RENDERSIZE.x, RENDERSIZE.y));
	
	float warp = floor(4.0*pow(pow(position.x,2.0)+pow(position.y,2.0), 0.5));
	float warpTime = warp+TIME;
	
	vec2 colpos = (1.0/(warp+1.0))*position.xy+0.1*vec2(sin(warpTime), cos(warpTime));
	float r = rand(102.0*colpos);
	float g = rand(101.0*colpos);
	float b = rand(100.0*colpos);
		
	gl_FragColor = vec4( r, g, b, 1 )*(warp+0.5)*0.2;

}