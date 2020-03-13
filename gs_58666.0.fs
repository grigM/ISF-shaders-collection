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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#58666.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float random (in vec2 _st) {
    return fract(sin(dot(_st.xy,
                         vec2(12.9898,78.233)))*
        43758.5453123);
}

vec2 truchetPattern(in vec2 _st, in float _index){
    _index = fract(((_index-0.5)*2.0));
    return _st;
}

void main( void ) {

	vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
   	st *= 1.0;
	
	vec2 ipos = floor(st);  // integer
    	vec2 fpos = fract(st);  // fraction
	
	
	vec2 tile = truchetPattern(fpos, random( ipos )) - mouse;
	
	float color = 0.0;
	
	// Maze
    	color = smoothstep(tile.x-0.2,tile.x,tile.y)-
            smoothstep(tile.x,tile.x+0.2,tile.y);
	
	 gl_FragColor = vec4(vec3(color),1.0);

}