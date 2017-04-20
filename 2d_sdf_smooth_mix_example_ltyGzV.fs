/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "distancefield",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ltyGzV by rtepub.  2d sdf smooth mix example",
  "INPUTS" : [

  ]
}
*/


// Author: Richard Eakin 
// Title: 2d sdf smooth mix example

float circle( vec2 p, float radius )
{
	return length( p ) - radius;
}

// http://mercury.sexy/hg_sdf/
float fOpUnionRound(float a, float b, float r)
{
	vec2 u = max(vec2(r - a,r - b), vec2(0));
	return max(r, min (a, b)) - length(u);
}

void main()
{
    vec2 p = ( gl_FragCoord.xy / RENDERSIZE.xy ) * 2.0 - 1.0;
    p.x *= RENDERSIZE.x / RENDERSIZE.y;

    vec3 color = vec3( 0.2 );    
    
    float radius = 0.4;
    float dist = 0.0; // this is the signed distance that we build the shape out of

    float c0 = circle( p + vec2( cos( TIME ) * 0.4, sin( cos( TIME ) * 2.0 ) * 0.55 ), radius );
    
    float c1 = circle( p + vec2( 0.5, -0.5 * sin( TIME * 1.3 ) ), radius - 0.1 );
	dist = fOpUnionRound( c0, c1, 0.2 );

    float c2 = circle( p + vec2( sin( TIME * 1.2 ), 0.5 * cos( TIME ) ), radius - 0.2 );
	dist = fOpUnionRound( dist, c2, 0.2 );

    float c3 = circle( p + vec2( sin( TIME * 0.7 ) * 0.8, 0.5 * cos( TIME ) ), radius - 0.2 );
	dist = fOpUnionRound( dist, c2, 0.2 );

	float w = smoothstep( - 0.01, 0.0, dist );    
    color = mix( color, vec3( 1 ), w );

    
    gl_FragColor = vec4(color,1.0);
}