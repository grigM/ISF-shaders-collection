/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#9239.3"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


vec3 grid(vec2 p)
{
	float s = 0.125;
	vec2 tp = vec2(mod(p,s)/s);
	
	float tile = max(abs(tp.x-0.5),abs(tp.y-0.5))*2.;
	
	return vec3(smoothstep(0.9,1.,tile)*32.);
}

void main( void ) {

	vec2 p = ( (gl_FragCoord.xy) / RENDERSIZE.xy );

	vec2 uv1 = normalize(vec3(p-mouse,0.05)).xy;
	vec2 uv2 = normalize(vec3(p-(1.-mouse),0.03)).xy;
	
	vec2 uvc = normalize(vec3(p-.5,0.04)).xy;
	
	vec2 fuv = (uvc+uv1+uv2)/3.;
	
	vec3 fc = grid(p+fuv.xy);
	
	//fc = vec3(fuv*.5+.5,.1);
		
	gl_FragColor = vec4( fc, 1.0 );

}