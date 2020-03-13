/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#38937.2",
  "INPUTS" : [
    {
      "NAME" : "iscolor",
      "TYPE" : "bool",
      "DEFAULT" : 0,
      "LABEL" : "iscolor"
    }
  ],
  "ISFVSN" : "2"
}
*/



// RingCarrouselAntiAliased.glsl    

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define AAValue 300.

#define TWOPI_F 6.28318530718

vec2 uv;  // centered pixel position -1 .. 1

// draw ring at given position
float draw_ring(vec2 pos, float radius, float thick)
{
  return clamp(((1.0-abs(length(uv-pos)-radius))-1.00+thick)*AAValue, 0.0, 1.0); 
}

void main( void ) 
{
	float aspectRatio = RENDERSIZE.x / RENDERSIZE.y;
	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy )-0.5;
	position.x *= aspectRatio;
	uv = 2.*position;
	vec2 dims = vec2(aspectRatio, 1.0);
	vec2 midpt = dims * 0.5;
	vec3 color = vec3(0.);
	float increment = dims.x / 20.0;
	float angle = atan(position.y - midpt.y, position.x - midpt.x);
	angle = mod(angle, TWOPI_F);
	for(float i = 1.0; i < 12.0; i += 1.0)
	{
		vec2 p2 = vec2(position.x + sin(i + TIME * 2.0) * increment * 0.5
			      ,position.y + cos(i + TIME * 2.0) * increment * 0.5);

		float rc = draw_ring(p2, i * increment, 0.02);
		if(rc > 0.0)
		{
			if(iscolor){
			color = rc * vec3(sin(i) * 0.25 + 0.5,
				         sin(i + TIME) * 0.5 + 0.5, 
					 sin(i + TIME * 1.2) * 0.5 + 0.5);
			}else{
				
				color = rc * vec3(sin(i) * 0.25 + 0.5,
				         sin(i) * 0.25 + 0.5, 
					 sin(i) * 0.25 + 0.5);
			}
			break;
		}
	}
	gl_FragColor = vec4( color, 1.0 );
}