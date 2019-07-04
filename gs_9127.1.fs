/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#9127.1"
}
*/


//Coded by T_S/RTX1911 @T_SRTX1911
//REAL TiME XPRESS "RTX1911" (www.rtx1911.net)

#ifdef GL_ES
precision mediump float;
#endif


void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );

	float sync;
	
	if(sin(TIME * 0.25) > 0.0){
		sync = sin(TIME * 0.25) + 1.0;
	}else{
		sync = -sin(TIME * 0.25) + 1.0;
	}
	vec3 col = vec3(mod(floor(position.x * 16.0) + floor(position.y * 9.0), sync), 0.0, 0.0);
	
	gl_FragColor = vec4(col, 1.0 );

}