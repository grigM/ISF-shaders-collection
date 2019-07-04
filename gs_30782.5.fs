/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "bpm",
			"TYPE": "float",
			"DEFAULT": 128.0,
			"MIN": 93.0,
			"MAX": 186.0
		},
		{
			"NAME": "glitch",
			"TYPE": "float",
			"DEFAULT": 5.0,
			"MIN": 0.0,
			"MAX": 50.0
		},
		{
			"NAME": "bg",
			"TYPE": "bool",
			"DEFAULT": 1.0
		},
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#30782.5"
}
*/


#ifdef GL_ES
precision mediump float;
#endif
#extension GL_OES_standard_derivatives : enable

//#define bpm 128.
//put in the bpm of the song you're listening to
//e.g. Savant - Starscream Forever
//also remember to set zoom to 1

//#define glitch 5.
//#define bg true

//fixed it.
//this is the same as the previous, but without the matrix stuff I didn't end up using.
//I was considering making some of the glitch diagonal, 
//but I'd have to redo a bunch of it to support that...

float timestep = 60.0/bpm;
vec2 center = RENDERSIZE.xy/2.0;

float rand(vec2 co)
{
    float a = 12.9898;
    float b = 78.233;
    float c = 43758.5453;
    float dt= dot(co.xy ,vec2(a,b));
    float sn= mod(dt,3.14);
    return fract(sin(sn) * c);
}
vec3 getColor(){
	float slowTime = TIME-mod(TIME,timestep);
	float r = rand(vec2(slowTime));
	float g = rand(vec2(slowTime)*2.0);
	float b = 1.0-r;
	return vec3(r,g,b);
}

float pulse(){
	return (timestep-mod(TIME,timestep))/timestep;
}

float glitchFactor(vec2 coords){
	float slowTime = TIME-mod(TIME,0.12);
	return pulse() * rand(coords*1000.0 + slowTime/10.0)*glitch;
}

void main( void ) {

	vec3 color = vec3(0.0);
	float px = gl_FragCoord.x;
	float py = gl_FragCoord.y;
	
	float wobble = sin(px/35.0+TIME*3.0);
	vec2 glitchPosB = vec2(px+sin(TIME/1.5)*80.0,py-cos(TIME/3.0)*50.0+wobble*2.0); //less wobbly, moves around (inner ring)
	vec2 glitchPos = vec2(px,py+wobble*10.0); //wobbly (outer ring)
	
	
	if (distance(glitchPosB,center)>15.0*glitchFactor(gl_FragCoord.yy)+100.0 && 
	    distance(glitchPos,center)<30.0*glitchFactor(gl_FragCoord.xx)+300.0+pulse()*80.0){
		color += getColor();
	}
	
	if (distance(glitchPosB,center)>19.23*glitchFactor(gl_FragCoord.xy)+50.0 && 
	    distance(glitchPos,center)<15.0*glitchFactor(gl_FragCoord.xx)+200.0){
		color += getColor();
	}
	
	if (bg){
	color += getColor() * pulse()/4.0 + rand(gl_FragCoord.xy + cos(gl_FragCoord.x+TIME-mod(TIME,timestep)))/10.0;
	}
	
	gl_FragColor = vec4( color, 1.0 );

}