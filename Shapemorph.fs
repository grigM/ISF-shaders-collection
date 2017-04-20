/*{
	"CREDIT": "by isak.burstrom",
	"DESCRIPTION": "Converted from http://glslsandbox.com/e#29198.10",
	"CATEGORIES": [
		"XXX"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "scale",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "modulation",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 1.0,
			"MAX": 5.0
		}
	]
}*/


void main() {
	
	vec2 position = -1.0 + 2.0 * gl_FragCoord.xy / RENDERSIZE.xy;
	float _time = speed * TIME;
	
	vec2 i = position * scale;
	i = i + vec2(cos(_time - i.x) , sin(_time - i.y));
	
	float luma = length(i);
	
	luma = mod(luma, modulation); 
	luma = floor(luma);
	
	
	gl_FragColor = vec4(luma);

}