/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "GLSLSandbox"
    ],
    "DESCRIPTION": "Automatically converted from http://glslsandbox.com/e#58801.1",
    "INPUTS": [
    ]
}

*/


#ifdef GL_ES
precision mediump float;
#endif


const float PI = 3.141592658;
const float TAU = 2.0 * PI;
const vec4 yellow = vec4(1,1,0,1);

// Doing the smoothstep... :)

void main(void)
{
	vec2 pos = vec2(gl_FragCoord.xy - 0.5 * RENDERSIZE.xy) / RENDERSIZE.y;
	float rad = length(pos);
	float angle = rad+TIME+atan(pos.y, pos.x*pos.x);
	float ma = mod(angle*0.4, TAU/5.0);
	float f = smoothstep(.1, .105, rad);
	gl_FragColor = mix(vec4(0, 0, 0, 1), yellow, smoothstep(PI/3.0, (PI/3.0)-.01, ma) * smoothstep(.0, .2, ma) * f-ma);
	gl_FragColor = mix(gl_FragColor, yellow, smoothstep(0.45, .455, rad) + smoothstep(0.06, .055, rad) ) ;
	gl_FragColor = mix(gl_FragColor, vec4(0, 0, 0, 1), smoothstep(0.38, .49, rad));
}