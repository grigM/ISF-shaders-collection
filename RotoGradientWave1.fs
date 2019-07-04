/*{
	"CREDIT": "by mojovideotech",
	"DESCRIPTION": "",
	"CATEGORIES": [
	],
  "INPUTS": [
      {
            "NAME": "R",
            "TYPE": "float",
           "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 0.9
        },
         {
            "NAME": "G",
            "TYPE": "float",
           "DEFAULT": 0.75,
            "MIN": 0.0,
            "MAX": 0.9
        },
         {
            "NAME": "B",
            "TYPE": "float",
           "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 0.9
        },
          {
            "NAME": "C",
            "TYPE": "float",
           "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 0.9
        },
         {
            "NAME": "M",
            "TYPE": "float",
           "DEFAULT": 0.9,
            "MIN": 0.0,
            "MAX": 0.9
        },
         {
            "NAME": "Y",
            "TYPE": "float",
           "DEFAULT": 0.9,
            "MIN": 0.0,
            "MAX": 0.9
        },
         {
            "NAME": "waves",
            "TYPE": "float",
           "DEFAULT": 0.005,
            "MIN": 0.005,
            "MAX": 3.14
        }
  ]
}
*/
// RotoGradientWave1 by mojovideotech
// based on :
// http://glslsandbox.com/e#26993.2

#ifdef GL_ES
precision mediump float;
#endif

float pi = 3.14159;
void main( void ) {

	vec2 position = ( gl_FragCoord.xy / vec2(RENDERSIZE.x + 6.5, RENDERSIZE.y + 4.5));
    float w = 3.145-waves;
	float color = length(position.xy-vec2(0.4+0.3*cos(TIME-sqrt(TIME)),0.499+0.599*sin(-TIME)));
	      color -= length(position.yx-vec2(0.399+0.299*sin(TIME),0.5+0.6*cos(TIME-inversesqrt(TIME))));
	      color += length(position.xy-vec2(0.411+0.311*cos(TIME),0.511+0.611*cos(-TIME)));
	gl_FragColor = vec4(vec3(cos(color*1.25+w*0.667/2.667),sin(color*1.333+w/3.333),sin(color*4.0/w)),1.0);
    gl_FragColor *= vec4(0.1+R,0.1+G,0.1+B,1.0);
    gl_FragColor += vec4(1.0-C,1.0-M,1.0-Y,1.0);
}