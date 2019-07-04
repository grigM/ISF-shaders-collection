/*{
	"DESCRIPTION": "33 Vision",
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
			"MAX":360.0
		},
			{
			"NAME": "blink",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN":0.0,
			"MAX":1.0
		}
	]
}*/

//33 Vision. All knowing, all seeing, sometimes blinking...

//dependencies


#define PI  3.14159265359
#define TAU 6.28318530717


float stroke(float x, float s, float w){
	float d = step(s, x+w * 0.5) - step(s, x - w * 0.5);
	return clamp(d,0.0,1.0);
	}
	
	
float fill(float x, float size){
	return 1.0 - step(size, x);
}

float hexSDF(vec2 st){
	st = abs(st*2.0 - 1.0);
	return max(abs(st.y), st.x * 0.866025 + st.y * 0.5);
}

float circleSDF(vec2 st){
	return length(st-0.5)*2.0;
}

float rectSDF(vec2 st, vec2 s){
	st = st*2.0 -1.0;
	return max(abs(st.x/s.x),abs(st.y/s.y));	
	}
	
float flip(float v, float pct){
	return mix(v,1.0-v,pct);
	
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
	
float starSDF(vec2 st, int V, float s){
	st = st*4.0-2.0;
	float a = atan(st.y,st.x)/TAU;
	float seg = a * float(V);
	a = ((floor(seg) + 0.5)/float(V) + mix(s, -s,step(.5,fract(seg))))*TAU;
	return abs ( dot ( vec2 (cos(a),sin(a)),st));
	}
	
float vesicaSDF(vec2 st, float w){
	vec2 offset = vec2(w*0.5,0.0);
	return max(circleSDF(st-offset),circleSDF(st+offset));
}
	
float raysSDF(vec2 st, int N){
	st -= 0.5;
	return fract(atan(st.y,st.x)/TAU*float(N));
	}



float map(float n, float i1, float i2, float o1, float o2){
	return o1 + (o2-o1) * (n-i1)/(i2-i1);
}


float t = TIME;

void main(){
	vec3 color = vec3(0.0);
	vec2 st = gl_FragCoord.xy/RENDERSIZE;
	vec2 rst = st;
	rst = rotate(rst, radians(-spin));
	float v1 = vesicaSDF(st,.5);
	vec2 st2 = st.yx + vec2(.04,0.0);
	
	float stretch = map(blink,0.0,1.0,10.0,1.0);
	float offset = map(blink,0.0,1.0,4.5,0.0);	
	float thickness = map(blink,0.0,1.0,0.5,.05);
	float vesicaOffset = map(blink,0.0,1.0,1.0,.7);
	
	st2 *= vec2(stretch,1.0);
	st2 -= vec2(offset,0.0);
	
	
	float v2 = vesicaSDF(st2, vesicaOffset);
	color += stroke(v2, 1.0, thickness);
	color += fill(v2, 1.0) * stroke(circleSDF(st), 0.3, .05);
	color += fill(raysSDF(rst,50),.2)*fill(v1,1.25)*step(1.0,v2);
	

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




