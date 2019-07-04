/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	
    {
      "NAME" : "size",
      "TYPE" : "float",
      "DEFAULT" : 40.0,
      "MIN" : 10.0,
      "MAX" : 150.0
    },
    {
      "NAME" : "speed",
      "TYPE" : "float",
      "DEFAULT" : 0.75,
      "MIN" : 0.0,
      "MAX" : 2.0
    },
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#30736.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

//based on a gif I saw on imgur. 
//looked easy to duplicate. It was. 
//https://i.imgur.com/zk2rtku.gif


//float speed= 0.75;

float randomize(vec2 coords){
	//http://byteblacksmith.com/improvements-to-the-canonical-one-liner-glsl-rand-for-opengl-es-2-0/
	 	float a = 12.9898;
    	float b = 78.233;
    	float c = 43758.5453;
    	float dt= dot(coords.xy ,vec2(a,b));
    	float sn= mod(dt,3.14);
    	return fract(sin(sn) * c);
}

vec3 getColor(vec2 coords){
	coords.x = coords.x-mod(coords.x, size);
	coords.y = coords.y-mod(coords.y, size);
	
	float r = randomize(coords.xy);
	float g = randomize(coords.xy * 20.0);
	float b = randomize(coords.xy * 37.0);
	return vec3(r,g,b);
}

float triangleWave(float x){
	x = mod(x,2.0);
	if (x > 1.0) x = -x+2.0;
	return x;
}

bool inSize(vec2 coords){
	vec2 box = coords.xy-mod(coords.xy, size);
	vec2 center = box+(size/2.0);
	float size = (triangleWave((TIME * speed)+(randomize(box*98.0)*2.0))/2.0)*(size);
	return (abs(coords.x-center.x) < size && abs(coords.y-center.y) < size);
}

void main( void ) {
	vec3 color = vec3(0.0);
	if (inSize(gl_FragCoord.xy)) color = getColor(gl_FragCoord.xy);
	gl_FragColor = vec4( color, 1.0 );

}