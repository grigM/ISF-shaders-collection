/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35322.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {
	vec2 p = ( gl_FragCoord.xy / RENDERSIZE.xy ) + mouse.xy;
	p = 2.0 * p - 1.0; // 0~2를 -1~1로 바꿔주는 역할
	p.x *= RENDERSIZE.x /RENDERSIZE.y; //화면비에 맞게 px.를 변형
//	p = mod(p, 0.5) / 0.5 - 0.25; //mod는 나머지연산
//	p = mod(p, 0.5);
	p = sin(p * 3.14 * 1.5);
//	p += sin(p * 3.14 * 0.4 +  TIME);
	
	
	float d = distance(vec2(0.0, 0.0), p);
	
//	float a = sin(p.x * 3.14 * 100.0 - TIME) * 0.1;
	float a = sin(d * 3.14 * 10.0 - TIME) * 0.1;
	float b = sin(d * 3.14 + TIME * 0.2);
	float c = a + b;
	
	gl_FragColor = vec4( a, d/c, c, 1.0 );
}