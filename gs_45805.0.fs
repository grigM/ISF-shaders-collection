/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#45805.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float square (vec2 position, float x, float y, float z, float q) {

	float sq  = smoothstep ( 0.0, 0.0, position.x - x);
	      sq *= smoothstep ( 0.0, 0.0, position.x - x);
	      sq *= smoothstep ( 0.0, 0.0, -position.y + y);
	      sq *= smoothstep ( 0.0, 0.0, -position.y + y);
	      sq *= smoothstep ( 0.0, 0.0, -position.x + z);
	      sq *= smoothstep ( 0.0, 0.0, -position.x + z);
	      sq *= smoothstep ( 0.0, 0.0, position.y - q);
	      sq *= smoothstep ( 0.0, 0.0, position.y - q);
return sq;
}

void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );
	
	vec4 backColor   = vec4(0.0,0.0,0.0,1.0);
	float sq,sq1,sq2,sq3,sq4,sq5,sq6,sq7,sq8;
	
	// shapes colors //
	vec4 shapeColor1 = vec4(0.2+sin(TIME),0.2,0.2+sin(TIME),1.00);
	vec4 shapeColor2 = vec4(0.2+cos(TIME),0.3,0.3+cos(TIME),1.00);
	vec4 shapeColor3 = vec4(0.2+sin(TIME),0.4,0.4+sin(TIME),1.00);
	vec4 shapeColor4 = vec4(0.2+cos(TIME),0.5,0.5+cos(TIME),1.00);
	vec4 shapeColor5 = vec4(0.2+sin(TIME),0.6,0.6+sin(TIME),1.00);
	vec4 shapeColor6 = vec4(0.2+cos(TIME),0.7,0.7+cos(TIME),1.00);
	vec4 shapeColor7 = vec4(0.2+sin(TIME),0.8,0.8+sin(TIME),1.00);
	vec4 shapeColor8 = vec4(0.2+cos(TIME),0.9,0.9+cos(TIME),1.00);
			
	// coordinates - squares  //			
	
	sq1 = square (position, 0.1,  0.9-clamp( sin(TIME), 0.0, 0.7), 0.2, 0.1); 
	sq2 = square (position, 0.21, 0.9-clamp( cos(TIME), 0.0, 0.7), 0.3, 0.1); 
	sq3 = square (position, 0.31, 0.9-clamp(-sin(TIME), 0.0, 0.7), 0.4, 0.1);
	sq4 = square (position, 0.41, 0.9-clamp(-cos(TIME), 0.0, 0.7), 0.5, 0.1);
	sq5 = square (position, 0.51, 0.9-clamp( 0.5*sin(TIME), 0.0, 0.7), 0.6, 0.1);
	sq6 = square (position, 0.61, 0.9-clamp( 0.5*cos(TIME), 0.0, 0.7), 0.7, 0.1);
	sq7 = square (position, 0.71, 0.9-clamp(-tan(TIME), 0.0, 0.7), 0.8, 0.1);
	sq8 = square (position, 0.81, 0.9-clamp( sin(TIME), 0.0, 0.7), 0.9, 0.1);
		
	gl_FragColor =  mix (backColor, shapeColor1, sq1);
	gl_FragColor += mix (backColor, shapeColor2, sq2);
	gl_FragColor += mix (backColor, shapeColor3, sq3);
	gl_FragColor += mix (backColor, shapeColor4, sq4);
	gl_FragColor += mix (backColor, shapeColor5, sq5);
	gl_FragColor += mix (backColor, shapeColor6, sq6);
	gl_FragColor += mix (backColor, shapeColor7, sq7);
	gl_FragColor += mix (backColor, shapeColor8, sq8);
		
	
}