/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
		{
			"NAME": "opacity",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -0.09,
			"MAX": 0.0099
			
		},
		{
			"NAME": "glow",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 3.0
			
		},
		{
			"NAME": "box_1_scale_amp",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.5,
			"MAX": 3.0
			
		},
		
		
		{
			"NAME": "box_1_scale_x",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.5,
			"MAX": 3.0
			
		},
		{
			"NAME": "box_1_scale_y",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.5,
			"MAX": 3.0
			
		},
		{
			"NAME": "box_1_scale_z",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.5,
			"MAX": 3.0
			
		},
		
		{
			"NAME": "box_2_scale_amp",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.5,
			"MAX": 4.0
			
		},
		
		{
			"NAME": "box_2_scale_x",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.1,
			"MAX": 0.6
			
		},
		{
			"NAME": "box_2_scale_y",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.1,
			"MAX": 0.6
			
		},
		{
			"NAME": "box_2_scale_z",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.1,
			"MAX": 0.6
			
		},
		
		{
			"NAME": "rot_speed_box_1",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 5.0
			
		},
		
		
		
		{
			"NAME": "rot_speed_box_2",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 5.0
			
		},
		
		{
			"NAME": "rot_offset_box_1",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.575
			
		},
		
		{
			"NAME": "rot_offset_box_2",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.575
			
		},
		

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#55484.1"
}
*/


//precision highp float;

mat2 rot(float a) {
    float c = cos(a), s = sin(a);
    return mat2(c,s,-s,c);
}

float box( vec3 p, vec3 b ) {
    vec3 d = abs(p) - b;
    return min(max(d.x,max(d.y,d.z)),0.0) + length(max(d,0.0));
}

float map(vec3 p, vec3 cPos) {
    vec3 q = p;
    q.xy *= rot((TIME*rot_speed_box_1)+rot_offset_box_1);
    q.xz *= rot((TIME*rot_speed_box_1)+rot_offset_box_1);
    float d = box(q, vec3(box_1_scale_x*box_1_scale_amp,box_1_scale_y*box_1_scale_amp,box_1_scale_z*box_1_scale_amp));

    vec3 q2 = p;
    q2.xy *= rot((-TIME*rot_speed_box_2)+rot_offset_box_2);
    q2.xz *= rot((-TIME*rot_speed_box_2)+rot_offset_box_2);
    float d2 = box(q2, vec3(box_2_scale_x*box_2_scale_amp,box_2_scale_y*box_2_scale_amp,box_2_scale_z*box_2_scale_amp));

    return max(d, -d2);
}

void main(void) {
    vec2 p = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
    vec3 ray = vec3(p, -1.0);
    vec3 cPos = vec3(0.0, 0.0, 4.0);

    // Phantom Mode https://www.shadertoy.com/view/MtScWW by aiekick
    float acc = 0.0;
    float t = 0.0;
    for (int i = 0; i < 99; i++) {
        vec3 pos = cPos + ray * t;
        float dist = map(pos, cPos);
        dist = max(abs(dist), 0.01-opacity);
        acc += exp(-dist*(3.0-glow));
        t += dist * 0.5;

        if (length(t) > 10.0) break;
    }

    vec3 col = vec3(acc * 0.01);
    gl_FragColor = vec4(col, 1.0);
}