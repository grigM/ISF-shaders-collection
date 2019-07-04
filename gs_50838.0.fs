/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "blue_speed",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0,
		"MAX": 30.0
		
	},
	{
		"NAME": "magent_speed",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0,
		"MAX": 30.0
		
	},
	{
		"NAME": "blue_line_w",
		"TYPE": "float",
		"DEFAULT": 0.0025,
		"MIN": 0,
		"MAX": 0.0525
		
	},
	{
		"NAME": "magenta_line_w",
		"TYPE": "float",
		"DEFAULT": 0.0025,
		"MIN": 0,
		"MAX": 0.0525
		
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#50838.0"
}
*/


//precision mediump float;

const float PI = 3.15;

vec3 hsv(float h, float s, float v){
    vec4 t = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(vec3(h) + t.xyz) * 6.0 - vec3(t.w));
    return v * mix(vec3(t.x), clamp(p - vec3(t.x), 0.0, 1.0), s);
}

vec3 BlueSpecial(float t, float s){
    vec3 color;
    float wavval =  0.05 * ((sin(t * 0.0575) + 1.0) / 2.0) + 0.60;
    color = hsv(wavval , 0.88, s);
    return color;
}

vec3 MagentaSpecial(float t, float s){
    vec3 color;
    float wavval =  0.05 * ((sin(t * 0.0575) + 1.0) / 2.0) + 0.90;
    color = hsv(wavval , 0.88, s);
    return color;
}

void main(){
    vec2 p = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / RENDERSIZE;
    vec3 line = vec3(0.0);
    
    for(float fi = 0.0; fi < 5.0; ++fi){
        float offset = fi * PI / 100.0;        
        float value = 3.0 - sin((TIME) * fi * 0.15 + 0.1) * 1.5;
        float timer = TIME * fi * 0.08;
        line += blue_line_w / abs(p.y + sin(p.x * 1.0 + (timer*blue_speed) + offset) * 0.75) * BlueSpecial(fi + (TIME*blue_speed) ,value);
        line += magenta_line_w / abs(p.y + cos(1.0 - p.x * 1.0 + (timer*magent_speed) + offset) * 0.75) * MagentaSpecial(fi + TIME ,value);        
    }
    
    gl_FragColor = vec4(line, 1.0);
}