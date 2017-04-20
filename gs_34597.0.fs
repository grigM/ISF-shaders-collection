/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
	{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 0.027,
			"MIN": 0.027,
			"MAX": 0.27
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34597.0"
}
*/




mat2 rotate = mat2(cos((TIME*speed)), -sin((TIME*speed)), sin((TIME*speed)), cos((TIME*speed)));

void main( void ) {
	vec2 aspect = RENDERSIZE.xy / min(RENDERSIZE.x, RENDERSIZE.y);
	vec2 position = (gl_FragCoord.xy / RENDERSIZE.xy) * aspect;
	vec2 center = 0.5 * aspect;
	vec3 color = vec3(0.0);
	
	position *= rotate;
	center *= rotate;
	
	vec2 next = vec2((speed), (speed)) * rotate;
	
	float a = 0.0;
	
	for(int i = 54; i > 0; i--) {
		next *= rotate;
		if(distance(position, center + next) < (speed*0.1)+ float(i) * (speed*0.1)) {
			color = vec3(sin(TIME + float(i)), sin(TIME + (speed*100.0) + float(i)), sin(TIME + 4.08 + float(i)));
			a = 1.0;
		}
		
	}
	
	gl_FragColor = vec4(color, a );

}