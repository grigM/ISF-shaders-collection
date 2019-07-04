/*{
	"DESCRIPTION": "37 Holding Together",
	"CREDIT": "Patricio Gonzalez Vivo ported by @colin_movecraft",
	"CATEGORIES": [
		"PIXELSPIRIT"
	],

	"INPUTS": [
		{
			"NAME": "spin",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN":0.0,
			"MAX":360.0		},
			{
			"NAME": "interlock",
			"VALUES": [
				0.0,
				1.0,
				2.0
			],
			"LABELS": [
				"locking",
				"support",
				"supported"
			],
			"DEFAULT": 0.5,
			"TYPE": "long"
		}
	]
}*/

//37 Holding Together, and supporting each other...

//dependencies

float rectSDF(vec2 st, vec2 s){
	st = st*2.0 -1.0;
	return max(abs(st.x/s.x),abs(st.y/s.y));	
	}
float stroke(float x, float s, float w){
	float d = step(s, x+w * 0.5) - step(s, x - w * 0.5);
	return clamp(d,0.0,1.0);
}
vec2 rotate(vec2 st, float a){
	st = mat2( cos(a) , -sin(a), sin(a), cos(a) ) * (st - 0.5);
	return st + 0.5;
}
vec3 bridge(vec3 c, float d, float s, float w){
	c *= 1.0 - stroke(d,s,w*2.0);
	return c + stroke(d,s,w);
}


void main(){
	vec3 color = vec3(0.0);
	vec2 st = gl_FragCoord.xy/RENDERSIZE;
	st = rotate(st, radians(spin));
	
	float lock = 0.5;
	if (interlock == 0) lock = 0.5;
	if (interlock == 1) lock = 1.5;
	if (interlock == 2) lock = 0.0;
		
	st.x = mix(1.0 - st.x, st.x, step(lock, st.y));
	vec2 o = vec2(0.05, 0.00);
	vec2 s = vec2(1.0);
	float a = radians(45.0);
	float l = rectSDF(rotate(st+o, a),s);
	float r = rectSDF(rotate(st-o, -a),s);
	color += stroke(l, 0.145, 0.098);
	color = bridge(color, r, 0.145,.098);
	gl_FragColor = vec4(color,1.0);
}


/*
https://github.com/patriciogonzalezvivo/PixelSpiritDeck

 Copyright (c) 2017 Patricio Gonzalez Vivo ( http://www.pixelspiritdeck.com )
 All rights reserved.
 
 Redistribution and use in source and binary forms, with or without
 modification, are permitted provided that the following conditions are
 met:
 
 Redistributions of source code must retain the above copyright notice,
 this list of conditions and the following disclaimer.
 
 Redistributions in binary form must reproduce the above copyright
 notice, this list of conditions and the following disclaimer in the
 documentation and/or other materials provided with the distribution.
 
 THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */



