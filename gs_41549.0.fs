/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 5
	},
	{
		"NAME": "TL",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": 0,
		"MAX": 10
	},
	{
		"NAME": "linesNum",
		"TYPE": "float",
		"DEFAULT": 0.75,
		"MIN": 0,
		"MAX": 3
	},
	{
		"NAME": "lineWIdth",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0,
		"MAX": 1
	},
	{
		"NAME": "lineBlur",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0,
		"MAX": 1
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#41549.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



float rand(float n){return fract(sin(n) * 43758.5453123);}
float noise(float p){
	float fl = floor(p);
  	float fc = fract(p);
	return mix(rand(fl), rand(fl + 1.0), fc);
}


void main( void ) {

		
	
    vec2 uv = (2.0 * gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.y;
    uv.x += 0.2 * sin(((TIME*speed)+TL) + uv.y * 4.);
    float numLines = 15.0 + gl_FragCoord.y * 0.4;
    float colNoise = noise(0.6 * uv.x * numLines);
    float colStripes = lineWIdth + lineBlur * sin(uv.x * numLines * linesNum);
    float col = mix(colNoise, colStripes, 0.5 + 0.5 * sin(((TIME*speed)+TL)));
    float aA = 1./(RENDERSIZE.x * 0.005) ;
    col = smoothstep(0.5 - aA, 0.5 + aA, col);
    gl_FragColor = vec4(vec3(col),1.0);
	
	

}



