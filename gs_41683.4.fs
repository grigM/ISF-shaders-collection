/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#41683.4"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


// thanks: https://wgld.org/d/glsl/

// Author: https://twitter.com/c0de4
// moisture~


float random (in vec2 st) { 
    return fract(sin(dot(st.xy,vec2(12.9898,78.233)))
		 * 43758.5453123);
}

// The MIT License
// Copyright © 2013 Inigo Quilez
float n( in vec2 p )
{
    vec2 i = floor( p );
    vec2 f = fract( p );
    
    vec2 u = f*f*(3.0-2.0*f);

    return mix( mix( random( i + vec2(0.0,0.0) ), 
                     random( i + vec2(1.0,0.0) ), u.x),
                mix( random( i + vec2(0.0,1.0) ), 
                     random( i + vec2(1.0,1.0) ), u.x), u.y);
}

void main( void ) {
	vec2 m = vec2(cos(TIME), sin(TIME));
	
	vec2 p = ( gl_FragCoord.xy * 2. - RENDERSIZE.xy ) / min(RENDERSIZE.x, RENDERSIZE.y);
	p *= 1.5;
	#define d p = abs(p+n(p+m.x)-m.xy) / (dot(p-n(p+m.xy), p-n(p+m.xy)));
	d;d;
	vec2 q = vec2(p.x-n(p/m.x), p.y-n(p*m.yy));
	
	float c1 =  0.08 / length(p+n(m.yy)*.1);
	float c2 = 0.8 / length(q-n(m.xx)*.1);
	
	float c = mix(c1, c2, .5);

	gl_FragColor = vec4( vec3( c ), 1.0 );

}