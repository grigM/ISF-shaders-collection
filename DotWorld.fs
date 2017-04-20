/*{
	"CREDIT": "by mojovideotech",
  "CATEGORIES": [
    ""
  ],
  "DESCRIPTION": "",
  "INPUTS": [
   {
      "MAX": [
        0.9,
        0.9
      ],
      "MIN": [
        0.01,
        0.01
      ],
      "NAME": "colorshift",
      "TYPE": "point2D"
    },
    	{
      "MAX": 300.0,
      "MIN": 2.0,
      "DEFAULT": 30.00,
      "NAME": "size",
      "TYPE": "float"
    },
    	{
      "MAX": 2.0,
      "MIN": -8.0,
      "DEFAULT": -3.0,
      "NAME": "brightness",
      "TYPE": "float"
    },
    	{
      "MAX": 2.00,
      "MIN": -2.00,
      "DEFAULT": 0.5,
      "NAME": "rate",
      "TYPE": "float"
    },
        {
      "MAX": 0.8,
      "MIN": 0.05,
      "DEFAULT": 0.35,
      "NAME": "tint",
      "TYPE": "float"
    }
  ]
}
*/
// DotWorld by mojovideotech
// based on :
// http://glslsandbox.com/e#26933.0

#ifdef GL_ES
precision mediump float;
#endif


void main( void ) {
	float N = size;
	float invN = 1./N;
	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy ) + mod(TIME,invN) / 3.0;
	vec2 cell = vec2(ivec2(invN*gl_FragCoord.xy));
	vec2 center = N*vec2(cell)+vec2(0.5*N,0.5*N);
	float d = distance(gl_FragCoord.xy, center);
	float c = 1.-smoothstep(0.4 * N, 0.45*N, d*.9);
	vec4 bg = vec4(.9-tint,.1,.1+tint,1.);
	float a0 = colorshift.x+0.5*sin(0.9*cell.x +TIME)*sin(cell.y + 5.*cos(0.4*TIME));
	float a1 = colorshift.y+0.5*sin(0.1*cell.y+10.*sin(cell.x)*TIME*rate);
	
		
	float y = 0.5*(a0+a1);
	vec4 top_bw = vec4(y-tint,y,y+tint,y);
	vec4 top_c = vec4(a0,a1,0.,1.0);
//	     top_c -= vec4(a0,a1,0.5,0.5*(brightness*0.5));
	float d2 = distance(RENDERSIZE.xy*inversesqrt(-TIME), center);
	float s = smoothstep(-0.5*N, 3.*N,d2)-smoothstep(3.*N,6.*N,d2);
	s = step(8.*N,d2)-step(9.*N,d2) + 1. - step(0.5*N,d2);
	vec4 top = mix(0.5*top_bw, top_c, s*s);
	gl_FragColor = mix(bg,top,c);
	gl_FragColor *= (top_c,bg,s+c)/mod(y,TIME)+brightness;
}