/*{
	"DESCRIPTION": "14 Hope",
	"CREDIT": "Patricio Gonzalez Vivo ported by @colin_movecraft",
	"CATEGORIES": [
		"PIXELSPIRIT"
	],

	"INPUTS": [
	
		{
			"NAME": "pulse",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN":0.0,
			"MAX":0.5
		}
	]
}*/

//14 Hope. Pulsing with heart.  

//dependencies



float circleSDF(vec2 st){
	return length(st-0.5)*2.0;
}

float fill(float x, float size){
	return 1.0 - step(size, x);
}

float flip(float v, float pct){
	return mix(v,1.0-v,pct);
	
}

float vesicaSDF(vec2 st, float w){
	vec2 offset = vec2(w*0.5,0.0);
	return max(circleSDF(st-offset),circleSDF(st+offset));
}


//  Function from Iñigo Quiles 
//  www.iquilezles.org/www/articles/functions/functions.htm
float impulse( float k, float x ){
    float h = k*x;
    return h*exp(1.0-h);
}

float map(float n, float i1, float i2, float o1, float o2){
	return o1 + (o2-o1) * (n-i1)/(i2-i1);
	
}




#define PI 3.14159265359


void main(){
	vec3 color = vec3(0.0);
	vec2 st = gl_FragCoord.xy/RENDERSIZE;
	
	float t = fract(TIME*pulse);
	
	float p = map(impulse(10.0,t),0.0,1.0,0.2,0.6) ;
	 
	float sdf = vesicaSDF(st,p);
	color += flip(fill(sdf, 0.5), 
				step( (st.x + st.y) * 0.5, 0.5  ));
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



