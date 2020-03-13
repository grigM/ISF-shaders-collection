/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "GLSLSandbox"
    ],
    "DESCRIPTION": "Automatically converted from http://glslsandbox.com/e#58772.1",
    "INPUTS": [
    ]
}

*/


#ifdef GL_ES
precision mediump float;
#endif


void main (void) {
	vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	vec3 color = vec3(sin(0.2 * TIME), 0.1 * sin(TIME), 0.5 * sin(TIME));
	
	float a = pow(sin(uv.x*2.1416),.9)*pow(sin(TIME+1.5/uv.y*4.1416),.9);
	gl_FragColor = mix(vec4(vec3(a),1.0), vec4(color,1.), 0.1);
}