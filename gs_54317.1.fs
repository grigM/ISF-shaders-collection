/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54317.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define PI 3.141592
#define TAU 2.*PI

float SDF(vec3 p)
{
	p+=vec3(sin(-TIME+p.z*3.0)*0.3,0.0,0.0);
	return length(p.xy)-0.5;
}

void main( void ) {

	// Normalized pixel coordinates (from 0 to 1)
	vec2 uv = 2.*(gl_FragCoord.xy/RENDERSIZE.xy)-1.;
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
	
	vec3 p = vec3 (0.0,0.0,0.0);
	vec3 dir = normalize(vec3(uv*2.,1.));
	
	for (int i=0;i<60;i++)
	{
		float d = SDF(p);
		if (abs(d)<0.001)
		{
			break;
		}
		p += d*dir*0.5;
	}
	
	// Time varying pixel color
	vec3 col = vec3(1.0-length(p)*0.5);
	
	// Output to screen
	gl_FragColor = vec4(col,1.0);
}