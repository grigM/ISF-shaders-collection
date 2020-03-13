/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "8de3a3924cb95bd0e95a443fff0326c869f9d4979cd1d5b6e94e2a01f5be53e9.jpg"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3tdSRj by ajlende.  I saw a cool effect on https:\/\/burohaus.cargo.site\/ but I thought it would be even cooler with hexagonal tiling, so I borrowed some code from https:\/\/www.shadertoy.com\/view\/ltlSW4 and made it happen\n",
  "INPUTS" : [
  
  {
      "TYPE" : "image",
      "NAME" : "inputImage"
    }

  ]
}
*/


#define ISQRT3 0.5773502691896258
#define HEXES 5.0
#define MOVE 0.02

void main() {

    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
	vec2 p = gl_FragCoord.xy / RENDERSIZE.y;
    
    vec2 q = HEXES * vec2( p.x * 2.0 * ISQRT3, p.y + p.x * ISQRT3 );
	
	vec2 qi = floor( q );
	vec2 qf = fract( q );
	float v = mod( qi.x + qi.y, 3.0 );
	float ca = step( 1.0, v );
	float cb = step( 2.0, v );
	vec2  ma = step( qf.xy, qf.yx );
    vec2 r = qi + ca - cb * ma;
	
	vec2 o = MOVE * sin( TIME + r );
    
        
    gl_FragColor = IMG_NORM_PIXEL(inputImage,mod(uv + o,1.0));
}
