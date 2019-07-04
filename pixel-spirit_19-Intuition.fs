/*{
	"DESCRIPTION": "19 Intuition",
	"CREDIT": "Patricio Gonzalez Vivo ported by @colin_movecraft",
	"CATEGORIES": [
		"PIXELSPIRIT"
	],

	"INPUTS": [
	
		{
			"NAME": "orient",
			"TYPE": "float",
			"DEFAULT": 25.0,
			"MIN":0.0,
			"MAX":360.0
		}
		
	]
}*/

//19 Intuition points the way...

//dependencies


float fill(float x, float size){
	return 1.0 - step(size, x);
}

float triSDF(vec2 st){
	st = (st * 2.0 - 1.0 ) * 2.0;
	return max(abs(st.x) * 0.866025 + st.y * 0.5 , - st.y * 0.5);
}

vec2 rotate(vec2 st, float a){
	st = mat2( cos(a) , -sin(a), sin(a), cos(a) ) * (st - 0.5);
	return st + 0.5;
}

float map(float n, float i1, float i2, float o1, float o2){
	return o1 + (o2-o1) * (n-i1)/(i2-i1);
	
}

float t = TIME;

void main(){
	vec3 color = vec3(0.0);
	vec2 st = gl_FragCoord.xy/RENDERSIZE;
	
	st = rotate(st, radians(-orient));
	
	float sdf = triSDF(st);
	sdf /= triSDF(st + vec2(0.0, 0.2));
	color += fill(abs(sdf), 0.56 );
	
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




