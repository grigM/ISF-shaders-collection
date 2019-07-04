/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#4024.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

//Bubbles by CuriousChettai@gmail.com

uniform vec2 
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	;

vec2 position(float TIME){
	TIME /= 10.0;
	return vec2((sin(TIME)-sin(TIME*3.0))/2.0, (sin(TIME+22.0)-cos(TIME*3.0))/5.0 );
}

void main( void ) {
	vec2 uPos = gl_FragCoord.xy/RENDERSIZE.y;
	uPos += vec2(-RENDERSIZE.x/RENDERSIZE.y/2.0, -0.5);

	vec3 color1 = vec3(0.0);
	const int count = 20;
	for(int i=0; i<count; i++){
		float dist = distance(position(TIME+float(i)*10.0), uPos);
		float radius = 0.1 * (float(i) + 10.0)/40.0;
		float thickness = 0.002;
		float color = 1.0 - smoothstep(radius+thickness, radius+2.0*thickness, dist) - smoothstep(radius+thickness, radius, dist)/1.2;
		
		color1 += vec3(color*(position(float(i*7)).x+1.0), color*(position(float(i*2)).x+1.0), color*(position(float(i*3)).x+1.0));
	}
	
	vec3 color2 = vec3(0.0);	
	for(int i=0; i<20; i++){
		float dist = distance(-position(23.0+TIME+float(i)*17.0), uPos);
		float radius = 0.05 * (float(i) + 10.0)/20.0;
		float thickness = 0.01 * (float(i*5)+10.0)/10.0;
		float color = 1.0 - smoothstep(radius+thickness, radius+2.0*thickness, dist) - smoothstep(radius+thickness, radius, dist)/1.5;
		
		color2 += vec3(color*(position(float(i*7)).x+1.0), color*(position(float(i*2)).x+1.0), color*(position(float(i*3)).x+1.0));
	}
	
	vec3 color3 = vec3(0.0);	
	for(int i=0; i<20; i++){
		float dist = distance(-position(TIME/5.0+float(i)*20.0), uPos);
		float radius = 0.3 * (float(i) + 10.0)/20.0;
		float thickness = 0.1;
		float color = 1.0 - smoothstep(radius+thickness, radius+2.0*thickness, dist) - smoothstep(radius+thickness, radius, dist)/1.5;
		
		color3 += vec3(color*(position(float(i*7)).x+1.0), color*(position(float(i*2)).x+1.0), color*(position(float(i*3)).x+1.0));
	}
	
	gl_FragColor = vec4(color1 + color2/10.0 + color3/10.0 , 1.0 );
}