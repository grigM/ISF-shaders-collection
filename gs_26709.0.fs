/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#26709.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


float fade(float value, float start, float end)
{
    //return (clamp(value,start,end)-start)/(end-start);
	return (value-start)/(end-start);
}

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

vec3 texture(vec2 uv) {
	return vec3(rand(floor(uv*10.)/10.),rand(floor(uv*10.)/12.),rand(floor(uv*10.)/14.));
}

void main( void ) {
	#define PI 3.1415926535897932384
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	vec2 road_uv = vec2(fade(uv.x,uv.y,0.5)*fade(1.-uv.x,uv.y,0.5),(uv.y*1.2)+(TIME/3.));
	float darken = pow(fade(uv.y,0.,0.5),6.);
	vec3 outputs = texture(road_uv)-darken;
	gl_FragColor = vec4(outputs, 1.0 );

}