/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#52939.0",
  "INPUTS" : [

  ],
  "PERSISTENT_BUFFERS" : [
    "backbuffer"
  ],
  "PASSES" : [
    {
      "TARGET" : "backbuffer",
      "PERSISTENT" : true
    }
  ]
}
*/


#ifdef GL_ES
precision mediump float;
#endif


float circle(vec2 pos, float radius){
	vec2 diff = pos - gl_FragCoord.xy;
	return 1.0 - step(radius * radius, dot(diff, diff));
}

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

float line(float x){
	float radius = RENDERSIZE.y * 0.1;
	vec2 pos = vec2(RENDERSIZE * 0.5) + vec2(cos(x * 1.51) * radius, sin(x * 1.01) * radius);
	pos += vec2(cos(x * 0.123) * radius * 0.25, sin(x * 0.124)) * radius * 0.251;
	pos += vec2(cos(x * 0.0695) * radius * 0.125, sin(x * 0.249)) * radius * 0.1251;
	pos.y += sin(x * 0.245) * RENDERSIZE.y * 0.25;
	
	float circlerad = (sin(x) * 0.5 + 0.5);
	return circle(pos, RENDERSIZE.y * 0.01 * circlerad) * circlerad;
}

void main( void ) {
	//vec2 position = ( gl_FragCoord.xy / RENDERSIZE.y) * 2.0 - vec2(1.0);
	//float a = step(position.y, sin(TIME + position.x * 2.0));
	
	
	
	//vec4 color = IMG_NORM_PIXEL(backbuffer,mod(gl_FragCoord.xy,1.0));
	
	for(int i = 0; i < 60; i++){
		float a = TIME * 1.5 + float(i) * 95.471234987234;
		float x = line(a);
		
		if(x > 0.2){
			gl_FragColor = vec4(hsv2rgb(vec3(a * 0.125, (sin(a) + 1.0) * 0.5, 0.9)), 1.0);
			return;
		}
	}
	
	//float a = line(TIME * 1.5);
	
	//if(a >= 0.1) gl_FragColor = vec4(hsv2rgb(vec3(TIME * 0.25, (sin(TIME) + 1.0) * 0.5, 0.9)), 1.0);
	//else gl_FragColor = IMG_NORM_PIXEL(backbuffer,mod(vec2(gl_FragCoord.xy/RENDERSIZE.xy),1.0)) - 0.002;
	gl_FragColor = IMG_NORM_PIXEL(backbuffer,mod(vec2(gl_FragCoord.xy/RENDERSIZE.xy),1.0)) - 0.002;
}