/*{
	"DESCRIPTION": "45 Inner Truth",
	"CREDIT": "Patricio Gonzalez Vivo ported by @colin_movecraft",
	"CATEGORIES": [
		"PIXELSPIRIT"
	],

	"INPUTS": [
	
			{
			"NAME": "count",
			"TYPE": "float",
			"DEFAULT": 20.0,
			"MIN":8.0,
			"MAX":64.0		}

		
	]
}*/

//45 Inner Truth to Polar...

//dependencies

#define PI  3.14159265359
#define TAU 6.28318530717

float fill(float x, float size){
	return 1.0 - step(size, x);
}
float triSDF(vec2 st){
	st = (st * 2.0 - 1.0 ) * 2.0;
	return max(abs(st.x) * 0.866025 + st.y * 0.5 , - st.y * 0.5);
}

float rhombSDF(vec2 st){
	return max(triSDF(st),triSDF(vec2(st.x, 1.0-st.y)));
}
vec2 rotate(vec2 st, float a){
	st = mat2( cos(a) , -sin(a), sin(a), cos(a) ) * (st - 0.5);
	return st + 0.5;
}

float starSDF(vec2 st, int V, float s){
	st = st*4.0-2.0;
	float a = atan(st.y,st.x)/TAU;
	float seg = a * float(V);
	a = ((floor(seg) + 0.5)/float(V) + mix(s, -s,step(.5,fract(seg))))*TAU;
	return abs ( dot ( vec2 (cos(a),sin(a)),st));
	}
	
vec2 scale(vec2 st, vec2 s){
	return (st - 0.5) * s + 0.5;
}

float map(float n, float i1, float i2, float o1, float o2){
	return o1 + (o2-o1) * (n-i1)/(i2-i1);
	
}

void main(){
	vec3 color = vec3(0.0);
	vec2 st = gl_FragCoord.xy/RENDERSIZE;

	st -= 0.5;
	
	float r = dot(st,st);
	float a = (atan(st.y, st.x) / PI);
	vec2 uv = vec2(a,r);
	vec2 grid= vec2(5.0, log(r)*count);
	vec2 uv_i = floor(uv * grid);
	uv.x += 0.5* mod(uv_i.y, 2.0);
	vec2 uv_f = fract(uv*grid);
	float shape = rhombSDF(uv_f);
	color += fill(shape, 0.9) * step(0.5, 1.0 - r);

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



