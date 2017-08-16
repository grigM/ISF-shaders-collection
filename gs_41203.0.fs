/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#41203.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float random (in vec2 st) { 
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233))) 
                * 43758.5453123);
}

// The MIT License
// Copyright © 2013 Inigo Quilez
float noise( in vec2 p )
{
    vec2 i = floor( p );
    vec2 f = fract( p );
    
    vec2 u = f*f*(3.0-2.0*f);

    return mix( mix( random( i + vec2(0.0,0.0) ), 
                     random( i + vec2(1.0,0.0) ), u.x),
                mix( random( i + vec2(0.0,1.0) ), 
                     random( i + vec2(1.0,1.0) ), u.x), u.y);
}


// Author: https://twitter.com/c0de4
void main( void ) {

  vec2 p = ( gl_FragCoord.xy * 2.0 - RENDERSIZE ) / min(RENDERSIZE.x, RENDERSIZE.y);

	float color = 0.0;
	
	for(float i = 0.; i < 4.; i++) {
		float j = i + 1.0;
		p += vec2(noise(p+j)-cos(TIME*.5)+noise(vec2(TIME))*.1, noise(p*j)-cos(TIME*j) * .01 );
		vec2 q = vec2(noise(vec2(p+j)) );
		float a = length(p-q);
		color += .02/length(p-a);
	}

  gl_FragColor = vec4( vec3( color ), 1.0 );	

}
