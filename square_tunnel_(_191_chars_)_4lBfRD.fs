/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4lBfRD by FabriceNeyret2.  .",
  "INPUTS" : [

  ]
}
*/





void main() {

	vec2 uv = gl_FragCoord.xy;
	

	vec2 R = RENDERSIZE.xy;
	
    uv.xy = (uv.xy+uv.xy-R)/R.y;
    float t = .1*(TIME-9.9), r = 1., c,s;
    
    gl_FragColor -= gl_FragColor;
    for( int i=0; i< 99; i++)
	    uv.xy *= mat2(c=cos(t),s=sin(t),-s,c),
        r /= abs(c) + abs(s),
        gl_FragColor = smoothstep(3./R.y, 0., max(abs(uv.x),abs(uv.y)) - r) - gl_FragColor;
}


