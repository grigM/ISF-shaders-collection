/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43267.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


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

// author: c0de4 https://twitter.com/c0de4
// なんや
void main( void ) {
	float t = TIME;

	vec3 p = vec3(( gl_FragCoord.xyx * 3. - RENDERSIZE.xyy  ) / min(RENDERSIZE.x, RENDERSIZE.y));
	p -= .8;
	
	// float c = 0.;
	
	for(float i = 0.; i < 3.; i++) {
		vec3 q = p + n(vec2(p.yyy+t));
		p.xyz = vec3( (abs(p*q.xxy*cos(t)) + dot(p-n(q.zz)+cos(t+q.xxz), p) - p.zyz * n(vec2(q+p)) ) );
	}
	
	gl_FragColor = vec4( vec3( p ), 1. );

}