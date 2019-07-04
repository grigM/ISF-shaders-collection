/*{
	"DESCRIPTION": "09 The Moon",
	"CREDIT": "Patricio Gonzalez Vivo ported by @colin_movecraft",
	"CATEGORIES": [
		"PIXELSPIRIT"
	],

	"INPUTS": [

		{
			"NAME": "offset",
			"TYPE": "point2D",
			"DEFAULT": [
				0.1,
				0.05
			],
			
			"MIN": [
				-0.2,
				-0.2
			],
			
			"MAX": [
				0.2,
				0.2
			]
		}

	]
}*/

//09 The Moon her glory be makes sad sight of infiniti...

//dependencies
	
float circleSDF(vec2 st){
	return length(st-0.5)*2.0;
}

float fill(float x, float size){
	return 1.0 - step(size, x);
}

void main(){
	vec3 color = vec3(0.0);
	vec2 st = gl_FragCoord.xy/RENDERSIZE;
	
	color += fill(circleSDF(st),.65);
	vec2 offsetMoon = vec2(.1+offset.x,.05+offset.y);
	color -= fill(circleSDF(st-offsetMoon),.5);
	
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




