/*{
	"DESCRIPTION": "40 Turning Point",
	"CREDIT": "Patricio Gonzalez Vivo ported by @colin_movecraft",
	"CATEGORIES": [
		"PIXELSPIRIT"
	],

	"INPUTS": [


		
	]
}*/

//40 Turning Point

//dependencies

#define PI  3.14159265359
#define TAU 6.28318530717

float rectSDF(vec2 st, vec2 s){
	st = st*2.0 -1.0;
	return max(abs(st.x/s.x),abs(st.y/s.y));	
	}
float stroke(float x, float s, float w){
	float d = step(s, x+w * 0.5) - step(s, x - w * 0.5);
	return clamp(d,0.0,1.0);
}
float flip(float v, float pct){
	return mix(v,1.0-v,pct);
	
	}
	float polySDF(vec2 st, int V){
	
	st = st*2.0 - 1.0;
	float a = atan(st.x, st.y) + PI;
	float r = length(st);
	float v = TAU / float(V);
	return cos(floor(0.5 + (a / v) ) * v - a) * r;
	
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
	
	st = rotate(st, radians(-60.));
	
	st = (st - 0.5) * vec2(2) + 0.5;
	
	
	
	st.y = flip(st.y, step(0.5,st.x));
	st.y += .25;
	
	float down = polySDF(st, 3);
	
	st.y = 1.5 - st.y;
	
	float top = polySDF(st, 3);
	
	color += stroke(top, 0.4, 0.15);
	
	color = bridge(color, down, 0.4, 0.15);

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



