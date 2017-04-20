/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "rotate",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": -1,
		"MAX": 1
	},
	{
		"NAME": "zoom",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 4
	},
	{
      "NAME" : "color_1",
      "TYPE" : "color",
      "DEFAULT" : [
        1.0,
        1.0,
        1.0,
        1
      ],
      "LABEL" : ""
    },
    {
      "NAME" : "color_2",
      "TYPE" : "color",
      "DEFAULT" : [
        0.0,
        0.0,
        0.0,
        0.0
      ],
      "LABEL" : ""
    }

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39887.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define PI 3.14159265359


// Author @patriciogv - 2015
// http://patriciogonzalezvivo.com




float box(in vec2 st, in vec2 size){
    size = vec2(0.5) - size*0.5;
    vec2 uv = smoothstep(size,
                        size+vec2(0.001),
                        st);
    uv *= smoothstep(size,
                    size+vec2(0.001),
                    vec2(1.0)-st);
    return uv.x*uv.y;
}

float cross(in vec2 st, float size){
    return  box(st+(size), vec2(size,size/4.)) + 
            box(st+(size), vec2(size/4.,size));
}

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

float smoothedge(float v, float f) {
    return smoothstep(0.0, f / RENDERSIZE.x, v);
}

void main(){
    //vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
	vec2 st = 2.0*vec2(gl_FragCoord.xy-0.5*RENDERSIZE.xy)/RENDERSIZE.y;
	
    //rotate canvas
 	st = rotate2d(rotate*PI ) * st;
 	st /= zoom;
 	
 	
 	//vec3 color = mix(vec3(color_2), vec3(color_1), smoothedge(cross(st,0.5), smoothEdges));
 	vec3 color = mix(vec3(color_2), vec3(color_1), cross(st,0.5));
 	
 	gl_FragColor = vec4( color ,1.0);
    
 	
    //gl_FragColor = vec4( vec3( cross(st,0.5) ) ,1.0);
}