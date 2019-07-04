/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    
    
    {
      "NAME" : "x_val",
      "TYPE" : "float",
      "DEFAULT" : 0.5,
      "MIN" : 0.0,
      "MAX" : 1.0
    },
    {
      "NAME" : "y_val",
      "TYPE" : "float",
      "DEFAULT" : 0.5,
      "MIN" : 0.0,
      "MAX" : 1.0
    }
    
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#27146.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


float random (vec2 st) { 
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233+0.0001*x_val)))* 
        43758.5453123);
}

float eq(float v, float compareTo)
{
    return step(compareTo-1.,v) * step(v, compareTo+1.); 	
}

void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );
	position.x *= 100.;
	position.y *= 100.;
	
	
	float line = floor(position.y);
	position.x += TIME*40.*(mod(line,2.)*2. -1.)*random(vec2(line));
	
	
	vec2 ipos = floor(position);
	vec2 fpos = fract(position);
	
	
	
	
	vec3 color = vec3(step(y_val*random(vec2(line)), x_val*random(ipos)));
	//vec3 color = vec3(fpos,0.);
	
	
	gl_FragColor = vec4( color, 1.0 );

}