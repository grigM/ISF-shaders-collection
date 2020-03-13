/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	 {
      "NAME" : "itterations",
      "TYPE" : "float",
      "MAX" : 30.0,
      "DEFAULT" : 30.0,
      "MIN" : 1.0
    },
    {
      "NAME" : "zoom_out",
      "TYPE" : "float",
      "DEFAULT" : 7.0,
      "MIN" : 0.0,
      "MAX" : 20.0,
      
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#51190.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float createFractal(vec2 p){
	//const int itterations = 60;
	
	float sum = 0.0;
	
	float l = (p - 0.2).x;
	
	for (int i = 1; i < int(itterations); ++i){
		float n = float(i);
		float n2 = n * n;
		
		sum += sin(l * n2) / n2;
	}
	return abs(sum);
}

void main( void ) {

	vec2 position = gl_FragCoord.xy / RENDERSIZE.xy * zoom_out;

	vec3 color = vec3(0.0);
	color += createFractal(position);

	gl_FragColor = vec4(color, 1.0 );

}