/*{
	"DESCRIPTION": "41 Trinity",
	"CREDIT": "Patricio Gonzalez Vivo ported by @colin_movecraft",
	"CATEGORIES": [
		"PIXELSPIRIT"
	],

	"INPUTS": [
	
			{
			"NAME": "spread",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN":0.0,
			"MAX":1.0		}

		
	]
}*/

//41 Trinity triforce...

//dependencies

#define PI  3.14159265359
#define TAU 6.28318530717

float stroke(float x, float s, float w){
	float d = step(s, x+w * 0.5) - step(s, x - w * 0.5);
	return clamp(d,0.0,1.0);
}
vec2 rotate(vec2 st, float a){
	st = mat2( cos(a) , -sin(a), sin(a), cos(a) ) * (st - 0.5);
	return st + 0.5;
}
float polySDF(vec2 st, int V){
	
	st = st*2.0 - 1.0;
	float a = atan(st.x, st.y) + PI;
	float r = length(st);
	float v = TAU / float(V);
	return cos(floor(0.5 + (a / v) ) * v - a) * r;
	
}
vec3 bridge(vec3 c, float d, float s, float w){
	c *= 1.0 - stroke(d,s,w*2.0);
	return c + stroke(d,s,w);
}
float map(float n, float i1, float i2, float o1, float o2){
	return o1 + (o2-o1) * (n-i1)/(i2-i1);
	
}

void main(){
	vec3 color = vec3(0.0);
	vec2 st = gl_FragCoord.xy/RENDERSIZE;
	
	st.y = 1.0 - st.y;
	
	float s = 0.25;
	
	
	
	float t1 = polySDF(st + vec2(0.0, map(spread,0.0,1.0,0.0,0.175)), 3);
	float t2 = polySDF(st + vec2(map(spread,0.0,1.0,0.0,0.1), 0.0), 3);
	float t3 = polySDF(st - vec2(map(spread,0.0,1.0,0.0,0.1), 0.0), 3);
	
	color += stroke(t1, s, 0.08) + stroke(t2, s, 0.08) + stroke(t3, s, 0.08);
	float bridges = mix(mix(t1, t2,step(0.5,st.y)), mix(t3, t2, step(0.5, st.y)), step(0.5, st.x));
	color = bridge(color, bridges, s, 0.08);
	
	
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


