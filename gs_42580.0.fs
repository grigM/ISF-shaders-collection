/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#42580.0"
}
*/


#ifdef GL_ES
precision highp float;
#endif

#extension GL_OES_standard_derivatives : enable


float hash( vec2 p ) {
	float h = dot(p,vec2(127.1,311.7));	
    return fract(sin(h)*4758.5453123);
}

float noise( in vec2 p ) {
    vec2 i = floor( p );
    vec2 f = fract( p );	
	vec2 u = f*f*(3.0-2.0*f);
    return -1.0+2.0*mix( mix( hash( i + vec2(0.0,0.0) ), 
                     hash( i + vec2(1.0,0.0) ), u.x),
                mix( hash( i + vec2(0.0,1.0) ), 
                     hash( i + vec2(1.0,1.0) ), u.x), u.y);
}

void main( void ) {

	vec2 position = 2.0*( gl_FragCoord.xy / RENDERSIZE.xy )-1.0;
	vec3 colore = vec3(1.0,1.0,1.0);
	float rg=0.2;
	float f=0.0;
	
	for(float an=0.0;an<360.0;an+=45.0)
	{
		vec2 v2=vec2(TIME+an+position.x,TIME);
		float v=abs(noise(v2)/8.0);
		float x=(rg+v)*cos(an*3.141/180.0);
		float y=(rg+v)*sin(an*3.141/180.0);
		float d=sqrt( (position.x-x)*(position.x-x)+(position.y-y)*(position.y-y) );
		if(d<(rg+v))
		{
			colore=vec3(0,0,0);
			f=1.0;
			break;
		}
	}
	
	if(f==0.0)
	{
		float v=pow(sqrt( position.x*position.x+position.y*position.y ),1.0);
		colore=vec3(v,v,v);
	}
	

	
	gl_FragColor = vec4( colore, 1.0 );

}