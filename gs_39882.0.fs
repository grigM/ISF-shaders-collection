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
		"DEFAULT": 10,
		"MIN": 0,
		"MAX": 50
	},
	{
		"NAME": "zoom",
		"TYPE": "float",
		"DEFAULT": 2.360,
		"MIN": 1.0,
		"MAX": 5
	},
	{
		"NAME": "cosAmp",
		"TYPE": "float",
		"DEFAULT": 3.0,
		"MIN": 1,
		"MAX": 20
	},
	{
		"NAME": "lineW1",
		"TYPE": "float",
		"DEFAULT": 0.8,
		"MIN": 0,
		"MAX": 6
	},{
		"NAME": "lineW2",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0,
		"MAX": 8
	},{
		"NAME": "lineW3",
		"TYPE": "float",
		"DEFAULT": 1.5,
		"MIN": 0,
		"MAX": 9
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39882.0"
}
*/


// Author: Patricio Gonzalez Vivo
// Title: PICES


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

#define PI 3.14159265359	
#define TWO_PI 6.28318530718






vec2 polarCoords(vec2 st) {
	st = vec2(0.5)-st;
    float r = dot(st,st);
    float a = atan(st.y,st.x);
    return vec2(a,r);
}

float cosLine(vec2 st, float freq, float width) {
    float d = (cos(st.x*float(int(cosAmp)))*.4*freq)+0.8;
    return smoothstep(d-width,d,st.y)-smoothstep(d,d+width,st.y);
}

void main() {
	vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
    st = (st-.5)*zoom+.5;
    if (RENDERSIZE.y > RENDERSIZE.x ) {
        st.y *= RENDERSIZE.y/RENDERSIZE.x;
        st.y -= (RENDERSIZE.y*.5-RENDERSIZE.x*.5)/RENDERSIZE.x;
    } else {
        st.x *= RENDERSIZE.x/RENDERSIZE.y;
        st.x -= (RENDERSIZE.x*.5-RENDERSIZE.y*.5)/RENDERSIZE.y;
    }
    
	vec3 color = vec3(0.0);
    
    
    float t = (TIME*.2)*speed;
    
    vec2 polar = polarCoords(st);
    
    float m = abs(mod(polar.x+t,PI))/PI;
    color += (cosLine(polar,m,0.02)-m)*vec3(lineW3);

	
    polar.x += (.31415);
    color = max(color,vec3(lineW1)*cosLine(polar,m,0.022)*(1.-m));
    
    //polar.y +=  tan(TIME*0.5);
    polar.x += .31415;
    color = max(color,vec3(lineW2)*cosLine(polar,m,0.025)*(1.-m));
    

	gl_FragColor = vec4( color, 1.0 );
}