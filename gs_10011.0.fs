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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#10011.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


float pi = atan(1.)*4.;

vec3 LinearGrad(vec2 p1,vec2 p2,vec2 px)
{
	vec2 dir = normalize(p2-p1);
	float g = dot(px-p1,dir)/length(p1-p2);
	return vec3(clamp(g,0.,1.));
}

vec3 RadialGrad(vec2 p1,vec2 p2,vec2 px)
{
	float g = distance(p1,px)/length(p1-p2);
	return vec3(1.-clamp(g,0.,1.));
}

vec3 SquareGrad(vec2 p1,vec2 p2,vec2 px)
{
	vec2 p1x = abs(p1 - px);
	vec2 p12 = abs(p1-p2);
	float g = max(p1x.x,p1x.y)/max(p12.x,p12.y);
	return vec3(1.-clamp(g,0.,1.));
}

vec3 ConicalGrad(vec2 p1,vec2 p2,vec2 px)
{
	float ap1x = atan((p1-px).x,(p1-px).y)+pi;
	float ap12 = atan((p1-p2).x,(p1-p2).y);
	float g = abs((abs(mod((ap1x + pi -  ap12),(pi*2.))) - pi))/(pi);
	return vec3(clamp(g,0.,1.));
}

void main( void ) {

	vec2 p = ( gl_FragCoord.xy );
	vec2 sect = floor(clamp(p - RENDERSIZE/2.,0.,1.)+0.5);

	vec3 c = vec3(0);
	
	vec2 qres = RENDERSIZE/4.;
	
	if(sect == vec2(0,0))
	{
		c = ConicalGrad(qres,mouse*RENDERSIZE,p);
	}
	if(sect == vec2(1,0))
	{
		c = RadialGrad(vec2(qres*vec2(3,1)),mouse*RENDERSIZE,p);
	}
	if(sect == vec2(1,1))
	{
		c = SquareGrad(vec2(qres*vec2(3,3)),mouse*RENDERSIZE,p);
	}
	if(sect == vec2(0,1))
	{
		c = LinearGrad(vec2(qres*vec2(1,3)),mouse*RENDERSIZE,p);
	}
	
	gl_FragColor = vec4( vec3( c ), 1.0 );

}