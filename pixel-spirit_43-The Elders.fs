/*{
	"DESCRIPTION": "43 The Elders",
	"CREDIT": "Patricio Gonzalez Vivo ported by @colin_movecraft",
	"CATEGORIES": [
		"PIXELSPIRIT"
	],

	"INPUTS": [
	
			{
			"NAME": "count",
			"TYPE": "float",
			"DEFAULT": 3.0,
			"MIN":3.0,
			"MAX":24.0		}

		
	]
}*/

//43 The Elders

//dependencies

#define PI  3.14159265359
#define TAU 6.28318530717

float stroke(float x, float s, float w){
	float d = step(s, x+w * 0.5) - step(s, x - w * 0.5);
	return clamp(d,0.0,1.0);
	}

float circleSDF(vec2 st){
	return length(st-0.5)*2.0;
}

float vesicaSDF(vec2 st, float w){
	vec2 offset = vec2(w*0.5,0.0);
	return max(circleSDF(st-offset),circleSDF(st+offset));
}
vec3 bridge(vec3 c, float d, float s, float w){
	c *= 1.0 - stroke(d,s,w*2.0);
	return c + stroke(d,s,w);
}

vec2 rotate(vec2 st, float a){
	st = mat2( cos(a) , -sin(a), sin(a), cos(a) ) * (st - 0.5);
	return st + 0.5;
}


float map(float n, float i1, float i2, float o1, float o2){
	return o1 + (o2-o1) * (n-i1)/(i2-i1);
	
}

void main(){
	vec3 color = vec3(0.0);
	vec2 st = gl_FragCoord.xy/RENDERSIZE;
	
	float n = count;
	float a = TAU/n;
	for ( float i = 0.0; i < n*2.0; i++){
		vec2 xy =rotate(st, a * i);
		xy.y -= 0.09;
		float vsc = vesicaSDF(xy, 0.3);
		
		color = mix(color + stroke(vsc, 0.5, 0.1), mix(color, bridge(color, vsc, .5,.1), step(xy.x,0.5)-step(xy.y,0.4)),step(3.0,i));
		
		}
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



