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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#53726.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

#define GS (20.0)
#define ms mouse*RENDERSIZE

vec2 rotate(float angle,vec2 center,vec2 v)
{
	float ca = cos(angle);
	float sa = sin(angle);
	return mat2(ca,sa,-sa,ca)*(v-center)+center;
}

mat3 rotMat(float angle,vec2 center)
{
	float ca = cos(angle);
	float sa = sin(angle);
	mat3 m = mat3(vec3(ca,-sa,0.0),vec3(+sa,ca,0.0),vec3(center.x*(1.0-ca)-sa*center.y,center.y*(1.0-ca)+center.x*sa,1.0));
	
	return m;
}

mat3 rangeMat(mat3 m,mat3 mi,float c)
{
	
	return mat3(vec3(mix(m[0],mi[0],c)),vec3(mix(m[1],mi[1],c)),vec3(mix(m[2],mi[2],c)));
}

vec3 hash32(vec2 p)
{
	vec3 p3 = fract(vec3(p.xyx) * vec3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yxz+19.19);
    return fract((p3.xxy+p3.yzz)*p3.zyx);
}

void main( void ) {

	vec2 c = gl_FragCoord.xy;
	
	c = vec2(rangeMat(rotMat(TIME/20.0,ms),rotMat(-TIME/20.0,ms),0.005*length(c-RENDERSIZE/2.0))*vec3(c,1.0));
	
	//c = rotate(TIME/16.0,RENDERSIZE/2.0,c)*rotate(TIME/10.0,mouse*RENDERSIZE.xy,c)/RENDERSIZE.xy;
	
	vec2 pd = floor(c/GS)*GS;
	
	
vec3 color = hash32(pd);


	
	gl_FragColor = vec4(color, 1.0 );

}