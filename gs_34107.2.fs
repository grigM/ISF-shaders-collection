/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34107.2"
}
*/


#ifdef GL_ES
precision highp float;
#endif
#extension GL_OES_standard_derivatives : enable
#define PI 3.141592
mat2 rotate(float a) {
    return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float atan2(vec2 u){return atan(u.x,u.y);}
void main(void) {
	vec2 uv=(gl_FragCoord.xy*2.-RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y);
	float s=exp2(fract(TIME));//pow(2.,fract(TIME)+1.); //Zoom
	uv*=s;
	float c=7.; //Count
	float r=exp2(floor(log2(length(uv))-(1./c))-1.); // pow(2.,floor(log2(length(uv))-(1./c))-1.); //Radius
	float d=r/sin(PI/c); //Space distance
	float a=(atan2(rotate(PI/2.)*uv)+PI)/(PI*2.);
	a=floor(fract(a+(.5/c))*c)/c;
	vec2 uv2=uv*rotate(a*PI*2.);
	float b=length(uv2-vec2(d,0));
	float v = (b-r)*(200./s);
	gl_FragColor = vec4(vec3(v),1.);
}